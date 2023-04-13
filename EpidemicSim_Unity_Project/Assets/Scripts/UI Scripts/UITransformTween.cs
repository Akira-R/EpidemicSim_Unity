using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransformTween : MonoBehaviour
{
    [Header("Animation")]
    public GameObject gameobjectToTween;
    public LeanTweenType tweenType;
    [Range(0.0f, 2.0f)] public float animationDuration = 0.5f;
    [Range(0.0f, 2.0f)] public float delay = 0.5f;

    [Header("Position Tween")]
    public Vector3 originalPos;
    public Vector3 targetPos;
    [SerializeField] private bool toggle = false;

    private LTDescr rotTween;

    [SerializeField] private GameObject[] panels;

    // Start is called before the first frame update
    void Start()
    {
        panels = GameObject.FindGameObjectsWithTag("Panel");
    }

    private void Update()
    {

    }

    public void ButtonToggleEase() 
    {
        Debug.Log(toggle);

        //foreach (GameObject panel in panels)
        //{
        //    if (panel.GetComponent<UITransformTween>().GetToggleStatus() && panel.gameObject != gameObject)
        //        panel.GetComponent<UITransformTween>().OnUIEaseOut();
        //}

        AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);

        if (toggle)
        {
            OnUIEaseOut();
        }
        else 
        {
            OnUIEaseIn();
        }

        foreach (GameObject panel in panels)
        {
            if (panel.GetComponent<UITransformTween>().GetToggleStatus() && panel.gameObject != gameObject)
                panel.GetComponent<UITransformTween>().OnUIEaseOut();
        }
    }

    public void OnUIEaseIn() 
    {
        //if (toggle == true)
        //    return;

        if (toggle == false)
            toggle = true;

        LeanTween.moveLocal(gameobjectToTween, targetPos, animationDuration)
                .setDelay(delay)
                .setEase(tweenType)
                .setIgnoreTimeScale(true);
    }
    public void OnUIEaseOut()
    {
        //if (toggle == false)
        //    return;

        if (toggle == true)
            toggle = false;

        LeanTween.moveLocal(gameobjectToTween, originalPos, animationDuration)
                .setDelay(delay)
                .setEase(tweenType)
                .setIgnoreTimeScale(true);
    }

    public void OnPointerEnter() 
    {
        rotTween = LeanTween.rotate(this.gameObject, new Vector3(0, 0, 180), animationDuration)
                            .setDelay(delay)
                            .setEase(tweenType)
                            .setIgnoreTimeScale(true);
    }
    public void OnPointerExit()
    {
        LeanTween.cancel(this.gameObject);
    }

    public bool GetToggleStatus() 
    {
        return toggle;
    }
}
