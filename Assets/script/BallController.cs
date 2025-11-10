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
        if (collision.gameObject.CompareTag("floor"))
        {
            if (rb.linearVelocity.y < 0)
            {
                Vector3 vel = rb.linearVelocity;
                vel.y = 12f; // fuerza del rebote
                rb.linearVelocity = vel;
            }
            Debug.Log("💥 La pelota rebotó en el piso.");
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

            Debug.Log($"🏓 Golpe detectado con: {colliderName} (ángulo {angle:F2}°)");

            // 🔸 CENTRO
            if (colliderName == "collider center")
                targetZone = angle > 45f ? Zona1 : Zona2;

            // 🔸 MEDIOS
            else if (colliderName == "collider medio abajo")
                targetZone = angle > 45f ? Zona3 : Zona4;

            else if (colliderName == "collider medio arriba")
                targetZone = angle > 45f ? Zona4 : Zona3;

            // 🔸 EXTERIORES
            else if (colliderName == "collider lejos abajo")
                targetZone = angle > 45f ? Zona5 : Zona6;

            else if (colliderName == "collider lejos arriba")
                targetZone = angle > 45f ? Zona6 : Zona5;

            // 🔹 Redirigir pelota
            if (targetZone != null)
            {
                Vector3 dir = (targetZone.transform.position - transform.position).normalized;
                float fuerzaGolpe = Mathf.Clamp(inVel.magnitude * 0.8f + 6f, 8f, 18f);

                // 🟢 Línea visual para depuración (solo en modo editor)
                Debug.DrawLine(transform.position, targetZone.transform.position, Color.green, 1.5f);

                // 🔹 Aplicar dirección y fuerza
                rb.linearVelocity = dir * fuerzaGolpe;
                rb.AddForce(Vector3.down * fuerzaExtraCaida, ForceMode.VelocityChange);

                Debug.Log($"🎯 Pelota dirigida hacia: {targetZone.name} con fuerza {fuerzaGolpe:F2}");
            }
            else
            {
                Debug.LogWarning($"⚠️ No se encontró una zona asignada para el collider '{colliderName}'.");
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
        Debug.Log("🕳️ La pelota fue destruida después del retardo.");
        Destroy(gameObject);
    }
}
