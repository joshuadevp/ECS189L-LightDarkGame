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
    protected List<GameObject> enemies;
    [SerializeField]
    [Tooltip("Must sum to 1")]
    protected List<float> spawnChances;
    // Generate a DarknessSpec given a position
    virtual public DarknessSpec Generate(Vector2 pos) { return new DarknessSpec(); }
    // Generates a default DarknessSpec
    virtual public DarknessSpec GenerateDefault() { return new DarknessSpec(); }
}
