using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDarknessGenerator : ScriptableObject
{
    [SerializeField]
    protected DarknessSettings defaultSettings;
    [SerializeField]
    protected DarknessSettings globalSettings;
    [SerializeField]
    protected GameObject enemy;
    // Generate a DarknessSpec given a position
    virtual public DarknessSpec Generate(Vector2 pos) { return new DarknessSpec(); }
    // Generates a default DarknessSpec
    virtual public DarknessSpec GenerateDefault() { return new DarknessSpec(); }
}
