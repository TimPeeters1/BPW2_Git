using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] GameObject respawnPrefab;
    

    public void InitRespawn()
    {
        Instantiate(respawnPrefab);
    }
}
