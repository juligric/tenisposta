using UnityEngine;
using TMPro;

public class TennisScoreManager : MonoBehaviour
{
    public int puntaje = 0;
    public TMP_Text textoPuntaje;

    private void Start()
    {
        ActualizarTexto();
    }

    public void SumarPuntos(int puntos)
    {
        puntaje += puntos;
        ActualizarTexto();
        Debug.Log($"Puntos sumados: {puntos} | Total: {puntaje}");
    }

    void ActualizarTexto()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = "Puntaje: " + puntaje;
    }
}
