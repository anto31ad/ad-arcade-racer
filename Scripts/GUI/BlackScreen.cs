using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class BlackScreen : MonoBehaviour
{
    private float fadeTimeSecs;
    private bool _invert;
    private float _timeElapsed;
    private Image image;
    private Color imageColor;
    private bool _fadeEnabled;

    void Start()
    {   
        image = GetComponent<Image>();
        imageColor = image.color;
    }
    
    void Update()
    {
        if (!_fadeEnabled) {
            return;
        }

        float value = _invert ? _timeElapsed / fadeTimeSecs : 1 - _timeElapsed / fadeTimeSecs;
        imageColor.a = Mathf.Clamp01(value);
        image.color = imageColor;

        _timeElapsed += Time.deltaTime;
        if (_timeElapsed > fadeTimeSecs) {
            _fadeEnabled = false;
            return;
        }
    }

    public void FadeIn(float secs) {
        _timeElapsed = 0f;
        _invert = false;
        fadeTimeSecs = secs;
        _fadeEnabled = true;
    }

    public void FadeOut(float secs) {
        _timeElapsed = 0f;
        _invert = true;
        fadeTimeSecs = secs;
        _fadeEnabled = true;
    }
}
