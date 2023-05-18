using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuletConnector : MonoBehaviour
{
    [SerializeField] GameObject LevelGenerator;
    Level l = null;

    private void Awake()
    {
        l = LevelGenerator.GetComponent<Level>();
    }

    public void UpToPoint(float x = 5f)
    {
        if (l != null)
        {
            l.UpToPoint(x);
        }
    }

    public void DownToPoint(float x = 5f)
    {
        if (l != null)
        {
            l.DownToPoint(x);
        }
    }
}
