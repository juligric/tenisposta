using UnityEngine;
using UnityEngine.UI;

public class MedidorBarra : MonoBehaviour
{
    public Image barra;
    public float incremento = 0.2f;

    void Start()
    {
        if (barra != null)
        {
            barra.fillAmount = 0.3f; // Aparece vacía
            Color c = barra.color;
            c.a = 1f; // Asegurarse que sea opaca
            barra.color = c;
        }
    }

    public void UsarObjeto()
    {
        if (barra != null)
        {
            barra.fillAmount = Mathf.Clamp01(barra.fillAmount + 0.3f);
        }
    }
}