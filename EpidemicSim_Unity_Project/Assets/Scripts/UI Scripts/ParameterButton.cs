using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterButton : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animation _animation;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    public void OnParameterButtonPressed()
    {
        _animation.Play("Settings_UI Hover");
    }
}
