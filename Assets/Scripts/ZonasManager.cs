using UnityEngine;

public class ZonasManager : MonoBehaviour
{
    public static ZonasManager Instance;
    public IndicadorObjetivo zona0;
    public IndicadorObjetivo zona1;
    public float tiempo;
    public int zona = 0;

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

    public void ActivarZona()
    {
        GameManager.Instance.UpdateDebug("Activando zona " + zona);
        if (zona == 0)
        {
            zona0.Iluminar(tiempo);
            zona = 1;
            Debug.Log("Zona 0");
        }
        else
        {
            zona1.Iluminar(tiempo);
            zona = 0;
            Debug.Log("Zona 1");
        }
    }
}
