using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPositionTween : MonoBehaviour
{
    [Header("Animation")]
    public Vector3 targetPos;
    public LeanTweenType tweenType;
    [Range(0.1f, 2.0f)] public float animationSpeed = 0.5f;
    [Range(0.1f, 2.0f)] public float delay = 0.5f;

    private RectTransform _initialTransform;

    [SerializeField] private bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        //_initialTransform = gameObject.GetComponent<RectTransform>();
        //Debug.Log(_initialTransform.position);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q)) 
        //{
        //    Debug.Log("Q");
        //    OnUIEaseIn();
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Debug.Log("E");
        //    OnUIEaseOut();
        //}
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
        LeanTween.moveLocal(this.gameObject, targetPos, animationSpeed)
                .setDelay(delay)
                .setEase(tweenType);
    }
    private void OnUIEaseOut()
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(0, -600, 0), animationSpeed)
                .setDelay(delay)
                .setEase(tweenType);
    }
}
