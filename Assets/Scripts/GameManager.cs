using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//Para controlar las vidas y puntuaciones del jugador
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Seccion de vidas en el inspector
    [Header("Vidas")]
    public int vidas = 2; //inician en 2 vidas
    public TextMeshProUGUI textoVidas; //Se muestra en pantalla

    //seccion de puntuacion 
    [Header("Puntuación")]
    public int puntos = 0; //Se inicializa en 0
    public TextMeshProUGUI textoPuntos;

    //seccion de tiempo
    [Header("Timer")]
    public float tiempoJuego = 0f; //lo que tarda el jugador(tiempo desde el inicio)
    public TextMeshProUGUI textoTiempo;

    //Donde inicia
    [Header("Spawn")]
    public Transform puntoInicial; //Donde reaparece el jugador 

    //el jugador
    private GameObject jugador;

    //antes de start, para inicializar referencias
    void Awake()
    {
        if (instance == null)//instancia de esta clase en la escena
        {
            instance = this; //se convierte en la instancia principal para ejecutar el codigo
        }
        else //si existe otra instancia
        {
            Destroy(gameObject); //destruye este objeto duplicado(para no tener multiples objetos)
        }
    }

    void Start()
    {
        //Busca el jugador  en la escena (por el tag player)
        jugador = GameObject.FindGameObjectWithTag("Player");

        //actualizar los textos de la interfaz con los valores
        ActualizarUI();
    }

    //Se ejecuta cada frame (60 veces por segundo)
    void Update()
    {
        //Incrementa el tiempo del jugador + el tiempo desde el ultimo frame
        tiempoJuego += Time.deltaTime;
        ActualizarTiempo(); //actualizar el texto en pantalla
    }

    //metodo para dar puntos al jugador
    public void SumarPuntos(int cantidad)
    {
        //suma la cantidad especificada a los puntos actuales
        puntos += cantidad;
        ActualizarUI(); //actualiza el texto 
    }

    //Cuando el jugador recibe daño
    public void PerderVida()
    {
        vidas--;
        ActualizarUI();

        if (vidas <= 0) //verifica si acaban las vidas
        {
            GameOver(); //termina el juego
        }
        else
        {
            ReiniciarPosicion(); //regresa el jugador al punto inicial
            DestruirTodosLosEnemigos(); //elimina los enemigos de la escena
            tiempoJuego = 0f; // Reiniciar el cronometro(tiempo)
        }
    }

    //El jugador gana vidas cuando mata un enemigo
    public void SumarVida()
    {
        vidas++;//suma
        ActualizarUI();//y actualiza texto
    }

    //transporta al jugador al punto del inicio
    void ReiniciarPosicion()
    {
        //obtiene  el componente character del jugador
        CharacterController controller = jugador.GetComponent<CharacterController>();
        controller.enabled = false;// desativa temporalmente (character)para cambiar la posicion
        jugador.transform.position = puntoInicial.position; //Mueve el jugador
        controller.enabled = true;//reactiva el character
    }

    //elimina todos los enemigos de la escena
    void DestruirTodosLosEnemigos()
    {
        //busca todos los enemigos en escena(tag enemigo), en un array
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        foreach (GameObject enemigo in enemigos)//recorre cada enemigo
        {
            Destroy(enemigo); //destruye ese enemigo en especifico
        }
    }

    //actualizar textos de vida y puntuacion en pantalla
    void ActualizarUI()
    {
        if (textoVidas != null) //virifica el texto de vidas(que no sea nula)
            textoVidas.text = "Vidas: " + vidas; 

        if (textoPuntos != null) //verifica que el texto de puntos (igual al de vidas)
            textoPuntos.text = "Puntos: " + puntos; //actualiza
    }

    //actualizar el cronometro
    void ActualizarTiempo()
    {
        //verifica que el texto  del tiempo  no sea nula
        if (textoTiempo != null)
        {
            int minutos = Mathf.FloorToInt(tiempoJuego / 60); //calcula los minutos
            int segundos = Mathf.FloorToInt(tiempoJuego % 60); //calcula los segundos
            textoTiempo.text = string.Format("Tiempo: {0:00}:{1:00}", minutos, segundos); //formatea el texto con el tiempo en minutos/segundos
        }
    }

    //se llama cuando el jugador pierde todas las vidas
    void GameOver()
    {
        //carga la escena "GameOver"
        PlayerPrefs.SetInt("PuntosFinal", puntos);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameOver");
    }
}
