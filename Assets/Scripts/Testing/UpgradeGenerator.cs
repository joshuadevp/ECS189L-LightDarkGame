using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeGenerator : MonoBehaviour
{
    [SerializeField]
    ScriptableObject[] upgrades;

    public IUpgradeTest GenerateUpgrade()
    {
        return (IUpgradeTest)upgrades[Random.Range(0,upgrades.Length)];
    }
}
