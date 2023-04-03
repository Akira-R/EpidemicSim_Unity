using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem instance;

    [Header("Tooltip")]
    [SerializeField] public Tooltip _tooltip;

    [Header("Image Component")]
    [SerializeField] private Image _image;
    [Range(0.1f, 1.0f)] public float fadeRate = 0.1f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        instance._tooltip.SetText("");
        //_tooltip.gameObject.SetActive(false);
    }

    public static void ShowTooltip(string content, string header = "") 
    {
        instance._tooltip.SetText(content, header);
        //instance._tooltip.gameObject.SetActive(true);
    }
    public static void HideTooltip()
    {
        instance._tooltip.SetText("");
        //instance._tooltip.gameObject.SetActive(false);
    }
}
