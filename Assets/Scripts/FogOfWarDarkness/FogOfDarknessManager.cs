using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfDarknessManager : MonoBehaviour
{
    [SerializeField]
    Vector2Int mapHeightAndWidth; // Height and width of points of darkness.
    [SerializeField]
    float distanceBetweenPoints; // Distance between points of darkness, assumes all space between two points contains darkness.
    [SerializeField]
    Vector2Int activeHeightAndWidth; // Height and width of set of active points of darkness in world space.
    [SerializeField]
    Transform activeCenter; // Center point where we want darkness to be active around
    [SerializeField]
    GameObject darknessPrefab;
    [SerializeField]
    float pointHeight;
    [SerializeField]
    float updateInterval; // Time in seconds for manager to update fog system
    private float intervalWatch = 0;
    private DarknessPoint[,] darknessArray;
    private HashSet<DarknessPoint> activePoints; // set of active points
    private GameObject[,] objectPool; // Pool of objects to be placed in active area
    private Vector3 oldPosition;

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
            oldPosition = activeCenter.position;
        }
    }

    // Returns true if the given world point is within darkness
    public bool IsDarkness(Vector3 loc)
    {
        Vector2Int index = worldToIndex(loc);
        if (index.x == -1)
        {
            return false;
        }
        return IsDarknessIndex(index);
    }

    // Creates all darkness points and game objects
    // EXPENSIVE!!! Only use when initializing game
    public void InitAllDarkness()
    {
        // Init data
        activePoints = new HashSet<DarknessPoint>();

        // Spawn darkness points
        darknessArray = new DarknessPoint[mapHeightAndWidth.x, mapHeightAndWidth.y];
        for (int x = 0; x < mapHeightAndWidth.x; x++)
        {
            for (int y = 0; y < mapHeightAndWidth.y; y++)
            {
                CreateDarknessPointIndex(new Vector2Int(x, y), new DarknessSpec());
            }
        }

        // Spawn pooled game objects
        objectPool = new GameObject[activeHeightAndWidth.x, activeHeightAndWidth.y];
        for (int x = 0; x < activeHeightAndWidth.x; x++)
        {
            for (int y = 0; y < activeHeightAndWidth.y; y++)
            {
                objectPool[x, y] = Instantiate(darknessPrefab, Vector3.zero, Quaternion.identity);
            }
        }
    }

    // Creates a point of darkness at the given world coord if possible
    // Returns true on success
    public bool CreateDarknessPoint(Vector3 coordinates, DarknessSpec spec)
    {
        Vector2Int index = worldToIndex(coordinates);
        if (index.x == -1) return false; // fail on invalid coord
        CreateDarknessPointIndex(index, spec);
        return true;
    }

    // Remove darkness point at given location
    public void RemoveDarkness(Vector3 loc)
    {
        Vector2Int index = worldToIndex(loc);

        RemoveDarkness(index);
    }

    // Create a circle of darkness points at the given world position and world
    // based radius
    public void CreateDarknessPointsCircle(Vector3 center, float radius)
    {
        Vector2Int index = worldToIndex(center);
        int indexRadius = Mathf.RoundToInt(radius / distanceBetweenPoints);

        CreateDarknessPointsCircleIndex(index, indexRadius);
    }

    private void CreateDarknessPointsCircleIndex(Vector2Int center, int radius)
    {

    }

    // Remove darkness at given index by setting max health to 0
    private void RemoveDarkness(Vector2Int index)
    {
        var point = darknessArray[index.x, index.y];
        point.maxHealth = point.health = 0;
        point.SetActive(false);
    }

    // Returns if index has darkness
    private bool IsDarknessIndex(Vector2Int index)
    {
        return darknessArray[index.x, index.y].IsAlive();
    }

    // Creates a point of darkness given index in array and associated data
    // Returns created DarknessPoint
    private DarknessPoint CreateDarknessPointIndex(Vector2Int index, DarknessSpec spec)
    {
        DarknessPoint point = new DarknessPoint();
        point.worldPosition = indexToWorld(index);
        point.indexPosition = index;
        point.Init(spec);
        point.SetActive(false);
        darknessArray[index.x, index.y] = point;
        return point;
    }

    // Converts world coordinates to index in internal array
    // Invalid conversion results in (-1,-1) return
    private Vector2Int worldToIndex(Vector3 loc)
    {
        if (loc.x < 0 || loc.x > mapHeightAndWidth.x * distanceBetweenPoints || loc.z < 0 || loc.z > mapHeightAndWidth.y * distanceBetweenPoints)
        {
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(Mathf.RoundToInt(loc.x / distanceBetweenPoints), Mathf.RoundToInt(loc.z / distanceBetweenPoints));
    }

    // Converts index to coordinates in world
    // Assumes any point within the bounds o
    private Vector3 indexToWorld(Vector2Int index)
    {
        return new Vector3(index.x * distanceBetweenPoints, pointHeight, index.y * distanceBetweenPoints);
    }

    // Darkness exists on a 2D plane but in a 3D game we need 3D coords
    // 
    private Vector3 Get3DVector(Vector2 loc)
    {
        return new Vector3(loc.x, pointHeight, loc.y);
    }

    // Returns true if index position is surrounded by darkness and thus cannot spread
    private bool IsSurrounded(Vector2Int index)
    {
        return darknessArray[clampIndexX(index.x + 1), index.y].IsActive() &&
        darknessArray[clampIndexX(index.x - 1), index.y].IsActive() &&
        darknessArray[index.x, clampIndexY(index.y + 1)].IsActive() &&
        darknessArray[index.x, clampIndexY(index.y - 1)].IsActive();
    }

    // Clamp index within array ranges
    private int clampIndexX(int x)
    {
        return Mathf.Clamp(x, 0, mapHeightAndWidth.x - 1);
    }
    private int clampIndexY(int y)
    {
        return Mathf.Clamp(y, 0, mapHeightAndWidth.y - 1);
    }

    private void DeactivatePoints()
    {
        // Copy points so we can remove items from active set
        DarknessPoint[] points = new DarknessPoint[activePoints.Count];
        activePoints.CopyTo(points);
        foreach (DarknessPoint p in points)
        {
            if (IsSurrounded(p.indexPosition))
            {
                p.SetActive(false);
                //activePoints.Remove(p);
            }
        }

        Vector2Int oldCenter = worldToIndex(oldPosition);
        if (oldCenter.x == -1) return; // Invalid position

        var corners = GetCorners(oldCenter);
        Vector2Int bottomLeftIndex = corners.Item1;
        Vector2Int topRightIndex = corners.Item2;

        for (int x = bottomLeftIndex.x; x < topRightIndex.x; x++)
        {
            for (int y = bottomLeftIndex.y; y < topRightIndex.y; y++)
            {
                DarknessPoint p = darknessArray[x, y];
                p.SetActive(false);
            }
        }
    }

    private void ActivatePoints()
    {
        Vector2Int center = worldToIndex(activeCenter.position);
        // Exit early if location outside grid
        if (center.x == -1)
        {
            return;
        }

        var corners = GetCorners(center);
        Vector2Int bottomLeftIndex = corners.Item1;
        Vector2Int topRightIndex = corners.Item2;

        for (int x = bottomLeftIndex.x; x < topRightIndex.x; x++)
        {
            for (int y = bottomLeftIndex.y; y < topRightIndex.y; y++)
            {
                DarknessPoint p = darknessArray[x, y];
                p.SetActive(true);
                //activePoints.Add(darknessArray[x, y]);
                // Use object pool to setup colliders
                GameObject obj = objectPool[x - bottomLeftIndex.x, y - bottomLeftIndex.y];
                if (p.IsAlive())
                {
                    obj.transform.position = p.worldPosition;
                }
                else
                {
                    obj.transform.position = new Vector3(-1, -1, -1);
                }
            }
        }
    }

    // Returns pair of Vector2Int representing bottom left and top right corner
    // of active rectangle around center
    private (Vector2Int, Vector2Int) GetCorners(Vector2Int center)
    {
        int centerX = center.x;
        int centerY = center.y;

        int indexX = Mathf.RoundToInt(activeHeightAndWidth.x / 2 * distanceBetweenPoints);
        int indexY = Mathf.RoundToInt(activeHeightAndWidth.y / 2 * distanceBetweenPoints);

        Vector2Int topRightIndex = new Vector2Int(clampIndexX(centerX + indexX), clampIndexY(centerY + indexY));
        Vector2Int bottomLeftIndex = new Vector2Int(clampIndexX(centerX - indexX), clampIndexY(centerY - indexY));

        return (bottomLeftIndex, topRightIndex);
    }

#if UNITY_EDITOR

    // Draw darkness points as cells in grid
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int x = 0; x < mapHeightAndWidth.x; x++)
        {
            for (int y = 0; y < mapHeightAndWidth.y; y++)
            {
                if (darknessArray != null)
                {
                    var point = darknessArray[x, y];
                    Gizmos.color = point.IsAlive() ? Color.green : Color.red;
                    if (point.IsActive()) Gizmos.color = Gizmos.color + Color.yellow; // Tint active cells yellow
                }
                Gizmos.DrawWireCube(indexToWorld(new Vector2Int(x, y)), new Vector3(distanceBetweenPoints, 0f, distanceBetweenPoints));
            }
        }
    }
#endif
}
