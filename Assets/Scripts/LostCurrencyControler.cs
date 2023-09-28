using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrencyControler : MonoBehaviour
{
    public int currency;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.currency += currency;
            Destroy(this.gameObject);
        }
    }
}
