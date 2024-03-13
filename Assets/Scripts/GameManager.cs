using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // creating a static Instance
    public static GameManager Instance;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
