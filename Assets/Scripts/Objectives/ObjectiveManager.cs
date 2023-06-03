using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    ScriptableObject[] possibleObjectives;
    [SerializeField]
    float distFromPlayer;
    [SerializeField]
    GameObject player;
    [SerializeField]
    FogOfDarknessManager darknessManager;
    private IObjective activeObjective;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ScriptableObject o in possibleObjectives)
        {
            if (!(o is IObjective))
            {
                Debug.LogError("Possible objective is not of type IObjective!");
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Basic flow
        // Generate new objective(s)
        // Generate reward(s)? Might not get this far
        // Determine location to place obj
        // Setup objective at loc
        // If objective is completed, give player reward(s) choice?
        // Destroy objective object
        // Goto to generate new objective(s)

        if (activeObjective == null)
        {
            activeObjective = GenerateNewObjective();
            activeObjective.Setup(GenerateLocation());
            // Create reward
        }
        else
        {
            activeObjective.ManualUpdate();
            if (activeObjective.Completed())
            {
                Debug.Log("Completed objective");
                // Give reward
                Destroy((ScriptableObject)activeObjective);
                activeObjective = null;
            }
        }
    }

    private IObjective GenerateNewObjective()
    {
        return (IObjective)Instantiate(possibleObjectives[0]);
    }

    private Vector2 GenerateLocation()
    {
        Vector2 playerLoc = player.transform.position;
        // Attempt to find a suitable location to place objective
        // Only try X number of times
        for (int i = 0; i < 100; i++)
        {
            float radian = Mathf.Deg2Rad * Random.Range(0, 361);
            Vector2 tempLoc = playerLoc + new Vector2(Mathf.Cos(radian) * distFromPlayer, Mathf.Sin(radian) * distFromPlayer);
            if (darknessManager.IsDarkness(tempLoc))
            {
                return tempLoc;
            }
        }

        return new Vector2(0, 0);
    }
}
