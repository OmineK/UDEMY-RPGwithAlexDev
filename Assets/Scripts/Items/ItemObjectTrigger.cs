using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    ItemObject myItemObject => GetComponentInParent<ItemObject>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<PlayerStats>().isDead) { return; }

            myItemObject.PickupItem();
        }
    }
}
