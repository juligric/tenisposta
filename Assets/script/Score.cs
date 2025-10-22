using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Score : MonoBehaviour
{
    public int puntosPorZona = 10;
    public TennisScoreManager scoreManager;

    private bool yaSumado = false; // evita sumar dos veces por la misma caída

    void ResetSumado()
    {
        yaSumado = false;
    }

    private void TryScore(Collider otherCollider)
    {
        if (yaSumado) return;

        var manager = ZonaActiva.Instance;
        if (manager == null)
        {
            Debug.LogWarning("Score: ActiveZoneManager.Instance es null.");
            return;
        }

        Collider zonaActiva = manager.GetActiveZone();
        if (zonaActiva == null)
        {
            Debug.Log("Score: No hay zona activa al momento del impacto.");
            return;
        }

        // Comprueba si el collider que tocó pertenece a la zona activa
        if (CollidersMatch(otherCollider, zonaActiva))
        {
            scoreManager?.SumarPuntos(puntosPorZona);
            Debug.Log($"🏆 Pelota en zona activa ({zonaActiva.name}) -> +{puntosPorZona} puntos");
            yaSumado = true;
            Invoke(nameof(ResetSumado), 0.2f); // permite sumar en futuros impactos
        }
        else
        {
            Debug.Log($"Pelota tocó {otherCollider.name} pero la zona activa es {zonaActiva.name} -> no suma.");
        }
    }

    private bool CollidersMatch(Collider a, Collider b)
    {
        if (a == null || b == null) return false;
        if (a == b) return true;

        // comparar objeto raíz (útil si los colliders están en hijos)
        if (a.transform.root == b.transform.root) return true;

        // comprobar si uno es hijo del otro
        if (a.transform.IsChildOf(b.transform) || b.transform.IsChildOf(a.transform)) return true;

        // si usan tags (opcional) y ambas zonas tienen el mismo tag
        if (!string.IsNullOrEmpty(b.tag) && b.tag != "Untagged" && a.CompareTag(b.tag)) return true;

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Score: OnCollisionEnter con " + collision.collider.name);
        TryScore(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Score: OnTriggerEnter con " + other.name);
        TryScore(other);
    }
}
