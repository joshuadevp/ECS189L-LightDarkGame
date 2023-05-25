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
    private HashSet<DarknessPoint> activePoints; // set of active points
    private GameObject[,] objectPool; // Pool of objects to be placed in active area
    private Vector2 oldPosition;

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
    public bool IsDarkness(Vector2 loc)
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
        darknessArray = new DarknessPoint[mapWidthAndHeight.x, mapWidthAndHeight.y];
        for (int x = 0; x < mapWidthAndHeight.x; x++)
        {
            for (int y = 0; y < mapWidthAndHeight.y; y++)
            {
                CreateDarknessPointIndex(new Vector2Int(x, y), darknessGenerator.Generate(new Vector2(x, y)));
            }
        }

        // Spawn pooled game objects
        objectPool = new GameObject[activeWidthAndHeight.x+1, activeWidthAndHeight.y+1];
        for (int x = 0; x < activeWidthAndHeight.x+1; x++)
        {
            for (int y = 0; y < activeWidthAndHeight.y+1; y++)
            {
                objectPool[x, y] = Instantiate(darknessPrefab, Vector2.zero, Quaternion.identity);
            }
        }
    }

    // Creates a point of darkness at the given world coord if possible
    // Returns true on success
    public bool CreateDarknessPoint(Vector2 coordinates, DarknessSpec spec)
    {
        Vector2Int index = worldToIndex(coordinates);
        if (index.x == -1) return false; // fail on invalid coord
        CreateDarknessPointIndex(index, spec);
        return true;
    }

    // Remove darkness point at given location
    public void RemoveDarkness(Vector2 loc)
    {
        Vector2Int index = worldToIndex(loc);

        RemoveDarkness(index);
    }

    // Create a circle of darkness points at the given world position and world
    // based radius
    public void CreateDarknessPointsCircle(Vector2 center, float radius, DarknessSpec spec)
    {
        Vector2Int index = worldToIndex(center);
        int indexRadius = Mathf.RoundToInt(radius / distanceBetweenPoints);

        CreateDarknessPointsCircleIndex(index, indexRadius, spec);
    }

    public void RemoveDarknessPointsCircle(Vector2 center, float radius)
    {
        CreateDarknessPointsCircle(center, radius, DarknessSpec.GetNullSpec());
    }

    public DarknessPoint[,] GetActivePoints()
    {
        DarknessPoint[,] points = new DarknessPoint[activeWidthAndHeight.x+1, activeWidthAndHeight.y+1];

        var corners = GetCorners(worldToIndex(activeCenter.position));
        var bottomLeft = corners.Item1;
        var topRight = corners.Item2;

        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                points[x - bottomLeft.x, y - bottomLeft.y] = darknessArray[x, y];
            }
        }

        return points;
    }

    private void CreateDarknessPointsCircleIndex(Vector2Int center, int radius, DarknessSpec spec)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (y * y + x * x <= radius * radius + radius && ValidIndex(new Vector2Int(x + center.x, y + center.y)))
                {
                    CreateDarknessPointIndex(new Vector2Int(x + center.x, y + center.y), spec);
                }
            }
        }
    }

    private bool ValidIndex(Vector2Int index)
    {
        return index.x >= 0 && index.x < mapWidthAndHeight.x && index.y >= 0 && index.y < mapWidthAndHeight.y;
    }

    // Remove darkness at given index by setting max health to 0
    private void RemoveDarkness(Vector2Int index)
    {
        var point = darknessArray[index.x, index.y];
        point.maxHealth = point.currentHealth = 0;
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
    private Vector2Int worldToIndex(Vector2 loc)
    {
        if (loc.x < 0 || loc.x > mapWidthAndHeight.x * distanceBetweenPoints || loc.y < 0 || loc.y > mapWidthAndHeight.y * distanceBetweenPoints)
        {
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(Mathf.RoundToInt(loc.x / distanceBetweenPoints), Mathf.RoundToInt(loc.y / distanceBetweenPoints));
    }

    // Converts index to coordinates in world
    // Assumes any point within the bounds o
    private Vector2 indexToWorld(Vector2Int index)
    {
        return new Vector2(index.x * distanceBetweenPoints, index.y * distanceBetweenPoints);
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
        return Mathf.Clamp(x, 0, mapWidthAndHeight.x-1);
    }
    private int clampIndexY(int y)
    {
        return Mathf.Clamp(y, 0, mapWidthAndHeight.y-1);
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
        Vector2Int center = worldToIndex(activeCenter.position);
        // Exit early if location outside grid
        if (center.x == -1)
        {
            return;
        }

        var corners = GetCorners(center);
        Vector2Int bottomLeftIndex = corners.Item1;
        Vector2Int topRightIndex = corners.Item2;

        for (int x = bottomLeftIndex.x; x <= topRightIndex.x; x++)
        {
            for (int y = bottomLeftIndex.y; y <= topRightIndex.y; y++)
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
                    obj.transform.position = new Vector2(-1, -1);
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

        for (int x = 0; x < mapWidthAndHeight.x; x++)
        {
            for (int y = 0; y < mapWidthAndHeight.y; y++)
            {
                if (darknessArray != null)
                {
                    var point = darknessArray[x, y];
                    Gizmos.color = point.IsAlive() ? Color.green: Color.red;
                    if (point.IsActive()) Gizmos.color = Gizmos.color + Color.yellow; // Tint active cells yellow
                    Gizmos.color *= point.density; // shade with point density
                }
                Gizmos.DrawWireCube(indexToWorld(new Vector2Int(x, y)), new Vector2(distanceBetweenPoints, distanceBetweenPoints));
            }
        }
    }
#endif
}
