using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTestCommand : PlayerCommand
{
    [SerializeField] FlickerGenericUpgrade flickerGenericUpgrade;
    public override void Execute(GameObject player)
    {
        Debug.Log("Given test buff WOW");
        flickerGenericUpgrade.ApplyUpgrade();
    }
}
