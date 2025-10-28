using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI textoPuntaje;
    private int puntos = 0;

    private void Start()
    {
        ActualizarTexto();
    }

    public void SumarPuntos(int puntosASumar)
    {
        puntos += puntosASumar;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = puntos.ToString();
    }
}


