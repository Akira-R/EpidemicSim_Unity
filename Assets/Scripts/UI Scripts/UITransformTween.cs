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

    [Header("Rotation Tween")]
    public Vector3 rotationVector;
    private LTDescr rotTween;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {

    }

    public void ButtonToggleEase() 
    {
        if (toggle)
        {
            OnUIEaseOut();
        }
        else 
        {
            OnUIEaseIn();
        }

        toggle = !toggle;
    }

    private void OnUIEaseIn() 
    {
        Debug.Log(targetPos);
        LeanTween.moveLocal(gameobjectToTween, targetPos, animationDuration)
                .setDelay(delay)
                .setEase(tweenType)
                .setIgnoreTimeScale(true);
    }
    private void OnUIEaseOut()
    {
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
}
