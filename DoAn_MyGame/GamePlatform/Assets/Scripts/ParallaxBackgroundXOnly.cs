using UnityEngine;

public class ParallaxBackgroundXOnly : MonoBehaviour
{
    private float length, startPosX;
    public GameObject Camera;
    public float parallaxEffect;
    public float yOffset;

    void Start()
    {
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float tempX = Camera.transform.position.x * (1 - parallaxEffect);
        float distX = Camera.transform.position.x * parallaxEffect;
        float posY = Camera.transform.position.y + yOffset;

        transform.position = new Vector3(startPosX + distX, posY, transform.position.z);

        if (tempX > startPosX + length)
        {
            startPosX += length;
        }
        else if (tempX < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
