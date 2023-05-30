using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [BoxGroup("Volume")]
    [SerializeField] private Slider slider;
    [BoxGroup("Volume")]
    [SerializeField] private AudioMixer mixer;
    [BoxGroup("Volume")]
    [SerializeField] private string mixerParam = "MasterVolume";

    [BoxGroup("Reset Camera")]
    [SerializeField] private CameraControl camControl;

    [BoxGroup("Reset Simulation")]
    [SerializeField] private SimulationManager simulationManager;

    [BoxGroup("Keybinds Panel")]
    [SerializeField] private GameObject keyPanel;
    [SerializeField] private bool keybindsActiveStatus = false;

    // Start is called before the first frame update
    void Start()
    {
        float initialVal;
        mixer.GetFloat(mixerParam, out initialVal);
        slider.value = initialVal;

        slider.onValueChanged.AddListener(val =>
        {
            AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);
            mixer.SetFloat(mixerParam, val);
        });

        keyPanel.SetActive(keybindsActiveStatus);
    }

    private void Update()
    {
        if (keybindsActiveStatus)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnKeybindsButtonPressed();
            }
        }
    }

    public void OnResetCameraButtonPressed() 
    {
        AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);
        camControl.ResetCameraState();
    }
    public void OnResetSimulationButtonPressed()
    {
        AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);
        simulationManager.ResetSimulation();
    }
    public void OnKeybindsButtonPressed() 
    {
        //AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);
        keybindsActiveStatus = !keybindsActiveStatus;
        keyPanel.SetActive(keybindsActiveStatus);
    }
}
