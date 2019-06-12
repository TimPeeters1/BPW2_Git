using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;

    [Space]
    [Header("Flight Controls")]
    public KeyCode ForwardButton;
    [Space]
    public KeyCode LeftButton;
    public KeyCode LeftRotate;
    [Space]
    public KeyCode RightButton;
    public KeyCode RightRotate;
    [Space]
    public KeyCode BoostButton;

    [Space]
    [Header("Mouse Axis")]
    public string VerticalAxis;

    public string HorizontalAxis;

    [Space]
    [Header("Gun Control")]
    [SerializeField] KeyCode ShotButton;
    public float LaserRange;


    public static event Action HandleMovement = delegate { };
    public static event Action HandleRotation = delegate { };
    public static event Action HandleShooting = delegate { };
    public static event Action UpdateEffects = delegate { };

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void Update()
    {
        UpdateEffects();
        if (Input.GetKeyDown(ShotButton)) HandleShooting();
    }
}
