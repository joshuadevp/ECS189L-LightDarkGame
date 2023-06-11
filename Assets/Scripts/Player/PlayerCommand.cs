using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCommand : MonoBehaviour
{
    public abstract void Execute(GameObject gameObject);
}
