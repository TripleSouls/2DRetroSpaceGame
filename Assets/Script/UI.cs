using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TMP_Text maxPoint;
    [SerializeField] TMP_Text point;
    [SerializeField] TMP_Text targetTEXT;
    [SerializeField] GameObject LevelGenerator;

    private Level l;

    private void Awake()
    {
        l = LevelGenerator.GetComponent<Level>();
    }

    private void Start()
    {
        updateTargetFPS();
    }

    private void FixedUpdate()
    {
        point.text = "Point : " + l.Point.ToString("F2");
        maxPoint.text = "Max Point : " + l.MaxPoint.ToString("F2");
    }

    private void updateTargetFPS()
    {
        targetTEXT.text = "TargetFPS : " + Application.targetFrameRate;
    }

    public void FPS144()
    {
        Application.targetFrameRate = 144;
        updateTargetFPS();
    }
    public void FPS90()
    {
        Application.targetFrameRate = 90;
        updateTargetFPS();
    }
    public void FPS60()
    {
        Application.targetFrameRate = 60;
        updateTargetFPS();
    }
    public void FPS30()
    {
        Application.targetFrameRate = 30;
        updateTargetFPS();
    }

    public void Continue()
    {
        l.Continue();
    }

    public void Exit()
    {
        Application.Quit();
    }

}
