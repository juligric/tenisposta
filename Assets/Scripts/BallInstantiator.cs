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
        if (ballController != null)
        {
            ballController.LaunchTowards(bouncePointTR.position, serviceForce, BallController.BallState.EnemyServe);
            Destroy(ballController.gameObject, 8);
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
}
