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

    private Rigidbody rb;
    private Vector3 targetPosition;

    // NUEVO: Para memorizar la velocidad real antes del impacto físico
    public Vector3 lastFrameVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Guardamos la velocidad de este frame ANTES de que Unity procese colisiones
        if (currentState == BallState.EnemyServe)
        {
        }
            lastFrameVelocity = rb.linearVelocity;
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

        float V_x = sX / time;
        float V_y = (sY / time) - (0.5f * Physics.gravity.y * time);

        Vector3 velocityTarget = distanceXZ.normalized * V_x;
        velocityTarget.y = V_y;

        return velocityTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 1. REBOTE EN EL PISO (Cancha del jugador)
        if (collision.gameObject.CompareTag("Floor") && (currentState == BallState.EnemyServe || currentState == BallState.PlayerReturned))
        {
            // SOLUCIÓN DEFINITIVA: Usamos la velocidad que MEMORIZAMOS en el FixedUpdate anterior
            Vector3 incomingVelocity = lastFrameVelocity;

            // Si por alguna razón la velocidad guardada era ridículamente baja, usamos un fallback
            if (incomingVelocity.magnitude < 0.5f)
            {
                incomingVelocity = -collision.relativeVelocity;
            }

            // Extraemos la dirección horizontal pura (plano XZ)
            Vector3 horizontalDirection = new Vector3(incomingVelocity.x, 0f, incomingVelocity.z);
            float horizontalSpeed = horizontalDirection.magnitude;

            // Calculamos las nuevas magnitudes artificiales del rebote
            float newVy = Mathf.Abs(incomingVelocity.y) * bounceHeightFactor;
            float newHorizontalSpeed = horizontalSpeed * bounceSpeedRetention;

            // Reseteamos por completo el Rigidbody para anular la fricción de Unity
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Despegamos la pelota hacia arriba para evitar que el colisionador se quede enganchado
            transform.position += Vector3.up * 0.08f;

            // Reconstruimos el vector de velocidad apuntando hacia adelante y arriba
            Vector3 newVelocity = (horizontalDirection.normalized * newHorizontalSpeed);
            newVelocity.y = newVy;

            // Asignamos la velocidad calculada de forma manual
            rb.linearVelocity = newVelocity;

            // Al final, cambia el estado según quién haya pateado/tocado la pelota si lo necesitas,
            // o simplemente déjala en un estado neutral de pique.
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