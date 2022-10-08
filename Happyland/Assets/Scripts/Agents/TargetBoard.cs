using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBoard : Agent
{
   public override void Update()
    {
        /* Quick Plan
        After Spawning -> Idle
        Sees enemy 40% chance to -> Evade State
            -> Cannot see enemy -> Idle
            -> If has zero health -> Die State
        Sees enemy 60% change to -> Attack State
            -> Cannot see enemy => Idle
            -> If has zero health -> Die State
        */
    }
}
