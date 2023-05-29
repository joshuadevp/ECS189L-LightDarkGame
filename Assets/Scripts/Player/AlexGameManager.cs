using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlexGameManager : MonoBehaviour
{
    public static AlexGameManager instance;
    private float startTime;
    private float level = 1;

    [SerializeField] GameObject enemyPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            this.enabled = false;
        }
        startTime = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab);
    }

    /// <summary>
    /// To be fine tuned for game feel: strengthen enemy stats based on time and level
    /// </summary>
    /// <returns> The modifier to be applied on enemy's stats</returns>
    public float CalculateEnemyModifier()
    {
        return startTime / 10f * level;
    }
}
