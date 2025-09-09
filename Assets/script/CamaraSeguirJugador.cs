using UnityEngine;

public class CamaraSeguirJugador : MonoBehaviour
{
    public Transform jugador;
    public Vector3 offset = new Vector3(0, 10, -10);

    void LateUpdate()
    {
        if (jugador != null)
        {
            transform.position = jugador.position + offset;
            transform.LookAt(jugador);
        }
    }
}
