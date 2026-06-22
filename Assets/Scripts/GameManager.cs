using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtDebug;
    public int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Si ya existe una instancia Y no soy yo, me destruyo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; // Cortamos la ejecución aquí
        }

        // Si no existe, yo me convierto en la instancia principal
        Instance = this;

        // Opcional: Hace que este Manager sobreviva a los cambios de escena
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore()
    {
        score++;
        txtScore.text = score.ToString();
    }
    public void UpdateDebug(string msg)
    {
        txtDebug.text = msg;
    }

}
