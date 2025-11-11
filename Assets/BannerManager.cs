using UnityEngine;
using System.Collections;

public class BannerSquareRotatorTexturas : MonoBehaviour
{
    [Header("Configuración de los Banners")]
    public Texture2D[] bannerImages; // 7 imágenes PNG desde Figma
    public float lado = 20f; // tamaño del cuadrado
    public float altura = 3f; // altura
    public float escalaBase = 2f; // tamaño del banner
    public float tiempoCambio = 3f; // segundos entre cambios
    public float velocidadRotacion = 5f; // velocidad de rotación del conjunto

    private GameObject[] banners;
    private int bannerIndex = 0;

    void Start()
    {
        CrearBanners();
        StartCoroutine(CambiarBanners());
    }

    void CrearBanners()
    {
        if (bannerImages.Length == 0)
        {
            Debug.LogError("⚠️ No hay imágenes asignadas en el array bannerImages.");
            return;
        }

        banners = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            switch (i)
            {
                case 0: pos = new Vector3(0, altura, lado / 2f); rot = Quaternion.LookRotation(Vector3.back); break;   // frente
                case 1: pos = new Vector3(lado / 2f, altura, 0); rot = Quaternion.LookRotation(Vector3.left); break;    // derecha
                case 2: pos = new Vector3(0, altura, -lado / 2f); rot = Quaternion.LookRotation(Vector3.forward); break; // atrás
                case 3: pos = new Vector3(-lado / 2f, altura, 0); rot = Quaternion.LookRotation(Vector3.right); break;  // izquierda
            }

            GameObject banner = GameObject.CreatePrimitive(PrimitiveType.Quad);
            banner.name = "Banner_Lado_" + i;
            banner.transform.parent = transform;
            banner.transform.localPosition = pos;
            banner.transform.localRotation = rot;

            // Asignar textura como material
            Material mat = new Material(Shader.Find("Standard"));
            mat.mainTexture = bannerImages[i % bannerImages.Length];
            banner.GetComponent<MeshRenderer>().material = mat;

            // Ajustar escala proporcional
            Texture tex = mat.mainTexture;
            if (tex != null)
            {
                float proporcion = (float)tex.width / tex.height;
                banner.transform.localScale = new Vector3(escalaBase * proporcion, escalaBase, 1f);
            }

            banners[i] = banner;
        }
    }

    void Update()
    {
        // 🔄 Rotar todo el conjunto de banners alrededor del centro
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime, Space.World);
    }

    IEnumerator CambiarBanners()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoCambio);
            bannerIndex = (bannerIndex + 1) % bannerImages.Length;

            // Cambiar imágenes en los 4 lados
            for (int i = 0; i < banners.Length; i++)
            {
                int nextIndex = (bannerIndex + i) % bannerImages.Length;
                banners[i].GetComponent<MeshRenderer>().material.mainTexture = bannerImages[nextIndex];
            }
        }
    }
}
