using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SpaceshipGun : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip[] shotSounds;

    [Space]
    [SerializeField]
    GameObject[] LaserPositions;

    [SerializeField]LineRenderer[] LaserTracers;

    Camera mainCamera;

    [SerializeField] int gunDamage = 10;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        source = GetComponent<AudioSource>();

        LaserTracers = new LineRenderer[LaserPositions.Length];
        for (int i = 0; i < LaserPositions.Length; i++)
        {
            LaserTracers[i] = LaserPositions[i].GetComponent<LineRenderer>();
        }
    }

    private void OnEnable()
    {
        InputManager.HandleShooting += Shot;
    }

    private void OnDisable()
    {
        InputManager.HandleShooting -= Shot;
    }

    void Shot()
    {
        Ray camRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawLine(camRay.origin, camRay.GetPoint(FindObjectOfType<InputManager>().LaserRange), Color.yellow);
        for (int i = 0; i < LaserPositions.Length; i++)
        {
                Ray weaponRay = new Ray(LaserPositions[i].transform.position, camRay.GetPoint(FindObjectOfType<InputManager>().LaserRange) - LaserPositions[i].transform.position);

                StartCoroutine(ShowLaser(weaponRay));
        }

        if (Physics.Raycast(camRay, out hit))
        {
            try
            {
                if (hit.collider.GetComponent<IDamagable>() != null)
                {
                    hit.collider.GetComponent<IDamagable>().TakeDamage(gunDamage, GetComponentInParent<SpaceshipHealth>().team);
                }
                else
                {
                    hit.collider.GetComponentInParent<IDamagable>().TakeDamage(gunDamage, GetComponentInParent<SpaceshipHealth>().team);
                }
            }
            catch { }
        }

        source.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Length)]);
    }

    IEnumerator ShowLaser(Ray ray)
    {
        for (int i = 0; i < LaserPositions.Length; i++)
        {
            
            LaserTracers[i].enabled = true;
            LaserTracers[i].SetPosition(0, LaserPositions[i].transform.position);
            LaserTracers[i].SetPosition(1, ray.GetPoint(FindObjectOfType<InputManager>().LaserRange));

            yield return new WaitForSeconds(0.02f);

            LaserTracers[i].enabled = false;

        }
    }
}
