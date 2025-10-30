using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("pruebasarte"); // Cambiá "Materiales" por el nombre de tu escena del juego
    }

    public void Salir()
    {
        Application.Quit(); // Por si querés agregar un botón de salir
    }
}