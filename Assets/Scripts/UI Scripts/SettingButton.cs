using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
