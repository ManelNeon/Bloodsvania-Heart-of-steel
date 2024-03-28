using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the code that contains the spawn manager
public class SpawnManager : MonoBehaviour
{
    //we check if the spawn manager is playing
    [HideInInspector] public bool isPlaying;

    //we get the enemies prefab
    [SerializeField] GameObject[] enemyPrefabs;

    //we get the positions the enemies will spawn on
    [SerializeField] Transform[] enemySpawnPositions;

    //we get the player's position
    public Transform playerPosition;

    //we get the maximum position (necessary as there are two spawn managers, a easy one and a hard one, according to the player's position, one will deactivate and the other activate;
    public float maxPosition = 5.4f;

    //we will store the enemies here
    GameObject[] enemy = new GameObject[7];

    //and a index
    int index;

    //on enable we destroy all the enemies that still exist, put the index to zero and start a Invoke Repeating of the enemy spawn function, with a first delay of a second and then 2.5f
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

        InvokeRepeating("EnemySpawn", 1, 2.5f);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        //in here we check if the player's position is above the max position, in case it is the spawner is no longer playing, in case it isnt, the spawner is playing
        if (playerPosition.position.x > maxPosition)
        {
            isPlaying = false;

        }

        if (playerPosition.position.x < maxPosition)
        {
            isPlaying = true;
        }
    }

    //spawning the enemies
    void EnemySpawn()
    {
        //we check if it's playing
        if (isPlaying)
        {
            //if the index is bigger than the enemis length we set it back to zero
            if (index >= enemy.Length)
            {
                index = 0;
            }

            //if the stored enemy isnt null according to the index, destroy it
            if (enemy[index] != null)
            {
                Destroy(enemy[index]);
            }

            //getting a random position for the enemies
            int randomNumberPosition = Random.Range(0, enemySpawnPositions.Length);

            //getting a random enemy 
            int randomNumberEnemy = Random.Range(0, enemyPrefabs.Length);

            Debug.Log(enemyPrefabs.Length);

            Debug.Log(randomNumberEnemy);

            Debug.Log(randomNumberPosition);

            //we instantiate that enemy in the according position and store it in the index
            enemy[index] = Instantiate(enemyPrefabs[randomNumberEnemy], enemySpawnPositions[randomNumberPosition].position, enemyPrefabs[randomNumberEnemy].transform.rotation);

            //we get a diff value that is the distance between the player and the enemy
            float diff = playerPosition.position.y - enemySpawnPositions[randomNumberPosition].position.y;

            //if that difference is bigger than ten we destroy the enemy as to not have enemies around doing nothing occupying space
            if ( diff > 10 || diff < -10)
            {
                Destroy(enemy[index]);
            }

            //we add to the index
            index++;
        }
    }
}
