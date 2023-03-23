using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceAway : AIMovement
{
    public override SteeringOutput getkinematic(AIagent agent)
    {
        var output = base.getkinematic(agent);

        // TODO: calculate angular component
        Vector3 dir = transform.position - agent.targetPosition;
        if (dir.normalized == transform.forward || Mathf.Approximately(dir.magnitude, 0))
        {
            output.angular = transform.rotation;
            return output;
        }
        output.angular = Quaternion.LookRotation(dir);

        return output;
    }
}
