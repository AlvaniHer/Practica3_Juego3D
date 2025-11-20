using UnityEngine;

public class Bala : MonoBehaviour
{
    public float fuerzaDisparo = 30f;
    public int danio = 1; // Número de hits para matar

    void Start()
    {
        // Aplicar fuerza hacia adelante
        GetComponent<Rigidbody>().AddForce(transform.forward * fuerzaDisparo, ForceMode.Impulse);

        // Destruir bala después de 3 segundos
        Destroy(gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            // Dar puntos
            GameManager.instance.SumarPuntos(10);

            // Destruir enemigo
            Destroy(collision.gameObject);
        }

        // Destruir bala
        Destroy(gameObject);
    }
}

