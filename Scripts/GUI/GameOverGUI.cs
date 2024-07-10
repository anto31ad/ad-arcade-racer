using UnityEngine;
using TMPro;

public class GameOverGUI : MonoBehaviour
{
    
    private TextMeshProUGUI winTextBox;
    private TextMeshProUGUI gamelostTextBox;
    private AnnouncerController announcer;

    void Start()
    {
        winTextBox = GameObject.Find("text_win").GetComponent<TextMeshProUGUI>();
        winTextBox.enabled = false;
        gamelostTextBox = GameObject.Find("text_gamelost").GetComponent<TextMeshProUGUI>();
        gamelostTextBox.enabled = false;

        announcer = GameObject.Find("_audio_announcer").GetComponent<AnnouncerController>();
    }

    public void ShowGameWonPanel(bool value) {
        winTextBox.enabled = value;
        announcer.Win();
    }

    public void ShowGameLostPanel(bool value) {
        gamelostTextBox.enabled = value;
        announcer.Lose();
    }


}
