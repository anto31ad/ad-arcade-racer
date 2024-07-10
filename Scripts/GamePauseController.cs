using UnityEngine;

public class GamePauseController : MonoBehaviour
{
    public static GamePauseController instance;

    private bool _paused;


    void Awake() {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("Escape press detected!");
            if (!_paused) {
                Pause();
            } else {
                Unpause();
            }
        }
    }

    public void Pause() {
        _paused = true;
        PauseGUI.instance.Show(true);
        PhysicsController.instance.Pause();
        AudioManager.instance.PauseProcedure();
    }

    public void Unpause() {
        _paused = false;
        PauseGUI.instance.Show(false);
        PhysicsController.instance.Unpause();
        AudioManager.instance.UnpauseProcedure();
    }
}
