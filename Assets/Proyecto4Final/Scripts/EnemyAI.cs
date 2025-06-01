using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EstadoEnemigo { Patrullando, Persiguiendo, Atacando, Quieto }
    private EstadoEnemigo estadoActual;

    [Header("Componentes")]
    public NavMeshAgent agente;
    public Animator animator;
    public Transform jugador;
    public EnemyHealthSystem enemyHealth;
    private string tagJugador = "Player";

    [Header("Velocidades")]
    public float velocidadPatrulla = 2f;
    public float velocidadPersecucion = 4f;

    [Header("Rangos")]
    public float rangoVision = 10f;
    public float rangoAtaque = 2f;

    [Header("Puntos de patrulla")]
    public Transform[] puntosPatrulla;
    private int indicePatrullaActual = 0;

    [Header("Ataque")]
    public float tiempoEntreAtaques = 1.5f;
    private float tiempoProximoAtaque = 0f;

    private void Start()
    {
        BuscarJugador();

        if (!enemyHealth) enemyHealth = GetComponent<EnemyHealthSystem>();

        agente.stoppingDistance = rangoAtaque * 0.9f;
        agente.updateRotation = false; // rotación manual
        agente.acceleration = 4f;
        agente.angularSpeed = 720f;
        agente.stoppingDistance = rangoAtaque * 0.95f;
        agente.autoBraking = true;

        if (puntosPatrulla.Length > 0)
        {
            estadoActual = EstadoEnemigo.Patrullando;
            agente.speed = velocidadPatrulla;
            agente.SetDestination(puntosPatrulla[indicePatrullaActual].position);
        }
        else
        {
            estadoActual = EstadoEnemigo.Quieto;
        }
    }

    private void Update()
    {
        if (!enemyHealth || enemyHealth.estaMuerto)
        {
            agente.ResetPath();
            animator.SetFloat("Velocidad", 0f);
            return;
        }

        if (jugador == null)
            BuscarJugador();

        if (jugador == null)
        {
            animator.SetFloat("Velocidad", 0f);
            return;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (estadoActual != EstadoEnemigo.Quieto)
        {
            if (distancia <= rangoAtaque)
                CambiarEstado(EstadoEnemigo.Atacando);
            else if (distancia <= rangoVision)
                CambiarEstado(EstadoEnemigo.Persiguiendo);
            else
                CambiarEstado(EstadoEnemigo.Patrullando);
        }

        EjecutarEstado();

        animator.SetFloat("Velocidad", agente.velocity.magnitude);
        RotarHaciaMovimiento();
        
        if (estadoActual == EstadoEnemigo.Atacando && agente.hasPath)
        {
            agente.ResetPath();
        }
    }

    void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tagJugador);
        if (obj != null)
            jugador = obj.transform;
    }

    void EjecutarEstado()
    {
        switch (estadoActual)
        {
            case EstadoEnemigo.Patrullando:
                Patrullar();
                break;
            case EstadoEnemigo.Persiguiendo:
                Perseguir();
                break;
            case EstadoEnemigo.Atacando:
                Atacar();
                break;
        }
    }

    void Patrullar()
    {
        agente.speed = velocidadPatrulla;

        // Si no tiene un destino válido, establecer uno
        if (!agente.hasPath || agente.remainingDistance < 0.5f)
        {
            indicePatrullaActual = (indicePatrullaActual + 1) % puntosPatrulla.Length;
            agente.SetDestination(puntosPatrulla[indicePatrullaActual].position);
        }
    }


    void Perseguir()
    {
        agente.speed = velocidadPersecucion;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia > rangoAtaque * 0.95f) // más conservador
        {
            if (!agente.hasPath || agente.destination != jugador.position)
                agente.SetDestination(jugador.position);
        }
        else
        {
            // Estamos muy cerca, detener para evitar clipeo
            if (agente.hasPath)
                agente.ResetPath();
        }
    }
    
    public void ReiniciarTiempoAtaque()
    {
        tiempoProximoAtaque = Time.time + tiempoEntreAtaques;
    }



    void Atacar()
    {
        agente.ResetPath(); // forzar detención

        // Solo rotación horizontal hacia jugador
        Vector3 direccion = jugador.position - transform.position;
        direccion.y = 0f;

        if (direccion.sqrMagnitude > 0.01f)
        {
            Quaternion rotacion = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5f);
        }

        if (Time.time >= tiempoProximoAtaque)
        {
            tiempoProximoAtaque = Time.time + tiempoEntreAtaques;
            animator.SetTrigger("Atacar");
        }
    }


    void RotarHaciaMovimiento()
    {
        Vector3 velocidad = agente.velocity;
        if (velocidad.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(velocidad.normalized);
            rot = Quaternion.Euler(0f, rot.eulerAngles.y, 0f); // evita rotar en X y Z
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10f);
        }
    }

    void CambiarEstado(EstadoEnemigo nuevoEstado)
    {
        estadoActual = nuevoEstado;
    }
}
