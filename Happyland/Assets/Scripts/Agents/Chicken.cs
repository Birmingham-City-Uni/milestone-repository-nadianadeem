using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Agent
{
    // Start is called before the first frame update
    public override void Start()
    {
        pathfindingComponent.grid = GameObject.FindGameObjectWithTag("OutsideGrid").GetComponent<Grid>();
    }
}
