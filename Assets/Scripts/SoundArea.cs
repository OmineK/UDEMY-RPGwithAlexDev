using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundArea : MonoBehaviour
{
    [SerializeField] int areaSoundIndex;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.instance.PlaySFX(areaSoundIndex, null);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.instance.StopSFXWithTime(areaSoundIndex);
    }
}
