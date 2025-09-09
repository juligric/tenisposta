using UnityEngine;

public class GolpearPelota : MonoBehaviour
{
    [Header("Referencia a la cancha")]
    public Transform canchaContraria;

    [Header("Parámetros del golpe")]
    public float fuerzaGolpe = 60f;
    public float elevacion = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pelota"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Resetear velocidades para que el golpe sea limpio
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // Calculamos dirección hacia el centro de la cancha contraria
                Vector3 direccion = (canchaContraria.position - collision.transform.position).normalized;

                // Ajustamos altura para que no se quede baja
                direccion.y = elevacion;

                // Aplicamos fuerza
                rb.AddForce(direccion * fuerzaGolpe, ForceMode.Impulse);
            }
        }
    }
}
