using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TankHealth : MonoBehaviour
{
    //the amount of health each tank starts with
    public float m_StartingHealth = 100;

    //a prefab that will be instantiated in awake then used whenever the tank dies
    public GameObject m_ExplosionPrefab;

    private float m_CurrentHealth;
    private bool m_Dead;
    // The particle system that iwll play when the tank is destroyed
    private ParticleSystem m_ExplosionParticles;

    private void Awake()
    {
        //Instantiate the explosion prefab and get a reference to the particle system on it
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

        //Disable the prefab so it can be activated when it's required
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }

    private void SetHealthUI()
    {
        //TODO
    }

    public void TakeDamage(float amount)
    {
        // reduce current health by the amount of damage done
        m_CurrentHealth -= amount;

        //Change the ui elements appropriately
        SetHealthUI();

        //If the current health is at or below zero and it has not yet been registered, call OnDeath
        if(m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }

       
    }
    private void OnDeath()
    {
        m_Dead= true;

        //Move the instantiated explosion prefab to the tank's position and turn it on
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        //play the particle system of the tank exploding
        m_ExplosionParticles.Play();

        //turn the tank off
        gameObject.SetActive(false);
    }
}
