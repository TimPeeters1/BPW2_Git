using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{

    InputManager mgr;
    [Header("Spaceship Control Variables")]

    [Range(0, 0.05f)]
    [SerializeField] float noTurn = 0.01f;
    [SerializeField] float turnSpeed = 100;

    [Header("Spaceship Movement Variables")]
    [SerializeField] float NormalThrust = 1000; //The normal amount of thrust on the spaceship
    [SerializeField] float BoostThrust = 1200;//The maximum amount of thrust when boost is enabled (shift by default)
    [Space]
    [SerializeField] float Acceleration = 50;//How fast the ship accelerates

    float ThrustOriginal;

    [HideInInspector] public float currentSpeed;

    [Space]
    [Range(0, 1f)]
    [SerializeField] float Inertia = 0.99f; //Inertial Dampening 

    Camera mainCamera;
    Rigidbody rb;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        
        mgr = GetComponent<InputManager>();

        ThrustOriginal = NormalThrust;

    }

    private void OnEnable()
    {
        InputManager.HandleMovement += shipMovement;
        InputManager.HandleRotation += shipTurning;
    }

    private void OnDisable()
    {
        InputManager.HandleMovement -= shipMovement;
        InputManager.HandleRotation -= shipTurning;
    }

    void shipTurning()
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 delta = (Input.mousePosition - center) / Screen.height;

        float t = 0;
        t += Time.deltaTime;

        if (delta.y > noTurn)
        {
            transform.Rotate(-(delta.y - noTurn) * Time.deltaTime * turnSpeed, 0, 0);
        }
        if (delta.y < -noTurn)
        {
            transform.Rotate(-(delta.y + noTurn) * Time.deltaTime * turnSpeed, 0, 0);
        }
        if (delta.x > noTurn)
        {
            transform.Rotate(0, (delta.x - noTurn) * Time.deltaTime * turnSpeed, 0);
        }
        if (delta.x < -noTurn)
        {
            transform.Rotate(0, (delta.x + noTurn) * Time.deltaTime * turnSpeed, 0);
        }


        if (Input.GetKey(mgr.LeftRotate))
        {
            transform.RotateAround(transform.position, transform.forward, Time.deltaTime * turnSpeed);
        }

        if (Input.GetKey(mgr.RightRotate))
        {
            transform.RotateAround(transform.position, -transform.forward, Time.deltaTime * turnSpeed);
        }
    }

    void shipMovement()
    {
        currentSpeed = rb.velocity.magnitude;


        if (Input.GetKey(mgr.ForwardButton))
        {
            if (currentSpeed < NormalThrust)
            {
                rb.AddForce(transform.forward * NormalThrust * Acceleration);
                rb.angularVelocity = rb.angularVelocity * Inertia;
            }

            /*
            if (!Input.GetKey(mgr.BoostButton))
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 80, 4f * Time.deltaTime);
            */
        }


        if (Input.GetKey(mgr.LeftButton))
        {
            if (currentSpeed < NormalThrust)
            {
                rb.AddForce(-transform.right * NormalThrust * Acceleration);
                rb.angularVelocity = rb.angularVelocity * Inertia;
            }

        }

        if (Input.GetKey(mgr.RightButton))
        {
            if (currentSpeed < NormalThrust)
            {
                rb.AddForce(transform.right * NormalThrust * Acceleration);
                rb.angularVelocity = rb.angularVelocity * Inertia;
            }

        }

        if (!Input.GetKey(mgr.ForwardButton) && !Input.GetKey(mgr.LeftButton) && !Input.GetKey(mgr.RightButton))
        {
            rb.velocity = rb.velocity * Inertia;
            rb.angularVelocity = rb.angularVelocity * Inertia;
            //mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 75, 4f * Time.deltaTime);

        }

        /*
        if (Input.GetKey(mgr.BoostButton))
        {
            NormalThrust = BoostThrust;

            if (Input.GetKey(mgr.ForwardButton) || Input.GetKey(mgr.LeftButton) || Input.GetKey(mgr.RightButton))
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 100, 4f * Time.deltaTime);
        }
        else
        {
            NormalThrust = ThrustOriginal;
        }
        */
    }
}


