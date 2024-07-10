using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapSystem : MonoBehaviour
{
    public static LapSystem instance;

    public GameObject player;
    public Checkpoint[] checkpoints;

    public int lapsToComplete = 3;
    
    [Header("Debug")]
    private int _curTargetCheckpoint = 0;
    private int _laps = 0;
    private bool _timingEnabled;
    private float _timeElapsedSecs;
    private float _timeElapsedSinceStartOfCurrentLapSecs;
    private List<float> _lapTimesSecs;
    private int _bestLaptimeIndex;

    public float timeElapsedSecs {
        get => _timeElapsedSecs;
    }

    public float CurrentLapTimeElapsedSecs {
        get => _timeElapsedSecs - _timeElapsedSinceStartOfCurrentLapSecs;
    }
    
    public int CurrentLapNumber {
        get => _laps + 1;
    }

    public int CompletedLaps {
        get => _laps;
    }

    public float LastLapTimeSecs {
        get => _lapTimesSecs.Last();
    }

    public float BestLapTimeSecs {
        get => _lapTimesSecs[_bestLaptimeIndex];
    }

    public long BestLapNumber {
        get => _bestLaptimeIndex + 1;
    }

    void Awake() {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        checkpoints = GameObject.Find("Checkpoints").GetComponentsInChildren<Checkpoint>();

        _lapTimesSecs = new List<float>();
    }

    void Update() {
        //sums deltaTime to elapsed time
        if (_timingEnabled) {
            _timeElapsedSecs += Time.deltaTime;
        }
    }

    public void StartTiming() {
        _timeElapsedSecs = 0f;
        _timingEnabled = true;
    }

    public void StopTiming() {
        _timingEnabled = false;
    }

    public bool CheckTarget(GameObject hitter) {
        return hitter == player;
    }
    public bool IsNext(Checkpoint checkpoint) {
        return checkpoint == checkpoints[_curTargetCheckpoint];
    }

    public void NextCheckpoint() {
        //sets new target checkpoint
        _curTargetCheckpoint++;
        //checks if out of bounds
        if (_curTargetCheckpoint > checkpoints.Length - 1) {
            //sets the first checkpoint as target
            _curTargetCheckpoint = 0;
            RecordLapTime();
            UpdateBest();
            NextLap();
        }
    }

    public bool IsFinalLap() {
        return CurrentLapNumber == lapsToComplete;
    }

    public bool IsRaceOver() {
        return CurrentLapNumber > lapsToComplete;
    }

    private void NextLap() {
        _laps++;
        RaceManager.instance.NextLapProcedure();
    }


    private void UpdateBest() {
        if (_laps == 0) {
            return;
        }
        float bestLapTimeSecs = _lapTimesSecs[_bestLaptimeIndex];
        float lastLaptimeMillis = _lapTimesSecs.Last();
        if (lastLaptimeMillis < bestLapTimeSecs) {
            _bestLaptimeIndex = _laps;
        }
    }

    private void RecordLapTime() {
        float lapTimeSec = _timeElapsedSecs - _timeElapsedSinceStartOfCurrentLapSecs;
        Debug.Log("Laptime: " + lapTimeSec);
        _lapTimesSecs.Add(lapTimeSec);
        _timeElapsedSinceStartOfCurrentLapSecs = _timeElapsedSecs;
    }
}
