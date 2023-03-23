using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public  struct SteeringOutput
{
    public Vector3 Linear;
    public Quaternion angular;
}
public abstract class AIMovement : MonoBehaviour
{
    public bool debug;
    public virtual SteeringOutput getkinematic(AIagent agent)
    {
        return new SteeringOutput { angular = agent.transform.rotation };
    }
}
