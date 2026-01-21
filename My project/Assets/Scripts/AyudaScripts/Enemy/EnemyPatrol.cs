using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;

    public bool showGizmos;

    void Start()
    {
        transform.LookAt(new Vector3(patrolPoints[currentPointIndex].position.x, transform.position.y, patrolPoints[currentPointIndex].position.z), Vector3.up);
    }

    void Update()
    {
        if (patrolPoints.Length == 0) return;
        Vector3 targetPoint = new Vector3(patrolPoints[currentPointIndex].position.x, transform.position.y, patrolPoints[currentPointIndex].position.z);
        Vector3 direction = (targetPoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if ((targetPoint - transform.position).magnitude < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            transform.LookAt(new Vector3(patrolPoints[currentPointIndex].position.x, transform.position.y, patrolPoints[currentPointIndex].position.z), Vector3.up);
        }
    }
}
