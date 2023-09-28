using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera playerFrozenYFollowCamera;
    [SerializeField] CinemachineVirtualCamera playerFollowCamera;

    [SerializeField] float basemenstEnterX;
    [SerializeField] float basemenstEnterY;

    Player player;

    void Start()
    {
        player = PlayerManager.instance.player;
    }

    void Update()
    {
        SetupCameraSettings();
    }

    void SetupCameraSettings()
    {
        if ((player.transform.position.x > basemenstEnterX) && (player.transform.position.y < basemenstEnterY)) //eneter to the castle basemenst position
        {
            playerFollowCamera.enabled = true;
            playerFrozenYFollowCamera.enabled = false;
        }
        else
        {
            playerFollowCamera.enabled = false;
            playerFrozenYFollowCamera.enabled = true;
        }
    }
}
