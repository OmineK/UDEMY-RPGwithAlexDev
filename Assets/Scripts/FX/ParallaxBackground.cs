using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] float parallaxFX;

    float xPos;
    float length;
    GameObject cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPos = transform.position.x;
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxFX);
        float distanceToMove = cam.transform.position.x * parallaxFX;

        transform.position = new Vector2(xPos + distanceToMove, transform.position.y);

        if (distanceMoved > xPos + length)
            xPos += length;
        else if (distanceMoved < xPos - length)
            xPos -= length;
    }
}
