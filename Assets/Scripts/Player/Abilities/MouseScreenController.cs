using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScreenController : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        var worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;
        transform.position = worldMousePos;
    }
}
