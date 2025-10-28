using UnityEngine;

public class Score1 : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool puntoSumado = false;
    private bool puedeContar = false;

    private void Start()
    {
        // Espera un breve tiempo antes de permitir sumar (para evitar colisión inicial con raqueta)
        Invoke(nameof(HabilitarConteo), 0.5f);
    }

    void HabilitarConteo()
    {
        puedeContar = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!puedeContar || puntoSumado) return;

        // Ignorar colisiones con la raqueta
        if (collision.collider.CompareTag("Racket"))
        {
            Debug.Log("🎾 Pelota tocó la raqueta (sin puntuar)");
            return;
        }

        // Obtener la zona activa
        Collider zonaActiva = ZonaActiva1.Instance.GetActiveZone();

        // Verificar si el collider pertenece a la zona activa
        if (zonaActiva != null &&
            (collision.collider == zonaActiva || collision.collider.transform.IsChildOf(zonaActiva.transform)))
        {
            puntoSumado = true;
            scoreManager.SumarPuntos(10);
            Debug.Log("✅ ¡Pelota en zona activa! +10 puntos");

            // Destruir pelota después de breve delay
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Debug.Log("❌ Pelota fuera de la zona activa (chocó con " + collision.collider.name + ")");
        }
    }
}
