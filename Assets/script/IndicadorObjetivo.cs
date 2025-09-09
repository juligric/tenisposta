using UnityEngine;

public class IndicadorObjetivo : MonoBehaviour
{
    [Header("Colores")]
    public Color colorNormal = Color.gray;
    public Color colorIluminado = Color.green;

    [Header("Animaci�n")]
    public float escalaExtra = 1.3f;       // Escala al iluminar
    public float velocidadEscala = 5f;     // Velocidad de la animaci�n

    [Header("Part�culas (opcional)")]
    public ParticleSystem particulas;

    private Renderer rend;
    private Material matInstancia;
    private Vector3 escalaOriginal;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        // Usamos instancia del material para modificar solo este objeto
        matInstancia = rend.material;
        escalaOriginal = transform.localScale;

        Apagar();
    }

    public void Iluminar(float tiempo)
    {
        // Color y emisi�n
        matInstancia.color = colorIluminado;
        matInstancia.SetColor("_EmissionColor", colorIluminado);

        // Escala animada
        StopAllCoroutines();
        StartCoroutine(EscalarAnimacion(escalaOriginal * escalaExtra));

        // Part�culas
        if (particulas != null)
        {
            particulas.Play();
        }

        CancelInvoke(nameof(Apagar));
        Invoke(nameof(Apagar), tiempo);
    }

    public void Apagar()
    {
        // Color y emisi�n apagada
        matInstancia.color = colorNormal;
        matInstancia.SetColor("_EmissionColor", Color.black);

        // Vuelve a la escala original
        StopAllCoroutines();
        StartCoroutine(EscalarAnimacion(escalaOriginal));

        // Detener part�culas
        if (particulas != null)
        {
            particulas.Stop();
        }
    }

    private System.Collections.IEnumerator EscalarAnimacion(Vector3 objetivo)
    {
        while (Vector3.Distance(transform.localScale, objetivo) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, objetivo, Time.deltaTime * velocidadEscala);
            yield return null;
        }
        transform.localScale = objetivo;
    }
}
