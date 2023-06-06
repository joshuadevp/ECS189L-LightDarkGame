using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityCommand
{
    float Execute(GameObject gameObject, float timeSinceLastShot);
}
