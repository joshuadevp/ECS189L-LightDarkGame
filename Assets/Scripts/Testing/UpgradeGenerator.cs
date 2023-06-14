using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradeGenerator : MonoBehaviour
{
    // List of upgrades available to be selected, usually after clearing an objective.
    [SerializeField]
    List<ScriptableObject> possibleUpgrades;

    // Returns a list of random IUpgrades, no duplicates
    public List<IUpgrade> GetRandomUpgrades(int n)
    {
        List<IUpgrade> result = new List<IUpgrade>(n);
        List<IUpgrade> randomUpgrades = new List<IUpgrade>();
        foreach(ScriptableObject o in possibleUpgrades)
        {
            randomUpgrades.Add((IUpgrade)o);
        }
        randomUpgrades = Shuffle<IUpgrade>(randomUpgrades);
        for(int i = 0; i < n; i++)
        {
            result.Add(randomUpgrades[i]);
        }
        return result;
    }

    private List<T> Shuffle<T>(List<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0,n+1);
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }

        return list;
    }
}
