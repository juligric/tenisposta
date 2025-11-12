using UnityEngine;

public class CambioDeImagenes : MonoBehaviour
{
    public Renderer planoRenderer;   // Renderer del plano
    public Texture[] imagenes;       // Arrastrá las 8 imágenes (texturas)
    public float tiempoEntreCambios = 8f;

    private int indiceActual = 0;

    void Start()
    {
        if (planoRenderer == null)
            planoRenderer = GetComponent<Renderer>();

        if (imagenes.Length > 0)
            planoRenderer.material.mainTexture = imagenes[0];

        // Empieza el ciclo
        InvokeRepeating("CambiarImagen", tiempoEntreCambios, tiempoEntreCambios);
    }

    void CambiarImagen()
    {
        if (imagenes.Length == 0) return;

        // Pasar a la siguiente imagen
        indiceActual++;
        if (indiceActual >= imagenes.Length)
            indiceActual = 0; // volver al principio

        planoRenderer.material.mainTexture = imagenes[indiceActual];
    }
}