using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


    public class Hide : Node
    {
        public LayerMask HidableLayers;
        public NavMeshAgent Agent;
        public Transform transform;
        public float HideSensitivity = 0;
        private Collider[] Colliders = new Collider[10];
        public float MinEnenydistance = 15f;
        public Transform Player;

        public Hide(Transform transform,LayerMask layer,float s)
        {
            Agent=transform.GetComponent<NavMeshAgent>();
            this.transform = transform;
            HidableLayers = layer;
            HideSensitivity= s;
        }

        public override NodeState Evaluate()
        {
            for(int i=0; i< Colliders.Length; i++)
            {
                Colliders[i] = null;
            }
            Transform Target = (Transform)GetData("target");
            Player = Target;
            int hits = Physics.OverlapSphereNonAlloc(Agent.transform.position, transform.GetComponent<Vision>().viewRadius, Colliders, HidableLayers);
            int hitReduction = 0;
            for(int i=0; i< hits; i++)
            {
                if (Vector3.Distance(Colliders[i].transform.position, Target.position) < MinEnenydistance)
                {
                    Colliders[i] = null;
                    hitReduction++;
                }
            }
            hits -= hitReduction;
            System.Array.Sort(Colliders, SortCollider);
                for(int i = 0 ; i < hits; i++)
                {
                    if (NavMesh.SamplePosition(Colliders[i].transform.position,out NavMeshHit hit, 20f, Agent.areaMask))
                    {
                        if(!NavMesh.FindClosestEdge(hit.position,out hit,Agent.areaMask))
                        {
                            Debug.LogError($"Unbale to find edge close to{hit.position}");
                        }

                        if(Vector3.Dot(hit.normal,(Target.position-hit.position).normalized)<HideSensitivity)
                        {
                        
                             if (Vector3.Distance(transform.position, hit.position) > 2f)
                             {
                        //Debug.Log($"hide hit1:{Vector3.Distance(transform.position, hit.position)}");
                        transform.GetComponent<Animator>().SetBool("Walking", true);
                        Agent.SetDestination(hit.position);
                             }
                             else
                             {
                        Debug.Log("stop");
                        transform.GetComponent<Animator>().SetBool("Walking", false);
                        state = NodeState.SUCCESS;
                        return state;
                    }
                        //transform.GetComponent<SmoothAgentMovement>().HandleWaypoint(hit.position);
                        state = NodeState.RUNNING;
                        return state;

                        }
                        else
                        {
                            if (NavMesh.SamplePosition(Colliders[i].transform.position-(Target.position-hit.position).normalized*15, out NavMeshHit hit2, 20f, Agent.areaMask))
                            {
                                if (!NavMesh.FindClosestEdge(hit2.position, out hit2, Agent.areaMask))
                                {
                                    Debug.LogError($"Unbale to find edge close to{hit2.position}");
                                }

                                if (Vector3.Dot(hit2.normal, (Target.position - hit2.position).normalized) < HideSensitivity)
                                {
                                
                            if (Vector3.Distance(transform.position, hit2.position) > 2f)
                            {
                                transform.GetComponent<Animator>().SetBool("Walking", true);
                                //Debug.Log($"hide hit1:{Vector3.Distance(transform.position, hit2.position)}");
                                Agent.SetDestination(hit2.position);
                            }
                            else
                            {
                                Debug.Log("stop2");
                                transform.GetComponent<Animator>().SetBool("Walking", false);
                                state = NodeState.SUCCESS;
                                return state;
                            }
                            //transform.GetComponent<SmoothAgentMovement>().HandleWaypoint(hit2.position);
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
        public int SortCollider(Collider A, Collider B)
        {
            if (A == null & B != null)
            {
                return 1;
            }
            else if(A!=null && B==null) {
                return -1;

            }else if(A==null&& B == null)
            {
                return 0;
            }
            else
            {
                return Vector3.Distance(Agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(Agent.transform.position,B.transform.position));
            }
        }
    }
