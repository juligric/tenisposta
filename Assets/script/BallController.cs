using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Configuración de Rebote")]
    [Range(0f, 1f)] public float reduccionFuerza = 1; // cuánto se reduce la velocidad al golpear
    public float fuerzaExtraCaida = 2f;
    public PhysicsMaterial NuevaPelota;
    public Collider col;

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
        col = GetComponent<Collider>();
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
                vel.y = 12f;
                rb.linearVelocity = vel;
            }
            Debug.Log("💥 La pelota rebotó en el piso.");
            return;
        }

        // 🔹 Golpe de raqueta: solo reducir velocidad
        if (collision.gameObject.CompareTag(racketTag))
        {
            Debug.Log($"🏓 Velocidad orginal al {rb.linearVelocity }");
            rb.linearVelocity *= reduccionFuerza; // reduce la velocidad actual
            col.material = NuevaPelota;
            rb.AddForce(Vector3.down * fuerzaExtraCaida, ForceMode.VelocityChange);
            Debug.Log($"🏓 Velocidad reducida al {rb.linearVelocity }% tras golpe de raqueta.");
           

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

