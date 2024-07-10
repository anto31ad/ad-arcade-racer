using UnityEngine;
using TMPro;

public class SpeedGUI : MonoBehaviour
{
    private ArcadeCar playerCar;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        playerCar = GameObject.FindWithTag("Player").GetComponent<ArcadeCar>();
        textMesh = GameObject.Find("text_speed").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = Mathf.FloorToInt(playerCar.SpeedKph).ToString();
    }
}
