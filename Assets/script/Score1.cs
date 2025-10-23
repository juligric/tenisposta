using UnityEngine;
using TMPro;

public class Score1 : MonoBehaviour
{
    public ScoreManager scoreManager; // Referencia al manejador global del puntaje

    private bool puntoSumado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (puntoSumado) return;  // Evita sumar más de una vez

        if (other.CompareTag("ZonaIluminada"))
        {
            if (ZonaActiva1.Instance.GetActiveZone() == other)
            {
                scoreManager.SumarPuntos(10);
                puntoSumado = true;
            }
        }
    }
}
