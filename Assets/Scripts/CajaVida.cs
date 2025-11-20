using UnityEngine;

public class CajaVida : MonoBehaviour
{
    public Transform[] posicionesCaja; // Diferentes posiciones donde puede aparecer

    void Start()
    {
        CambiarPosicion();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.SumarVida();
            CambiarPosicion();
        }
    }

    void CambiarPosicion()
    {
        if (posicionesCaja.Length > 0)
        {
            int indiceAleatorio = Random.Range(0, posicionesCaja.Length);
            transform.position = posicionesCaja[indiceAleatorio].position;
        }
    }

    void Update()
    {
        // Rotación para que sea más visible
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
}