using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDarknessGenerator
{
    // Generate a DarknessSpec given a position
    abstract DarknessSpec Generate(Vector2 pos);
}