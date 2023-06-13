using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGenerator : MonoBehaviour
{
    // List of upgrades available to be selected, usually after clearing an objective.
    [SerializeField]
    ScriptableObject[] possibleUpgrades;

    public IUpgrade GetRandomUpgrade()
    {
        var selectedUpgrade = possibleUpgrades[Random.Range(0, possibleUpgrades.Length)];
        return (IUpgrade) selectedUpgrade;
    }
}
