using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //boton Jugar
    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }

    //boton Creditos
    public void Creditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    //boton salir
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
