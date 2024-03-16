using UnityEngine;

public class Chest : MonoBehaviour
{
    private ItemDrop itemDrop;
    private SpriteRenderer sp;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeIntensity = 0.1f;

    private Vector3 initialPosition;
    private float shakeTimer = 0f;
    private int spriteIndex = 0;

    private int attackCountThreshold;
    private int attackCount = 0;

    public bool isOpened;

    private void Start()
    {
        initialPosition = transform.position;
        itemDrop = GetComponent<ItemDrop>();
        sp = GetComponent<SpriteRenderer>();
        attackCountThreshold = sprites.Length;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetZ = Random.Range(-1f, 1f) * shakeIntensity;

            transform.position = initialPosition + new Vector3(offsetX, offsetY, offsetZ);

            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = initialPosition;
        }
    }

    public void StartShakeAndChange()
    {
        if (isOpened) return;

        shakeTimer = shakeDuration;

        if (sprites.Length > 0)
        {
            if (sp != null)
            {
                sp.sprite = sprites[spriteIndex];
                spriteIndex = (spriteIndex) + 1 % sprites.Length;
            }
        }

        attackCount++;

        if (attackCount >= attackCountThreshold)
        {
            itemDrop.GenerateDrop();
            isOpened = true;
        }
    }
}
