using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommand : MonoBehaviour, IPlayerCommand
{
    [SerializeField] float delay = 0.5f;
    float lastCast = 0f;
    public void Execute(GameObject gameObject) {
        if (Time.time - lastCast > delay)
        {
            print("Test Command printing something");
            lastCast = Time.time;
        }
    }
}
