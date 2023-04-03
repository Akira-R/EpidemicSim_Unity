using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName ="AudioSystem/AudioData", fileName ="AudioData")]
public class AudioObject : ScriptableObject
{
    public AudioMixerGroup mixerGroup;
    public AudioClip clip;
    public bool isLoop = false;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(-3f, 3f)]
    public float pitch = 1f;
}
