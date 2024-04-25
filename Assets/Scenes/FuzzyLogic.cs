 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic
{
    //Variables
    float MaxplayerHealth;
    float playerHealth;
    int ammo;
    int MaxAmmo;
    float distancePlayer;
    float distanceAmmo;
    float distanceHealth;
    float health;
    float MaxHealth;

    //enum TAGS {NULL, CERCA, LEJOS, INTERMEDIO}
    //TAGS distanceAmo, distancePlay, distanceHeal;

    //FUZZY VARIABLES
    float fuzzyPlayerHealth;
    float fuzzyHealth;
    float fuzzyAmmo;
    float fuzzydistancePlayer;
    float fuzzydistanceAmmo;
    float fuzzydistanceHealth;


    public FuzzyLogic(float _MaxplayerHealth, float _playerHealth, int _ammo, float _distancePlayer,
        float _distanceAmmo , float _distanceHealth, float _health, float _MaxHealth, int _MaxAmmo)
    {
        MaxplayerHealth = _MaxplayerHealth;
        playerHealth = _playerHealth;
        ammo = _ammo;
        MaxAmmo = _MaxAmmo;
        distancePlayer =_distancePlayer;
        distanceAmmo = _distanceAmmo;
        distanceHealth = _distanceHealth;
        health = _health;
        MaxHealth = _MaxHealth;

        //distanceAmo = TAGS.NULL;
        //distanceHeal = TAGS.NULL;
        //distancePlay = TAGS.NULL;


    }

    public void Fuzzify()
    {
        fuzzyAmmo = (ammo * 100) / MaxAmmo;
        fuzzyPlayerHealth = (playerHealth * 100) / MaxplayerHealth;
        fuzzyHealth = (health * 100) / MaxHealth;

       

        /*if( distanceAmmo < distanceHealth && distanceAmmo < distancePlayer)
        {
            distanceAmo = TAGS.CERCA;

            if (distanceHealth > distanceAmmo && distanceHealth < distancePlayer)
            {
                distanceHeal = TAGS.INTERMEDIO;
                distancePlay = TAGS.LEJOS;
            }
        }*/


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
