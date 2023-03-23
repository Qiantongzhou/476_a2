using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookWhereYouAreGoing : AIMovement
{
    public override SteeringOutput getkinematic(AIagent agent)
    {
        var output= base.getkinematic(agent);

        if (agent.Velocity == Vector3.zero)
        {
            return output;
        }
        if(agent.Velocity != Vector3.zero)
        {
            output.angular=Quaternion.LookRotation(agent.Velocity);
        }
        return output;
    }
}
