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
    [SerializeField]
    ParticleSystem particles;
    [SerializeField]
    MeshFilter planeFilter;
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
            this.transform.position -= new Vector3(0, speed, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            fogManager.RemoveDarknessPointsCircle(this.transform.position, radius);
            //fogManager.RemoveDarknessPointsCircle(this.transform.position, radius);

            Mesh mesh = fogManager.GetMesh();
            var shape = particles.shape;
            shape.mesh = mesh;
            planeFilter.mesh = mesh;
        }

    }

    void FixedUpdate()
    {
        var activePoints = fogManager.GetActivePoints();
        for (int i = 0; i < 300; i++)
        {
            DarknessPoint p = activePoints.Item1[Random.Range(0, activePoints.Item2)];
            ParticleSystem.EmitParams param = new ParticleSystem.EmitParams()
            {
                position = p.WorldPosition + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)),
                startColor = new Color32(50, 50, 50, (byte)(255 * p.Density)),
                startLifetime = 1,
                startSize = p.Density * 4,
            };
            particles.Emit(param, 1);
        }
    }
}
