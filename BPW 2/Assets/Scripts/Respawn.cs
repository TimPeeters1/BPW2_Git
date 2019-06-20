using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Respawn : MonoBehaviour
{
    [SerializeField] float RespawnTime;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject respawnEffect;

    GameObject playerSpawn;
    CinemachineVirtualCamera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
        mainCamera = Camera.main.GetComponent<CinemachineVirtualCamera>();

        StartCoroutine(SpawnPlayer()); 

    }
    
    IEnumerator SpawnPlayer()
    {
        yield return new WaitForSeconds(RespawnTime);

        Instantiate(respawnEffect, playerSpawn.transform.position, playerSpawn.transform.rotation);
        GameObject Player = Instantiate(playerPrefab, playerSpawn.transform.position, playerSpawn.transform.rotation);
        mainCamera.m_Follow = Player.transform;
        mainCamera.m_LookAt = Player.transform;

        UI_LookAt[] ui = FindObjectsOfType<UI_LookAt>();
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].enabled = true;
        }



        Destroy(this.gameObject);
    }
}
