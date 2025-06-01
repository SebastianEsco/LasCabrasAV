using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthSystem : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 100;
    private int vidaActual;

    [Header("Animaciones")]
    public Animator animator;
    public string animacionMuerte = "Death";
    public string animacionIdleMuerte = "DeathIdle";
    public string animHitLight = "HitLight";
    public string animHitHeavy = "HitHeavy";

    [Header("Daño")]
    public float umbralDanioFuerte = 30f;
    public bool estaMuerto = false;
    public bool IsDead => estaMuerto;
    private bool puedeRecibirDanio = true;

    [Header("UI")]
    public Slider vidaSlider;
    
    public bool mostrarDañoRecibido;
    
    public CanvasGroup dañoUI;
    public TextMeshProUGUI damageText;

    void Start()
    {
        vidaActual = vidaMaxima;
        if (vidaSlider != null) vidaSlider.value = (float)vidaActual / vidaMaxima;
    }

    public void DamageDone(int cantidad)
    {
        
        if (!puedeRecibirDanio || estaMuerto) return;

        vidaActual -= cantidad;
        if (vidaSlider != null) vidaSlider.value = (float)vidaActual / vidaMaxima;

        if (vidaActual <= 0)
        {
            Morir();
        }
        else
        {
            StartCoroutine(ReaccionarAlGolpe(cantidad));
            if (mostrarDañoRecibido) StartCoroutine(MostrarDañoRecibido(cantidad));
        }
    }

    IEnumerator ReaccionarAlGolpe(int cantidad)
    {
        string animacionGolpe = cantidad >= umbralDanioFuerte ? animHitHeavy : animHitLight;
        animator.CrossFade(animacionGolpe, 0.1f);

        float duracion = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == animacionGolpe)?.length ?? 0.5f;

        yield return new WaitForSeconds(duracion);
        puedeRecibirDanio = true;
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

    void Morir()
    {
        Destroy(vidaSlider.gameObject);
        animator.SetBool("IsAlive", false);
        estaMuerto = true;
        puedeRecibirDanio = false;
        animator.CrossFade(animacionMuerte, 0.1f);
        StartCoroutine(CambiarAIdleDeMuerte());
    }

    IEnumerator CambiarAIdleDeMuerte()
    {
        float duracion = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(c => c.name == animacionMuerte)?.length ?? 1.5f;

        yield return new WaitForSeconds(duracion);
        animator.CrossFade(animacionIdleMuerte, 0.1f);
    }

    [ContextMenu("Revive")]
    public void Revivir()
    {
        vidaActual = vidaMaxima;
        estaMuerto = false;
        puedeRecibirDanio = true;
        if (vidaSlider != null) vidaSlider.value = (float)vidaActual / vidaMaxima;
        animator.SetBool("IsAlive", true);
    }

    public bool EstaMuerto() => estaMuerto;
}
