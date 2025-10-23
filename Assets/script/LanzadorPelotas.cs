using UnityEngine;

public class LanzadorPelotas : MonoBehaviour
{
    [Header("Sistema de puntaje")]
    public ScoreManager scoreManager; // Correcto para referenciar el controlador global


    [Header("Zonas del campo")]
    public IndicadorObjetivo ZonaIzquierda;
    public IndicadorObjetivo ZonaDerecha;

    [Header("Pelotas")]
    public GameObject prefabPelota;
    public Transform puntoLanzamiento;
    public float fuerzaLanzamiento = 300f;
    public float intervaloLanzamiento = 3f;

    [Header("Configuración")]
    public bool alternarLados = true;
    private bool proximaIzquierda = true;
    public float tiempoIluminado = 2f;

    [Range(0f, 1f)] public float anguloElevacion = 0.3f;
    [Range(0f, 1f)] public float desviacionMaxX = 0.5f;

    private void Start()
    {
        InvokeRepeating(nameof(LanzarPelota), 1f, intervaloLanzamiento);
    }

    private void LanzarPelota()
    {
        if (prefabPelota == null || puntoLanzamiento == null)
        {
            Debug.LogWarning("⚠️ Falta asignar el prefab de la pelota o el punto de lanzamiento.");
            return;
        }

        GameObject pelota = Instantiate(prefabPelota, puntoLanzamiento.position, puntoLanzamiento.rotation);

        Rigidbody rb = pelota.GetComponent<Rigidbody>();

        Score1 scoring = pelota.GetComponent<Score1>();
        if (scoring != null)
        {
            scoring.scoreManager = scoreManager;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;  // cambia rb.linearVelocity a rb.velocity

            float desviacionX = Random.Range(-desviacionMaxX, desviacionMaxX);
            Vector3 direccion = (puntoLanzamiento.forward + puntoLanzamiento.up * anguloElevacion + puntoLanzamiento.right * desviacionX).normalized;

            rb.AddForce(direccion * fuerzaLanzamiento, ForceMode.VelocityChange);
        }

        // Activar e iluminar la zona correspondiente
        ActivarZona();
    }

    private void ActivarZona()
    {
        // Apagar ambas zonas antes de activar la nueva
        ZonaIzquierda.Apagar();
        ZonaDerecha.Apagar();

        Collider zonaActiva;

        if (alternarLados)
        {
            proximaIzquierda = !proximaIzquierda;
            zonaActiva = proximaIzquierda
                ? ZonaIzquierda.GetComponent<Collider>()
                : ZonaDerecha.GetComponent<Collider>();
        }
        else
        {
            zonaActiva = (Random.value > 0.5f ? ZonaIzquierda : ZonaDerecha).GetComponent<Collider>();
        }

        // Iluminar la zona activa
        if (zonaActiva == ZonaIzquierda.GetComponent<Collider>())
            ZonaIzquierda.Iluminar(tiempoIluminado);
        else
            ZonaDerecha.Iluminar(tiempoIluminado);

        // Registrar la zona activa globalmente
        ZonaActiva1.Instance.SetActiveZone(zonaActiva);
    }
}
