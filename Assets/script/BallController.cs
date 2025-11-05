using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Configuración de Rebote")]
    [Range(0f, 1f)] public float reduccionFuerza = 0.6f; // 🔹 cuanto menor, más suave el rebote
    public float fuerzaExtraCaida = 2f; // 🔹 empuje hacia abajo tras el golpe

    [Header("Configuración General")]
    public string racketTag = "Racket";
    public float vanishDelay = 40f;
    public float vanishScaleTime = 0.5f;
    public bool useGravity = true;

    private Rigidbody rb;
    private bool wasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = useGravity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (wasHit) return;

        // Si golpea la raqueta
        if (collision.gameObject.CompareTag(racketTag))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 inVel = rb.linearVelocity;

            Vector3 racketVel = Vector3.zero;
            Rigidbody racketRb = collision.rigidbody;
            if (racketRb != null) racketVel = racketRb.linearVelocity;

            // Refleja la dirección de la pelota
            Vector3 reflected = Vector3.Reflect(inVel, contact.normal);

            if (reflected.sqrMagnitude < 0.01f)
                reflected = contact.normal * -1f;

            // 🔹 En vez de multiplicar, reducimos la fuerza para que se desacelere
            Vector3 finalVel = (reflected.normalized * inVel.magnitude * reduccionFuerza)
                             + (racketVel * 0.3f); // menor influencia de la raqueta

            rb.linearVelocity = finalVel;

            // 🔹 Empuje extra hacia abajo para simular peso real
            rb.AddForce(Vector3.down * fuerzaExtraCaida, ForceMode.Impulse);

            wasHit = true;
            StartCoroutine(VanishAfterDelay(vanishDelay));
        }
    }

    IEnumerator VanishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 startScale = transform.localScale;
        float t = 0f;
        while (t < vanishScaleTime)
        {
            t += Time.deltaTime;
            float frac = Mathf.Clamp01(t / vanishScaleTime);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, frac);
            yield return null;
        }
        Destroy(gameObject);
    }
}
