using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IngresoNombre : MonoBehaviour
{
    public TMP_InputField inputNombre;

    public void GuardarNombreYContinuar()
    {
        if (!string.IsNullOrEmpty(inputNombre.text))
        {
            GameDataManager.Instance.currentPlayerName = inputNombre.text;
            SceneManager.LoadScene("Instrucciones");
        }
    }
}
