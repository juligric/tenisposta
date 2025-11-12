using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerVR : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoTotal = 5f; // Duración del juego
    private float tiempoRestante;
    private bool activo = true;

    [Header("Referencias")]
    public TextMeshProUGUI textoTimer; // Texto que muestra el tiempo
    public Transform cabezaJugador;    // Cámara principal (VR)
    public Vector3 offset = new Vector3(0, -0.3f, 1.5f); // Posición frente al jugador

    private ScoreManager scoreManager;

    private void Start()
    {
        tiempoRestante = tiempoTotal;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Update()
    {
        if (!activo) return;

        // Actualiza tiempo restante
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante < 0)
        {
            tiempoRestante = 0;
            activo = false;

            // ⏰ Cuando se acaba el tiempo → termina el juego
            if (scoreManager != null)
                scoreManager.TerminarJuego();
            else
                SceneManager.LoadScene("final"); // Por si no se encuentra el ScoreManager
        }

        // Formato MM:SS
        int minutos = Mathf.FloorToInt(tiempoRestante / 60);
        int segundos = Mathf.FloorToInt(tiempoRestante % 60);
        textoTimer.text = $"{minutos:00}:{segundos:00}";

        // 🔄 Hace que el Canvas siga la vista del jugador
        if (cabezaJugador != null)
        {
            transform.position = cabezaJugador.position + cabezaJugador.forward * offset.z + Vector3.up * offset.y;
            transform.rotation = Quaternion.LookRotation(transform.position - cabezaJugador.position);
        }
    }
}
