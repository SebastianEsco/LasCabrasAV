using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksController : MonoBehaviour
{
    [Header("Configuración de ataques disponibles")]
    public List<Attack> ataquesDisponibles;

    [Header("Animación")]
    public Animator animator;

    private bool estaAtacando = false;
    private bool esperandoCombo = false;
    private Attack ataqueActual = null;

    private TipoDeAtaque? inputBuffered = null;
    private Coroutine comboCoroutine = null;

    void Update()
    {
        // Buffer de input
        if (Input.GetMouseButtonDown(0))
        {
            inputBuffered = TipoDeAtaque.Ligero;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            inputBuffered = TipoDeAtaque.Pesado;
        }

        // Solo podemos iniciar ataque si no estamos atacando ni esperando combo
        if (!estaAtacando && !esperandoCombo && inputBuffered != null)
        {
            IntentarIniciarAtaque(inputBuffered.Value);
            inputBuffered = null;
        }
    }

    void IntentarIniciarAtaque(TipoDeAtaque tipo)
    {
        // Si estamos en combo, buscar continuación
        if (ataqueActual != null)
        {
            var posibles = ataqueActual.ataquesSiguientes;
            foreach (var nombre in posibles)
            {
                var siguiente = ataquesDisponibles.Find(a => a.nombre == nombre && a.tipo == tipo);
                if (siguiente != null && CumpleRequisitos(siguiente))
                {
                    EjecutarAtaque(siguiente);
                    inputBuffered = null;
                    return;
                }
            }
        }

        // Si no hay continuación válida, buscar ataque inicial
        var inicial = ataquesDisponibles.Find(a =>
            a.tipo == tipo &&
            (a.ataquesRequeridos == null || a.ataquesRequeridos.Count == 0));

        if (inicial != null)
        {
            EjecutarAtaque(inicial);
        }
        else
        {
            Debug.Log("No se encontró ataque válido para " + tipo);
        }

        inputBuffered = null;
    }

    void EjecutarAtaque(Attack ataque)
    {
        // Cancelar corrutina previa si la hay
        if (comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }

        ataqueActual = ataque;
        estaAtacando = true;
        esperandoCombo = false;

        if (ataque.animacion != null)
        {
            animator.CrossFade(ataque.animacion.name, 0.1f);
        }

        comboCoroutine = StartCoroutine(ControlarTransicion(ataque));
    }

    IEnumerator ControlarTransicion(Attack ataque)
    {
        yield return new WaitForSeconds(ataque.animacion.length);

        estaAtacando = false;
        esperandoCombo = true;

        float tiempoCombo = ataque.tiempoParaCombo;
        float contador = 0f;

        while (contador < tiempoCombo)
        {
            if (inputBuffered != null)
            {
                IntentarIniciarAtaque(inputBuffered.Value);
                esperandoCombo = false;
                comboCoroutine = null;
                yield break;
            }

            contador += Time.deltaTime;
            yield return null;
        }

        // Tiempo agotado: volver a idle
        LimpiarEstado();
    }

    void LimpiarEstado()
    {
        ataqueActual = null;
        estaAtacando = false;
        esperandoCombo = false;
        inputBuffered = null;
        comboCoroutine = null;

        animator.CrossFade("Idle", 0.1f);
    }

    bool CumpleRequisitos(Attack ataque)
    {
        if (ataque.ataquesRequeridos == null || ataque.ataquesRequeridos.Count == 0)
            return true;

        return ataque.ataquesRequeridos.Contains(ataqueActual.nombre);
    }
}
