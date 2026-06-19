using UnityEngine;

public class ZonaActiva1 : MonoBehaviour
{
    public static ZonaActiva1 Instance { get; private set; }
    private Collider zonaActiva;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SetActiveZone(Collider zona)
    {
        zonaActiva = zona;
    }

    public Collider GetActiveZone()
    {
        return zonaActiva;
    }
}
