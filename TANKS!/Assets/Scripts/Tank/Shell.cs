using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    // The time in seconds before the shell is removed
    public float m_MaxLifeTime = 2f;
    // The amount of damage done if the explosion is centred on a tank
    public float m_MaxDamage = 34f;
    // The maximum distance away from the explosion tanks can be and are still affected
    public float m_ExplosionRadius = 5;
    // The amount of force added to a tank at the centre of the explosion
    public float m_ExplosionForce = 100f;

    // Reference to the particles that will play on explosion
    public ParticleSystem m_ExplosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // find the rigidbody of the collision object
        Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();
        // only tanks will have rigidbody scripts
        if (targetRigidbody != null)
        {
            // Add an explosion force
            targetRigidbody.AddExplosionForce(m_ExplosionForce,
           transform.position, m_ExplosionRadius);
            // find the TankHealth script associated with the rigidbody
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            if (targetHealth != null)
            {
                // Calculate the amount of damage the target should take
                // based on it's distance from the shell.
                float damage = CalculateDamage(targetRigidbody.position);
                // Deal this damage to the tank
                targetHealth.TakeDamage(damage);
            }
        }
        // Unparent the particles from the shell
        m_ExplosionParticles.transform.parent = null;
        // Play the particle system
        m_ExplosionParticles.Play();
        // Once the particles have finished, destroy the gameObject they are on
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        // Destroy the shell
        Destroy(gameObject);
    }

    private float CalculateDamage (Vector3 targetPosition)
    {
        //Create a vector from the shell to the target
        Vector3 explosionToTarget = targetPosition - transform.position;    

        //Calculate the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;

        //Calculate the proportion of the maximum distance (the explosion radius) the target is away
        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        //Calculate damage as this proportion of the maximum possible damage
        float damage = relativeDistance * m_MaxDamage;

        //make sure the minimum damage is always 0
        damage = Mathf.Max(0,damage);

        return damage;
    }
}
