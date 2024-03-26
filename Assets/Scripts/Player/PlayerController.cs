using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //variable that controls the speed of the player
    [SerializeField] float moveSpeed;

    [SerializeField] Sprite[] spritesDirection;

    SpriteRenderer spriteRenderer;

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

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
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

        //if he isnt, we will record the inputs in the input variable
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        ChangingPlayersDireciton();

        if (input == Vector2.zero)
        {
            FixingSprite();
            isMoving = false;
        }

        if (input != Vector2.zero)
        {
            animator.enabled = true;
            isMoving = true;
        }
    }

    public void FixingSprite()
    {
        animator.enabled = false;

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
        }
        if (lookDirection == Vector2.down)
        {
            spriteRenderer.sprite = spritesDirection[0];
        }
    }

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

        if (input.y != 0)
        {
            input.x = 0;
        }

        if (input != Vector2.zero)
        {
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
