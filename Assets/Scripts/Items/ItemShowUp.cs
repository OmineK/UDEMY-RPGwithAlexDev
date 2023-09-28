using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShowUp : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] BoxCollider2D collision;
    [SerializeField] BoxCollider2D trigger;

    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (boss != null)
        {
            sr.enabled = false;
            collision.enabled = false;
            trigger.enabled = false;
        }
        else
        {
            sr.enabled = true;
            collision.enabled = true;
            trigger.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            collision.transform.position += new Vector3(1, 0);
        }
    }
}
