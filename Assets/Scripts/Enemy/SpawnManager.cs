using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [HideInInspector] public bool isPlaying;

    [SerializeField] GameObject[] enemyPrefabs;

    [SerializeField] Transform[] enemySpawnPositions;

    public Transform playerPosition;

    public float maxPosition = 5.4f;

    GameObject[] enemy = new GameObject[7];

    int index;

    private void OnEnable()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i] != null)
            {
                Destroy(enemy[i]);
            }
        }

        index = 0;

        InvokeRepeating("EnemySpawn", 1, 3.5f);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (playerPosition.position.x > maxPosition)
        {
            isPlaying = false;

        }

        if (playerPosition.position.x < maxPosition)
        {
            isPlaying = true;
        }
    }

    void EnemySpawn()
    {
        if (isPlaying)
        {
            if (index >= enemy.Length)
            {
                index = 0;
            }

            if (enemy[index] != null)
            {
                Destroy(enemy[index]);
            }

            int randomNumberPosition = Random.Range(0, enemySpawnPositions.Length);

            int randomNumberEnemy = Random.Range(0, enemyPrefabs.Length);

            Debug.Log(enemyPrefabs.Length);

            Debug.Log(randomNumberEnemy);

            Debug.Log(randomNumberPosition);

            enemy[index] = Instantiate(enemyPrefabs[randomNumberEnemy], enemySpawnPositions[randomNumberPosition].position, enemyPrefabs[randomNumberEnemy].transform.rotation);

            float diff = playerPosition.position.y - enemySpawnPositions[randomNumberPosition].position.y;

            if ( diff > 10 || diff < -10)
            {
                Destroy(enemy[index]);
            }

            index++;
        }
    }
}
