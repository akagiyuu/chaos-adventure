using UnityEngine;

[System.Serializable]
class ParallaxLayer
{
    [SerializeField] private GameObject layer;

    [SerializeField]
    [Range(0, 1)]
    private float parralaxFactor;

    private float start, length;

    public void Awake()
    {
        start = layer.transform.position.x;
        length = layer.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void FixedUpdate(Vector3 camera)
    {
        float distance = camera.x * parralaxFactor;
        float movement = camera.x * (1 - parralaxFactor);
        layer.transform.position = new Vector3(start + distance, camera.y, layer.transform.position.z);

        if (movement > start + length) start += length;
        else if (movement < start - length) start -= length;
    }
}