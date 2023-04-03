using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIAudioList 
{
    public AudioObject sfx_uiClicked;
}

public class AudioManager : MonoBehaviour
{
    public UIAudioList uiAudioList = new UIAudioList();

    public static AudioManager instance;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);

        instance = this;
    }

    public Coroutine Play(AudioObject audio, float delay = 0f) 
    {
        return StartCoroutine(PlayAudio(audio, delay));
    }

    private IEnumerator PlayAudio(AudioObject audioRef, float delayRef) 
    {
        yield return new WaitForSeconds(delayRef);

        GameObject obj = new GameObject("AudioObject");
        AudioSource sfx = obj.AddComponent<AudioSource>();

        sfx.clip = audioRef.clip;
        sfx.volume = audioRef.volume;
        sfx.pitch = audioRef.pitch;
        sfx.outputAudioMixerGroup = audioRef.mixerGroup;

        Debug.Log(audioRef.volume);
        Debug.Log(sfx.volume);

        if (audioRef.isLoop)
            sfx.loop = true;
        else
            sfx.loop = false;

        sfx.Play();
        Destroy(obj, audioRef.clip.length);
    }
}
