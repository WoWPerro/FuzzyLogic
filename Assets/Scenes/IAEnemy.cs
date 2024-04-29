using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI; 

public class IAEnemy : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject Jugador;
    public GameObject BaseVida;
    public GameObject BaseBalas;
    public GameObject Pistola;
    public NavMeshAgent agent;
    public GameObject proyectil;
    public GameObject[] cantidadBalas;

    public FuzzyLogic fuzzyLogic;

    [Header("Variables")]
    public float vida =100;
    public float Maxvida = 150;
    public int balas = 30;
    public int Maxbalas = 30;
    public float distance;
    public float DistanceA;
    public float DistanceV;
    
    public bool atacando;
    public bool recargando;
    public bool healing;

    public int prueba = 0;

    private void Awake()
    {
        PlayerManager pl = Jugador.GetComponent<PlayerManager>();
        fuzzyLogic = new FuzzyLogic(pl.Maxvida, pl.Vida, balas, distance, DistanceA, DistanceV, vida, Maxvida, Maxbalas);
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void FixedUpdate()
    {
        DistanceA = Vector3.Distance(this.gameObject.transform.position, BaseBalas.transform.position);
        DistanceV = Vector3.Distance(this.gameObject.transform.position, BaseVida.transform.position);
        distance = Vector3.Distance(this.gameObject.transform.position, Jugador.transform.position);

        if (fuzzyLogic.fuzzydistancePlayer == 50)
        {
            HuirPlayer();
            agent.speed = 3.5f;

        }
        else if (fuzzyLogic.fuzzydistancePlayer < 50)
        {

            HuirPlayer();
            agent.speed = 5.5f;
        }

    }
    void Update()
    {
        LookAtPlayer();

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    public float rotationSpeed = 1f;
    public void LookAtPlayer()
    {
        Vector3 direction = (Jugador.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    }
    float timeTocharge = 1.0f;
    float timeToNextcharge = 1.0f;
    public void recharge()
    {
        timeTocharge -= Time.deltaTime;

        if (timeTocharge < 0)
        {
            balas++;
            timeTocharge = timeToNextcharge;
        }


    }
    public float velBala = 1;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    public void Shoot() 
    {
        Vector3 a = new Vector3(-90, 0, 0);
        GameObject tempBala = Instantiate(proyectil, Pistola.transform.position, new Quaternion(a.x,0,0,90));
        Rigidbody rb = tempBala.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * velBala*10);
        balas--;
//        fuzzyLogic.Ammo = balas;
        fuzzyLogic.Fuzzify();
        Destroy(tempBala,2.75f);

    }

    public void HuirVida()
    {
        agent.destination = BaseVida.transform.position;
    }

    public void HuirArmor()
    {
        agent.destination = BaseBalas.transform.position;
    }
    public void HuirPlayer()
    {
        agent.SetDestination(Jugador.transform.position * -1 );
    }
   
}
