using UnityEngine;
using UnityEngine.AI;

public class EnemigoSeguidor : MonoBehaviour
{
    public Transform jugador;
    public float velocidadBase = 3f;
    private NavMeshAgent agente;
    private float velocidadActual;
    public float multiplicadorInicial = 1f;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        agente = GetComponent<NavMeshAgent>();
        velocidadActual = velocidadBase*multiplicadorInicial;
        agente.speed = velocidadActual;
    }

    void Update()
    {
        if (jugador != null)
        {
            agente.SetDestination(jugador.position);
        }
    }

    public void AumentarVelocidad(float multiplicador)
    {
        velocidadActual = velocidadBase * multiplicador;
        agente.speed = velocidadActual;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PerderVida();
            Destroy(gameObject);
        }
    }
}
