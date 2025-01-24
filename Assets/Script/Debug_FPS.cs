using System;
using UnityEngine;
using TMPro;
public class Debug_FPS : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    void Start()
    {
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        _text.text ="FPS : "+((int)(1.0f / Time.unscaledDeltaTime )).ToString();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.targetFrameRate = 30;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Application.targetFrameRate = 60;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Application.targetFrameRate = 120;
        }
    }
}
