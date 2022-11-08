using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SettingButton : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _animation.wrapMode = WrapMode.Loop;
    }

    public void OnPointerEnter()
    {
        _animation.Play("Settings_UI Hover");
    }
    public void OnPointerExit() 
    {
        _animation.Stop();
    }
}
