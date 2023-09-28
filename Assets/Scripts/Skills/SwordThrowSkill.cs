using System;
using UnityEngine;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordThrowSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Sword throw skill info")]
    [SerializeField] UI_SkillTreeSlot swordThrowUnlockButton;
    public bool swordThrowUnlocked { get; private set; }
    [SerializeField] GameObject swordPref;
    [SerializeField] Vector2 launchForce;
    [SerializeField] float returnSpeed;
    [SerializeField] float swordGravity;
    [SerializeField] float freezeTimeDuration;
    Vector2 finalDir;

    [Header("Sword bouncig info")]
    [SerializeField] UI_SkillTreeSlot bounceSwordUnlockButton;
    [SerializeField] int bounceAmount;
    [SerializeField] float bounceGravity;
    [SerializeField] float bounceSpeed;

    [Header("Sword piercing info")]
    [SerializeField] UI_SkillTreeSlot pierceSwordUnlockButton;
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;

    [Header("Sword spinning info")]
    [SerializeField] UI_SkillTreeSlot spinSwordUnlockButton;
    [SerializeField] float hitCooldown;
    [SerializeField] float maxTravelDistance; 
    [SerializeField] float spinDuration;
    [SerializeField] float spinGravity;

    [Header("Passive skills")]
    [SerializeField] UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] UI_SkillTreeSlot vulnerableUnlockButton;
    public bool vulnerableUnlocked { get; private set; }


    [Header("Aim dots")]
    [SerializeField] int numberOfDots;
    [SerializeField] float spaceBeetwenDots;
    [SerializeField] GameObject dotPref;
    [SerializeField] Transform dotsParent;
    GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();

        swordThrowUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordThrow);
        bounceSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordBounce);
        pierceSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordPierce);
        spinSwordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwordSpin);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        vulnerableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnerable);
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonUp(1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetMouseButton(1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPos(i * spaceBeetwenDots);
            }
        }
    }

    void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPref, player.transform.position, transform.rotation);
        SwordController newSwordScript = newSword.GetComponent<SwordController>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.Pierce)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Sword skills unlock

    protected override void CheckUnlock()
    {
        UnlockSwordThrow();
        UnlockSwordBounce();
        UnlockSwordPierce();
        UnlockSwordSpin();
        UnlockTimeStop();
        UnlockVulnerable();
    }

    void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    void UnlockVulnerable()
    {
        if (vulnerableUnlockButton.unlocked)
            vulnerableUnlocked = true;
    }

    void UnlockSwordThrow()
    {
        if (swordThrowUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordThrowUnlocked = true;
        }
    }

    void UnlockSwordBounce()
    {
        if (bounceSwordUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    void UnlockSwordPierce()
    {
        if (pierceSwordUnlockButton.unlocked)
            swordType = SwordType.Pierce;
    }

    void UnlockSwordSpin()
    {
        if (spinSwordUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }

    #endregion

    #region Aim region

    void GenerateDots()
    {
        dots = new GameObject[numberOfDots];

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPref, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - playerPos;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    Vector2 DotsPos(float t)
    {
        Vector2 pos = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);

        return pos;
    }

#endregion

}
