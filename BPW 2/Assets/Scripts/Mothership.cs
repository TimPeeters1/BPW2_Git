using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mothership : MonoBehaviour, IDamagable
{

  

    [SerializeField] float speed;
    [SerializeField] float smoothing;

    [SerializeField] float maxDistance;

    [Space]
    [Header("Health Settings")]
    [SerializeField] float maxHealth;
    float currentHealth;

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


    Vector3 randomPoint;
    float distanceToPoint;
 
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
        currentHealth -= damage;

        MothershipInfo.text = "Mothership HP: " + currentHealth + "/" + maxHealth;
    }

    void IDamagable.Die()
    {
        
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
