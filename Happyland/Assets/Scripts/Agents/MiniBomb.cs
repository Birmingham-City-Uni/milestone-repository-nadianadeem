using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniBomb : Agent
{
    public IdleWanderState idle;
    public TextMeshProUGUI nameplate;

    private bool isReadyToDie = false;

    // Start is called before the first frame update
    public override void Start()
    {
        idle = new IdleWanderState(this, stateManager);
        stateManager.InitSpawn();
        sensor = this.gameObject.GetComponent<sensors>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        stateManager.Update();

        if(stateManager.GetCurrStateOnStack().GetType() == typeof(IdleWanderState))
        {
            nameplate.text = idle.wanderStateManager.GetCurrStateOnStack().GetType().ToString().Replace("State", "");
        }
        else
        {
            nameplate.text = stateManager.GetCurrStateOnStack().GetType().ToString().Replace("State", "");
        }

        if ((sensor.Hit == true) && sensor.info.collider.gameObject.CompareTag("Player") && (stateManager.GetCurrStateOnStack().GetType() == typeof(IdleWanderState)))
        {
            stateManager.PushAttackState();
        }

        if (!isReadyToDie && Vector3.Distance(this.transform.position, sensor.info.point) < 2f && (stateManager.GetCurrStateOnStack().GetType() == typeof(AttackState)))
        {
            stateManager.PushDieState();
            isReadyToDie = true;
        }

        if (stateManager.GetCurrStateOnStack().GetType() == typeof(SpawnState))
        {
            stateManager.PushState(idle);
            
        }
    }
}
