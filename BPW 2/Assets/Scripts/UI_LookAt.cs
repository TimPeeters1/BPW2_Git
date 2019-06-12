using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LookAt : MonoBehaviour
{
    GameObject Player;

    [SerializeField] float minSize;
    [SerializeField] float maxSize;

    [SerializeField] float distance;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        transform.LookAt(Player.transform);

        distance = Vector3.Distance(transform.position, Player.transform.position) /2000f;

        distance = Mathf.Pow(distance, 2f);

        //transform.localScale = new Vector3(distance, distance);

        transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(maxSize, maxSize), distance);

        if(transform.localScale.x <= minSize)
        {
            //transform.localScale = Vector3.zero;
        }

        if(transform.localScale.x >= maxSize)
        {
            //transform.localScale = new Vector3(maxSize, maxSize);
        }
        
    }
}
