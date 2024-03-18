using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // creating a static Instance
    public static GameManager Instance;

    //getting the fight scene
    [SerializeField] GameObject FightScene;

    //getting the walk scene
    [SerializeField] GameObject WalkScene;

    // Start is called before the first frame update
    void Start()
    {
        //checking if there's other Instance
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        //if there isnt, this is the Instance
        Instance = this;
    }

    //activating the fight scene and deactivating the walk scene
    public void ActivateFightScene()
    {
        WalkScene.SetActive(false); 
        FightScene.SetActive(true);
    }

    //activating the walk scene and deactivating the fight scene
    public void ActivateWalkScene()
    {
        FightScene.SetActive(false);
        WalkScene.SetActive(true);
    }

}
