using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class TopScoresUI : MonoBehaviour
{
    public TextMeshProUGUI tablaTexto;

    void Start()
    {
        MostrarTopScores();
    }

    void MostrarTopScores()
    {
        var scores = GameDataManager.Instance.topScores;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < scores.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {scores[i].nombre}  -  {scores[i].puntaje}");
        }

        tablaTexto.text = sb.ToString();
    }

    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("IngresoNombre");
    }
}
