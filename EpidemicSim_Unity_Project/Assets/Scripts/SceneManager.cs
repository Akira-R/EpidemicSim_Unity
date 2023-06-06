using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private InputAction mainUIcontrols;
    private InputAction applicationQuit;
    private InputAction profilerUI;

    [Header("Profiler")]
    [SerializeField] private GameObject _profilerUI;
    [SerializeField] private bool _showProfilerUI = false;

    public VariableObject variableObject;
    public VariableData variableData;

    [Header("MainUI")]
    public UITransformTween[] mainUI;

    private void Awake()
    {
        //Application.targetFrameRate = 60;
        LeanTween.reset();
        playerControls = new PlayerControls();

        variableData = SaveLoadSystem.LoadData();
    }
    private void OnEnable()
    {
        applicationQuit = playerControls.System.Quit;
        profilerUI = playerControls.System.Profiler;
        playerControls.MainUI.Enable();
        playerControls.System.Enable();
    }
    private void OnDisable()
    {
        playerControls.MainUI.Disable();
        playerControls.System.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (variableData != null) 
        {
            variableObject.SetDatafromSave(variableData);
        }

        _profilerUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        playerControls.MainUI.ParameterUI.performed += context =>
        {
            if (SimulationManager.Instance.GetSimState() == 0)
                mainUI[0].ButtonToggleEase();
        };
        playerControls.MainUI.PlaceUI.performed += context =>
        {
            if (SimulationManager.Instance.GetSimState() == 0)
                mainUI[1].ButtonToggleEase();
        };
        
        playerControls.MainUI.FIlterUI.performed += context =>
        {
            mainUI[2].ButtonToggleEase();
        };
        playerControls.MainUI.GraphUI.performed += context =>
        {
            mainUI[3].ButtonToggleEase();
        };

        profilerUI.performed += context =>
        {
            _showProfilerUI = !_showProfilerUI;
            _profilerUI.SetActive(_showProfilerUI);
        };
        applicationQuit.performed += context =>
        {
            QuitApplication();
        };
    }

    public void QuitApplication() 
    {
        SaveLoadSystem.SaveData(variableObject);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
