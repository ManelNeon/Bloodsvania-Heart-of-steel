using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //variable that controls the speed of the player
    [SerializeField] float moveSpeed;

    //all the sprites direction
    [SerializeField] Sprite[] spritesDirection;

    //the sprite rendered
    SpriteRenderer spriteRenderer;

    //the players rigidbody
    Rigidbody2D playerRb;

    //the players animator
    Animator animator;

    //variable that checks if the player is moving
    bool isMoving;

    //the players look direction
    Vector2 lookDirection = new Vector2(1, 0);

    //storing input
    Vector2 input;

    private void Start()
    {
        //getting the players rb
        playerRb = GetComponent<Rigidbody2D>();

        //getting the players animator
        animator = GetComponent<Animator>();

        //getting the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //if the player isnt moving and he presses F
        if (Input.GetKeyDown(KeyCode.F) && !isMoving)
        {
            //we raycast in the direction the player is looking (the ray is bigge on the sides so that it reaches the NPC)
            RaycastHit2D hit;

            if (lookDirection.x != 0)
            {
                hit = Physics2D.Raycast(playerRb.position + Vector2.down * 0.2f, lookDirection, .8f, LayerMask.GetMask("NPC"));
            }
            else
            {
                hit = Physics2D.Raycast(playerRb.position + Vector2.down * 0.2f, lookDirection, .3f, LayerMask.GetMask("NPC"));
            }

            //if it hits a NPC character we start the dialogue
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();

                if (character != null)
                {
                    character.StartDialogue();
                }
            }
        }

        //getting the inputs
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //function that changes the players direction according to the inputs
        ChangingPlayersDireciton();

        //if the input is zero (no input)
        if (input == Vector2.zero)
        {
            //we fix the sprite and we are no longer moving
            FixingSprite();
            isMoving = false;
        }

        //if the input is different than zero
        if (input != Vector2.zero)
        {
            // the animator is enabled and the player is moving
            animator.enabled = true;
            isMoving = true;
        }
    }

    //function that fixes the sprite
    public void FixingSprite()
    {
        // we disable the animator
        animator.enabled = false;

        //we check the look direction and put the sprites direction to the corresponding one
        if (lookDirection == Vector2.right)
        {
            spriteRenderer.sprite = spritesDirection[2];
            return;
        }
        if (lookDirection == Vector2.left)
        {
            spriteRenderer.sprite = spritesDirection[3];
            return;
        }
        if (lookDirection == Vector2.up)
        {
            spriteRenderer.sprite = spritesDirection[1];
            return;
        }
        if (lookDirection == Vector2.down)
        {
            spriteRenderer.sprite = spritesDirection[0];
            return;
        }
    }

    //on the fixed update we start the movement
    void FixedUpdate()
    {
        //start movement function
        StartMovement();

    }

    //start movement function
    void StartMovement()
    {

        //stopping the player from moving diagonally by making it so that if he's pressing A/D, the y value is 0
        if (input.x != 0)
        {
            input.y = 0;
        }

        //if the y input is differnt than zero we put the x input to zero
        if (input.y != 0)
        {
            input.x = 0;
        }

        //these two checks make it so that the player cannot move diagonally

        //if the input is different than zero we move the player
        if (input != Vector2.zero)
        {
            Vector2 position = playerRb.position;
            position.x = position.x + moveSpeed * input.x * Time.deltaTime;
            position.y = position.y + moveSpeed * input.y * Time.deltaTime;
            playerRb.MovePosition(position);
        }
    }
    
    //function that changes the players direction
    void ChangingPlayersDireciton()
    {
        //according to the input, the look direction changes, the sprite too, and the animator's float is changed too
        if (input.x > 0)
        {
            lookDirection = Vector2.right;

            spriteRenderer.sprite = spritesDirection[2];

            animator.SetFloat("walkUp", 0);
            animator.SetFloat("walkDown", 0);
            animator.SetFloat("walkLeft", 0);
            animator.SetFloat("walkRight", 1);

            return;
        }
        if (input.x < 0)
        {
            lookDirection = Vector2.left;

            spriteRenderer.sprite = spritesDirection[3];

            animator.SetFloat("walkUp", 0);
            animator.SetFloat("walkDown", 0);
            animator.SetFloat("walkRight", 0);
            animator.SetFloat("walkLeft", 1);

            return;
        }
        if (input.y > 0)
        {
            lookDirection = Vector2.up;

            spriteRenderer.sprite = spritesDirection[1];

            animator.SetFloat("walkDown", 0);
            animator.SetFloat("walkRight", 0);
            animator.SetFloat("walkLeft", 0);
            animator.SetFloat("walkUp", 1);

            return;
        }
        if (input.y < 0)
        {
            lookDirection = Vector2.down;

            spriteRenderer.sprite = spritesDirection[0];

            animator.SetFloat("walkRight", 0);
            animator.SetFloat("walkLeft", 0);
            animator.SetFloat("walkUp", 0);
            animator.SetFloat("walkDown", 1);

            return;
        }
    }
}
