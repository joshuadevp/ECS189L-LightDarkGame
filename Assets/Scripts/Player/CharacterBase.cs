using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Scriptable object for different character classes. For now it only affects base hp and speed of the character.
/// </summary>
[CreateAssetMenu(fileName = "CharacterBase", menuName = "Characters")]
public class CharacterBase : ScriptableObject
{
    public float hp;
    public float speed;
}
