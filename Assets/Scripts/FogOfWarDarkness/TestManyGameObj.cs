using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManyGameObj : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int sideLength;
    [SerializeField]
    int activeLength;
    [SerializeField]
    Transform activeCenter;
    private GameObject[,] arr;

    // Start is called before the first frame update
    void Start()
    {
        arr = new GameObject[sideLength, sideLength];
        for (int i = 0; i < sideLength; i++)
        {
            for (int j = 0; j < sideLength; j++)
            {
                var obj = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                arr[i, j] = obj;
                obj.SetActive(false);
            }
        }
    }

    void Update()
    {
        Vector3Int center = Vector3Int.RoundToInt(activeCenter.position);
        int x = center.x;
        int y = center.z;

        (int, int) topRight = (clampIndex(x + activeLength), clampIndex(y + activeLength));
        (int, int) bottomLeft = (clampIndex(x - activeLength), clampIndex(y - activeLength));

        for (int i = bottomLeft.Item1; i < topRight.Item1; i++)
        {
            for (int j = bottomLeft.Item2; j < topRight.Item2; j++)
            {
                GameObject o = arr[i, j];
                o.SetActive(true);
            }
        }
    }

    private int clampIndex(int i)
    {
        return Mathf.Clamp(i, 0, sideLength);
    }

}
