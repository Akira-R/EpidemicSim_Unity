using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [BoxGroup("Time Bar")]
    [SerializeField] private Image _timeBarImage;
    [BoxGroup("Time Bar")]
    [SerializeField] private TMP_Text _dayCounterText;
    [BoxGroup("Time Bar")]
    [SerializeField] private int _dayCounter = 1;
    private int _timerSpeed = 1;
    private int timeTracker = 0;
    private int _maxTime = 300;

    [BoxGroup("Time Controls")]
    [SerializeField] private GameObject _playPauseButton;
    [BoxGroup("Time Controls")]
    [SerializeField] private Sprite _playSprite, _pauseSprite;
    private bool _isSimPlaying;

    [BoxGroup("Speed Controls")]
    [SerializeField] TMP_Text _speedControlButtonText;
    [BoxGroup("Speed Controls")]
    public List<int> _speedIncrements;
    [BoxGroup("Speed Controls")]
    [SerializeField] private int _speedIncrementsIdx = 0;

    private PlayerControls playerControls;
    private InputAction playPauseControl;
    private InputAction speedOne;
    private InputAction speedTwo;
    private InputAction speedThree;

    [BoxGroup("Graph")]
    public ChartValueInit chart;

    [BoxGroup("Panel")]
    [SerializeField] private Button parameterButton;
    [BoxGroup("Panel")]
    [SerializeField] private Image parameterButtonImg;
    [BoxGroup("Panel")]
    [SerializeField] private Button placeButton;
    [BoxGroup("Panel")]
    [SerializeField] private Image placeButtonImg;
    [BoxGroup("Panel")]
    [SerializeField] private bool panelActiveStatus = true;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playPauseControl = playerControls.TimeControl.PlayPauseControl;
        speedOne = playerControls.TimeControl.TimeSpeed1;
        speedTwo = playerControls.TimeControl.TimeSpeed2;
        speedThree = playerControls.TimeControl.TimeSpeed3;
        playerControls.TimeControl.Enable();
    }
    private void OnDisable()
    {
        playerControls.TimeControl.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        _playPauseButton.GetComponent<Image>().sprite = _playSprite;

        _isSimPlaying = false;

        parameterButton.interactable = panelActiveStatus;
        placeButton.interactable = panelActiveStatus;

        parameterButtonImg.color = Color.white;
        placeButtonImg.color = Color.white;
    }

    private void FixedUpdate()
    {
        if (_isSimPlaying == true) 
        {
            //timeTracker += _timerSpeed;
            timeTracker++;

            if (timeTracker > _maxTime)
            {
                timeTracker = 0;
                _dayCounter++;

                // Update Chart and R0
                chart.UpdateGraphData(_dayCounter);
            }

            //EntityManager.Instance.UpdateStateCount();

            //Debug.Log((float)timeTracker / (float)_fillEvery);
            _timeBarImage.fillAmount = (float)timeTracker / (float)(_maxTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SimulationManager.Instance.GetSimState() != 0)
        {
            panelActiveStatus = false;
            parameterButton.interactable = panelActiveStatus;
            placeButton.interactable = panelActiveStatus;

            parameterButtonImg.color = Color.gray;
            placeButtonImg.color = Color.gray;
        }
        else 
        {
            panelActiveStatus = true;
            parameterButton.interactable = panelActiveStatus;
            placeButton.interactable = panelActiveStatus;

            parameterButtonImg.color = Color.white;
            placeButtonImg.color = Color.white;
        }

        _dayCounterText.text = "Day: " + _dayCounter;
        _speedControlButtonText.text = "x" + (_timerSpeed).ToString();

        UpdateInput();
    }

    public void OnPlayPauseButtonPressed()
    {
        _isSimPlaying = !_isSimPlaying;

        AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);

        if (_isSimPlaying == true)
        {
            _playPauseButton.GetComponent<Image>().sprite = _playSprite;
            Time.timeScale = _timerSpeed;
        }
        else
        {
            _playPauseButton.GetComponent<Image>().sprite = _pauseSprite;
            Time.timeScale = 0;
        }
    }

    public void OnSpeedContolButtonPressed() 
    {
        AudioManager.instance.Play(AudioManager.instance.uiAudioList.sfx_uiClicked);

        _speedIncrementsIdx = (_speedIncrementsIdx + 1) % _speedIncrements.Count;
        SimulationSpeed(_speedIncrementsIdx);
    }

    private void SimulationSpeed(int incrementIdx)
    {
        //if (!_isSimPlaying)
        //    OnPlayPauseButtonPressed();

        _timerSpeed = _speedIncrements[incrementIdx];
        Time.timeScale = _timerSpeed;
    }

    private void UpdateInput() 
    {
        playPauseControl.performed += context => 
        {
            if (SimulationManager.Instance.GetSimState() == 0)
                SimulationManager.Instance.StartSimulation();

            OnPlayPauseButtonPressed();
        };

        speedOne.performed += context =>
        {
            SimulationSpeed(0);
        };
        speedTwo.performed += context =>
        {
            SimulationSpeed(1);
        };
        speedThree.performed += context =>
        {
            SimulationSpeed(2);
        };
    }

    public void ResetTime() 
    {
        _isSimPlaying = false;
        timeTracker = 0;
        _dayCounter = 1;

        _playPauseButton.GetComponent<Image>().sprite = _playSprite;
        _timeBarImage.fillAmount = 0;
        chart.ResetGraph();
    }

    //private void OnFirstSimulationStart()
    //{
    //    _isSimPlaying = true;
    //    SimulationManager.Instance.StartSimulation();
    //}
}
