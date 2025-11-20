using UnityEngine;
using System.Collections;

public class GeneradorEnemigos : MonoBehaviour
{
    [Header("Prefabs de enemigos")]
    public GameObject[] prefabsEnemigos; // Array con los 2 tipos

    [Header("Puntos de spawn")]
    public Transform[] puntosSpawn;

    [Header("Configuración de dificultad")]
    public float tiempoEntreSpawnsInicial = 5f;
    public float tiempoMinimoEntreSpawns = 1f;
    public float incrementoVelocidadEnemigos = 1f;
    public float tiempoParaAumentarDificultad = 30f;

    private float tiempoActualEntreSpawns;
    private float multiplicadorVelocidad = 1f;

    void Start()
    {
        tiempoActualEntreSpawns = tiempoEntreSpawnsInicial;
        StartCoroutine(GenerarEnemigos());
        StartCoroutine(AumentarDificultad());
    }

    IEnumerator GenerarEnemigos()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoActualEntreSpawns);

            // Elegir tipo de enemigo aleatorio
            int tipoEnemigo = Random.Range(0, prefabsEnemigos.Length);

            // Elegir punto de spawn aleatorio
            int puntoAleatorio = Random.Range(0, puntosSpawn.Length);

            // Crear enemigo
            GameObject enemigo = Instantiate(
                prefabsEnemigos[tipoEnemigo],
                puntosSpawn[puntoAleatorio].position,
                Quaternion.identity
            );

            // Aplicar velocidad aumentada
            EnemigoSeguidor script = enemigo.GetComponent<EnemigoSeguidor>();
            if (script != null)
            {
                script.multiplicadorInicial=multiplicadorVelocidad;
            }
        }
    }

    IEnumerator AumentarDificultad()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoParaAumentarDificultad);

            // Reducir tiempo entre spawns
            tiempoActualEntreSpawns = Mathf.Max(
                tiempoActualEntreSpawns - 0.5f,
                tiempoMinimoEntreSpawns
            );

            // Aumentar velocidad de enemigos
            multiplicadorVelocidad += 0.2f;

            Debug.Log("¡Dificultad aumentada! Spawn: " + tiempoActualEntreSpawns + "s, Velocidad: x" + multiplicadorVelocidad);
        }
    }
}
