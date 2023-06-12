using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BossObjective : ScriptableObject, IObjective
{
    [SerializeField]
    GameObject bossPrefab;
    [SerializeField]
    float distToSpawnFromPlayer;
    [SerializeField]
    float speedBossMultiplier;
    [SerializeField]
    float damageBossMultiplier;
    [SerializeField]
    float healthBossMultiplier;
    private bool completed = false;
    private bool spawned;
    private Vector2 objPos;
    private GameObject bossInstance;
    private GameObject player;

    public void ManualUpdate()
    {
        // Only spawn the boss once the player is close enough
        if (!spawned)
        {
            if (Vector3.Magnitude(player.transform.position - (Vector3)objPos) < distToSpawnFromPlayer)
            {
                bossInstance = Instantiate(bossPrefab, objPos, Quaternion.identity);
                bossInstance.transform.localScale = new Vector3(2,2,2);
                spawned = true;
            }
            return;
        }

        // If the boss has been defeated it no longer exists
        if (bossInstance == null)
        {
            completed = true;
        }
    }

    public void Setup(Vector2 loc)
    {
        objPos = loc;
        // Find player and Spawn boss enemy
        FogOfDarknessManager manager = FindObjectOfType<FogOfDarknessManager>();
        player = FindObjectOfType<Player>().gameObject;
        spawned = false;
    }

    public bool Completed()
    {
        return completed;
    }

    public Vector2 GetLocation()
    {
        if (!spawned || bossInstance == null)
        {
            return objPos;
        }
        else
        {
            return bossInstance.transform.position;
        }
    }
}
