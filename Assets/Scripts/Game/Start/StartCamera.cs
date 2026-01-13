using UnityEngine;

public class StartCamera : MonoBehaviour
{
    [SerializeField] private float moveDistance;

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
    }
}
