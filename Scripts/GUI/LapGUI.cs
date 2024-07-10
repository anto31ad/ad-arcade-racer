using UnityEngine;
using TMPro;

public class LapGUI : MonoBehaviour
{
    public TextMeshProUGUI completedLapsTextBox;
    public TextMeshProUGUI curLaptimeTextBox;
    public TextMeshProUGUI bestLaptimeTextBox;
    public TextMeshProUGUI lastLaptimeTextBox;

    void Start()
    {
        //lap info panel
        lastLaptimeTextBox = GameObject.Find("text_laptime_last").GetComponent<TextMeshProUGUI>();
        bestLaptimeTextBox = GameObject.Find("text_laptime_best").GetComponent<TextMeshProUGUI>();
        curLaptimeTextBox = GameObject.Find("text_laptime_current").GetComponent<TextMeshProUGUI>();
        completedLapsTextBox = GameObject.Find("text_laps").GetComponent<TextMeshProUGUI>();

        completedLapsTextBox.text = "Lap " + LapSystem.instance.CurrentLapNumber + " / " + LapSystem.instance.lapsToComplete;
    }

    void Update()
    {
        //current laptime
        curLaptimeTextBox.text = "Current " + FormatLaptime(LapSystem.instance.CurrentLapTimeElapsedSecs);
    }

    public void SingleLapUpdate() {
        completedLapsTextBox.text = "Lap " + LapSystem.instance.CurrentLapNumber + " / " + LapSystem.instance.lapsToComplete;
        lastLaptimeTextBox.text = "Last: " + FormatLaptime(LapSystem.instance.LastLapTimeSecs);
        bestLaptimeTextBox.text = "Best: " + FormatLaptime(LapSystem.instance.BestLapTimeSecs) + " (" + LapSystem.instance.BestLapNumber + ")";
    }

    private string FormatLaptime(float seconds) {
        float temp = seconds;
        
        //minute digits
        int lapMin = Mathf.FloorToInt(temp / 60f);
        //seconds
        temp -= lapMin * 60f;
        int lapSecs = Mathf.FloorToInt(temp);
        //tenths
        temp -= lapSecs;
        int lapTenths = Mathf.FloorToInt(temp * 10f);
        //hundreds
        temp -= lapTenths / 10f;
        int lapHundreds = Mathf.FloorToInt(temp * 100f);
        //millis
        temp -= lapHundreds / 100f;
        int lapMillis = Mathf.FloorToInt(temp * 1000f);
        //formats the string
        return lapMin + "." + lapSecs + "." + lapTenths + lapHundreds + lapMillis; 
    }
}
