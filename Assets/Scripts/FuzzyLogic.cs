 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic
{
    //Variables
    float maxplayerHealth;
    float playerHealth; 
    int MaxAmmo;
    int ammo; 
    float distancePlayer; 
    float distanceAmmo; 
    float distanceHealth; 
    float health; 
    float MaxHealth;

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
        MaxAmmo = _MaxAmmo;
        distancePlayer =_distancePlayer;
        distanceAmmo = _distanceAmmo;
        distanceHealth = _distanceHealth;
        health = _health;
        MaxHealth = _MaxHealth;

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
        fuzzyAmmo = FuzzifyV(ammo,MaxAmmo);
        fuzzyPlayerHealth = FuzzifyV(playerHealth,maxplayerHealth);
        fuzzyHealth = FuzzifyV(health, MaxHealth);

        //Distancias
        fuzzydistancePlayer = FuzzyfyD(distancePlayer);
        fuzzydistanceAmmo = FuzzyfyD(distanceAmmo);
        fuzzydistanceHealth = FuzzyfyD(distanceHealth);

    }
    //rangos
    float FuzzyfyD(float distance)
    {

        //Debug.Log(distance);
        if (distance < 5) { return 100; }// Cerca
        else if (distance >= 3 && distance < 10) { return 50; }// Intermedio
        else { return 0; }// Lejos
        
    }
    //regla de 3
    float FuzzifyV(float a, float maxValue)
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
