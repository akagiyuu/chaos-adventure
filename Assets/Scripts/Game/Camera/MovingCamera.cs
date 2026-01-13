using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    [SerializeField] private float moveDistance = 0.1f;

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
    }
}