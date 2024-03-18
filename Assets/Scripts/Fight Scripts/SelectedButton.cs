using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for when you hover above a button, so that the hand appears
public class SelectedButton : MonoBehaviour
{
    //getting the hand, there's only one hand on the whole scene, that teleports from outside the canvas to where I want it in
    [SerializeField] Transform hand;

    //the position it will teleport to
    [SerializeField] Transform position;

    //the code for when the pointer enters the buttons range
    public void PointerEnter()
    {
        //putting the hand in the position i want it in
        hand.position = position.position;
    }

    //the code for when the pointer exits the button range
    public void PointerExit()
    {
        //teleports really far away from the canvas
        hand.position = new Vector3(-694f, -658f, 0);
    }
}
