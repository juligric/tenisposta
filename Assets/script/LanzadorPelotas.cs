using UnityEngine;

public class LanzadorPelotas : MonoBehaviour
{
    [Header("Sistema de puntaje")]
    public TennisScoreManager scoreManager;

    [Header("Zonas del campo")]
    public IndicadorObjetivo ZonaIzquierda;
    public IndicadorObjetivo ZonaDerecha;

    [Header("Pelotas")]
    public GameObject prefabPelota;
    public Transform puntoLanzamiento;
    public float fuerzaLanzamiento = 300f;
    public float intervaloLanzamiento = 3f;

    [Header("Configuración del lanzamiento")]
    public bool alternarLados = true;
    private bool proximaIzquierda = true;
    public float tiempoIluminado = 2f;

    [Range(0f, 1f)] public float anguloElevacion = 0.3f;
    [Range(0f, 1f)] public float desviacionMaxX = 0.5f;

    private string ladoActivo = ""; // "Izquierda" o "Derecha"

    void Start()
    {
        InvokeRepeating(nameof(LanzarPelota), 1f, intervaloLanzamiento);
    }

    void LanzarPelota()
    {
        if (prefabPelota == null || puntoLanzamiento == null)
        {
            Debug.LogWarning("Falta asignar prefabPelota o puntoLanzamiento en el inspector.");
            return;
        }

        GameObject pelota = Instantiate(prefabPelota, puntoLanzamiento.position, puntoLanzamiento.rotation);
        Rigidbody rb = pelota.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;

            float desviacionX = Random.Range(-desviacionMaxX, desviacionMaxX);
            Vector3 direccion = (puntoLanzamiento.forward + puntoLanzamiento.up * anguloElevacion + puntoLanzamiento.right * desviacionX).normalized;

            rb.AddForce(direccion * fuerzaLanzamiento, ForceMode.VelocityChange);
        }

        IluminarMitadObjetivo();
    }

    void IluminarMitadObjetivo()
    {
        Debug.Log("🧠 LanzadorPelotas: IluminarMitadObjetivo ejecutado. ActiveZoneManager = " + ActiveZoneManager.Instance);

        if (ZonaIzquierda == null) Debug.LogError("❌ ZonaIzquierda no asignada");
        if (ZonaDerecha == null) Debug.LogError("❌ ZonaDerecha no asignada");
        if (ActiveZoneManager.Instance == null) Debug.LogError("❌ ActiveZoneManager.Instance es null");

        if (ZonaIzquierda != null) ZonaIzquierda.Apagar();
        if (ZonaDerecha != null) ZonaDerecha.Apagar();

        if (alternarLados)
        {
            if (proximaIzquierda)
            {
                ZonaIzquierda?.Iluminar(tiempoIluminado);
                ladoActivo = "Izquierda";
            }
            else
            {
                ZonaDerecha?.Iluminar(tiempoIluminado);
                ladoActivo = "Derecha";
            }

            proximaIzquierda = !proximaIzquierda;
        }
        else
        {
            if (Random.value > 0.5f)
            {
                ZonaIzquierda?.Iluminar(tiempoIluminado);
                ladoActivo = "Izquierda";
            }
            else
            {
                ZonaDerecha?.Iluminar(tiempoIluminado);
                ladoActivo = "Derecha";
            }
        }

        ActiveZoneManager.Instance.SetActiveZone("Izquierda");
        ActiveZoneManager.Instance.SetActiveZone("Derecha");


    }
}
