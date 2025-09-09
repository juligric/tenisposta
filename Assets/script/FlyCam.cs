using UnityEngine;

public class SimpleCameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    void Update()
    {
        // Movimiento WASD
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v) * moveSpeed * Time.deltaTime;
        transform.Translate(move);

        // Mirar con el mouse
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
        transform.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.left * mouseY);
    }
}
