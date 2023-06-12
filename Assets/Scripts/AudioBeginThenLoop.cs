using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
     public class ExampleClass : MonoBehaviour
     {
         public AudioSource startClip;
         public AudioSource loopClip;
         void Start()
         {
            startClip.Play();
            loopClip.PlayDelayed(startClip.clip.length);
         }
     }