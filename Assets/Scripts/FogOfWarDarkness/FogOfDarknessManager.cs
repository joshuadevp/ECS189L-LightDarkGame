using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FogOfDarknessManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int mapWidthAndHeight; // Height and width of points of darkness.
    [SerializeField]
    float distanceBetweenPoints; // Distance between points of darkness, assumes all space between two points contains darkness.
    [SerializeField]
    Vector2Int activeWidthAndHeight; // Height and width of set of active points of darkness in world space.
    [SerializeField]
    Transform activeCenter; // Center point where we want darkness to be active around
    [SerializeField]
    GameObject darknessPrefab;
    [SerializeField]
    float updateInterval; // Time in seconds for manager to update fog system
    [SerializeField]
    IDarknessGenerator darknessGenerator;

    private float intervalWatch = 0;

    private DarknessPoint[][] darknessArray;
    private HashSet<DarknessPoint> spreadablePoints; // set of points that can spread
    private GameObject[][] objectPool; // Pool of objects to be placed in active area

    private Vector2 oldPosition;
    private int activeWidth;
    private int activeHeight;
    private int mapWidth;
    private int mapHeight;

    private Mesh customMesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals;
    private float meshHeight = 0f;

    private DarknessPoint[] activePoints;
    private int activeSize;
    private Vector2[] openPointsBuffer = new Vector2[4];
    private DarknessPoint[] spreadablePointsBuffer;

    // Start is called before the first frame update
    void Start()
    {
        InitAllDarkness();
    }

    // Update is called once per frame
    void Update()
    {
        intervalWatch += Time.deltaTime;
        if (intervalWatch > updateInterval)
        {
            intervalWatch = 0;
            DeactivatePoints();
            ActivatePoints();
            SpreadPoints();
            oldPosition = activeCenter.position;
        }
    }

    // Returns true if the given world point is within darkness
    public bool IsDarkness(Vector2 loc)
    {
        (int x, int y) = worldToIndex(loc);
        if (x == -1)
        {
            return false;
        }
        return IsDarknessIndex(x, y);
    }

    // Creates all darkness points and game objects
    // EXPENSIVE!!! Only use when initializing game
    public void InitAllDarkness()
    {
        // Init data
        spreadablePoints = new HashSet<DarknessPoint>();
        activeWidth = activeWidthAndHeight.x;
        activeHeight = activeWidthAndHeight.y;
        mapWidth = mapWidthAndHeight.x;
        mapHeight = mapWidthAndHeight.y;
        vertices = new Vector3[activeHeight * activeWidth * 4];
        triangles = new int[activeHeight * activeWidth * 6];
        normals = new Vector3[activeHeight * activeWidth * 4];
        customMesh = new Mesh() { name = "DarknessMesh" };
        activePoints = new DarknessPoint[activeWidth * activeHeight];
        activeSize = 0;
        spreadablePointsBuffer = new DarknessPoint[mapWidth * mapHeight];

        // Spawn darkness points
        darknessArray = new DarknessPoint[mapWidth][];
        for (int i = 0; i < mapWidth; i++)
        {
            darknessArray[i] = new DarknessPoint[mapHeight];
        }
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                CreateDarknessPointIndex(x, y, darknessGenerator.Generate(new Vector2(x, y)));
            }
        }

        // Spawn pooled game objects
        objectPool = new GameObject[activeWidth][];
        for (int i = 0; i < activeWidth; i++)
        {
            objectPool[i] = new GameObject[activeHeight];
        }
        for (int x = 0; x < activeWidth; x++)
        {
            for (int y = 0; y < activeHeight; y++)
            {
                objectPool[x][y] = Instantiate(darknessPrefab, Vector2.zero, Quaternion.identity);
                objectPool[x][y].transform.parent = this.gameObject.transform;
            }
        }
    }

    // Creates a point of darkness at the given world coord if possible
    // Returns true on success
    public bool CreateDarknessPoint(Vector2 coordinates, DarknessSpec spec)
    {
        (int x, int y) = worldToIndex(coordinates);
        if (x == -1) return false; // fail on invalid coord
        CreateDarknessPointIndex(x, y, spec);
        return true;
    }

    // Remove darkness point at given location
    // public void RemoveDarkness(Vector2 loc)
    // {
    //     (int x, int y) = worldToIndex(loc);

    //     RemoveDarkness(index);
    // }

    // Create a circle of darkness points at the given world position and world
    // based radius
    public void CreateDarknessPointsCircle(Vector2 center, float radius, DarknessSpec spec)
    {
        (int x, int y) = worldToIndex(center);
        int indexRadius = Mathf.RoundToInt(radius / distanceBetweenPoints);

        CreateDarknessPointsCircleIndex(x, y, indexRadius, spec);
    }

    public void RemoveDarknessPointsCircle(Vector2 center, float radiusf)
    {
        (int x, int y) = worldToIndex(center);
        int radius = Mathf.RoundToInt(radiusf / distanceBetweenPoints);

        for (int ix = -radius; ix <= radius; ix++)
        {
            for (int iy = -radius; iy <= radius; iy++)
            {
                if (iy * iy + ix * ix <= radius * radius + radius && ValidIndex(x + ix, y + iy))
                {
                    RemoveDarkness(x + ix, y + iy);
                }
            }
        }
    }

    // Returns array of active darkness points
    // Array may be larger than active points so also
    // returns number of active points
    public (DarknessPoint[], int) GetActivePoints()
    {
        return (activePoints, activeSize);
    }

    // Returns the custom mesh representing darkness points
    public Mesh GetMesh()
    {
        return customMesh;
    }

    // Call to cause damage to individual darkness point
    // Returns true if point is destroyed
    public bool DamageDarkness(Vector2 location, int damage)
    {
        (var x, var y) = worldToIndex(location);
        DarknessPoint p = darknessArray[x][y];
        p.CurrentHealth -= damage;
        if (p.CurrentHealth <= 0)
        {
            RemoveDarkness(x, y);
            return true;
        }
        return false;
    }

    private void CreateDarknessPointsCircleIndex(int x, int y, int radius, DarknessSpec spec)
    {
        for (int ix = -radius; ix <= radius; ix++)
        {
            for (int iy = -radius; iy <= radius; iy++)
            {
                if (iy * iy + ix * ix <= radius * radius + radius && ValidIndex(x + ix, y + iy))
                {
                    CreateDarknessPointIndex(x + ix, y + iy, spec);
                }
            }
        }
    }

    private bool ValidIndex(int x, int y)
    {
        return x >= 0 && x < mapWidth && y >= 0 && y < mapHeight;
    }

    // Remove darkness at given index by setting to null settings
    private void RemoveDarkness(int x, int y)
    {
        spreadablePoints.Remove(darknessArray[x][y]);
        darknessArray[x][y].Init(DarknessSpec.GetNullSpec());
    }

    // Returns if index has darkness
    private bool IsDarknessIndex(int x, int y)
    {
        if (ValidIndex(x, y))
            return darknessArray[x][y].IsAlive();
        return false;
    }

    // Creates a point of darkness given index in array and associated data
    // Returns created DarknessPoint or null if index invalid
    private DarknessPoint CreateDarknessPointIndex(int x, int y, DarknessSpec spec)
    {
        if (!ValidIndex(x, y))
        {
            Debug.LogError("Invalid index for creating darkness point! x: " + x + " y: " + y);
            return null;
        }
        DarknessPoint point = new DarknessPoint();
        point.WorldPosition = indexToWorld(x, y);
        point.IndexPosition = new Vector2Int(x, y);
        point.Init(spec);
        point.SetActive(false);
        if (darknessArray[x][y] != null)
        {
            spreadablePoints.Remove(darknessArray[x][y]);
        }
        darknessArray[x][y] = point;
        spreadablePoints.Add(point);
        return point;
    }

    // Converts world coordinates to index in internal array
    // Invalid conversion results in (-1,-1) return
    private (int, int) worldToIndex(Vector2 loc)
    {
        if (loc.x < 0 || loc.x > mapWidth * distanceBetweenPoints || loc.y < 0 || loc.y > mapHeight * distanceBetweenPoints)
        {
            return (-1, -1);
        }
        return (Mathf.RoundToInt(loc.x / distanceBetweenPoints), Mathf.RoundToInt(loc.y / distanceBetweenPoints));
    }

    // Converts index to coordinates in world
    // Assumes any point within the bounds o
    private Vector2 indexToWorld(int x, int y)
    {
        return new Vector2(x * distanceBetweenPoints, y * distanceBetweenPoints);
    }

    // Returns true if index position is surrounded by darkness and thus cannot spread
    private bool IsSurrounded(int x, int y)
    {
        return darknessArray[x + 1][y].IsAlive() &&
        darknessArray[x - 1][y].IsAlive() &&
        darknessArray[x][y + 1].IsAlive() &&
        darknessArray[x][y - 1].IsAlive();
    }

    // Returns array of available points to spread to around index location
    // And the size of the array
    private (Vector2[], int) OpenSurrounding(int x, int y)
    {
        DarknessPoint p;
        int index = 0;
        p = darknessArray[x + 1][y];
        if (p.CurrentHealth <= 0)
        {
            openPointsBuffer[index] = p.WorldPosition;
            index++;
        }
        p = darknessArray[x - 1][y];
        if (p.CurrentHealth <= 0)
        {
            openPointsBuffer[index] = p.WorldPosition;
            index++;
        }
        p = darknessArray[x][y + 1];
        if (p.CurrentHealth <= 0)
        {
            openPointsBuffer[index] = p.WorldPosition;
            index++;
        }
        p = darknessArray[x][y - 1];
        if (p.CurrentHealth <= 0)
        {
            openPointsBuffer[index] = p.WorldPosition;
            index++;
        }
        return (openPointsBuffer, index);
    }

    // Clamp index within array ranges
    // Clamped such that the outer edge is clamped so we can ignore clamping
    // while looping through points
    private int clampIndexX(int x)
    {
        return Mathf.Clamp(x, 1, mapWidth - 2);
    }
    private int clampIndexY(int y)
    {
        return Mathf.Clamp(y, 1, mapHeight - 2);
    }

    // Deactivate all points in old position
    private void DeactivatePoints()
    {
        (int oldCenterX, int oldCenterY) = worldToIndex(oldPosition);
        if (oldCenterX == -1) return; // Invalid position

        var corners = GetCorners(oldCenterX, oldCenterY);
        Vector2Int bottomLeftIndex = corners.Item1;
        Vector2Int topRightIndex = corners.Item2;
        int bLX = bottomLeftIndex.x;
        int bLY = bottomLeftIndex.y;
        int tRX = topRightIndex.x;
        int tRY = topRightIndex.y;

        for (int x = bLX; x <= tRX; x++)
        {
            for (int y = bLY; y <= tRY; y++)
            {
                DarknessPoint p = darknessArray[x][y];
                p.SetActive(false);
            }
        }
    }
    private void ActivatePoints()
    {
        (int x, int y) = worldToIndex(activeCenter.position);
        // Exit early if location outside grid
        if (x == -1)
        {
            return;
        }

        var corners = GetCorners(x, y);
        Vector2Int bottomLeftIndex = corners.Item1;
        Vector2Int topRightIndex = corners.Item2;

        int bottomLeftX = bottomLeftIndex.x;
        int bottomLeftY = bottomLeftIndex.y;
        int topRightX = topRightIndex.x;
        int topRightY = topRightIndex.y;

        int meshIndex = 0;
        customMesh.Clear();

        for (int ix = bottomLeftX; ix <= topRightX; ix++)
        {
            for (int iy = bottomLeftY; iy <= topRightY; iy++)
            {
                DarknessPoint p = darknessArray[ix][iy];
                p.SetActive(true);
                // Use object pool to setup colliders
                GameObject obj = objectPool[ix - bottomLeftX][iy - bottomLeftY];
                if (p.IsAlive())
                {
                    activePoints[meshIndex] = p;
                    obj.transform.position = p.WorldPosition;
                    if (!IsSurrounded(p.IndexPosition.x, p.IndexPosition.y))
                    {
                        spreadablePoints.Add(p);
                    }

                    // Try spawning enemy
                    p.Spawn();

                    /*
                    *   Add square to custom mesh
                    */
                    float worldX = p.WorldPosition.x;
                    float worldY = p.WorldPosition.y;

                    int verticesIndex = meshIndex * 4;
                    vertices[verticesIndex] = new Vector3(worldX - distanceBetweenPoints / 2, worldY - distanceBetweenPoints / 2, meshHeight); // bottom left
                    vertices[verticesIndex + 1] = new Vector3(worldX - distanceBetweenPoints / 2, worldY + distanceBetweenPoints / 2, meshHeight); // top left
                    vertices[verticesIndex + 2] = new Vector3(worldX + distanceBetweenPoints / 2, worldY - distanceBetweenPoints / 2, meshHeight); // bottom right
                    vertices[verticesIndex + 3] = new Vector3(worldX + distanceBetweenPoints / 2, worldY + distanceBetweenPoints / 2, meshHeight); // top right

                    int trianglesIndex = meshIndex * 6;
                    triangles[trianglesIndex] = 0 + verticesIndex;
                    triangles[trianglesIndex + 1] = 1 + verticesIndex;
                    triangles[trianglesIndex + 2] = 2 + verticesIndex;
                    triangles[trianglesIndex + 3] = 2 + verticesIndex;
                    triangles[trianglesIndex + 4] = 1 + verticesIndex;
                    triangles[trianglesIndex + 5] = 3 + verticesIndex;

                    normals[verticesIndex] = Vector3.back;
                    normals[verticesIndex + 1] = Vector3.back;
                    normals[verticesIndex + 2] = Vector3.back;
                    normals[verticesIndex + 3] = Vector3.back;

                    meshIndex++;
                }
                else
                {
                    obj.transform.position = new Vector2(-1, -1);
                }
            }
        }

        // Record how many active points we have
        activeSize = meshIndex;
        // Fill rest of active points with null
        for (int i = meshIndex; i < activePoints.Length; i++)
        {
            activePoints[i] = null;
        }

        // Set mesh data
        customMesh.SetVertices(vertices, 0, meshIndex * 4);
        customMesh.SetTriangles(triangles, 0, meshIndex * 6, 0);
        customMesh.SetNormals(normals, 0, meshIndex * 4);
    }

    // Call spread function of all darkness points
    // and removes those that can't spread
    private void SpreadPoints()
    {
        // Copy points so we can remove items from active set
        spreadablePoints.CopyTo(spreadablePointsBuffer);
        for (int i = 0; i < spreadablePoints.Count; i++)
        {
            DarknessPoint p = spreadablePointsBuffer[i];
            (Vector2[] availablePoints, int size) = OpenSurrounding(clampIndexX(p.IndexPosition.x), clampIndexY(p.IndexPosition.y));
            if (size > 0)
            {
                // If it spreads and there was only 1 open spot we can
                // immediately remove it
                if (p.Spread(availablePoints, size, this) && size == 1)
                {
                    spreadablePoints.Remove(p);
                }
            }
            else
            {
                // No open spaces, point can't spread and should be removed
                spreadablePoints.Remove(p);
            }
        }
    }

    // Returns pair of Vector2Int representing bottom left and top right corner
    // of active rectangle around center
    private (Vector2Int, Vector2Int) GetCorners(int x, int y)
    {
        int centerX = x;
        int centerY = y;

        int indexX = Mathf.FloorToInt(activeWidth / 2 * distanceBetweenPoints);
        int indexY = Mathf.FloorToInt(activeHeight / 2 * distanceBetweenPoints);

        Vector2Int topRightIndex = new Vector2Int(clampIndexX(centerX + indexX), clampIndexY(centerY + indexY));
        Vector2Int bottomLeftIndex = new Vector2Int(clampIndexX(centerX - indexX), clampIndexY(centerY - indexY));

        return (bottomLeftIndex, topRightIndex);
    }

#if UNITY_EDITOR

    // Draw darkness points as cells in grid
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (darknessArray != null)
                {
                    var point = darknessArray[x][y];
                    Gizmos.color = point.IsAlive() ? Color.green : Color.red;
                    if (point.IsActive()) Gizmos.color = Gizmos.color + Color.yellow; // Tint active cells yellow
                    Gizmos.color *= point.Density; // shade with point density
                }
                Gizmos.DrawWireCube(indexToWorld(x, y), new Vector2(distanceBetweenPoints, distanceBetweenPoints));
            }
        }
    }
#endif
}
