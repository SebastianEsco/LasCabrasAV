using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int vidaMaxima = 100;
    public Animator animator;
    public string animacionMuerte = "Death";
    public string animacionIdleMuerte = "DeathIdle";

    public string animHitFront = "HitFront";
    public string animHitBack = "HitBack";
    public string animHitLeft = "HitLeft";
    public string animHitRight = "HitRight";

    public GameObject hipsRef;

    private int vidaActual;
    private bool estaMuerto = false;
    public bool IsDead => estaMuerto;
    private bool puedeRecibirDanio = true;
    
    
    public bool mostrarDañoRecibido;
    
    public CanvasGroup dañoUI;
    public Slider vidaSlider;
    public TextMeshProUGUI damageText;
   

    void Start()
    {
        vidaActual = vidaMaxima;
        if(vidaSlider != null)  vidaSlider.value = ((float)vidaActual / vidaMaxima);
    }

    

    public void DamageDone(int cantidad, Vector3 origen)
    {
        if (!puedeRecibirDanio || estaMuerto) return;

        vidaActual -= cantidad;
        if(vidaSlider != null)  vidaSlider.value = ((float)vidaActual / vidaMaxima);

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(ReaccionarAlGolpe(origen));
            if (mostrarDañoRecibido) StartCoroutine(MostrarDañoRecibido(cantidad));
        }
    }

    IEnumerator MostrarDañoRecibido(int cantidad)
    {
        damageText.text = cantidad.ToString();

        dañoUI.alpha = 1f;

        yield return new WaitForSeconds(1f);

        while (dañoUI.alpha > 0f)
        {
            dañoUI.alpha -= Time.deltaTime * 2;
            if (dañoUI.alpha < 0f)
                dañoUI.alpha = 0f;

            yield return null;
        }
    }

    System.Collections.IEnumerator ReaccionarAlGolpe(Vector3 origen)
    {
        puedeRecibirDanio = false;

        Vector3 direccionDelGolpe = (origen - hipsRef.transform.position);
        direccionDelGolpe.y = 0f; 
        direccionDelGolpe.Normalize();
        float angulo = Vector3.SignedAngle(hipsRef.transform.forward, direccionDelGolpe, Vector3.up);
        
        string animacionImpacto = SeleccionarAnimacionPorAngulo(angulo);

        animator.CrossFade(animacionImpacto, 0.1f);

        // Obtener duración de la animación de golpe
        float duracion = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == animacionImpacto)?.length ?? 0.5f;

        yield return new WaitForSeconds(duracion);

        // Volver a Idle
        animator.CrossFade("LocoMotion", 0.1f);

        puedeRecibirDanio = true;
    }

    string SeleccionarAnimacionPorAngulo(float angulo)
    {
        angulo = (angulo + 360f) % 360f;

        if (angulo >= 315f || angulo < 45f)
            return animHitFront;
        else if (angulo >= 45f && angulo < 135f)
            return animHitLeft;
        else if (angulo >= 135f && angulo < 225f)
            return animHitBack;
        else
            return animHitRight;
    }


    void Morir()
    {
        animator.SetBool("IsAlive", false);
        estaMuerto = true;
        puedeRecibirDanio = false;
        animator.CrossFade(animacionMuerte, 0.1f);
        StartCoroutine(CambiarAIdleDeMuerte());
    }

    System.Collections.IEnumerator CambiarAIdleDeMuerte()
    {
        float duracion = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == animacionMuerte)?.length ?? 1.5f;

        yield return new WaitForSeconds(duracion);
        animator.CrossFade(animacionIdleMuerte, 0.1f);
    }

    public bool EstaMuerto() => estaMuerto;

    [ContextMenu("Revive")]
    public void Revivir()
    {
        vidaActual = vidaMaxima;
        estaMuerto = false;
        puedeRecibirDanio = true;
        if(vidaSlider != null)  vidaSlider.value = ((float)vidaActual / vidaMaxima);

        // Reiniciar animación
        animator.SetBool("IsAlive", true);
    }
    
}
