using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDarknessGenerator : ScriptableObject
{
    // Generate a DarknessSpec given a position
    virtual public DarknessSpec Generate(Vector2 pos) { return new DarknessSpec(); }
}