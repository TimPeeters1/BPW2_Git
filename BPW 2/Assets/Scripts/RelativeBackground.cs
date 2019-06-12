using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeBackground : MonoBehaviour
{
    GameObject Player;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        transform.position = Player.transform.position;
    }
}
