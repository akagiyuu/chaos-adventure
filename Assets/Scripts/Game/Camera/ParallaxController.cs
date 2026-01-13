using UnityEngine;

public class ParallaxController : MonoBehaviour
{

    [SerializeField] private ParallaxLayer[] layers;

    void Awake()
    {
        foreach (var layer in layers)
        {
            layer.Awake();
        }
    }

    void FixedUpdate()
    {
        foreach (var layer in layers)
        {
            layer.FixedUpdate(transform.position);
        }
    }
}