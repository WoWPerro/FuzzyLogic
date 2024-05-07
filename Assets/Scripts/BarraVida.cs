using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI textvalue;
    public float max;
    public float actual;
    public GameObject GameOver;


    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void VidaMaxima(float vidaMaxima)
    {
        Debug.Log(vidaMaxima);
        slider.maxValue = vidaMaxima;
        max = vidaMaxima;
    }

    public void VidaActual(float vidaActual)
    {
        Debug.Log(vidaActual);
        slider.value = vidaActual;
        actual = vidaActual;
        textvalue.text = actual + " / " + max;
        if (actual <=0)
        {

            GameOver.SetActive(true);
            MenuPausa.instance.botonPausa.SetActive(false);
            MenuPausa.instance.menuPausa.SetActive(false);

        }
    }

    public void UpdateSlider(float cantidadVida)
    {
        VidaActual(cantidadVida);
    }
}
