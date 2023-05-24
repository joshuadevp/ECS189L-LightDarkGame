using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeactive : MonoBehaviour
{
    [SerializeField]
    GameObject[] objs;
    public GameObject prefabRoot;
    void Start()
    {
        prefabRoot = this.gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    public void ManualUpdate()
    {
        ;
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (GameObject o in objs)
        {
            o.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        foreach (GameObject o in objs)
        {
            o.SetActive(false);
        }
    }
}
