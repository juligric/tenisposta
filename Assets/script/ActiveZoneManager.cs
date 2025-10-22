using UnityEngine;

public class ActiveZoneManager : MonoBehaviour
{
    public static ActiveZoneManager Instance;

    [Header("Colliders de las mitades (asignar desde Inspector)")]
    public Collider zonaIzquierda;
    public Collider zonaDerecha;

    private Collider zonaActiva;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogWarning("Ya existe otra instancia de ActiveZoneManager");
    }

    public void SetActiveZone(string lado)
    {
        if (lado == "Izquierda")
            zonaActiva = zonaIzquierda;
        else if (lado == "Derecha")
            zonaActiva = zonaDerecha;
        else
            zonaActiva = null;

        Debug.Log($"SetActiveZone -> {lado} | zonaActiva = {(zonaActiva != null ? zonaActiva.name : "null")}");
    }

    public Collider GetActiveCollider()
    {
        return zonaActiva;
    }

    // Método de comprobación por posición (opcional)
    public bool EstaEnZonaActiva(Vector3 posicion)
    {
        if (zonaActiva == null) return false;
        return zonaActiva.bounds.Contains(posicion);
    }
}
