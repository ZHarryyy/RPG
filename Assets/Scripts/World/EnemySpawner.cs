using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyWave
    {
        public List<GameObject> enemyPrefabs;
        public List<int> quantities;
    }

    public List<EnemyWave> waveList;
    public float spawnInterval = 1f;
    public float timeBetweenWaves = 5f;

    private int currentWaveIndex;
    private List<GameObject> spawnedEnemies;

    private void Start()
    {
        spawnedEnemies = new List<GameObject>();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (currentWaveIndex < waveList.Count)
        {
            EnemyWave currentWave = waveList[currentWaveIndex];

            // 生成当前波次的敌人
            for (int i = 0; i < currentWave.enemyPrefabs.Count; i++)
            {
                GameObject enemyPrefab = currentWave.enemyPrefabs[i];
                int quantity = currentWave.quantities[i];

                for (int j = 0; j < quantity; j++)
                {
                    GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                    spawnedEnemies.Add(enemy);
                    yield return new WaitForSeconds(spawnInterval);
                }
            }

            // 等待所有敌人死亡
            yield return new WaitUntil(() => AllEnemiesDead());

            // 等待一段时间后刷新下一波敌人
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWaveIndex++;
        }
    }

    private bool AllEnemiesDead()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }

        return spawnedEnemies.Count == 0;
    }
}