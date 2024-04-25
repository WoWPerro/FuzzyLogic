using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] float vida = 100;
    [SerializeField] float maxvida = 100;
    [SerializeField] float timeToHeal = 5f;
    public bool healing= false;

    [Header("Recibe Daño")]
    [SerializeField] float timeToDamage = 1.5f;
    [SerializeField] float timeCurrent = 1.5f;
    public bool damn = false;

    [Header("Hacer Daño")]
    [SerializeField] float damage = 15;
    [SerializeField] float timeCooldown = 2f;
    [SerializeField] float timeCurrentt = 2f;
    public bool attacked;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }

        Logic();
    }

    public void Logic()
    {
        if (attacked)
        {
            timeCurrentt -= Time.deltaTime;
            if (timeCurrentt < 0)
            {
                attacked = false;
                timeCurrentt = timeCooldown;
            }
        }

        if (damn)
        {
            timeCurrent -= Time.deltaTime;
            if (timeCurrent <= 0)
            {
                damn = false;
                timeCurrent = timeToDamage;

            }
        }


        if (healing)
        {
            if (vida < maxvida)
            {
                vida += 0.5f * Time.deltaTime;
            }
            if (vida >= maxvida)
            {
                vida = maxvida;
                healing = false;
            }

        }
        else
        {
            
        }
    }

    float cooldownNoDamage = 0;
    IEnumerator checkHeal()
    {
        if (!damn)
        {
            cooldownNoDamage = timeToHeal;
            cooldownNoDamage -= Time.deltaTime;

            if (cooldownNoDamage <= 0)
            {
                healing = true;
            }
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(checkHeal());
    }
    public void attack()
    {
        Debug.Log("Atacado daño hecho: " + damage);
        attacked = true;
    }

    public void Damage(float damage)
    {
        Debug.Log("Atacado daño recibido: " + damage);
        vida -= damage;

        damn = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Damage(15);
        }

        if(other.tag == "Health")
        {
            healing = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Health")
        {
            healing = false;
        }
    }
}
