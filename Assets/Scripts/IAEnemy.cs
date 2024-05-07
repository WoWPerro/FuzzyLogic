using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class IAEnemy : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject Jugador;
    public GameObject BaseVida;
    public GameObject BaseBalas;
    public GameObject Pistola;
    public NavMeshAgent agent;
    public GameObject proyectil;
    public BarraVida barra;

    public FuzzyLogic fuzzyLogic;

    [Header("Variables")]
    public float vida =300;
    public float Maxvida = 300;
    public int balas = 30;
    public int Maxbalas = 30;
    public float distance;
    public float DistanceA;
    public float DistanceV;
    public Transform PosClave;
    public int damage = 15;

    [Header("Acciones")]
    public bool atacar;
    public bool recargando;
    public bool curando;
    public bool danado;


    private void Awake()
    {
        PlayerManager pl = Jugador.GetComponent<PlayerManager>();
        ActDist();
        fuzzyLogic = new FuzzyLogic(pl.Maxvida, pl.Vida, balas, distance, DistanceA, DistanceV, vida, Maxvida, Maxbalas);

        agent = GetComponent<NavMeshAgent>();

    }

    // Start is called before the first frame update
    void Start()
    {
        barra.VidaMaxima(Maxvida);
        barra.VidaActual(vida);
        
        StartCoroutine(FuzzifyUpdate());
        StartCoroutine(Shoot());

    }
    void ActDist()
    {
        DistanceA = Vector3.Distance(this.gameObject.transform.position, BaseBalas.transform.position);
        DistanceV = Vector3.Distance(this.gameObject.transform.position, BaseVida.transform.position);
        distance = Vector3.Distance(this.gameObject.transform.position, Jugador.transform.position);
    }
    private void FixedUpdate()
    {
        ActDist();
        fuzzyLogic.ActDist(distance, DistanceA, DistanceV);

        if (vida <= 0)
        {
            Debug.Log("You Win");
            barra.VidaActual(0);
            transform.gameObject.SetActive(false);
            recargando = false;
            atacar = false;
        }

    }
    void Update()
    {
        LookAtPlayer();
    }


    public IEnumerator FuzzifyUpdate()
    {
        fuzzyLogic.ActVar(balas, Jugador.GetComponent<PlayerManager>().Vida, vida);
        fuzzyLogic.Fuzzify();
        barra.VidaActual(vida);
        FuzzyBrain();
        EvaluarAtaque(fuzzyLogic.fuzzyPlayerHealth);
        yield return new WaitForSeconds(3);
        StartCoroutine(FuzzifyUpdate());
    }


    //Fuzzy Actions and Desitions
    
    public void FuzzyBrain()
    {
        if (fuzzyLogic.fuzzyAmmo<45 && !PrioVida())
        {
            switch (fuzzyLogic.DAmmo)
            {
                case FuzzyLogic.Distancia.CERCA:
                    MovAMMO();
                    break;

                case FuzzyLogic.Distancia.MEDIO:
                    if (PrioVida())
                    {
                        MovLife();
                        Debug.Log("Ammo Do Nothing");
                    }
                    else
                    {
                        MovAMMO();
                    }
                    break;

                case FuzzyLogic.Distancia.LEJOS:
                    if (fuzzyLogic.fuzzyAmmo <10 || !PrioVida())
                    {
                        MovAMMO();
                    }
                    else
                    {
                        if (PrioVida())
                        {
                            MovLife();
                        }
                        else
                        {
                            Debug.Log("Ammo Do Nothing");
                        }
                    }
                    break;

                default:
                        Debug.Log("Ammo Do Nothing");
                    break;

            }
            VelHuida(fuzzyLogic.fuzzyAmmo);
        }
        else if (Jugador.GetComponent<PlayerManager>().Vida < 1)
        {
            MovPos();
            atacar = false;
            StopAllCoroutines();
        }
        else if (PrioVida())
        {
            MovLife();
        }
        else if (fuzzyLogic.fuzzyAmmo > 40 && fuzzyLogic.fuzzyPlayerHealth <50)
        {
            EvaluarAtaque(fuzzyLogic.fuzzyPlayerHealth);
            Huir();
        }
        else
        {
            EvaluarAtaqueD(fuzzyLogic.fuzzydistancePlayer);
            Huir();
        }
    }
    bool PrioVida()
    {
        Debug.Log(fuzzyLogic.fuzzyHealth);
        if (fuzzyLogic.fuzzyHealth <25)
        {
            switch (fuzzyLogic.Vida)
            {
                case FuzzyLogic.Distancia.CERCA:
                    return true;
                case FuzzyLogic.Distancia.MEDIO:
                    return true;
                case FuzzyLogic.Distancia.LEJOS:

                    return false;
                default:
                    Debug.Log("PrioVida Do Nothing");
                    return false;
            }
        }
        else
        {
            Debug.Log("PrioVida Do Nothing");
            return false;
        }
    }

    public void VelHuida(float fuzzydata)
    {
         if (fuzzydata == 50)
         {
             agent.speed = 4.5f;
         }
         else if (fuzzydata > 50)
         {
             agent.speed = 3.5f;
         }
         else if (fuzzydata < 50)
         {
             agent.speed = 5.5f;
         }
    }

    public void MovLife()//Correr hacia la base de vida
    {
        switch (fuzzyLogic.Vida)
        {
            case FuzzyLogic.Distancia.CERCA:
        agent.SetDestination(BaseVida.transform.position);
                break;
            case FuzzyLogic.Distancia.MEDIO:
        agent.SetDestination(BaseVida.transform.position);
                break;
            case FuzzyLogic.Distancia.LEJOS:
                Debug.Log("PrioVida Do Nothing For distance");
                break;
            default:
                Debug.Log("PrioVida Do Nothing");
                break;
        }
        EvaluarAtaqueD(fuzzyLogic.fuzzydistancePlayer);
        Debug.Log(agent.destination);
    }

    public void MovAMMO() //Correr a recargar municion
    {
        agent.SetDestination(BaseBalas.transform.position);
        Debug.Log(agent.destination);
        Debug.Log("Ammo");
    }
    public void MovPos() //Correr a recargar municion
    {
        agent.SetDestination(PosClave.transform.position);
        Debug.Log(agent.destination);
        Debug.Log("Lose");
    }

    public float distanciaSegura = 5;
    public void Huir() //Alejarse del jugador
    {
        agent.SetDestination((PosClave.position - Jugador.transform.position) + transform.position);
        Debug.Log(agent.destination);
        Debug.Log("Player");
    }

    public void Locked()
    {
        curando = false;
        recargando = false;
        atacar = false;
        StopAllCoroutines();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().ResetInertiaTensor();
        agent.ResetPath();
        agent.SetDestination(new Vector3(0,0,0));
        StartCoroutine(FuzzifyUpdate());
        StartCoroutine(Shoot());

    }

    public void EvaluarAtaqueD(float evaluarDist)//Conforme a la vida del jugador ataca mas rapido o lento
    {
        switch (evaluarDist)
        {
            case float n when (n > 7 && n < 100):
                fireRate = 1.5f;
                break;
            case float n when (n > 5 && n < 7):
                fireRate = 1f;
                break;
            case float n when (n < 5):
                fireRate = .5f;
                break;
            default:
                fireRate = 1.5f;
                break;
        }
    }
    public void EvaluarAtaque(float evaluarPlayer)//Conforme a la vida del jugador ataca mas rapido o lento
    {
        switch (evaluarPlayer)
        {
            case float n when (n > 50 && n < 100):
                fireRate = 1.5f;
                break;
            case float n when (n > 25 && n < 50):
                fireRate = 1f;
                break;
            case float n when (n < 25):
                fireRate = .5f;
                break;
            default:
                fireRate = 1.5f;
                break;
        }
    }


    //Look and shoot Logic
    [Header("Variables de Disparos")]
    public float velBala = 2.5f;
    public float fireRate = 1f;

    public float rotationSpeed = 1f;
    float timeTocharge = 1.0f;

    public void LookAtPlayer()//rota para seguir al jugador
    {
        Vector3 direction = (Jugador.transform.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }


    public IEnumerator Recharge()//
    {
        if (balas < Maxbalas && recargando)
        {
            balas += 5;
            yield return new WaitForSeconds(timeTocharge);
            StartCoroutine(Recharge());
        }
        else if (balas >=30)
        {
            balas = 30;
            recargando = false;
            StopCoroutine(Recharge());

        }else
        {
            recargando = false;
            Debug.Log("Full Ammo");
            StopCoroutine(Recharge());
        }

    }

    public IEnumerator Shoot()
    {
        if (balas >0 && !recargando)
        {
            Vector3 a = new Vector3(-90, 0, 0);
            GameObject tempBala = Instantiate(proyectil, Pistola.transform.position, new Quaternion(a.x, 0, 0, 90));
            Rigidbody rb = tempBala.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * velBala * 30);
            balas--;
            fuzzyLogic.Ammo = balas;
            Destroy(tempBala, 3.75f);
        }
        else if(recargando)
        {
            Debug.Log("recargando no puede disparar");
        }
        else
        {
            Debug.Log("No Ammo");
        }
        yield return new WaitForSeconds(fireRate);
        StartCoroutine(Shoot());

    }

    IEnumerator Healing()
    {
        if (curando)
        {

        if (vida > fuzzyLogic.MaxVida)
        {
            curando = false;
            Debug.Log("Curado");
            vida = fuzzyLogic.MaxVida;
            StopCoroutine(Healing());
        }
        else
        {
            vida += 15;
            barra.VidaActual(vida);
            yield return new WaitForSeconds(fireRate);
            StartCoroutine(Healing());
        }
        }
        else
        {
            Debug.Log("No Curando");
        }


    }

    public void GetDamage(float damage)
    {
        if (danado)
        {
            Debug.Log("A1");
        }
        else
        {

        vida -= damage;
            StartCoroutine(danao());
        }
    }

    IEnumerator danao()
    {
        yield return new WaitForSeconds(fireRate);
        danado = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage")
        {
            GetDamage(Jugador.GetComponent<PlayerManager>().damage);
            barra.VidaActual(vida);
            danado = true;
        }

        if (other.tag == "Ammo")
        {
            Debug.Log("Recarganding");
            recargando = true;
            StartCoroutine(Recharge());
        }

        if (other.tag == "Health")
        {
            curando = true;
            StartCoroutine(Healing());
        }

        if (other.tag == "Locked")
        {
            Locked();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Ammo")
        {
            Debug.Log("Rec");
            recargando = false;
            StopCoroutine(Recharge());
        }

        if (other.tag == "Health")
        {
            curando = false;
            StopCoroutine(Healing());
        }

    }

}
