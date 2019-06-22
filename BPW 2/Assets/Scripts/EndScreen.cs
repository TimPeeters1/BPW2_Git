using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public bool hasLost;

    [SerializeField] GameObject lost;
    [SerializeField] GameObject win;

    private void Start()
    {
        if (hasLost)
        {
            lost.SetActive(true);
            win.SetActive(false);
        }
        else
        {
            lost.SetActive(false);
            win.SetActive(true);
        }

        GameObject mainCamera = Camera.main.gameObject;
        mainCamera.SetActive(false);

        GameObject mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        mainCanvas.SetActive(false);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
