using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoPuntaje;  // Texto que muestra los puntos
    public TextMeshProUGUI textoTiempo;   // Texto que muestra el tiempo restante (añadilo al Canvas)

    [Header("Configuración de juego")]
    public float tiempoTotal = 5f; // duración del juego
    private float tiempoRestante;
    private bool juegoTerminado = false;

    private int puntos = 0;

    private void Start()
    {
        tiempoRestante = tiempoTotal;
        ActualizarTexto();
    }

    private void Update()
    {
        if (juegoTerminado) return;

        // Contador regresivo
        tiempoRestante -= Time.deltaTime;
        if (textoTiempo != null)
            textoTiempo.text = Mathf.CeilToInt(tiempoRestante).ToString();

        // Cuando llega a 0, termina el juego
        if (tiempoRestante <= 0)
        {
            TerminarJuego();
        }
    }

    public void SumarPuntos(int puntosASumar)
    {
        if (juegoTerminado) return;

        puntos += puntosASumar;
        ActualizarTexto();
    }

    private void ActualizarTexto()
    {
        if (textoPuntaje != null)
            textoPuntaje.text = puntos.ToString();
    }

    // 🔹 Se llama automáticamente al acabar el tiempo
    public void TerminarJuego()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;

        // Guarda los datos actuales del jugador
        GuardarPuntaje();

        // Cambia a la escena final de resultados (ajustá el nombre)
        SceneManager.LoadScene("Final");
    }

    private void GuardarPuntaje()
    {
        // Recupera el nombre del jugador de la escena inicial
        string nombreJugador = PlayerPrefs.GetString("NombreJugador", "Invitado");

        // Verifica si ya tiene un puntaje guardado
        int puntajePrevio = PlayerPrefs.GetInt(nombreJugador + "_Score", 0);
        if (puntos > puntajePrevio)
        {
            PlayerPrefs.SetInt(nombreJugador + "_Score", puntos);
        }

        // Guarda este como el último puntaje para mostrar en la escena final
        PlayerPrefs.SetString("UltimoJugador", nombreJugador);
        PlayerPrefs.SetInt("UltimoPuntaje", puntos);

        PlayerPrefs.Save();
    }
}
