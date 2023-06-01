using System.Collections;
using System.Collections.Generic;
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

    private DarknessPoint[,] darknessArray;
    private HashSet<DarknessPoint> spreadablePoints; // set of points that can spread
    private GameObject[,] objectPool; // Pool of objects to be placed in active area

    private Vector2 oldPosition;
    private int activeWidth;
    private int activeHeight;

    private

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
        activeWidth = mapWidthAndHeight.x;
        activeHeight = mapWidthAndHeight.y;

        // Spawn darkness points
        darknessArray = new DarknessPoint[activeWidth, activeHeight];
        for (int x = 0; x < activeWidth; x++)
        {
            for (int y = 0; y < activeHeight; y++)
            {
                CreateDarknessPointIndex(x, y, darknessGenerator.Generate(new Vector2(x, y)));
            }
        }

        // Spawn pooled game objects
        objectPool = new GameObject[activeWidthAndHeight.x + 1, activeWidthAndHeight.y + 1];
        for (int x = 0; x < activeWidthAndHeight.x + 1; x++)
        {
            for (int y = 0; y < activeWidthAndHeight.y + 1; y++)
            {
                objectPool[x, y] = Instantiate(darknessPrefab, Vector2.zero, Quaternion.identity);
                objectPool[x, y].transform.parent = this.gameObject.transform;
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

    public void RemoveDarknessPointsCircle(Vector2 center, float radius)
    {
        CreateDarknessPointsCircle(center, radius, DarknessSpec.GetNullSpec());
    }

    // Returns list of active darkness points
    public List<DarknessPoint> GetActivePoints()
    {
        List<DarknessPoint> points = new List<DarknessPoint>();
        var (ix, iy) = worldToIndex(activeCenter.position);
        var corners = GetCorners(ix, iy);
        var bottomLeft = corners.Item1;
        var topRight = corners.Item2;

        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                DarknessPoint p = darknessArray[x, y];
                if (p.IsActive())
                {
                    points.Add(p);
                }
            }
        }

        return points;
    }

    // public DarknessPoint[,] GetActivePoints()
    // {
    //     DarknessPoint[,] points = new DarknessPoint[activeWidthAndHeight.x + 1, activeWidthAndHeight.y + 1];

    //     var corners = GetCorners(worldToIndex(activeCenter.position));
    //     var bottomLeft = corners.Item1;
    //     var topRight = corners.Item2;

    //     for (int x = bottomLeft.x; x <= topRight.x; x++)
    //     {
    //         for (int y = bottomLeft.y; y <= topRight.y; y++)
    //         {
    //             points[x - bottomLeft.x, y - bottomLeft.y] = darknessArray[x, y];
    //         }
    //     }

    //     return points;
    // }

    // Builds and returns a mesh created from the current list of active points.
    public Mesh BuildMesh()
    {
        Mesh mesh = new Mesh { name = "DarknessMesh" };

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector3> normals = new List<Vector3>();

        int index = 0;
        foreach (DarknessPoint p in this.GetActivePoints())
        {
            // Points of square
            vertices.Add(new Vector3(p.worldPosition.x - distanceBetweenPoints / 2, p.worldPosition.y - distanceBetweenPoints / 2, 0));
            vertices.Add(new Vector3(p.worldPosition.x - distanceBetweenPoints / 2, p.worldPosition.y + distanceBetweenPoints / 2, 0));
            vertices.Add(new Vector3(p.worldPosition.x + distanceBetweenPoints / 2, p.worldPosition.y - distanceBetweenPoints / 2, 0));
            vertices.Add(new Vector3(p.worldPosition.x + distanceBetweenPoints / 2, p.worldPosition.y + distanceBetweenPoints / 2, 0));
            // Two triangles per square
            triangles.AddRange(new int[] { 0 + index, 1 + index, 2 + index });
            triangles.AddRange(new int[] { 2 + index, 1 + index, 3 + index });
            // Vertice normals
            normals.AddRange(new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back });

            index += 4;
        }

        mesh.vertices = vertices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
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
        return x >= 0 && x < activeWidth && y >= 0 && y < activeHeight;
    }

    // Remove darkness at given index by setting max health to 0
    private void RemoveDarkness(int x, int y)
    {
        var point = darknessArray[x, y];
        point.maxHealth = point.currentHealth = 0;
        point.SetActive(false);
    }

    // Returns if index has darkness
    private bool IsDarknessIndex(int x, int y)
    {
        return darknessArray[x, y].IsAlive();
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
        point.worldPosition = indexToWorld(x, y);
        point.indexPosition = new Vector2Int(x, y);
        point.Init(spec);
        point.SetActive(false);
        if (darknessArray[x, y] != null) spreadablePoints.Remove(darknessArray[x, y]);
        darknessArray[x, y] = point;
        spreadablePoints.Add(point);
        return point;
    }

    // Converts world coordinates to index in internal array
    // Invalid conversion results in (-1,-1) return
    private (int, int) worldToIndex(Vector2 loc)
    {
        if (loc.x < 0 || loc.x > activeWidth * distanceBetweenPoints || loc.y < 0 || loc.y > activeHeight * distanceBetweenPoints)
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
        return darknessArray[clampIndexX(x + 1), y].IsAlive() &&
        darknessArray[clampIndexX(x - 1), y].IsAlive() &&
        darknessArray[x, clampIndexY(y + 1)].IsAlive() &&
        darknessArray[x, clampIndexY(y - 1)].IsAlive();
    }

    // Returns list of available points to spread to around index location
    private List<Vector3> OpenSurrounding(int x, int y)
    {
        List<Vector3> open = new List<Vector3>();

        DarknessPoint p;
        p = darknessArray[clampIndexX(x + 1), y];
        if (!p.IsAlive()) open.Add(p.worldPosition);
        p = darknessArray[clampIndexX(x - 1), y];
        if (!p.IsAlive()) open.Add(p.worldPosition);
        p = darknessArray[x, clampIndexY(y + 1)];
        if (!p.IsAlive()) open.Add(p.worldPosition);
        p = darknessArray[x, clampIndexY(y - 1)];
        if (!p.IsAlive()) open.Add(p.worldPosition);

        return open;
    }

    // Clamp index within array ranges
    private int clampIndexX(int x)
    {
        return Mathf.Clamp(x, 0, activeWidth - 1);
    }
    private int clampIndexY(int y)
    {
        return Mathf.Clamp(y, 0, activeHeight - 1);
    }

    // Deactivate all points in old position
    private void DeactivatePoints()
    {
        (int oldCenterX, int oldCenterY) = worldToIndex(oldPosition);
        if (oldCenterX == -1) return; // Invalid position

        var corners = GetCorners(oldCenterX, oldCenterY);
        Vector2Int bottomLeftIndex = corners.Item1;
        Vector2Int topRightIndex = corners.Item2;

        for (int x = bottomLeftIndex.x; x <= topRightIndex.x; x++)
        {
            for (int y = bottomLeftIndex.y; y <= topRightIndex.y; y++)
            {
                DarknessPoint p = darknessArray[x, y];
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
        for (int ix = bottomLeftX; ix <= topRightIndex.x; ix++)
        {
            for (int iy = bottomLeftY; iy <= topRightIndex.y; iy++)
            {
                DarknessPoint p = darknessArray[ix, iy];
                p.SetActive(true);
                // Use object pool to setup colliders
                GameObject obj = objectPool[ix - bottomLeftX, iy - bottomLeftY];
                if (p.IsAlive())
                {
                    obj.transform.position = p.worldPosition;
                    if (!IsSurrounded(p.indexPosition.x, p.indexPosition.y))
                    {
                        spreadablePoints.Add(p);
                    }
                }
                else
                {
                    obj.transform.position = new Vector2(-1, -1);
                }
            }
        }
    }

    // Call spread function of all darkness points
    // and removes those that can't spread
    private void SpreadPoints()
    {
        // Copy points so we can remove items from active set
        DarknessPoint[] points = new DarknessPoint[spreadablePoints.Count];
        spreadablePoints.CopyTo(points);
        foreach (DarknessPoint p in points)
        {
            List<Vector3> open = OpenSurrounding(p.indexPosition.x, p.indexPosition.y);
            if (open.Count > 0)
            {
                // If it spreads and there was only 1 open spot we can
                // immediately remove it
                if (p.Spread(open, this) && open.Count == 1)
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

        int indexX = Mathf.RoundToInt(activeWidthAndHeight.x / 2 * distanceBetweenPoints);
        int indexY = Mathf.RoundToInt(activeWidthAndHeight.y / 2 * distanceBetweenPoints);

        Vector2Int topRightIndex = new Vector2Int(clampIndexX(centerX + indexX), clampIndexY(centerY + indexY));
        Vector2Int bottomLeftIndex = new Vector2Int(clampIndexX(centerX - indexX), clampIndexY(centerY - indexY));

        return (bottomLeftIndex, topRightIndex);
    }

#if UNITY_EDITOR

    // Draw darkness points as cells in grid
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x < activeWidth; x++)
        {
            for (int y = 0; y < activeHeight; y++)
            {
                if (darknessArray != null)
                {
                    var point = darknessArray[x, y];
                    Gizmos.color = point.IsAlive() ? Color.green : Color.red;
                    if (point.IsActive()) Gizmos.color = Gizmos.color + Color.yellow; // Tint active cells yellow
                    Gizmos.color *= point.density; // shade with point density
                }
                Gizmos.DrawWireCube(indexToWorld(x, y), new Vector2(distanceBetweenPoints, distanceBetweenPoints));
            }
        }
    }
#endif
}
