using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Regular Sword info")]
    [SerializeField] private float regularGravity = 3.5f;
    [SerializeField] private float rotationSwordHitDistance = 0.15f;
    [SerializeField] private float freezeDuration = 0.7f;

    [Header("Bounce Sword info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity = 3.5f;
    [SerializeField] private float maxBounceDistance = 20;
    [SerializeField] private float bounceSpeed = 20;

    [Header("Pierce Sword info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity = 0.1f;

    [Header("Spin Sword info")]
    [SerializeField] private float maxSpinDistance = 7f;
    [SerializeField] private float spinDuration = 1.5f;
    [SerializeField] private float spinGravity = 0.1f;
    [SerializeField] private float spinMoveSpeed = 2f;
    [SerializeField] private float hitCoolDown = 0.35f;

    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    
    private float swordGravity;
    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetUpGravity();
    }

    private void SetUpGravity()
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
            default:
                swordGravity = regularGravity;
                return;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(
                AimDirection().normalized.x * launchForce.x,
                AimDirection().normalized.y * launchForce.y
             );

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordSript = newSword.GetComponent<Sword_Skill_Controller>();

        SwitchSword(newSwordSript);

        newSwordSript.SetUpSword(finalDir, swordGravity, rotationSwordHitDistance, freezeDuration, player);

        player.AssignNewSword(newSword);

        ActiveDots(false);
    }

    private void SwitchSword(Sword_Skill_Controller newSwordSript)
    {
        switch (swordType)
        {
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                newSwordSript.SetUpBounce(true, bounceAmount, maxBounceDistance, bounceSpeed);
                break;
            case SwordType.Pierce:
                swordGravity = pierceGravity;
                newSwordSript.SetUpPeirce(pierceAmount);
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                newSwordSript.SetUpSpin(true, maxSpinDistance, spinDuration, hitCoolDown, spinMoveSpeed);
                break;
        }
    }

    #region Aim 

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    public void ActiveDots(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
}
