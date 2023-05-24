using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float speed;
    [SerializeField]
    FogOfDarknessManager fogManager;
    [SerializeField]
    float radius;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= new Vector3(0, 0, speed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            fogManager.CreateDarknessPointsCircle(this.transform.position, radius,new DarknessSpec(){currentHealth= 0,maxHealth=0});
            fogManager.GetActivePoints();
        }
    }
}
