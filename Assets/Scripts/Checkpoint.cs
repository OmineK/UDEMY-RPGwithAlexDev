using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string checkpointID;
    public bool activationStatus;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint ID")]
    void GenerateID()
    {
        checkpointID = System.Guid.NewGuid().ToString();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.GetComponent<Player>() != null) && !activationStatus)
            ActivateCheckpoint();

    }

    public void ActivateCheckpoint()
    {
        AudioManager.instance.PlaySFX(5, transform);
        activationStatus = true;
        anim.SetBool("active", true);
    }
}
