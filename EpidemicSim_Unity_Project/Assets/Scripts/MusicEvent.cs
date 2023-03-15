using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum LayerType 
{
    ADDITIVE,
    SINGLE  
}

[CreateAssetMenu(menuName = "SoundSystem/Music Event", fileName = "MuE_")]
public class MusicEvent : ScriptableObject
{
    [SerializeField] private AudioClip[] musicLayers;
    [SerializeField] private LayerType musicLayerType = LayerType.ADDITIVE;
    [SerializeField] private AudioMixer musicMixer;

    public AudioClip[] MusicLayers => musicLayers;
    public LayerType MusicLayerType => musicLayerType;
    public AudioMixer MusicMixer => musicMixer;

    public void Play() 
    {
        
    }
}
