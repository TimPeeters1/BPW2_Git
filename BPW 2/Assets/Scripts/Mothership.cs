using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mothership : MonoBehaviour, IDamagable
{
    #region Serialized Variables
    [SerializeField] float speed;
    [SerializeField] float smoothing;

    [SerializeField] float maxDistance;

    [Space]
    [Header("Health Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;

    [SerializeField] Transform endCamPosition;
    [SerializeField] GameObject endCameraPrefab;
    [SerializeField] GameObject deathParticle;

    [Space]
    public TeamComponent Team;
    public float FighterAmount;

    [Space]
    [Header("UI Settings")]
    [SerializeField] Text FighterInfo;
    [SerializeField] Text MothershipInfo;

    [Space]
    [Header("Spawn Settings")]
    [SerializeField] GameObject fighterPrefab;
    [SerializeField] int maxSpawnSize;
    [SerializeField] GameObject spawnPosition;
    [SerializeField] float spawnTime;
    #endregion

    private Vector3 randomPoint;
    private float distanceToPoint;
    private bool hasDied = false;
 
    private void Start()
    {
        currentHealth = maxHealth;

        MothershipInfo.text = "Mothership HP: " + currentHealth + "/" + maxHealth;
        FighterInfo.text = "Fighters: " + FighterAmount;

        GeneratePosition();

        StartCoroutine(SpawnFighters());
    }

    private void Update()
    {
        FighterInfo.text = "Fighters: " + FighterAmount;
        //RandomPosition();
    }
    void RandomPosition()
    {
        distanceToPoint = Vector3.Distance(randomPoint, transform.position);

        Quaternion targetRotation = Quaternion.LookRotation(randomPoint, transform.up);
        transform.position = Vector3.MoveTowards(transform.position, randomPoint, (speed / 5) * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothing);
        //transform.LookAt(randomPoint);

        if (distanceToPoint < 10)
        {
            GeneratePosition();
        }
    }

    void GeneratePosition()
    {
        //randomPoint = transform.position + Random.insideUnitSphere * maxDistance;
        //randomPoint = new Vector3(randomPoint.x, Random.Range(0, 50) + transform.position.y, randomPoint.z);

        randomPoint = transform.position + transform.forward;
    }

    void IDamagable.TakeDamage(int damage, TeamComponent damagingTeam)
    {
        if(damage <= 0 || (currentHealth-=damage) <= 0) {
            if(!hasDied)
            GetComponent<IDamagable>().Die();
        }
        else {
            if(damagingTeam != this.Team)
            currentHealth -= damage;
        }
   
        MothershipInfo.text = "Mothership HP: " + currentHealth + "/" + maxHealth;
    }
 
    void IDamagable.Die()
    {
        hasDied = true;
        Instantiate(deathParticle, transform.position, transform.rotation);
        GameObject endCam = Instantiate(endCameraPrefab, endCamPosition.position, endCamPosition.rotation);

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<SpaceshipHealth>().team != Team)
        {
            endCam.GetComponent<EndScreen>().hasLost = false;
        }
        else
        {
            endCam.GetComponent<EndScreen>().hasLost = true;
        }

        Destroy(this.gameObject, 3);
    }

    IEnumerator SpawnFighters()
    {
        yield return new WaitForSeconds(Random.Range(30, spawnTime));

        for (int i = 0; i < Random.Range(maxSpawnSize/4, maxSpawnSize); i++)
        {
            Instantiate(fighterPrefab , spawnPosition.transform.position + (Random.insideUnitSphere * 50), spawnPosition.transform.rotation);
        }

        StartCoroutine(SpawnFighters());
    }

}
