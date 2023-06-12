using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    DarknessSettings defaultDarknessSettings;
    [SerializeField]
    DarknessSettings globalDarknessSettings;
    [SerializeField]
    FogOfDarknessManager fogManager;
    [SerializeField]
    GameObject camera;
    
    void OnEnable()
    {
        globalDarknessSettings.EnemySpawnChance = 0f;
        globalDarknessSettings.DarknessSpreadChance = 0.3f;
        this.transform.position = camera.transform.position;
        fogManager.InitAllDarkness();
    }

    void FixedUpdate()
    {
        if(Random.Range(0f,1f) < 0.01f)
        {
            fogManager.RemoveDarknessPointsCircle(camera.transform.position+new Vector3(Random.Range(-50f,50f),Random.Range(-50f,50f),0),25);
        }
    }
}
