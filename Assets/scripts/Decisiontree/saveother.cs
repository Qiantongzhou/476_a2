using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class saveother : Node
    {
        Transform transform;
        public saveother(Transform t)
        {
            transform = t;
        }
        public override NodeState Evaluate()
        {
            if (Gamestat.freezedplayers.Count > 0)
            {
                transform.GetComponent<Animator>().SetBool("Walking", true);
                transform.GetComponent<NavMeshAgent>().SetDestination(Gamestat.freezedplayers[0].position);
                state = NodeState.RUNNING;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}