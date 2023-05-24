using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjective
{
    // Set up objective
    abstract void SetUp();
    // Remove elements related to objective
    abstract void BreakDown();
    // Returns true if the objective is completed
    abstract bool Completed();
    // Update loop called per frame by another script
    abstract void ManualUpdate();
}
