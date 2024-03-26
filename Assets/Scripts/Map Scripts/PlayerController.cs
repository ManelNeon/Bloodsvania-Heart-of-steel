using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //variable that controls the speed of the player
    [SerializeField] float moveSpeed;

    Rigidbody2D playerRb;

    Animator animator;

    //variable that checks if the player is moving
    bool isMoving;

    Vector2 lookDirection = new Vector2(1, 0);

    //storing input
    Vector2 input;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (lookDirection.x != 0)
        {
            Debug.DrawRay(playerRb.position + Vector2.down * 0.2f, lookDirection * .8f, Color.red);
        }
        if (lookDirection.y != 0)
        {
            Debug.DrawRay(playerRb.position + Vector2.down * 0.2f, lookDirection * .1f, Color.red);
        }

        if (Input.GetKeyDown(KeyCode.F) && !isMoving)
        {
            RaycastHit2D hit;

            if (lookDirection.x != 0)
            {
                hit = Physics2D.Raycast(playerRb.position + Vector2.down * 0.2f, lookDirection, .8f, LayerMask.GetMask("NPC"));
            }
            else
            {
                hit = Physics2D.Raycast(playerRb.position + Vector2.down * 0.2f, lookDirection, .3f, LayerMask.GetMask("NPC"));
            }

            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();

                if (character != null)
                {
                    character.StartDialogue();
                }
            }
        }

        animator.SetFloat("speed_f", input.magnitude);
    }

    void FixedUpdate()
    {
        //start movement function
        StartMovement();
    }

    //start movement function
    void StartMovement()
    {
        //if he isnt, we will record the inputs in the input variable
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //stopping the player from moving diagonally by making it so that if he's pressing A/D, the y value is 0
        if (input.x != 0)
        {
            input.y = 0;
        }

        /*if the input is different than zero (the player isnt pressing anything = 0) we will create a new vector in which we store the player's position and
          then we add the inputs (its either 1 or -1), then we send that variable to a Coroutine*/
        if (input != Vector2.zero)
        {
            ChangingPlayersDireciton();

            Vector2 position = playerRb.position;
            position.x = position.x + moveSpeed * input.x * Time.deltaTime;
            position.y = position.y + moveSpeed * input.y * Time.deltaTime;
            playerRb.MovePosition(position);
        }
    }
    
    void ChangingPlayersDireciton()
    {
        if (input.x > 0)
        {
            lookDirection = new Vector2(1, 0);

            animator.SetInteger("walkDirection_i", 2);

            return;
        }
        if (input.x < 0)
        {
            lookDirection = new Vector2(-1, 0);

            animator.SetInteger("walkDirection_i", 3);

            return;
        }
        if (input.y > 0)
        {
            lookDirection = new Vector2(0, 1);

            animator.SetInteger("walkDirection_i", 1);

            return;
        }
        if (input.y < 0)
        {
            lookDirection = new Vector2(0, -1);

            animator.SetInteger("walkDirection_i", 0);

            return;
        }
    }
}
