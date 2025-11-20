using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI textoPuntuacionFinal;

    void Start()
    {
        // Aquí podrías guardar la puntuación entre escenas usando PlayerPrefs
        int puntosFinal = PlayerPrefs.GetInt("PuntosFinal", 0);
        if (textoPuntuacionFinal != null)
        {
            textoPuntuacionFinal.text = "Puntuación Final: " + puntosFinal;
        }
    }

    public void Reintentar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
