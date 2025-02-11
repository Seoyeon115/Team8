using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCheck : MonoBehaviour
{
    [Range(1, 100)]
    public int fFontSize;

    [Range(0, 1)]
    public float Red, Green, Blue;

    float deltaTime = 0.0f;

    void Start()
    {
        fFontSize = fFontSize == 0 ? 50 : fFontSize;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int width = Screen.width;
        int height = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, width, height * 0.02f);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / fFontSize;
        style.normal.textColor = new Color(Red, Green, Blue, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
