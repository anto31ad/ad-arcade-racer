using TMPro;
using UnityEngine;

public class StartCountdownGUI : MonoBehaviour
{
    public float holdSecs = 2f;
    public Color numberColor;
    public Color getAwayColor;

    private bool _started;
    private float _countdownSecs;
    private float _timeElapsed;
    public int _curSecLeft;

    private TextMeshProUGUI _text;
    private AnnouncerController announcer;

    // Start is called before the first frame update
    void Start()
    {
        _text = GameObject.Find("text_start").GetComponent<TextMeshProUGUI>();
        _text.enabled = false;

        announcer = GameObject.Find("_audio_announcer").GetComponent<AnnouncerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_text.enabled) {
            return;
        }
        
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > _countdownSecs + holdSecs) {
            _text.enabled = false;
        }

        if (!_started) {
            return;
        }

        int secsLeft = Mathf.CeilToInt(_countdownSecs - _timeElapsed);

        if (secsLeft > 0 && _curSecLeft != secsLeft) {
            _text.text = secsLeft.ToString();
            announcer.Countdown(secsLeft);
            _curSecLeft = secsLeft;
        }

        if (_timeElapsed > _countdownSecs) {
            _started = false;
            _text.text = "GO!";
            _text.color = getAwayColor;
            announcer.GetAway();
        }
    }

    public void StartCountdown(int secs) {
        _curSecLeft = -1;
        _started = true;
        _countdownSecs = secs;
        _text.enabled = true;
        _text.color = numberColor;
    }
}
