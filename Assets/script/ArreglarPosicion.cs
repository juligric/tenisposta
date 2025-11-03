using UnityEngine;

public class ArreglarPosicion : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void LateUpdate()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
