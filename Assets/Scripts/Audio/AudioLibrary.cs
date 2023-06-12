using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    public List<AudioClip> AudioClips;
}
