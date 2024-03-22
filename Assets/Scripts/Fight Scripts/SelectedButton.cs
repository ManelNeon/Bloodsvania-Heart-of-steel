using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//Script for when you hover above a button, so that the hand appears
public class SelectedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //getting the hand, there's only one hand on the whole scene, that teleports from outside the canvas to where I want it in
    [SerializeField] Transform hand;

    //the position it will teleport to
    [SerializeField] Transform position;

    //code when pointer enters area
    public void OnPointerEnter(PointerEventData eventData)
    {
        hand.position = position.position;
    }

    //code when pointer exits area
    public void OnPointerExit(PointerEventData eventData)
    {
        hand.position = new Vector3(-694f, -658f, 0);
    }
}
