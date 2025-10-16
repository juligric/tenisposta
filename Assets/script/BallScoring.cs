using UnityEngine;

public class BallScoring : MonoBehaviour
{
    public TennisScoreManager scoreManager;
    public Collider validZone;
    public bool isPlayerServing;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            if (validZone.bounds.Contains(transform.position))
            {
                // La pelota cayó adentro del lado contrario
                scoreManager.AddPoint(isPlayerServing ? "Player" : "Opponent");
            }
            else
            {
                // La pelota cayó afuera → punto para el otro
                scoreManager.AddPoint(isPlayerServing ? "Opponent" : "Player");
            }

            // Reiniciar la pelota
            ResetBall();
        }
    }

    private void ResetBall()
    {
        // Lógica para volver a lanzar
        transform.position = new Vector3(0, 1, 0);
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}

