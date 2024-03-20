using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Stats
{
    //the xp that the enemy will drop
    public int xpDrop;

    /* 1 - Savage (strong against 2 and 3)
       2 - Machines (strong against 3 and 4)
       3 - Humans (strong agains 4 and 5)
       4 - Fulgurite (strong against 5 and 1)
       5 - Nature (strong against 1 and 2) */
    public int typing;
}
