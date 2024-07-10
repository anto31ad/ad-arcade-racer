using UnityEngine;
using TMPro;

[RequireComponent (typeof(TextMeshProUGUI))]
public class FlashingText : MonoBehaviour
{
    public Color color1;
    public Color color2;
    public float timeIntervalSecs;

    private TextMeshProUGUI _textMesh;
    private bool _colorSwitch;
    private float _timeElapsed; 

    void Start() {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _textMesh.color = color1;
        _textMesh.ForceMeshUpdate();
    }

    void Update()
    {
        if (_textMesh.enabled) {
            _timeElapsed += Time.deltaTime;
            if (_timeElapsed > timeIntervalSecs) {
                _timeElapsed = 0f;
                _colorSwitch = !_colorSwitch;

                if (_colorSwitch) {
                    _textMesh.color = color2;
                } else {
                    _textMesh.color = color1;
                }
                _textMesh.ForceMeshUpdate();
            }
        }
    }
}
