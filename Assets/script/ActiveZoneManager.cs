using UnityEngine;

public class ActiveZoneManager : MonoBehaviour
{
    public static ActiveZoneManager Instance;

    [Header("Colliders de las mitades")]
    public Collider zonaIzquierda;
    public Collider zonaDerecha;

    private Collider zonaActiva;

    void Awake()
    {
        Instance = this;
    }

    public void SetActiveZone(string lado)
    {
        if (lado == "Izquierda")
            zonaActiva = zonaIzquierda;
        else if (lado == "Derecha")
            zonaActiva = zonaDerecha;
    }

    public bool EstaEnZonaActiva(Vector3 posicion)
    {
        if (zonaActiva == null) return false;
        return zonaActiva.bounds.Contains(posicion);
    }
}
