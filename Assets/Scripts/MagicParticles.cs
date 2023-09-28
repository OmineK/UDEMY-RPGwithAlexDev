using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicParticles : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if ((player != null) && (player.isGroundDetected() == false))
        {
            player.rb.AddForce(new Vector2(0, 20f));
        }
    }
}
