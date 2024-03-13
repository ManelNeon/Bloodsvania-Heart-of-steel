using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //variable that controls the speed of the player
    [SerializeField] float moveSpeed;

    //variable that checks if the player is moving
    bool isMoving;

    //storing input
    Vector2 input;

    void Update()
    {
        //start movement function
        StartMovement();
    }

    //start movement function
    void StartMovement()
    {
        //checks if he isnt moving
        if (!isMoving)
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
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                StartCoroutine(Move(targetPos));
            }
        }
    }
    
    //IEnumerator that moves the player
    IEnumerator Move(Vector3 targetPos)
    {
        //the player is now moving
        isMoving = true;

        //if the difference between the target position and the current position is different than zero (Mathf.Epsilon is a very very very small number)
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //moving towards the new position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            //end the function, repeat it until the player is in the same position as the targetpos;
            yield return null;
        }

        //once the difference is miniscule (Mathf.Epsilon) we set the players position as the target position to avoid errors in the x value
        transform.position = targetPos;

        //the player is no longer moving
        isMoving = false;
    }
}
