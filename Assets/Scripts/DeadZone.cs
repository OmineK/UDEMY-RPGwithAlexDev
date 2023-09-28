using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CharacterStats>() != null)
        {
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.GetComponent<CharacterStats>().KillEntity();
        }
        else
            Destroy(collision.gameObject);
    }
}
