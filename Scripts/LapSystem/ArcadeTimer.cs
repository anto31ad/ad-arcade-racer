using UnityEngine;

public class ArcadeTimer : MonoBehaviour
{
    public static ArcadeTimer instance;

    public float startingTimeLeftSecs = 30f;
    public float timeExtension = 15f;

    private float _timeLeftSecs;
    private bool _stopped;

    public bool Stopped {
        get => _stopped;
        set {
            _stopped = value;
        }
    }

    public float SecsLeft {
        get => _timeLeftSecs;
    }


    void Awake() {
        instance = this;
    }

    void Start()
    {
        _timeLeftSecs = startingTimeLeftSecs;
    }

    void FixedUpdate()
    {
        if (!_stopped) {
            _timeLeftSecs -= Time.deltaTime;

            if (_timeLeftSecs < 0) {
                _stopped = true;
                _timeLeftSecs = 0f;
                RaceManager.instance.GameLostProcedure();
            }
        }
    }

    public void ExtendTime() {
        if (_stopped) {
            Debug.Log("Tried to extend time when the timer was stopped.");
            return;
        }
        _timeLeftSecs += timeExtension;
        RaceManager.instance.TimeExtensionProcedure();
    }
}
