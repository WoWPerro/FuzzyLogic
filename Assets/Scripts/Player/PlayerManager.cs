using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] float vida = 100; public float Vida {  get { return vida; } }
    [SerializeField] float maxvida = 100; public float Maxvida { get { return maxvida; } }
    public bool healing= false;
    public BarraVida barra;

    [Header("Hacer Da�o")]
    [SerializeField] GameObject Arma;
    public float damage = 15;

    private void Start()
    {
        barra.VidaMaxima(maxvida);
        barra.VidaActual(vida);
    }
    private void FixedUpdate()
    {
        Logic();
    }
    public void Logic()
    {
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
    }

    public void Damage(float damage)
    {
        Debug.Log("Atacado da�o recibido: " + damage);
        vida -= damage;
        if (vida <= 0)
        {
            vida = 0;
            barra.VidaActual(vida);
            Debug.Log("GameOver"); 
            gameObject.SetActive(false);

        }
        else
        {
            barra.VidaActual(vida);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            Damage(15);
            Destroy(other.gameObject);
        }


    }

}
