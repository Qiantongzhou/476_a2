using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : AIMovement
{
    public bool flee=false;
    public override SteeringOutput getkinematic(AIagent agent)
    {
        var output= base.getkinematic(agent);
        if (flee)
        {
            Vector3 des = transform.position - agent.TargetPosition;
            des = des.normalized * agent.maxSpeed;
            output.Linear = des;
        }

        return output;
    }

}
