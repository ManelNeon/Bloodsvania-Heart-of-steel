using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerHard : SpawnManager
{

    // Update is called once per frame
    public override void Update()
    {
        if (playerPosition.position.x < maxPosition)
        {
            isPlaying = false;

        }

        if (playerPosition.position.x > maxPosition)
        {
            isPlaying = true;
        }
    }
}
