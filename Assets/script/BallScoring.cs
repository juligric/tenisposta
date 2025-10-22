using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallScoring : MonoBehaviour
{
    public int puntosPorZona = 1;
    public TennisScoreManager scoreManager;

    private void Start()
    {
        if (scoreManager == null)
            Debug.LogError("BallScoring: scoreManager no asignado en Inspector.");
    }

    // 🔹 Solo usamos colisiones normales
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter detectado con {collision.collider.name}");

        Collider zonaActiva = ActiveZoneManager.Instance != null ? ActiveZoneManager.Instance.GetActiveCollider() : null;

        if (zonaActiva == null)
        {
            Debug.Log("No hay zona activa definida en ActiveZoneManager.");
            return;
        }

        if (collision.collider == zonaActiva)
        {
            if (scoreManager != null)
            {
                scoreManager.SumarPuntos(puntosPorZona);
                Debug.Log("¡Sumó puntos! Zona activa golpeada.");
            }
            else
            {
                Debug.LogError("ScoreManager es null en BallScoring.");
            }
        }
        else
        {
            Debug.Log($"Colisionó con {collision.collider.name}, pero la zona activa es {zonaActiva.name} -> No suma.");
        }
    }
}
