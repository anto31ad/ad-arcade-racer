/*
    Useful tutorial: https://www.youtube.com/watch?v=pbuJUaO-wpY
    Helped me making this class a singleton class.
*/

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public static AudioManager instance;
    

    void Awake() {
        // if (instance == null) {
        //     instance = this;
        //     DontDestroyOnLoad(this.gameObject);
        // } else {
        //     Destroy(this.gameObject);
        // }

        instance = this;
    }


    void Start()
    {
        //makes sure to enable diegetic sounds:
        //upon restarting the race from the session, volumes are not reset to 0.
        AdjustVolumes();        
    }


    public void PauseProcedure() {
        audioMixer.SetFloat("d_vol", -80f);   
    }

    public void UnpauseProcedure() {
        audioMixer.SetFloat("d_vol", 0f);
    }

    private void AdjustVolumes() {
        audioMixer.SetFloat("d_vol", 0f);
    }
}
