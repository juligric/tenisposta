using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("IngresoNombre"); // Cambi� "Materiales" por el nombre de tu escena del juego
    }

    public void Salir()
    {
        Application.Quit(); // Por si quer�s agregar un bot�n de salir
    }
}