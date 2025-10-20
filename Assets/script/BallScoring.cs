using UnityEngine;

public class BallScoring : MonoBehaviour
{
    public TennisScoreManager scoreManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (ActiveZoneManager.Instance != null && ActiveZoneManager.Instance.EstaEnZonaActiva(transform.position))
            {
                scoreManager.SumarPuntos(10);
                Debug.Log("✅ +10 puntos");
            }
            else
            {
                Debug.Log("❌ No sumó puntos");
            }
        }
    }
}
