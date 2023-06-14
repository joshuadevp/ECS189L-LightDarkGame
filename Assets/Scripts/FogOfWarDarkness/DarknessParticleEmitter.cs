using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessParticleEmitter : MonoBehaviour
{
    [SerializeField]
    FogOfDarknessManager fogManager;
    [SerializeField]
    ParticleSystem particleSystem;
    [SerializeField]
    int particlesPerUpdate;
    [SerializeField]
    Color32 color;
    [SerializeField]
    [Range(0,255)]
    int defaultAlpha;
    [SerializeField]
    float defaultSize;
    
    void FixedUpdate()
    {
        var activePoints = fogManager.GetActivePoints();
        int numPoints = activePoints.Item2;
        float distance = fogManager.distanceBetweenPoints / 2;
        if(numPoints == 0)
        {
            return;
        }
        // Number of particles we make should be relative to our active size
        int particles = (int)(particlesPerUpdate * ((float)numPoints / fogManager.MaxActiveSize()));
        for (int i = 0; i < particles; i++)
        {
            DarknessPoint p = activePoints.Item1[Random.Range(0, numPoints)];
            color.a = (byte)(defaultAlpha * p.Density);
            ParticleSystem.EmitParams param = new ParticleSystem.EmitParams()
            {
                position = p.WorldPosition + new Vector2(Random.Range(-distance, distance), Random.Range(-distance, distance)),
                startColor = color,
                startSize = p.Density * defaultSize,
                
            };
            particleSystem.Emit(param, 1);
        }
    }
}
