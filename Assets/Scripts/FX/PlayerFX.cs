using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After image FX")]
    [SerializeField] GameObject afterImagePref;
    [SerializeField] float colorLooseRate;
    [SerializeField] float afterImageCooldown;
    float afterImageCooldownTimer;

    [Header("Screen shake FX")]
    [SerializeField] float shakeMultiplier;
    public Vector3 shakeSwordCatchImpact;
    public Vector3 shakeHighDmgImpact;
    CinemachineImpulseSource screenShake;

    [Header("Sword catch FX")]
    [SerializeField] ParticleSystem dustFX;

    protected override void Start()
    {
        base.Start();

        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            GameObject newAfterImage = Instantiate(afterImagePref, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
            afterImageCooldownTimer = afterImageCooldown;
        }
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }
}
