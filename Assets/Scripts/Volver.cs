using UnityEngine;
using UnityEngine.SceneManagement;

public class Volver : MonoBehaviour
{
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MenuInicio");
    }
}
