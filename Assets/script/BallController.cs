using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Configuración de Rebote")]
    [Range(0f, 1f)] public float reduccionFuerza = 0.6f;
    public float fuerzaExtraCaida = 2f;

    [Header("Configuración General")]
    public string racketTag = "Racket";
    public float vanishDelay = 40f;
    public float vanishScaleTime = 0.5f;
    public bool useGravity = true;

    [Header("Zonas")]
    public GameObject Zona1;
    public GameObject Zona2;
    public GameObject Zona3;
    public GameObject Zona4;
    public GameObject Zona5;
    public GameObject Zona6;

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

        // 🔹 Rebote con el piso
        if (collision.gameObject.CompareTag("Floor"))
        {
            float reboteFuerza = 12f;
            if (rb.linearVelocity.y < 0)
            {
                Vector3 vel = rb.linearVelocity;
                vel.y = reboteFuerza;
                rb.linearVelocity = vel;
            }
            return;
        }

        // 🔹 Golpe de raqueta
        if (collision.gameObject.CompareTag(racketTag))
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 inVel = rb.linearVelocity;

            float angle = Vector3.Angle(inVel.normalized, contact.normal);
            string colliderName = collision.collider.gameObject.name;

            GameObject targetZone = null;

            // -----------------------------------------------
            // 🔸 CENTRO
            if (colliderName == "collider center")
            {
                targetZone = angle > 90f ? Zona1 : Zona2;
            }
            // 🔸 MEDIOS
            else if (colliderName == "collider medio abajo")
            {
                targetZone = angle > 45f ? Zona3 : Zona4;
            }
            else if (colliderName == "collider medio arriba")
            {
                targetZone = angle > 45f ? Zona4 : Zona3;
            }
            else if (colliderName == "collider medio abajo")
            {
                targetZone = angle > 45f ? Zona1 : Zona2;
            }
            else if (colliderName == "collider medio arriba")
            {
                targetZone = angle > 45f ? Zona1 : Zona2;
            }
            // 🔸 EXTERIORES
            else if (colliderName == "collider lejos abajo")
            {
                targetZone = angle > 45f ? Zona5 : Zona6;
            }
            else if (colliderName == "collider lejos arriba")
            {
                targetZone = angle > 45f ? Zona6 : Zona5;
            }

            // 🔹 Redirigir pelota
            if (targetZone != null)
            {
                Vector3 dir = (targetZone.transform.position - transform.position).normalized;
                float fuerzaGolpe = inVel.magnitude * 1.2f + 5f;
                rb.linearVelocity = dir * fuerzaGolpe;
            }

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
