using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpaceshipAI_Gun : MonoBehaviour
{
    AudioSource source;
    [SerializeField] AudioClip[] shotSounds;

    [Space]
    public GameObject[] LaserPositions;

    [SerializeField] LineRenderer[] LaserTracers;

    Camera mainCamera;

    int gunDamage = 10;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();


        LaserTracers = new LineRenderer[LaserPositions.Length];
        for (int i = 0; i < LaserPositions.Length; i++)
        {
            LaserTracers[i] = LaserPositions[i].GetComponent<LineRenderer>();
        }
    }

    public void shot(Ray ray, float attackDistance)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, attackDistance)){
            try
            {
                hit.collider.GetComponent<IDamagable>().TakeDamage(gunDamage, GetComponentInParent<SpaceshipHealth>().team);
            }
            catch { }
        }

        StartCoroutine(ShowLaser(ray, attackDistance));

        source.PlayOneShot(shotSounds[Random.Range(0, shotSounds.Length)]);
    }

    IEnumerator ShowLaser(Ray ray, float distance)
    {
        for (int i = 0; i < LaserPositions.Length; i++)
        {

            LaserTracers[i].enabled = true;
            LaserTracers[i].SetPosition(0, LaserPositions[i].transform.position);
            LaserTracers[i].SetPosition(1, ray.GetPoint(distance));

            yield return new WaitForSeconds(0.1f);

            LaserTracers[i].enabled = false;
        }
    }
}
