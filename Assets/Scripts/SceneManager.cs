using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [Header("Profiler UI")]
    [SerializeField] private GameObject _fpsCounterUI;
    [SerializeField] private bool _showFPS = false;

    // Start is called before the first frame update
    void Start()
    {
        _fpsCounterUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) 
        {
            _showFPS = !_showFPS;
            _fpsCounterUI.SetActive(_showFPS);
        }
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
        }
    }
}
