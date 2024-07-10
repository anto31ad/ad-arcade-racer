using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager instance;

    public float timePreRace = 5f;
    public int startCountdown = 3;
    public float cooldownTime = 5f;
    public float quittingTime = 2f;


    public enum Phase {
        PreRace,
        StartCountdown,
        Start,
        RaceOn,
        Cooldown,
        Quitting
    }
    public float timeElapsed;

    public Phase _currentPhase;
    private bool _gameIsOver;

    private GameOverGUI _gameOverGUI;
    private LapGUI _lapGUI;
    private TimerGUI _timerGUI;
    private StartCountdownGUI _startCountdownGUI;

    private BlackScreen _blackScreen;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameOverGUI = GameObject.Find("gui_gameover").GetComponent<GameOverGUI>();

        ArcadeTimer.instance.Stopped = true;
        _timerGUI = GameObject.Find("gui_timer").GetComponent<TimerGUI>();

        _lapGUI = GameObject.Find("gui_lap").GetComponent<LapGUI>();

        _startCountdownGUI = GameObject.Find("gui_start").GetComponent<StartCountdownGUI>();


        //phase 0
        _currentPhase = 0;
        ArcadeCarController.instance.StopTheCar();
        CameraController.instance.CurrentMode = CameraController.Mode.Cinematic;

        //get blackscreent and fade from black to visible
        _blackScreen = GameObject.Find("blackscreen").GetComponent<BlackScreen>();
        _blackScreen.FadeIn(2f);
    }

    void Update() {
        timeElapsed += Time.deltaTime;

        switch (_currentPhase)
        {
            case Phase.PreRace:
                PreRaceUpdate();
                break;
            case Phase.StartCountdown:
                StartCountdownUpdate();
                break;
            case Phase.Start:
                GameStart();
                break;
            case Phase.Cooldown:
                CooldownUpdate();
                break;
            case Phase.Quitting:
                QuittingPhase();
                break;
            default:
                break;
        };
    }

    void PreRaceUpdate() {
        if (timeElapsed > timePreRace || Input.GetAxis("Jump") > 0) {
            NextPhase();
            CameraController.instance.CurrentMode = CameraController.Mode.Chase;
            _startCountdownGUI.StartCountdown(startCountdown);
        }
    }

    void StartCountdownUpdate() {
        if (timeElapsed > startCountdown) {
            NextPhase();
        }
    }

    private void GameStart() {
        ArcadeTimer.instance.Stopped = false;
        LapSystem.instance.StartTiming();
        
        //unlocks the controls
        ArcadeCarController.instance.ControlsEnabled = true;
        NextPhase();
    }

    public void CooldownUpdate() {
        //waits for cooldownTime before exiting to main menu.
        if (timeElapsed > cooldownTime) {
            NextPhase();
            _blackScreen.FadeOut(quittingTime);
        }
    }
    
    public void QuittingPhase() {
        if (timeElapsed > quittingTime) {
            Loader.Load(Loader.Scene.MainMenu);
        }
    }

    // GAME OVER
    public void GameWonProcedure() {
        if (_gameIsOver) return;
        Debug.Log("Game Won!");
        GameOverProcedure();
        _gameOverGUI.ShowGameWonPanel(true);
    }

    public void GameLostProcedure() {
        if (_gameIsOver) return;
        Debug.Log("Game Lost");
        GameOverProcedure();
        _gameOverGUI.ShowGameLostPanel(true);
    }

    private void GameOverProcedure() {
        _gameIsOver = true;
        ArcadeTimer.instance.Stopped = true;
        LapSystem.instance.StopTiming();

        CameraController.instance.CurrentMode = CameraController.Mode.Stare;

        ArcadeCarController.instance.StopTheCar();

        SetPauseMenuEnabled(false);
        StopLevelMusic();

        NextPhase();
    }

    private void StopLevelMusic() {
        GameObject.Find("_music").GetComponent<AudioSource>().Stop();
    }

    // LAP SYSTEM
    public void NextLapProcedure() {
        _lapGUI.SingleLapUpdate();

        if (LapSystem.instance.IsRaceOver()) GameWonProcedure();
    }

    public void TimeExtensionProcedure() {
        _timerGUI.NotifyTimeExtension();
    }

    public void SetPauseMenuEnabled(bool value) {
        GamePauseController.instance.enabled = value;
    }

    private void NextPhase() {
        timeElapsed = 0f;
        _currentPhase++;
    }
}
