using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoJugador : MonoBehaviour
{
    //variables publicas (visibles en el inspector)
    [Header("Movimiento")] //seccion en el inspector llamada movimiento
    public float velocidad = 5f; //velocidad al que se mueva el jugador 
    public float sensibilidadRaton = 2f; //sencibilidad de la camara al mover el raton

    [Header("Salto")] //seccion salto(inspector)
    public float fuerzaSalto = 8f; //fuerza con la que el jugador salta
    public float gravedad = -20f; //gravedad que afecta al jugador (negativa=hacia abajo)

    [Header("Límites cámara")] // seccion en el inspector
    public float limiteVertical = 80f; //angulo maximo que la camara puede mirar arriba/abajo (en grados) 

    //variables privadas  (uso dentro del script)
    private CharacterController controller; //referencia al componente CharacterController (maneja colisiones y movimiento)
    private Camera camaraJugador; //referencia a la camara del jugador 
    private Vector2 movimientoInput; //almacena el input del movimiento
    private Vector2 miradaInput;//almacena el input de la mirada (raton o joystick)
    private float rotacionVertical = 0f;//angulo actual de rotacion vertical de la camara
    private float velocidadVertical = 0f; //velocidad vertical actual (para gravedad y salto)
    private bool saltando = false;//Indica si el jugador esta intentado saltar

    //Se ejecuta una vez al inicio, cuando el objeto se activa
    void Start()
    {
        //obtiene el componente CharacterController adjunto al mismo GameObject
        controller = GetComponent<CharacterController>();
        //Busca el componente Camara en los hijos del GameObject(la camara hija)
        camaraJugador = GetComponentInChildren<Camera>();
        //Bloquea el cursor en el centro de la pantalla (no se puede mover)

        // Bloquear y ocultar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //se ejecuta cada frame (60 veces por segundo)
    void Update()
    {
        //llama a los respectivos metodos
        GestionarMovimiento(); 
        GestionarMirada();
        GestionarSalto();
    }

    //se ejecuta cuando el juegador presiona para mover 
    void OnMove(InputValue value)
    {
        //obtiene el valor del vector 2(movimientos)
        movimientoInput = value.Get<Vector2>();
    }

    //Se ejecuta cuando el jugador mueve el raton
    void OnLook(InputValue value)
    {
        //obtiene el valor del input como vector2 

        miradaInput = value.Get<Vector2>();
    }

    //Se ejecuta cuando el jugador presiona la tecla de salto
    void OnJump(InputValue value)
    {
        //Solo permite saltar si el jugador presiona la tecla de salto
        if (controller.isGrounded && !saltando)
        {
            saltando = true; //marca que si quiere saltar
        }
    }

    //Maneja el movimiento horizontal del jugador
    void GestionarMovimiento()
    {
        //Calcula la direccion del movimiento: ejex, eje z, input horizontal, input vertical
        // Movimiento horizontal
        Vector3 direccion = transform.right * movimientoInput.x + transform.forward * movimientoInput.y;
       
        //Mueve el jugador usando el charcterController: direccion, velocidad, tiempo entre frames
        controller.Move(direccion * velocidad * Time.deltaTime);
    }

    // Maneja la rotacion de la camara y el jugador
    void GestionarMirada()
    {
        // Rotación horizontal (jugador)
        float rotacionHorizontal = miradaInput.x * sensibilidadRaton;
        
        //Rota el jugador completo en el eje y (horizontal)
        transform.Rotate(0, rotacionHorizontal, 0);

        // Rotación vertical (cámara, mira arriba/abajo; control natural) 
        rotacionVertical -= miradaInput.y * sensibilidadRaton;

        //Limpia la rotacion vertical, Mathf mantiene el valor entre -/+limiteVertical 
        rotacionVertical = Mathf.Clamp(rotacionVertical, -limiteVertical, limiteVertical);
        
        //Aplica la rotacion SOLO a la camara
        camaraJugador.transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);
    }

    //Maneja el salto y aplica la gravedad
    void GestionarSalto()
    {
        //Verifica si el jugador esta tocando el suelo
        if (controller.isGrounded)
        {
            velocidadVertical = -2f; // Mantener al jugador pegado al suelo, con una pequeña fuerza 

            if (saltando) //si esta saltando
            {
                //Calcula la velocidad inicial del salto, con fisica
                velocidadVertical = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
                //resetea el estado del salto
                saltando = false;
            }
        }
        else //si esta en el aire
        {
            //aplica gravedad: aumenta la velocidad de caida cada frame
            velocidadVertical += gravedad * Time.deltaTime;
        }

        //Mueve el jugador verticalmente(arriba/abajo): 
        controller.Move(Vector3.up * velocidadVertical * Time.deltaTime);
    }
    // Añade estas variables al principio del script MovimientoJugador

    [Header("Disparo")]
    public GameObject prefabBala;
    public Transform puntoDisparo;
    public float cadenciaDisparo = 0.5f;
    private float tiempoUltimoDisparo = 0f;

    // Añade este método

    void OnFire(InputValue value)
    {
        if (Time.time >= tiempoUltimoDisparo + cadenciaDisparo)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }
    }

    void Disparar()
    {
        Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
    }
}
