using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class TopScoresUI : MonoBehaviour
{
    public TextMeshProUGUI tablaTexto;

    void Start()
    {
        // 🔹 Forzar recarga del JSON cada vez que se entra a esta escena
        if (GameDataManager.Instance != null)
        {
            Debug.Log("🔁 Recargando datos desde JSON...");
            GameDataManager.Instance.CargarDatos();
        }
        else
        {
            Debug.LogError("❌ No se encontró GameDataManager activo en escena.");
        }

        MostrarTopScores();
    }

    void MostrarTopScores()
    {
        if (GameDataManager.Instance == null)
        {
            tablaTexto.text = "Error: no se encontró GameDataManager.";
            return;
        }

        var scores = GameDataManager.Instance.topScores;
        StringBuilder sb = new StringBuilder();

        if (scores == null || scores.Count == 0)
        {
            sb.AppendLine("No hay puntajes guardados todavía.");
        }
        else
        {
            for (int i = 0; i < scores.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {scores[i].nombre}                       {scores[i].puntaje}");
            }
        }

        tablaTexto.text = sb.ToString();

        Debug.Log("✅ Tabla actualizada con " + scores.Count + " jugadores.");
    }

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("IngresoNombre");
    }
}
