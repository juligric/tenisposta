using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class JugadorMovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    public Transform mano;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.x += 1;
            if (Keyboard.current.sKey.isPressed) input.x -= 1;
            if (Keyboard.current.aKey.isPressed) input.y -= 1;
            if (Keyboard.current.dKey.isPressed) input.y += 1;
        }

        Vector3 move = new Vector3(input.x, 0, input.y);
        controller.Move(move * velocidad * Time.deltaTime);

        if (move != Vector3.zero)
            transform.forward = move.normalized;

        if (mano != null)
            mano.position = transform.position + transform.forward * 1f;
    }
}

