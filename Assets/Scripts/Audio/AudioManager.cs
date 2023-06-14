using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource sePlayer;
    [SerializeField] private AudioLibrary audioLibrary;
    Dictionary<string, AudioClip> dictionary;

    private void Awake()
    {
        dictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip audioClip in audioLibrary.AudioClips)
        {
            dictionary.Add(audioClip.name, audioClip);
        }
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1)
    {
        sePlayer.PlayOneShot(clip, volumeScale);
    }

    public void PlayOneShot(string clipName, float volumeScale = 1)
    {
        if (!dictionary.ContainsKey(clipName))
        {
            Debug.LogError($"The AudioClip {clipName} is not found.");
            return;
        }
        sePlayer.PlayOneShot(dictionary[clipName], volumeScale);
    }
}
