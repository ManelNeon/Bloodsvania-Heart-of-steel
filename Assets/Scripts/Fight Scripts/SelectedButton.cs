using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedButton : MonoBehaviour
{
    [SerializeField] GameObject hand;

    public void PointerEnter()
    {
        hand.SetActive(true);
        hand.GetComponent<UIAnimation>().StartCoroutine(hand.GetComponent<UIAnimation>().AnimatingSprite());
    }

    public void PointerExit()
    {
        hand.SetActive(false);
    }
}
