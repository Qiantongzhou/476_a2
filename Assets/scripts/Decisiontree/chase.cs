using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class chase : Node
    {
        private Transform _transform;

        public chase(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != null)
            {
                if (Vector3.Distance(_transform.position, target.position) > 1f)
                {
                    Debug.Log($"chase {target.name}");
                    _transform.GetComponent<NavMeshAgent>().SetDestination(target.position);
                }
                else
                {
                    _transform.GetComponent<Animator>().SetBool("Walking", false);
                }
                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                return NodeState.FAILURE;
            }
           
        }
    }
}