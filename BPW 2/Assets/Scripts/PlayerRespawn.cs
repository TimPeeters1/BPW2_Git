using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] GameObject respawnPrefab;

    public void InitRespawn()
    {
        UI_LookAt[] ui = FindObjectsOfType<UI_LookAt>();
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].enabled = false;
        }

        Instantiate(respawnPrefab);
    }
}
