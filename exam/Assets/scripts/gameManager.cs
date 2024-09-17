using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public float vidas;
    public Scrollbar scrollbar;
    public static event Action perdiste;
    protected virtual void Perdiste()
    {
        perdiste?.Invoke();
    }

    private void Update()
    {
        scrollbar.size = vidas / 10;
        if (vidas <= 0)
        {
            Perdiste();
        }
    }
    private void AumentarVida()
    {
        vidas = vidas + 1;
    }
    private void DisminuirVida()
    {
        vidas = vidas - 1;
    }

    private void OnEnable()
    {
        player.actualizarVida += AumentarVida;
        player.quitarVida += DisminuirVida;
    }
    private void OnDisable()
    {
        player.actualizarVida -= AumentarVida;
        player.quitarVida -= DisminuirVida;
    }
}
