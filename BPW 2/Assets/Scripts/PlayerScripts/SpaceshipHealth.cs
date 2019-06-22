using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage, TeamComponent damagingTeam);
    void Die();
}

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SpaceshipEffects))]
public class SpaceshipHealth : MonoBehaviour, IDamagable
{
    [Space] 
    public TeamComponent team;
    public Mothership mothership;

    [Space]
    public float currentHealth;

    [Space]
    public float maxHealth;
    
    [SerializeField] GameObject explosionPrefab;

    [Space]
    [Header("Team AI Settings")]
    public int maxFollowing;    
    public int following;


    void Start()
    {
        currentHealth = maxHealth;
        GetComponent<SpaceshipEffects>().UpdateHealthbar(maxHealth, currentHealth);
        Mothership[] ships = FindObjectsOfType<Mothership>();

        for (int i = 0; i < ships.Length; i++)
        {
            if(ships[i].Team== team)
            {
                mothership = ships[i];
                mothership.FighterAmount++;
            }
        }

        
    }

    void IDamagable.TakeDamage(int damage, TeamComponent damagingTeam)
    {
        if (damagingTeam != team)
        {
            currentHealth -= damage;
            GetComponent<SpaceshipEffects>().UpdateHealthbar(maxHealth, currentHealth);
        }

        if (currentHealth <= 0)
        {
            GetComponent<IDamagable>().Die();
        }
        
    }

    void IDamagable.Die()
    {
        mothership.FighterAmount--;
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        SpaceshipAI[] ai = FindObjectsOfType<SpaceshipAI>();
        for (int i = 0; i < ai.Length; i++)
        {
            if (ai[i].possibleTargets.Contains(this.gameObject))
            {
                ai[i].possibleTargets.Remove(this.gameObject);
            }
        }

        if (GetComponent<SpaceshipAI>() != null)
        {
            Destroy(GetComponent<TargetIndicator>().m_icon.gameObject);
        }



        Destroy(this.gameObject);
        try
        {
            GetComponent<PlayerRespawn>().InitRespawn();
        }
        catch { }

        

 

    }
}

