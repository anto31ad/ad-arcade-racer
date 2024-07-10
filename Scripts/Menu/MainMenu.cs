using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Transform showcar;
    [SerializeField] float showcarRotationSpeed = 10f;

    [SerializeField] Vector3 currentRot;

    void Start() {
        //makes sure to run the scene on default timescale.
        Time.timeScale = 1f;
    }

    public void PlayDemo() {
        Loader.Load(Loader.Scene.OvalRace);
    }

    public void Exit() {
        Debug.Log("Quitting...");
        Application.Quit();
    }

    void Update()
    {
        showcar.Rotate(0f, showcarRotationSpeed * Time.deltaTime, 0f);
    }
}
