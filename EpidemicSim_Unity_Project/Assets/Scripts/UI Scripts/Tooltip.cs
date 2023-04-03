using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    [Header("Tooltip Texts")]
    public TMP_Text _headerText;
    public TMP_Text _contentText;
    [Range(50, 100)] public int _characterWrapLimit = 80;

    private LayoutElement _layoutElement;

    [Header("Transform")]
    public RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Application.isEditor) 
        //{
        //    int headerLength = _headerText.text.Length;
        //    int contentLength = _contentText.text.Length;

        //    _layoutElement.enabled = (headerLength > _characterWrapLimit || contentLength > _characterWrapLimit) ? true : false;
        //}

        //Vector2 mousePosition = Input.mousePosition;

        //float pivotX = mousePosition.x / Screen.width;
        //float pivotY = mousePosition.y / Screen.height;

        //_rectTransform.pivot = new Vector2(pivotX, pivotY);
        //transform.position = mousePosition;
    }

    public void SetText(string content, string header = "") 
    {
        if (string.IsNullOrEmpty(header))
        {
            //_headerText.gameObject.SetActive(false);
        }
        else
        {
            //_headerText.gameObject.SetActive(true);
            _headerText.text = header;
        }

        _contentText.text = content;

        ////int headerLength = _headerText.text.Length;
        //int contentLength = _contentText.text.Length;

        //_layoutElement.enabled = (/*headerLength > _characterWrapLimit || */contentLength > _characterWrapLimit) ? true : false;
    }

    
}
