using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimation : MonoBehaviour
{
    //UI Animation Code, first we get the image object from the UI
    [SerializeField] Image m_Image;

    //Then we create an array with all the sprites from the animation
    [SerializeField] Sprite[] m_SpriteArray;

    //this is the speed for the animation
    [SerializeField] float m_Speed = .02f;

    //getting the index for the animation
    int m_IndexSprite;

    //on awake so that it starts again when the object is activated and deactivated
    void Awake()
    {
        StartCoroutine(Func_PlayAnimUI());
    }

    //Function starting the animation
    IEnumerator Func_PlayAnimUI()
    {
        //time between frames
        yield return new WaitForSeconds(m_Speed);

        //if the index is bigger than the length, it will become zero basically restarting the animation
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            m_IndexSprite = 0;
        }
        
        //we change the sprite in the image to the sprite in the corresponding array
        m_Image.sprite = m_SpriteArray[m_IndexSprite];

        //we add one 
        m_IndexSprite += 1;
    }
}
