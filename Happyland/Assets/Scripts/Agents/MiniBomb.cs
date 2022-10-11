using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBomb : Agent
{
    // Update is called once per frame
    public override void FixedUpdate()
    {
        /*Quick plan of how FSM should work.
        If finished spawning -> Wander (Another FSM)
        If agent hears or sees enemy -> Attack (move and explode)
        Attack -> Die
        */
    }
}
