using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("Time Bar")]
    [SerializeField] private Image _timeBarImage;
    [SerializeField] private TMP_Text _dayCounterText;
    [SerializeField] private int _dayCounter = 1;
    private int _timerSpeed = 1;
    private int timeTracker = 0;
    private int _maxTime = 300;

    [Header("Time Controls")]
    [SerializeField] private GameObject _playPauseButton;
    [SerializeField] private Sprite _playSprite, _pauseSprite;
    private bool _isSimPlaying = true;

    [Header("Speed Controls")]
    [SerializeField] TMP_Text _speedControlButtonText;
    public List<int> _speedIncrements;
    [SerializeField] private int _speedIncrementsIdx = 0;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        _playPauseButton.GetComponent<Image>().sprite = _playSprite;
    }

    private void FixedUpdate()
    {
        if (_isSimPlaying == true) 
        {
            timeTracker += _timerSpeed;

            if (timeTracker > _maxTime)
            {
                timeTracker = 0;
                _dayCounter++;
            }

            //Debug.Log((float)timeTracker / (float)_fillEvery);
            _timeBarImage.fillAmount = (float)timeTracker / (float)(_maxTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _dayCounterText.text = "Day: " + _dayCounter;
    }

    public void OnPlayPauseButtonPressed() 
    {
        _isSimPlaying = !_isSimPlaying;

        if (_isSimPlaying == true)
        {
            _playPauseButton.GetComponent<Image>().sprite = _playSprite;
        }
        else 
        {
            _playPauseButton.GetComponent<Image>().sprite = _pauseSprite;
        }
    }

    public void OnSpeedContolButtonPressed() 
    {
        _speedIncrementsIdx = (_speedIncrementsIdx + 1) % _speedIncrements.Count;
        _timerSpeed = _speedIncrements[_speedIncrementsIdx];

        _speedControlButtonText.text = "x" + (_timerSpeed).ToString();
    }
}
