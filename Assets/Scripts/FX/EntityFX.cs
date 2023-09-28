using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [Header("Flash FX")]
    [SerializeField] Material hitMat;
    Material orginalMat;

    [Header("Ailment colors")]
    [SerializeField] Color[] igniteColor;
    [SerializeField] Color[] chillColor;
    [SerializeField] Color[] shockColor;

    [Header("Ailment particles")]
    [SerializeField] ParticleSystem igniteFX;
    [SerializeField] ParticleSystem chillFX;
    [SerializeField] ParticleSystem shockFX;

    [Header("HitFX")]
    [SerializeField] GameObject normalHitFXPref;
    [SerializeField] GameObject criticalHitFXPref;

    [Header("Pop up text")]
    [SerializeField] GameObject popUpTextPref;
    [Space]

    protected SpriteRenderer sr;
    protected Player player;

    GameObject myHealthBar;

    protected virtual void Start()
    {
        myHealthBar = GetComponentInChildren<UI_HealthBar>().gameObject;
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        orginalMat = sr.material;
    }

    IEnumerator FlashFX()
    {
        sr.material = hitMat;

        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(0.15f);

        sr.color = currentColor;
        sr.material = orginalMat;
    }

    void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();

        InvokeRepeating(nameof(igniteColorFX), 0, 0.2f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();

        InvokeRepeating(nameof(chillColorFX), 0, 0.2f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();

        InvokeRepeating(nameof(shockColorFX), 0, 0.2f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar.SetActive(false);
            sr.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            sr.color = Color.white;
        }
    }

    void igniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    void chillColorFX()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    void shockColorFX()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void CreateHitFX(Transform _target, bool _critical)
    {
        float zRot = Random.Range(-90, 90);
        float xPos = Random.Range(-0.5f, 0.5f);
        float yPos = Random.Range(-0.5f, 0.5f);

        Vector3 hitFXnewRot = new Vector3(0, 0, zRot);
        Vector3 hitFXnewPos = new Vector3(xPos, yPos, 0);

        GameObject hitPrefab = normalHitFXPref;

        if (_critical)
        {
            float yRot = 0;
            int facingDir = GetComponent<Entity>().facingDir;

            zRot = Random.Range(0f, 25f);
            xPos = 0.7f;
            yPos = Random.Range(-0.3f, 0.6f);

            hitPrefab = criticalHitFXPref;

            if (facingDir == -1)
                yRot = 180;

            hitFXnewRot = new Vector3(0, yRot, zRot);
            hitFXnewPos = new Vector3(xPos, yPos, 0);
        }

        GameObject newHitFX = Instantiate(hitPrefab, _target.position, Quaternion.identity);

        if (_critical)
            newHitFX.transform.parent = _target.transform;

        newHitFX.transform.Rotate(hitFXnewRot);
        newHitFX.transform.Translate(hitFXnewPos);

        Destroy(newHitFX, 0.7f);
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-0.3f, 0.3f);
        float randomY = Random.Range(1.3f, 1.8f);

        Vector3 offset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPref, transform.position + offset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }
}
