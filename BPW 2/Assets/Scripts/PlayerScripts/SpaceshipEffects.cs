using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SpaceshipEffects : MonoBehaviour
{
    [SerializeField] bool isAI;

    [Space]
    [Header("Crosshair Settings")]
    [SerializeField] RectTransform Crosshair;

    [Space]
    [Header("Health UI")]
    [SerializeField] Image healthBar;

    [Space]
    [Header("Particle Systems")]
    [SerializeField] ParticleSystem Booster;
    [SerializeField] ParticleSystem SpaceParticles;
    [SerializeField] float BoosterMaxSize;
    [SerializeField] float ParticleMaxSize;

    [Space]
    [Header("Camera FOV Effect")]
    [SerializeField] float NormalFov;
    [SerializeField] float FlightFov;
    [SerializeField] float BoostFov;

    Camera mainCamera;
    CinemachineVirtualCamera vCam;
    SpaceshipController controller;

    void OnEnable()
    {
            InputManager.UpdateEffects += UpdateUI;
            InputManager.UpdateEffects += BoosterEffect;
            InputManager.UpdateEffects += ParticleEffect;
            InputManager.UpdateEffects += CameraEffect;
    }

    void OnDisable()
    {
            InputManager.UpdateEffects -= UpdateUI;
            InputManager.UpdateEffects -= BoosterEffect;
            InputManager.UpdateEffects -= ParticleEffect;
            InputManager.UpdateEffects -= CameraEffect;

    }

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        vCam = mainCamera.GetComponent<CinemachineVirtualCamera>();


        if (!isAI)
        {
            controller = GetComponent<SpaceshipController>();
            Crosshair = GameObject.FindGameObjectWithTag("Crosshair").GetComponent<RectTransform>();
            healthBar = GameObject.FindGameObjectWithTag("Healthbar").GetComponent<Image>();
        }
    }

    void UpdateUI()
    {
        if (!isAI)
        {
            Crosshair.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    } 

    void BoosterEffect()
    {
        if (!isAI)
        {
            var main = Booster.main;
            main.startSpeed = Mathf.Lerp( 0, BoosterMaxSize, controller.currentSpeed / 50f);
        }
    }
    
    void ParticleEffect()
    {
            if (!isAI)
            {
                var main = SpaceParticles.main;
                main.startSpeed = Mathf.Lerp( 0, ParticleMaxSize, controller.currentSpeed);
            }
    }

    void CameraEffect()
    {
        if (!isAI)
        {
            
            if (!Input.GetButton("Fire3"))
            {
                vCam.m_Lens.FieldOfView = Mathf.Lerp(NormalFov, FlightFov, controller.currentSpeed / 100f);
            }
            else
            {
                //vCam.m_Lens.FieldOfView = Mathf.Lerp(FlightFov, BoostFov, controller.currentSpeed / 100f);
            }
            
        }
    }

    public void UpdateHealthbar(float maxHealth, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth < maxHealth / 4)
        {
            healthBar.color = Color.red;
        }
        else
        {
            healthBar.color = Color.green;
        }
    }
}
