using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public string currentPlayerName;
    public int currentScore;
    public List<PlayerData> topScores = new List<PlayerData>();

    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 👈 Mantiene este objeto al cambiar de escena
            filePath = Path.Combine(Application.persistentDataPath, "scores.json");
            CargarDatos();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CargarDatos()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            ScoreWrapper wrapper = JsonUtility.FromJson<ScoreWrapper>(json);
            topScores = wrapper.scores;
        }
    }

    public void GuardarDatos()
    {
        ScoreWrapper wrapper = new ScoreWrapper { scores = topScores };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(filePath, json);
    }

    public void ActualizarScore(string nombre, int nuevoPuntaje)
    {
        PlayerData existente = topScores.Find(p => p.nombre == nombre);

        if (existente != null)
        {
            if (nuevoPuntaje > existente.puntaje)
                existente.puntaje = nuevoPuntaje;
        }
        else
        {
            topScores.Add(new PlayerData(nombre, nuevoPuntaje));
        }

        // Ordenar top 3 descendente
        topScores.Sort((a, b) => b.puntaje.CompareTo(a.puntaje));
        if (topScores.Count > 3)
            topScores = topScores.GetRange(0, 3);

        GuardarDatos();
    }
}

[System.Serializable]
public class ScoreWrapper
{
    public List<PlayerData> scores;
}
