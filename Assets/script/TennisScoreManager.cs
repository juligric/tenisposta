using UnityEngine;
using TMPro;

public class TennisScoreManager : MonoBehaviour
{
    public int puntaje = 0;
    public TMP_Text textoPuntaje;

    void Start()
    {
        ActualizarTexto();
    }

    public void SumarPuntos(int puntos)
    {
        puntaje += puntos;
        ActualizarTexto();
        Debug.Log($"✅ +{puntos} puntos (Total: {puntaje})");
    }

    void ActualizarTexto()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = "Puntaje: " + puntaje;
    }
}
