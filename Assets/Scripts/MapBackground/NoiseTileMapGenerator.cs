using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class NoiseTileMapGenerator : ITileMapGenerator
{
    [SerializeField]
    float simplexRefinement;
    [SerializeField]
    float cellF1Refinement;
    [SerializeField]
    float cellF2Refinement;
    private int noiseChoice;
    private int seed;

    public override void Generate(Tilemap map, Vector2Int widthAndHeight)
    {
        noiseChoice = UnityEngine.Random.Range(0, 3);
        Debug.Log("Noise choice: " + noiseChoice);
        seed = UnityEngine.Random.Range(0, 65535);

        for (int x = 0; x < widthAndHeight.x; x++)
        {
            for (int y = 0; y < widthAndHeight.y; y++)
            {
                map.SetTile(new Vector3Int(x, y, 0), orderedTiles[GetTileIndex(x, y)]);
            }
        }
    }

    private int GetTileIndex(int x, int y)
    {
        float noise = -1;
        switch (noiseChoice)
        {
            case 0:
                noise = Snoise(x, y);
                break;
            case 1:
                noise = CellF1(x, y);
                break;
            case 2:
                noise = CellF2(x, y);
                break;
        }
        if (noise < 0 || noise > 1)
        {
            Debug.LogError("Noise for map wrong! : " + noise);
            return 0;
        }

        return Mathf.CeilToInt(orderedTiles.Count * noise) - 1;
    }

    private float Snoise(int x, int y)
    {
        return Mathf.InverseLerp(-1, 1, noise.snoise(new Vector2(x * simplexRefinement + seed, y * simplexRefinement + seed)));
    }

    private float CellF1(int x, int y)
    {
        return Mathf.Clamp(noise.cellular(new Vector2(x * cellF1Refinement + seed, y * cellF1Refinement + seed)).x, 0f, 1f);
    }
    private float CellF2(int x, int y)
    {
        return Mathf.Clamp(noise.cellular(new Vector2(x * cellF2Refinement + seed, y * cellF2Refinement + seed)).y, 0f, 1f);
    }
}
