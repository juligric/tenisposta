using UnityEngine;

public class RaquetaVR : MonoBehaviour
{
    private Vector3 ultimaPos;
    private Vector3 velocidad;

    [Header("Rebote Realista")]
    public float fuerzaGolpe = 0;   // controla la potencia del golpe
    public float limiteVelocidad = 3f;  // evita disparos

    void FixedUpdate()
    {
        // Calcula la velocidad real de la raqueta (posición actual - anterior)
        velocidad = (transform.position - ultimaPos) / Time.fixedDeltaTime;

        // Limita la velocidad máxima para evitar picos absurdos del XR
        if (velocidad.magnitude > limiteVelocidad)
            velocidad = velocidad.normalized * limiteVelocidad;

        ultimaPos = transform.position;
    }

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Pelota")) return;

        Rigidbody rb = col.rigidbody;
        if (rb == null) return;

        // Dirección normal del contacto
        Vector3 normal = col.contacts[0].normal;

        // Proyecta la velocidad en la dirección del golpe
        Vector3 fuerza = Vector3.Project(velocidad, -normal);

        // Aplica impulso proporcional a la velocidad, pero controlado
        rb.AddForce(fuerza * fuerzaGolpe, ForceMode.Impulse);

        Debug.Log("fuerzaGolpe");
    }
}
