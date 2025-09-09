using UnityEngine;

public class LanzadorPelotas : MonoBehaviour
{
    [Header("Pelota")]
    public GameObject pelotaPrefab;
    public Transform puntoDisparo;
    public float fuerzaDisparo = 20f;
    public float intervalo = 3f;
    public float variacionAngulo = 20f;

    [Header("Indicadores de media cancha")]
    public IndicadorObjetivo zonaIzquierda;
    public IndicadorObjetivo zonaDerecha;
    public float retrasoIluminado = 1f;
    public float tiempoIluminado = 2f;
    public bool alternarLados = true;

    private bool proximaIzquierda = true;

    private void Start()
    {
        InvokeRepeating(nameof(DispararPelota), 0f, intervalo);
    }

    void DispararPelota()
    {
        // Crear y lanzar pelota
        GameObject pelota = Instantiate(pelotaPrefab, puntoDisparo.position, puntoDisparo.rotation);
        Rigidbody rb = pelota.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 dirBase = puntoDisparo.forward;
            Vector3 dirAleatoria = Quaternion.Euler(
                Random.Range(-variacionAngulo, variacionAngulo),
                Random.Range(-variacionAngulo, variacionAngulo),
                0f
            ) * dirBase;

            rb.AddForce(dirAleatoria.normalized * fuerzaDisparo, ForceMode.Impulse);
        }

        Destroy(pelota, 10f);

        // Iluminar mitad correcta tras un retraso
        Invoke(nameof(IluminarMitadObjetivo), retrasoIluminado);
    }

    void IluminarMitadObjetivo()
    {
        // Apagar ambos antes de iluminar uno
        zonaIzquierda?.Apagar();
        zonaDerecha?.Apagar();

        if (alternarLados)
        {
            if (proximaIzquierda)
                zonaIzquierda?.Iluminar(tiempoIluminado);
            else
                zonaDerecha?.Iluminar(tiempoIluminado);

            proximaIzquierda = !proximaIzquierda;
        }
        else
        {
            if (Random.value > 0.5f)
                zonaIzquierda?.Iluminar(tiempoIluminado);
            else
                zonaDerecha?.Iluminar(tiempoIluminado);
        }
    }
}
