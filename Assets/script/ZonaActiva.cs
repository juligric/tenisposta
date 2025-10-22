using UnityEngine;

public class ZonaActiva : MonoBehaviour
{
    public static ZonaActiva Instance;

    private Collider zonaActiva;

    void Awake()
    {
        Instance = this;
    }

    public void SetActiveZone(Collider zona)
    {
        zonaActiva = zona;
        Debug.Log("🔥 Zona activa: " + zona.name);
    }

    public Collider GetActiveZone()
    {
        return zonaActiva;
    }
}
