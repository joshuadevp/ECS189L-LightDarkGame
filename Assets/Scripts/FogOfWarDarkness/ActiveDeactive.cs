using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveDeactive : MonoBehaviour
{
    [SerializeField]
    GameObject []objs;
    void Start()
    {
        
    }

    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        foreach(GameObject o in objs)
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
