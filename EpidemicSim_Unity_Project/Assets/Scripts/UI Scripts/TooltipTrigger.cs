using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip String")]
    public string header;
    public string content;

    private static LTDescr delay;

    public void OnPointerEnter(PointerEventData eventData) 
    {
        delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.ShowTooltip(content, header);
        });
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(delay.uniqueId);
        TooltipSystem.HideTooltip();
    }
}
