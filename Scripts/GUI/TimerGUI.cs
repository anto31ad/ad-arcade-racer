using UnityEngine;
using TMPro;

public class TimerGUI : MonoBehaviour
{
    public float timeExtensionEffectDurationSecs = 2f;

    private TextMeshProUGUI timerText;
    private TextMeshProUGUI timeExtText;

    private float timeExtTimeElapsed;

    void Start()
    {
        timerText = GameObject.Find("text_timer").GetComponent<TextMeshProUGUI>();  
        timeExtText = GameObject.Find("text_timeext").GetComponent<TextMeshProUGUI>();
        timeExtText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //continuously update the timer text mesh to show the time left
        timerText.text = Mathf.FloorToInt(ArcadeTimer.instance.SecsLeft).ToString();

        HandleTimeExtension();
    }

    public void NotifyTimeExtension() {
        timeExtText.enabled = true;
        AnnouncerController.instance.TimeExtension();
    }

    private void HandleTimeExtension() {
        if (timeExtText.enabled) {
            timeExtTimeElapsed += Time.deltaTime;

            if (timeExtTimeElapsed > timeExtensionEffectDurationSecs) {
                timeExtTimeElapsed = 0f;
                timeExtText.enabled = false;
            }
        }
    }
}
