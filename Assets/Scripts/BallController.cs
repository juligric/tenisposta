using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    public enum BallState { Idle, EnemyServe, PlayerCourtBounce, PlayerReturned, OutOfBounds }
    public BallState currentState = BallState.Idle;

    [Header("Bounce Settings (Floor)")]
    [SerializeField] private float bounceHeightFactor = 0.8f;
    [SerializeField] private float bounceSpeedRetention = 0.75f;

    [Header("Arcade Trajectory Settings")]
    [Tooltip("Altura extra sobre el punto más alto que se le sumará al tiro para asegurar que pase la red.")]
    [SerializeField] private float arcAssistanceHeight = 2.0f;

    private Rigidbody rb;
    private Vector3 targetPosition;

    // Guardamos la velocidad real antes del impacto físico
    public Vector3 lastFrameVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // CORREGIDO: Guardamos la velocidad en todo momento que la bola se mueva para usarla de respaldo
        if (currentState != BallState.Idle)
        {
            lastFrameVelocity = rb.linearVelocity;
        }
    }

    public void LaunchTowards(Vector3 target, float duration, BallState newState)
    {
        currentState = newState;
        targetPosition = target;

        rb.isKinematic = false;
        rb.linearVelocity = CalculateVelocityToTarget(transform.position, target, duration);

        // Inicializamos la velocidad de respaldo
        lastFrameVelocity = rb.linearVelocity;
    }

    private Vector3 CalculateVelocityToTarget(Vector3 start, Vector3 end, float time)
    {
        Vector3 distance = end - start;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float sX = distanceXZ.magnitude;
        float sY = distance.y;

        // 1. Calculamos la velocidad horizontal constante para llegar a tiempo
        float V_x = sX / time;

        // 2. CALCULAR VELOCIDAD VERTICAL ASISTIDA (Para superar la red)
        float V_y;

        // Si el tiro es del jugador, nos aseguramos de que dibuje un arco alto para pasar la red
        if (currentState == BallState.PlayerReturned)
        {
            // Determinamos el punto más alto entre el origen y el destino, y le sumamos la asistencia
            float highestPoint = Mathf.Max(start.y, end.y);
            float targetApexHeight = highestPoint + arcAssistanceHeight;

            float g = Mathf.Abs(Physics.gravity.y);
            float h1 = targetApexHeight - start.y;

            // Ecuación cinemática para obtener la velocidad vertical necesaria para alcanzar esa altura máxima ($V_y = \sqrt{2gh}$)
            V_y = Mathf.Sqrt(2f * g * h1);

            // Corrección matemática: Si el destino está más abajo que el origen, la gravedad haría que caiga antes.
            // Para mantener la consistencia del tiempo de vuelo exacto, ajustamos sutilmente si da un valor plano:
            float standardVy = (sY / time) - (0.5f * Physics.gravity.y * time);
            V_y = Mathf.Max(V_y, standardVy);
        }
        else
        {
            // Tiro estándar (Saques enemigos o piques normales)
            V_y = (sY / time) - (0.5f * Physics.gravity.y * time);
        }

        // 3. Recomponer el vector final de velocidad lineal
        Vector3 velocityTarget = distanceXZ.normalized * V_x;
        velocityTarget.y = V_y;

        return velocityTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1. REBOTE EN EL PISO (Cancha del jugador o Cancha enemiga)
        if (collision.gameObject.CompareTag("Floor") && (currentState == BallState.EnemyServe || currentState == BallState.PlayerReturned))
        {
            Vector3 incomingVelocity = lastFrameVelocity;

            if (incomingVelocity.magnitude < 0.5f)
            {
                incomingVelocity = -collision.relativeVelocity;
            }

            Vector3 horizontalDirection = new Vector3(incomingVelocity.x, 0f, incomingVelocity.z);
            float horizontalSpeed = horizontalDirection.magnitude;

            float newVy = Mathf.Abs(incomingVelocity.y) * bounceHeightFactor;
            float newHorizontalSpeed = horizontalSpeed * bounceSpeedRetention;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.position += Vector3.up * 0.08f;

            Vector3 newVelocity = (horizontalDirection.normalized * newHorizontalSpeed);
            newVelocity.y = newVy;

            rb.linearVelocity = newVelocity;

            currentState = BallState.Idle;
            return;
        }
    }

    public void ResetBall(Vector3 position)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = position;
        currentState = BallState.Idle;
    }
}