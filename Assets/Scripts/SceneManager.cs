using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [Header("Profiler")]
    [SerializeField] private GameObject _profilerUI;
    [SerializeField] private bool _showProfilerUI = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        LeanTween.reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        _profilerUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) 
        {
            _showProfilerUI = !_showProfilerUI;
            _profilerUI.SetActive(_showProfilerUI);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
        }
    }
}
