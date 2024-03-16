using UnityEngine;

public class ThunderFieldController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private GameObject thunderPrefab;
    [SerializeField] private float thunderCooldown;
    private float lastTimeSpawn;

    private bool playerInArea = false;

    private void Update()
    {
        if (playerInArea)
        {
            lastTimeSpawn -= Time.deltaTime;

            if (lastTimeSpawn <= 0)
            {
                SpawnThunder();
                lastTimeSpawn = thunderCooldown;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            playerInArea = false;
        }
    }

    public void SpawnThunder()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        Vector3 thunderPosition = new Vector3(x, y);

        GameObject newThunder = Instantiate(thunderPrefab, thunderPosition, Quaternion.identity);
        newThunder.GetComponent<ThunderFxController>().FindPosition();
    }
}
