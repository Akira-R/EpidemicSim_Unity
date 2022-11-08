using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private GameObject _profilerUI;
    [SerializeField] private bool _showProfilerUI = false;

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
