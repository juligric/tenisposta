using System.Collections;
using UnityEngine;

public class BallInstantiator : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform launchPoint;
    public float interval = 2f;
    public float serviceForce = 1f;
    public Transform bouncePointTR;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        bouncePointTR = GameObject.Find("BouncePoint").transform;
        launchPoint = GameObject.FindGameObjectWithTag("LaunchPoint").transform;
        StartCoroutine(nameof(LaunchLoop));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InstantiateBall()
    {
        GameObject ball = Instantiate(ballPrefab,launchPoint.position,Quaternion.identity);
        BallController ballController = ball.GetComponent<BallController>();

        // Ejemplo de uso al generar un saque:
        Vector3 randomTarget = GetRandomBouncePoint();

        if (ballController != null)
        {
            ballController.LaunchTowards(randomTarget, serviceForce, BallController.BallState.EnemyServe);
            Destroy(ballController.gameObject, 8);
            GameManager.Instance.UpdateDebug("Bola lanzada");
        }
        else
        {
            Debug.Log("Ball not found");
        }
    }

    IEnumerator LaunchLoop()
    {
        while (true)
        {
            InstantiateBall();
            yield return new WaitForSeconds(interval);
        }
    }

    [Header("Random Serve Settings")]
    [SerializeField] private Transform playerCourtCenter; // Un GameObject vacío en el centro de la cancha del jugador
    [SerializeField] private float courtWidth = 4f;       // Ancho total del área válida de saque (Eje X)
    [SerializeField] private float courtDepth = 5f;       // Largo/Profundidad del área válida de saque (Eje Z)
    [SerializeField] private float floorY = 0f;           // Altura del piso (Eje Y)

    /// <summary>
    /// Devuelve un Vector3 aleatorio exactamente sobre el piso dentro de los límites de la cancha.
    /// </summary>
    public Vector3 GetRandomBouncePoint()
    {
        // Calculamos los extremos basados en el centro de la cancha
        float minX = playerCourtCenter.position.x - (courtWidth / 2f);
        float maxX = playerCourtCenter.position.x + (courtWidth / 2f);

        float minZ = playerCourtCenter.position.z - (courtDepth / 2f);
        float maxZ = playerCourtCenter.position.z + (courtDepth / 2f);

        // Elegimos coordenadas aleatorias dentro de ese rectángulo
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Retornamos el punto exacto en el piso
        return new Vector3(randomX, floorY, randomZ);
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCourtCenter != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(new Vector3(playerCourtCenter.position.x, floorY, playerCourtCenter.position.z), new Vector3(courtWidth, 0.1f, courtDepth));
        }
    }
}
