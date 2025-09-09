using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public float reflectSpeedMultiplier = 1.5f; // cu�nto aumenta la velocidad al pegarle
    public string racketTag = "Racket";
    public float vanishDelay = 1.2f; // tiempo despu�s del golpe para desaparecer
    public float vanishScaleTime = 0.5f;
    public bool useGravity = true;

    Rigidbody rb;
    bool wasHit = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = useGravity;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (wasHit) return;

        // Si golpea la raqueta (cubo)
        if (collision.gameObject.CompareTag(racketTag))
        {
            // Para calcular el reflejo m�s preciso tomamos el primer contacto
            ContactPoint contact = collision.contacts[0];
            Vector3 inVel = rb.linearVelocity;

            // Si la pelota ven�a lenta, podemos usar la velocidad relativa del objeto que la golpea
            Vector3 racketVel = Vector3.zero;
            Rigidbody racketRb = collision.rigidbody;
            if (racketRb != null) racketVel = racketRb.linearVelocity;

            // reflectamos la velocidad contra la normal de contacto
            Vector3 reflected = Vector3.Reflect(inVel, contact.normal);

            // si la velocidad era casi cero (spawn cerca), usa la direcci�n opuesta a la normal
            if (reflected.sqrMagnitude < 0.01f)
                reflected = contact.normal * -1f;

            // combinamos con la velocidad de la raqueta (opcional) para sentido m�s realista
            Vector3 finalVel = (reflected.normalized * inVel.magnitude * reflectSpeedMultiplier) + racketVel * 0.6f;

            rb.linearVelocity = finalVel;

            wasHit = true;
            StartCoroutine(VanishAfterDelay(vanishDelay));
            // opcional: reproducir sonido o part�culas ac�
        }
        else
        {
            // Si choca con cualquier otra cosa (suelo, paredes) pod�s dejar la f�sica normal
        }
    }

    IEnumerator VanishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // animaci�n de escala para "desaparecer"
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
