using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField]
    private bool _freezeXZAxis;

    private RectTransform _rectTransform;
    private Canvas _canvas;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = GameObject.FindGameObjectWithTag("OverlayCam").GetComponent<Camera>();
    }

    void Update()
    {
        if (_freezeXZAxis)
            _rectTransform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
}
