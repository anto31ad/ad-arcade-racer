using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    public bool extendsTime;


    void Start() {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other) {
        //_lapSystem.UpdateCheckpoints(other.transform.parent.gameObject, this);
        GameObject hitter = other.transform.parent.gameObject;
        if (!LapSystem.instance.CheckTarget(hitter)) {
            return;
        }
        Debug.Log("Player entered a checkpoint!");
        if (!LapSystem.instance.IsNext(this)) {
            return;
        }
        Debug.Log("Player entered the current checkpoint!");
        LapSystem.instance.NextCheckpoint();

        if (extendsTime) ArcadeTimer.instance.ExtendTime();
    }

}
