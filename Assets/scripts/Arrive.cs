using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrive : AIMovement
{
    public float slowRadius;
    public float stopRadius;
    public bool arrive=false;


    public override SteeringOutput getkinematic(AIagent agent)
    {
        var output=base.getkinematic(agent);
        if (arrive)
        {
            Vector3 v = agent.TargetPosition - transform.position;
            float distance = v.magnitude;
            v = v.normalized * agent.maxSpeed;
            if (distance < stopRadius)
            {
                v *= 0;
            }
            else if (distance < slowRadius)
            {
                v *= (distance / slowRadius);
            }
            output.Linear = v;
        }
        return output;
    }
}
