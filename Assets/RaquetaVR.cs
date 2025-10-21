using UnityEngine;

public class RaquetaVR : MonoBehaviour
{
    private Vector3 ultimaPos;
    private Vector3 velocidad;

    void Update()
    {
        // Calcula la velocidad de la raqueta por frame
        velocidad = (transform.position - ultimaPos) / Time.deltaTime;
        ultimaPos = transform.position;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Pelota"))
        {
            Rigidbody rb = col.rigidbody;
            if (rb != null)
            {
                // Aplica impulso proporcional a la velocidad de la raqueta
                rb.AddForce(velocidad * 1.2f, ForceMode.VelocityChange);
            }
        }
    }
}
