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

    private LTDescr EaseTween;
    private LTDescr rotTween;
    private Coroutine timeKeeper;

    //private bool isTweenComplete = false;

    [SerializeField] private GameObject[] panels;

    // Start is called before the first frame update
    void Start()
    {
        panels = GameObject.FindGameObjectsWithTag("Panel");
    }

    private void Update()
    {
        //Debug.Log("Tween: " + LeanTween.isTweening());
    }

    public void ButtonToggleEase() 
    {
        Debug.Log(toggle);

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
            if (panel.gameObject != gameObject)
                panel.GetComponent<UITransformTween>().OnUIEaseOut();
        }
    }

    public void OnUIEaseIn() 
    {
        //if (toggle == true)
        //    return;
        //isTweenComplete = false;
        //Debug.Log("Ease IN");

        if (toggle == false)
            toggle = true;

        /*EaseTween = */LeanTween.moveLocal(gameobjectToTween, targetPos, animationDuration)
                .setEase(tweenType)
                .setIgnoreTimeScale(true)
                .setOnStart(() =>
                {
                    if (timeKeeper != null)
                        StopCoroutine(timeKeeper);

                    timeKeeper = StartCoroutine(keepTime(targetPos));
                })
                .setOnComplete(() => { /*Debug.Log("Ease IN ------ DONE " + gameObject.name); isTweenComplete = true;*/ });   
    }
    public void OnUIEaseOut()
    {
        //if (toggle == false)
        //    return;
        //isTweenComplete = false;
        Debug.Log("Ease OUT");

        if (toggle == true)
            toggle = false;

        /*EaseTween = */LeanTween.moveLocal(gameobjectToTween, originalPos, animationDuration)
                .setEase(tweenType)
                .setIgnoreTimeScale(true)
                .setOnStart(() => 
                {
                    if (timeKeeper != null)
                        StopCoroutine(timeKeeper);

                    timeKeeper = StartCoroutine(keepTime(originalPos));
                })
                .setOnComplete(() => { /*Debug.Log("Ease OUT ------ DONE " + gameObject.name); isTweenComplete = true;*/ });
    }

    public bool GetToggleStatus() 
    {
        return toggle;
    }

    private IEnumerator keepTime(Vector3 setPos) 
    {
        yield return new WaitForSecondsRealtime(0.5f);
        //isTweenComplete = true;

        gameobjectToTween.transform.localPosition = setPos;
    }
}
