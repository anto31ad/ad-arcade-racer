using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGUI : MonoBehaviour
{
    public static PauseGUI instance;

    private GameObject _pausePanel;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        _pausePanel = GameObject.Find("panel_pause");
        _pausePanel.SetActive(false);
    }

    public void Pause() {
        GamePauseController.instance.Pause();
    }

    public void Unpause() {
        GamePauseController.instance.Unpause();
    }

    public void Show(bool value) {
        _pausePanel.SetActive(value);
    }

    public void ExitToMainMenu() {
        Loader.Load(Loader.Scene.MainMenu);
    }

    public void RestartRace() {
        Loader.Load(Loader.Scene.OvalRace);
    }
}
