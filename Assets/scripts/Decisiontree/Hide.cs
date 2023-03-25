using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class Hide : Node
    {
        public LayerMask HidableLayers;
        public NavMeshAgent Agent;
        private Transform transform;
        public float HideSensitivity = 0;
        private Collider[] Colliders = new Collider[10];

        public Hide(Transform transform,LayerMask layer,float s)
        {
            Agent=transform.GetComponent<NavMeshAgent>();
            this.transform = transform;
            HidableLayers = layer;
            HideSensitivity= s;
        }

        public override NodeState Evaluate()
        {
            Transform Target = (Transform)GetData("target");
            int hits = Physics.OverlapSphereNonAlloc(Agent.transform.position, transform.GetComponent<Vision>().viewRadius, Colliders, HidableLayers);

                for(int i = 0 ; i < hits; i++)
                {
                    if (NavMesh.SamplePosition(Colliders[i].transform.position,out NavMeshHit hit, 2f, Agent.areaMask))
                    {
                        if(!NavMesh.FindClosestEdge(hit.position,out hit,Agent.areaMask))
                        {
                            Debug.LogError($"Unbale to find edge close to{hit.position}");
                        }

                        if(Vector3.Dot(hit.normal,(Target.position-hit.position).normalized)<HideSensitivity)
                        {
                        Debug.Log("hide hit1");
                            Agent.SetDestination(hit.position);
                        state = NodeState.RUNNING;
                        return state;

                        }
                        else
                        {
                            if (NavMesh.SamplePosition(Colliders[i].transform.position-(Target.position-hit.position).normalized*2, out NavMeshHit hit2, 2f, Agent.areaMask))
                            {
                                if (!NavMesh.FindClosestEdge(hit2.position, out hit2, Agent.areaMask))
                                {
                                    Debug.LogError($"Unbale to find edge close to{hit2.position}");
                                }

                                if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < HideSensitivity)
                                {
                                Debug.Log("hide hit2");
                                Agent.SetDestination(hit2.position);
                                state = NodeState.RUNNING;
                                return state;

                                }
                            }

                        }
                    }
                    else
                    {
                    Debug.LogError($"Unable to find navmesh near object {Colliders[i].name} at {Colliders[i].transform.position}");
                    
                    }
                }

            state = NodeState.FAILURE;
            return state;

        }
    }
}