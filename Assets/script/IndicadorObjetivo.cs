using UnityEngine;

public class IndicadorObjetivo : MonoBehaviour
{
    [Header("ZonaID")]
    public int zonaID = 0;

    [Header("Colores")]
    public Color colorNormal = Color.gray;
    public Color colorIluminado = Color.green;

    [Header("Partículas (opcional)")]
    public ParticleSystem particulas;

    private Renderer rend;
    private Material matInstancia;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        matInstancia = rend.material;
        Apagar();
    }

    public void Iluminar(float tiempo)
    {
        Debug.Log("Iluminando " + gameObject.name);
        matInstancia.color = colorIluminado;
        matInstancia.SetColor("_EmissionColor", colorIluminado);

        if (particulas != null) particulas.Play();

        CancelInvoke(nameof(Apagar));
        Invoke(nameof(Apagar), tiempo);
    }

    public void Apagar()
    {
        matInstancia.color = colorNormal;
        matInstancia.SetColor("_EmissionColor", Color.black);

        if (particulas != null) particulas.Stop();
    }
}
