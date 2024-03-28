using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//child of the spawn manager, only difference is that this time the spawm manager will play if the player's position is bigger than the max position
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
