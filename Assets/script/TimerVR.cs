using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerVR : MonoBehaviour
{
    [Header("Configuración")]
    public float tiempoTotal = 150f; // Duración del juego
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
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();
    }

    private void Update()
    {
        if (!activo) return;

        // 🔽 Resta el tiempo en cada frame
        tiempoRestante -= Time.deltaTime;

        // 🔁 Si llega a 0, reinicia el contador
        if (tiempoRestante <= 0)
        {
            Debug.Log("⏰ Tiempo terminado — reiniciando temporizador...");
            ReiniciarTiempo();
        }

        // 🕒 Muestra el tiempo restante (solo segundos)
        int segundos = Mathf.FloorToInt(tiempoRestante);
        textoTimer.text = $"{segundos:00}";
     
       
    }

    private void ReiniciarTiempo()
    {
        tiempoRestante = tiempoTotal;
        activo = true;
    }
}
