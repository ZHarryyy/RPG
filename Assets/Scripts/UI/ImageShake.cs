using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageShake : MonoBehaviour
{
    private Image image; // 参考要抖动的Image组件
    [SerializeField] private float shakeDuration = 0.5f; // 震动时长
    [SerializeField] private float shakeIntensity = 10f; // 震动强度

    private Vector3 originalImagePosition; // 原始Image位置

    private void Start()
    {
        image = GetComponent<Image>();
        originalImagePosition = image.transform.localPosition;
    }

    public IEnumerator ShakeImageEvent()
    {
        float elapsedTime = 0f; // 已经过去的时间

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
            float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

            // 计算Image的新位置
            Vector3 newPosition = originalImagePosition + new Vector3(offsetX, offsetY, 0f);

            // 更新Image位置
            image.transform.localPosition = newPosition;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 震动结束后恢复Image原始位置
        image.transform.localPosition = originalImagePosition;
    }
}
