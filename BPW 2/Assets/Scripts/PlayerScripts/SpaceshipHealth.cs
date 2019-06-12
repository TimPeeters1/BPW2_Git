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

    [Space]
    public float currentHealth;

    [Space]
    public float maxHealth;
    
    [SerializeField] GameObject explosionPrefab;

    void Start()
    {
        currentHealth = maxHealth;
        GetComponent<SpaceshipEffects>().UpdateHealthbar(maxHealth, currentHealth);
        
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
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        if (GetComponent<SpaceshipAI>() != null)
        {
            Destroy(GetComponent<TargetIndicator>().m_icon.gameObject);
        }

        Destroy(this.gameObject);
    }
}

