using Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Screen shake fx")]
    [SerializeField] private float shakeMultiplier;
    private CinemachineImpulseSource screenShake;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;
    public Vector3 shakeNormalDamage;

    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Space]
    [SerializeField] private ParticleSystem dustFx;
    [SerializeField] private GameObject dashFx;
    [SerializeField] private ParticleSystem cureFx;


    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }
    }

    public void PlayDustFX()
    {
        if (dustFx != null) dustFx.Play();
    }

    public void PlayCureFx()
    {
        if (cureFx != null) cureFx.Play();
    }

    public void PlayDashFX(bool _isActive)
    {
        if (_isActive)
        {
            if (dashFx != null)
            {
                dashFx.SetActive(true);
                Invoke("DestroyDashFX", 1f);
            }
        }
    }

    private void DestroyDashFX()
    {
        dashFx.SetActive(false);
    }
}
