using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface IUpgrade
{
    // Applies the upgrade
    abstract void ApplyUpgrade();

    abstract Sprite GetIcon();

    abstract string GetDetails();
}
