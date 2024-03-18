using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //variables that sets the speed for the enemy
    [SerializeField] float speed = 3f;

    //rigidbody variable
    Rigidbody2D rb;

    //player's transform variable
    Transform player;

    //variable that stores the movement
    Vector2 movement;

    //variable that checks if the enemy is warned
    bool warned;

    [SerializeField] int enemyCode;
    void Start()
    {
        //getting the rigidbody
        rb = GetComponent<Rigidbody2D>();

        //getting the player's transform
        player = GameObject.Find("Player").transform;

        //the npc starts not warned
        warned = false;
    }
    
    //when the player enters the trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        //checks if its the player
        if (collision.gameObject.CompareTag("Player"))
        {
            //the player is now warned
            warned = true;

            //create the direction vector, which is the subtraction between the player and the enemys position
            Vector3 direction = collision.transform.position - transform.position;

            //normalizing it, so its either 1 or -1
            direction.Normalize();

            //storing it in the movement variable
            movement = direction;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        GameManager.Instance.ActivateFightScene(enemyCode);
    }

    //if the player exits the trigger
    void OnTriggerExit2D(Collider2D collision)
    {
        //checks if it is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            //it is not warned anymore
            warned = false;
        }
    }


    void Update()
    {
        //if it is warned
        if (warned)
        {
            //does the same process with the direction
            Vector3 direction = player.transform.position - transform.position;

            direction.Normalize();

            movement = direction;
        }
    }

    void FixedUpdate()
    {
        // if it is warned, it will move
        if (warned)
        {
            MoveCharacter(movement);
        }
    }

    //moves the enemy
    void MoveCharacter(Vector2 direction)
    {
        //making the difference between the x and the y, so that the enemy doenst move diagonally, prioritizing the x axis

        //checks if the direction (the movement) is not between 0.01 or -0.01, if it isnt, it will move
        if (!(direction.x < 0.01 && direction.x > -0.01))
        {
            Vector2 goPos = new Vector2(direction.x, 0);
            goPos.Normalize(); // we normalize it again because we dont want the enemy to slow down when close to the player
            rb.MovePosition((Vector2)transform.position + (goPos * speed * Time.deltaTime));
        }
        else if (!(direction.y < 0.01 && direction.y > -0.01))
        {
            Vector2 goPos = new Vector2(0, direction.y);
            goPos.Normalize();
            rb.MovePosition((Vector2)transform.position + (goPos * speed * Time.deltaTime));
        }
    }
}
