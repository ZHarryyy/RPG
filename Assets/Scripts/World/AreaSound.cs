using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;
    private Coroutine fadeCoroutine;
    private float currentVolume;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if(fadeCoroutine != null)
            {
                currentVolume = AudioManager.instance.sfx[areaSoundIndex].volume;
                StopCoroutine(fadeCoroutine);
            }

            if(AudioManager.instance.sfx[areaSoundIndex].isPlaying == false)
            {
                currentVolume = 0f;
            }

            AudioManager.instance.sfx[areaSoundIndex].Play();
            fadeCoroutine = StartCoroutine(AudioManager.instance.IncreaseVolumeOverTime(areaSoundIndex, currentVolume, 2f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if(fadeCoroutine != null)
            {
                currentVolume = AudioManager.instance.sfx[areaSoundIndex].volume;
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(AudioManager.instance.DecreaseVolumeOverTime(areaSoundIndex, currentVolume, 2f));
        }
    }
}
