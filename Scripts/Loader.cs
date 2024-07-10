using UnityEngine.SceneManagement;

public static class Loader {

    public enum Scene {
        MainMenu,
        OvalRace
    }

    public static void Load(Scene scene) {
        SceneManager.LoadScene(scene.ToString());
    }
}
