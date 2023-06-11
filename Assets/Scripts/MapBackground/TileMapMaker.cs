using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapMaker : MonoBehaviour
{
    [SerializeField]
    Tilemap tileMap;
    [SerializeField]
    ITileMapGenerator generator;
    [SerializeField]
    Vector2Int widthAndHeight;

    void Awake()
    {
        generator.Generate(tileMap, widthAndHeight);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            generator.Generate(tileMap, widthAndHeight);
        }
    }
}
