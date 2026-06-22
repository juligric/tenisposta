using UnityEngine;

public class BallScore : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Floor"))
        {
            IndicadorObjetivo scriptZona = collision.gameObject.GetComponent<IndicadorObjetivo>();
            GameManager.Instance.UpdateDebug("Colision con " + collision.gameObject.name);
            if (scriptZona)
            {
                GameManager.Instance.UpdateScore();
                ZonasManager.Instance.ActivarZona(scriptZona.zonaID);
            }
        }        
    }
}
