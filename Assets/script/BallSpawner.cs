using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    public float initialSpeed = 6f;

    void Start()
    {
        if (spawnPoint == null) spawnPoint = transform;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            GameObject ball = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.linearVelocity = spawnPoint.forward * initialSpeed;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
