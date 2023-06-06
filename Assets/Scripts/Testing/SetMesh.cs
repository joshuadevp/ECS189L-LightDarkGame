using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMesh : MonoBehaviour
{
    [SerializeField]
    FogOfDarknessManager fogManager;
    [SerializeField]
    ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(setMesh());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator setMesh()
    {
        yield return new WaitForSeconds(0.1f);
        var shape = particles.shape;
        shape.mesh = fogManager.GetMesh();
    }
}
