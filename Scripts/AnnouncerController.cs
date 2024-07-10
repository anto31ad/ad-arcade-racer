using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AnnouncerController : MonoBehaviour
{
    public static AnnouncerController instance;

    [Header ("Start")]
    public AudioClip[] countdownClips;
    public AudioClip getAwayClip;

    [Header ("Checkpoints")]
    public AudioClip[] timeExtClips;
    public AudioClip finalLapClip;

    [Header ("Game Over")]
    public AudioClip gameLostAnnouncerClip;
    public AudioClip gameWonAnnouncerClip;

    private AudioSource audioSource;

    private Queue<AudioClip> queuedClips;


    void Awake() {
        instance = this;
    }
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        queuedClips = new Queue<AudioClip>();
    }

    void Update() {

        if (queuedClips.Count > 0 && !audioSource.isPlaying) {
            audioSource.PlayOneShot(queuedClips.Dequeue());
        }
    }

    public void Win() {
        queuedClips.Enqueue(gameWonAnnouncerClip);
    }

    public void Lose() {
        queuedClips.Enqueue(gameLostAnnouncerClip);
    }

    public void Countdown(int secsLeft) {
        int index = countdownClips.Length - secsLeft;
        queuedClips.Enqueue(countdownClips[index]);
    }

    public void GetAway() {
        queuedClips.Enqueue(getAwayClip);
    }

    public void TimeExtension() {
        int index = Random.Range(0, timeExtClips.Length);
        queuedClips.Enqueue(timeExtClips[index]);

        if (LapSystem.instance.IsFinalLap()) {
            queuedClips.Enqueue(finalLapClip);
        }
    }

    public void FinalLap() {
        queuedClips.Enqueue(finalLapClip);  
    }
}
