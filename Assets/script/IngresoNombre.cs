using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IngresoNombre : MonoBehaviour
{
    public TMP_InputField inputNombre;

    public void GuardarNombreYContinuar()
    {
        string nombre = inputNombre.text.Trim();

        if (!string.IsNullOrEmpty(nombre))
        {
            // Guarda el nombre del jugador en PlayerPrefs
            PlayerPrefs.SetString("NombreJugador", nombre);
            PlayerPrefs.Save();

            Debug.Log("✅ Nombre guardado: " + nombre);

            // Carga la siguiente escena (asegurate de usar el nombre correcto)
            SceneManager.LoadScene("final");  // o la escena que siga después de ingresar el nombre
        }
        else
        {
            Debug.LogWarning("⚠️ No ingresaste un nombre válido.");
        }
    }
}
