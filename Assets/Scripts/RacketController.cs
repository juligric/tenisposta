using UnityEngine;
using System.Collections.Generic;

public class RacketController : MonoBehaviour
{
    [Header("Racket Settings")]
    [SerializeField] private float minimumSwingSpeed = 1.5f; // Evita rebotes por tocar la pelota quieta
    [SerializeField] private float defaultReturnDuration = 1.2f; // Cuánto tarda la pelota en llegar al rival
    [SerializeField] private float maxValidAngle = 65f; // Ángulo máximo permitido respecto al frente (para tirar fuera)
    [SerializeField] private float multiplicadorTiro = 3f;
    [SerializeField] private float canchaSize = 60f;

    public bool deteccionAsistida;

    private List<Transform> targetZones = new List<Transform>();
    private Vector3 lastPosition;
    private Vector3 racketVelocity;

    [Header("Debug")]
    public Transform debugVolume; // Objeto para visualizar el vector en el editor

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calcular la velocidad de la raqueta frame a frame basada en la posición del control VR
        racketVelocity = (transform.position - lastPosition) / Time.deltaTime;


        // --- NUEVO: DETECCIÓN PREVENTIVA PARA SWINGS ULTRA RÁPIDOS ---
       
        if (deteccionAsistida)
        {
            float distanceThisFrame = racketVelocity.magnitude * Time.deltaTime;

            if (distanceThisFrame > 0.01f) // Solo si la raqueta se está moviendo
            {
                // Lanzamos un rayo invisible desde la posición anterior a la actual
                RaycastHit hit;
                Vector3 directionOfMotion = racketVelocity.normalized;

                // Buscamos si en la trayectoria del movimiento de la raqueta nos cruzamos con la pelota
                if (Physics.Raycast(lastPosition, directionOfMotion, out hit, distanceThisFrame))
                {
                    if (hit.collider.CompareTag("Ball"))
                    {
                        // Forzamos manualmente el impacto simulando el OnTriggerEnter
                        OnTriggerEnter(hit.collider);
                    }
                }
            }
        }
        // -------------------------------------------------------------
        lastPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallController ball = other.GetComponent<BallController>();
            Debug.Log("Ball hit NO TAG");
            if (ball != null && (ball.currentState == BallController.BallState.Idle || ball.currentState ==  BallController.BallState.EnemyServe))
            {
                Debug.Log("Ball hit");
                // 1. OBTENER LA DIRECCIÓN DEL MOVIMIENTO (Eje horizontal XZ)
                // Usamos la dirección en la que viaja la mano, solucionando el problema del Revés
                Vector3 movementDirection = racketVelocity;
                movementDirection.y = 0;
                movementDirection.Normalize();

                // Evaluamos el ángulo respecto al "Frente Real" de la cancha (Vector3.forward)
                float angle = Vector3.Angle(Vector3.forward, movementDirection);


                // Calculamos la posición exacta en el piso hacia donde apunta el swing de la raqueta
                float alturaDelPiso = 0f; // Ajusta esto si tu piso de Unity no está en Y = 0
                Vector3 exactLandingPosition = CalculateFloorLandingPosition(transform.position, movementDirection, alturaDelPiso,canchaSize);

                // (Opcional) Podemos actualizar el debugVolume para que se mueva físicamente al lugar donde va a caer la pelota
                if (debugVolume != null)
                {
                    // Esto le pintará a tus alumnas un objeto en el piso del rival mostrando dónde picará
                    debugVolume.position = exactLandingPosition;
                }

                // Lanzamos la pelota directo a ese punto del piso calculado matemáticamente
                ball.LaunchTowards(exactLandingPosition, defaultReturnDuration, BallController.BallState.PlayerReturned);


                // Feedback Háptico opcional para VR (Añadir aquí si usas XR Toolkit)
                PlayHitEffects(other.transform.position);
            }
        }
    }

    private Transform GetBestTargetZone(Vector3 lookDirection)
    {
        Transform bestTarget = targetZones[0];
        float closestAngle = 180f;

        foreach (Transform zone in targetZones)
        {
            Vector3 directionToZone = (zone.position - transform.position);
            directionToZone.y = 0;
            directionToZone.Normalize();

            float angleToZone = Vector3.Angle(lookDirection, directionToZone);

            if (angleToZone < closestAngle)
            {
                closestAngle = angleToZone;
                bestTarget = zone;
            }
        }

        return bestTarget;
    }

    private void PlayHitEffects(Vector3 position)
    {
        // Efectos de sonido o partículas al impactar
    }
    /// <summary>
    /// Calcula el punto exacto de impacto en el suelo basándose en la dirección del golpe.
    /// </summary>
    /// <param name="racketPosition">Posición actual de la raqueta (transform.position)</param>
    /// <param name="hitDirection">Dirección del movimiento horizontal (movementDirection)</param>
    /// <param name="floorHeight">La coordenada Y del piso (normalmente 0)</param>
    /// <param name="maxDistance">Distancia máxima permitida para que el tiro no sea infinito (ej. 30 metros)</param>
    /// <returns>Vector3 con la posición exacta de llegada en el piso</returns>
    public Vector3 CalculateFloorLandingPosition(Vector3 racketPosition, Vector3 hitDirection, float floorHeight = 0f, float maxDistance = 30f)
    {
        // 1. Aseguramos que la dirección esté normalizada y sea puramente horizontal en el plano XZ
        Vector3 directionXZ = new Vector3(hitDirection.x, 0f, hitDirection.z).normalized;

        // 2. Si por algún motivo la dirección es cero, lanzamos por defecto hacia el frente de la red
        if (directionXZ == Vector3.zero)
        {
            directionXZ = Vector3.forward;
        }

        // 3. Proyectamos el punto hacia adelante basándonos en la inclinación vertical del golpe original.
        // Como en nuestro diseño "mentimos" y forzamos una parábola perfecta, calculamos la distancia 
        // en base al ángulo de salida deseado o simplemente estiramos el vector en la dirección del swing.
        // Para que sea un sistema arcade controlado, multiplicamos la dirección del swing por la fuerza/distancia estimada:

        // Conseguimos la distancia estimada de la cancha. Si el golpe fue muy fuerte, viaja más lejos.
        // Aquí puedes usar un multiplicador arcade. Por ejemplo, que la velocidad del swing defina la distancia.
        float swingMagnitude = racketVelocity.magnitude;
        float estimatedDistance = Mathf.Clamp(swingMagnitude * multiplicadorTiro, 5f, maxDistance);

        // 4. Calculamos el punto final en el espacio plano
        Vector3 landingPoint = racketPosition + (directionXZ * estimatedDistance);

        // 5. Forzamos que la altura (Y) sea exactamente la del suelo
        landingPoint.y = floorHeight;

        return landingPoint;
    }
}