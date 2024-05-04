 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic
{
    //Variables
    float maxplayerHealth;
    float playerHealth; 
    int maxAmmo; public int MaxAmmo { get { return maxAmmo; } set { maxAmmo = value; } }
    int ammo; public int Ammo { get { return ammo; } set { ammo = value; } }
    float distancePlayer; 
    float distanceAmmo; 
    float distanceHealth; 
    float health; 
    float maxHealth; public float MaxVida { get { return maxHealth; } set { maxHealth = value; } }
    public enum Distancia { CERCA, MEDIO, LEJOS }
    public Distancia DAmmo;
    public Distancia Vida;
    public Distancia Player;

    //FUZZY VARIABLES
    public float fuzzyPlayerHealth;
    public float fuzzyHealth;
    public float fuzzyAmmo;
    public float fuzzydistancePlayer;
    public float fuzzydistanceAmmo;
    public float fuzzydistanceHealth;


    public FuzzyLogic(float _MaxplayerHealth, float _playerHealth, int _ammo, float _distancePlayer,
        float _distanceAmmo , float _distanceHealth, float _health, float _MaxHealth, int _MaxAmmo)
    {
        maxplayerHealth = _MaxplayerHealth;
        playerHealth = _playerHealth;
        ammo = _ammo;
        maxAmmo = _MaxAmmo;
        distancePlayer =_distancePlayer;
        distanceAmmo = _distanceAmmo;
        distanceHealth = _distanceHealth;
        health = _health;
        maxHealth = _MaxHealth;

    }
    public void ActVar(int _ammo, float _vidaPlayer, float _vida)
    {
        ammo = _ammo;
        playerHealth = _vidaPlayer;
        health = _vida;
    }
    public void ActDist(float _distancePlayer, float _distanceAmmo, float _distanceHealth)
    {
        distancePlayer = _distancePlayer;
        distanceAmmo = _distanceAmmo;
        distanceHealth = _distanceHealth;
    }
    public void Fuzzify()
    {
        //Variables
        fuzzyAmmo = FuzzifyV(ammo,maxAmmo);
        fuzzyPlayerHealth = FuzzifyV(playerHealth,maxplayerHealth);
        fuzzyHealth = FuzzifyV(health, maxHealth);
        //Distancias
        fuzzydistancePlayer = FuzzyfyD(distancePlayer);
        fuzzydistanceAmmo = FuzzyfyD(distanceAmmo);
        fuzzydistanceHealth = FuzzyfyD(distanceHealth);

        Player = distancia(fuzzydistancePlayer);
        DAmmo = distancia(fuzzydistanceAmmo);
        Vida = distancia(fuzzydistanceHealth);
    }
    //rangos (Que tan cerca)
    float FuzzyfyD(float distance)
    {

        //Debug.Log(distance);
        if (distance < 5) { return 100; }// 100 = Cerca
        else if (distance >= 3 && distance < 10) { return 50; }// 50 = Intermedio
        else { return 0; }// Lejos = 0


    }

    Distancia distancia(float distEv)
    {
        if (distEv < 50) {
        return Distancia.CERCA;
        }else if (distEv > 50) {
        return Distancia.CERCA;
        }
        else
        {
        return Distancia.MEDIO;
        }
    }

    //regla de 3
    float FuzzifyV(float a, float maxValue) // rangos de 0 - 100
    {
        return (a*100)/ maxValue;
    }

    void SortList(List<float> list)
    {
        List<float> sorted = new List<float>();
        sorted.Add(list[0]);
        list.RemoveAt(0);
        int i = 0;
        foreach (float item in list)
        {
            if (sorted[0]>item)
            {
                sorted.Add(item);
                list.Remove(item);
            }
            i++;
        }
    }
}
