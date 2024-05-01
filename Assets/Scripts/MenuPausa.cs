using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour
{
   [SerializeField] private GameObject botonPausa;
   [SerializeField] private GameObject menuPausa;
   public void Pausa()
   {
        Time.timeScale = 0f;
        botonPausa.SetActive(false);
        menuPausa.SetActive(true);
   }

   public void Play()
   {
        Time.timeScale = 1f;
        botonPausa.SetActive(true);
        menuPausa.SetActive(false);
   }

   public void Quit()
   {
        Application.Quit();
        Debug.Log("Ya se cerro pa");
   }
}
