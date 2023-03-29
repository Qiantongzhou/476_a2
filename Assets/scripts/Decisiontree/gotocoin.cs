using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class gotocoin : Node
    {
        Transform transform;
        
        public gotocoin(Transform transform)
        {
            this.transform= transform;
        }
        public override NodeState Evaluate()
        {
            Debug.Log("gotocoin");
            GameObject coin = GameObject.Find("WarningBolt(Clone)");
            if (coin != null) {
                
                transform.GetComponent<Animator>().SetBool("Walking", true);
                transform.GetComponent<NavMeshAgent>().SetDestination(coin.transform.position);
                
                state = NodeState.RUNNING;
                return state;
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}