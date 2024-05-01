using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void VidaMaxima(float vidaMaxima)
    {
        slider.maxValue = vidaMaxima;
    }

    public void VidaActual(float vidaActual)
    {
        slider.value = vidaActual;
    }

    public void IniciarVida(float cantidadVida)
    {
        VidaMaxima(cantidadVida);
        VidaActual(cantidadVida);
    }
}
