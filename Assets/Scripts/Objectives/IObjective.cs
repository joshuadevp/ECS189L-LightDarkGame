using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{
    // Set up objective at given location
    abstract void Setup(Vector2 pos);
    // Update function that gets manually called by another script
    abstract void ManualUpdate();
    // Returns true if the objective is completed
    abstract bool Completed();
}
