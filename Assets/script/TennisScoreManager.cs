using UnityEngine;
using TMPro;

public class TennisScoreManager : MonoBehaviour
{
    public int puntaje = 0;
    public TMP_Text textoPuntaje;

    public void SumarPuntos(int puntos)
    {
        puntaje += puntos;
        ActualizarTexto();
    }

    void ActualizarTexto()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = "Puntaje: " + puntaje;
    }
}
