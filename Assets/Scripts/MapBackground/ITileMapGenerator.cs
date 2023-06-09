using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ITileMapGenerator : ScriptableObject
{
    [SerializeField]
    protected List<Tile> orderedTiles;

    // Generate tiles on the given tilemap and size.
    virtual public void Generate(Tilemap m, Vector2Int widthAndHeight) { }
}
