using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInput : MonoBehaviour
{
    private void Start()
    {
        GetComponent<InputManager>().enabled = false;

        Canvas mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        mainCanvas.enabled = false;
    }

    public void StartInput()
    {
        GetComponent<InputManager>().enabled = true;


        Canvas mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        mainCanvas.enabled = true;
    }
}
