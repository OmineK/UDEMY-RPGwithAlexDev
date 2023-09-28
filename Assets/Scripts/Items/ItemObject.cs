using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] Rigidbody2D rb;


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    void SetupVisuals()
    {
        if (itemData == null) { return; }

        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void PickupItem()
    {
        if (!Inventory.instance.CanAddItem() && (itemData.itemType == ItemType.Equipment))
        {
            rb.velocity = new Vector2(0, 7);
            return; 
        }

        AudioManager.instance.PlaySFX(18, transform);

        Inventory.instance.AddItem(itemData);
        Destroy(this.gameObject);
    }
}
