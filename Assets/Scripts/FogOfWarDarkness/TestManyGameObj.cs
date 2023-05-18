using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManyGameObj : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int sideLength;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < sideLength; i++)
        {
            for (int j = 0; j < sideLength; j++)
            {
                var obj = Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                //obj.SetActive(false);
            }
        }
    }

}
