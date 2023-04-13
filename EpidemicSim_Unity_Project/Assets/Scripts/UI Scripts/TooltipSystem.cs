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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _tooltip.SetText("");
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
