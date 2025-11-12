using UnityEngine;
using UnityEngine.SceneManagement;

public class Instrucciones : MonoBehaviour
{
    public void IrAlJuego()
    {
        SceneManager.LoadScene("final");
    }
}
