using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private BallController ballController;
    public Transform puntoPiqueCanchaJugador;
    public float duration;
    private InputAction launchAction;

    void Awake()
    {
        // Try to auto-assign if not set in Inspector
        if (ballController == null)
        {
            ballController = GetComponent<BallController>();
        }

        // Create an InputAction for the "launch" control: Enter key and gamepad South (A/X)
        launchAction = new InputAction("Launch", binding: "<Keyboard>/enter");
        launchAction.AddBinding("<Gamepad>/buttonSouth");
        launchAction.performed += OnLaunchPerformed;
    }

    void OnEnable()
    {
        launchAction?.Enable();
    }

    void OnDisable()
    {
        launchAction?.Disable();
    }

    void OnDestroy()
    {
        if (launchAction != null)
        {
            launchAction.performed -= OnLaunchPerformed;
            launchAction.Dispose();
            launchAction = null;
        }
    }

    private void OnLaunchPerformed(InputAction.CallbackContext ctx)
    {
        if (ballController == null || puntoPiqueCanchaJugador == null)
            return;

        // Example enemy serve: send the ball from its current position to the player's court point in 1.5 seconds
        ballController.LaunchTowards(puntoPiqueCanchaJugador.position, duration, BallController.BallState.EnemyServe);
    }
}
