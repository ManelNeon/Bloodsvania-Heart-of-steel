using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    //UI Animation Code, first we get the image object from the UI
    Image image;

    //Then we create an array with all the sprites from the animation
    [SerializeField] Sprite[] spriteArray;

    //this is the speed for the animation
    [SerializeField] float speed = .02f;

    //getting the index for the animation
    int indexSprite;

    //getting the image component in the start function and starting the coroutine
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(AnimatingSprite());
    }

    //function with the animation
    public IEnumerator AnimatingSprite()
    {
        while (true)
        {
            //if the index is bigger than the length, it will become zero basically restarting the animation
            if (indexSprite >= spriteArray.Length)
            {
                indexSprite = 0;
            }

            //we change the sprite in the image to the sprite in the corresponding array
            image.sprite = spriteArray[indexSprite];

            //we add one 
            indexSprite += 1;

            //time between frames
            yield return new WaitForSeconds(speed);
        }
    }
}
