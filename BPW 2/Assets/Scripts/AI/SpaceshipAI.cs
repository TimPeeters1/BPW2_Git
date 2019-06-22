﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(SpaceshipHealth))]
[RequireComponent(typeof(TargetIndicator))]
public class SpaceshipAI : MonoBehaviour
{

    #region Serialized Variables
    [Space]
    [SerializeField] AI_State State;

    [Space]
    [SerializeField] GameObject Target;

    [Space]
    [Header("AI Settings")]
    [SerializeField] float smoothing;

    [Space]
    [SerializeField] float followSpeed;

    [Space]
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    [Space]
    [SerializeField] float attackDistance;
    [SerializeField] float gunAccuracy;

    [Space]
    [SerializeField] float Fov;

    [SerializeField] float angle;

    [Space]
    [Header("UI Components")]
    [SerializeField] Image TargetIndicator; //The image that gets drawn on the ship/ai when in cameraview
    #endregion

    [HideInInspector] public List<GameObject> possibleTargets;

    public enum AI_State
    {
        Idle,
        Following,
        Evading,
        TargetSearch,
        AttackMothership
    }

    #region Private
    private Rigidbody rb;
    private Vector3 randomPoint;
    Quaternion rotation;

    private float distance;
    private float distanceToPoint;
    private float Timer;
    private float closest;

    private bool isFiring;
    private bool isTurning;

    private List<GameObject> enemyMotherships;
    #endregion


    private void Start()
    {
        //Target = this.gameObject;
        rb = GetComponent<Rigidbody>();
        GeneratePosition();

        // State = AI_State.Idle;
        TargetIndicator.color = GetComponent<SpaceshipHealth>().team.TeamColor;
        possibleTargets = new List<GameObject>();
        enemyMotherships = new List<GameObject>();

        SearchForTarget();

        Mothership[] motherships = FindObjectsOfType<Mothership>();

        foreach (Mothership ship in motherships)
        {
            if(ship.Team != GetComponent<SpaceshipHealth>().team)
            {
                enemyMotherships.Add(ship.gameObject);
            }
        }

        if (Target == null)
        {
            State = AI_State.TargetSearch;
        }
    }

    void StateSwitch()
    {
        switch (State)
        {
            case AI_State.Idle:
                RandomPosition();
                break;
            case AI_State.Following:
                FollowTarget();
                break;
            case AI_State.Evading:
                PerformEvasion();
                break;
            case AI_State.TargetSearch:
                SearchForTarget();
                break;
            case AI_State.AttackMothership:
                AttackMothership();
                break;
        }
    }

    void Update()
    {
        if(Target == null){
            State = AI_State.TargetSearch;
        }
        else
        {
            distance = Vector3.Distance(Target.transform.position, transform.position);
            angle = Mathf.Abs(Vector3.Angle(transform.forward, Target.transform.position - transform.position));
        }

        if (possibleTargets.Count >= 0 && enemyMotherships.Count >= 0)
        {
            if (distance < maxDistance && State != AI_State.Evading && angle < Fov && Target != null)
            {
                State = AI_State.Following;
            }
        }

        if(distance < attackDistance && angle < Fov)
        {
            AttackTarget();
        }

        if(possibleTargets.Count <= 0 && enemyMotherships.Count <= 0)
        {
            State = AI_State.Idle;
        }

        if (possibleTargets.Count <= 0 && enemyMotherships.Count > 0)
        {
            State = AI_State.AttackMothership;
        }

        StateSwitch();
    }

    void AttackTarget()
    {
        if (!isFiring)
        {
            StartCoroutine(FireLaser());
        }
    }

    IEnumerator FireLaser()
    {
        isFiring = true;

        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));    

         Vector3 targetDirection = Target.transform.position - transform.position;
         Vector3 randomizedDirection = new Vector3(targetDirection.x + Random.Range(0, gunAccuracy), targetDirection.y + Random.Range(0, gunAccuracy), targetDirection.z + Random.Range(0, gunAccuracy));

         Ray ray = new Ray(transform.position, randomizedDirection);
         

         SpaceshipAI_Gun gunScript = GetComponentInChildren<SpaceshipAI_Gun>();
         gunScript.shot(ray, attackDistance);

        isFiring = false;
    }

    void RandomPosition()   
    {
        distanceToPoint = Vector3.Distance(randomPoint, transform.position);

        Quaternion targetRotation = Quaternion.LookRotation(randomPoint, transform.up);
        transform.position = Vector3.MoveTowards(transform.position, randomPoint, (followSpeed/5) * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothing);

        if (distanceToPoint < 10)
        {
            GeneratePosition();
        }
    }

    void GeneratePosition()
    {
        randomPoint = transform.position + Random.insideUnitSphere * maxDistance;
    }

    void FollowTarget()
    {
        Vector3 targetDir = Target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Target.transform.up);

        Ray forwardRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.SphereCast(forwardRay, minDistance/2, out hit, minDistance)){
            Timer = Random.Range(0.5f, 2);

            State = AI_State.Evading;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothing);
            transform.position = Vector3.MoveTowards(transform.position, forwardRay.GetPoint(10f), followSpeed * Time.deltaTime);
        }

    }

    void PerformEvasion()
    {
        if (Timer > 0)
        {
            if (!isTurning)
            {
                rotation = Quaternion.Euler(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20));
                Timer = Random.Range(5, 10);
            }

            isTurning = true;
            Ray forwardRay;

            Vector3 modifiedRotation = rotation * transform.forward;
            forwardRay = new Ray(transform.position, modifiedRotation);

            Vector3 targetDir = forwardRay.GetPoint(100f);
            Quaternion targetRotation = Quaternion.LookRotation(targetDir, Target.transform.up);

            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 0.5f);
            transform.position = Vector3.MoveTowards(transform.position, targetDir, followSpeed * Time.deltaTime);

            Timer -= Time.deltaTime;
        }
        else
        {
            isTurning = false;

            State = AI_State.TargetSearch;
        }   

    }

    void SearchForTarget()
    {
        SpaceshipHealth[] spaceships = FindObjectsOfType<SpaceshipHealth>();

        closest = Mathf.Infinity;

            foreach (SpaceshipHealth ship in spaceships)
            {
                if (ship.gameObject != this.gameObject && ship.team != GetComponent<SpaceshipHealth>().team)
                {
                    if (!possibleTargets.Contains(ship.gameObject))
                    {
                         possibleTargets.Add(ship.gameObject);
                    }

                    Vector3 distanceToTarget = ship.transform.position - transform.position;
                    float distance = distanceToTarget.sqrMagnitude;

                    if (distance < closest)
                    {
                        closest = distance;
                        Target = ship.gameObject;
                    }
                }
            }

            if (State != AI_State.Idle)
            {
                State = AI_State.Following;
            }
        }

    void AttackMothership()
    {
        Mothership[] ships = FindObjectsOfType<Mothership>();

        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i].Team != GetComponent<SpaceshipHealth>().team)
            {
                Target = ships[i].gameObject;
            }
        }

        Vector3 targetDir = Target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir, Target.transform.up);

        Ray forwardRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(forwardRay, minDistance, out hit, minDistance))
        {
            Timer = Random.Range(1f, 3);

            State = AI_State.Evading;
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothing);
            transform.position = Vector3.MoveTowards(transform.position, forwardRay.GetPoint(10f), followSpeed * Time.deltaTime);
        }

    }
}


