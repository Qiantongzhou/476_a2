using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.iOS;

namespace BehaviorTree
{
    public class chase : Node
    {
        public Transform transform;
        public NavMeshAgent Agent;
        public Transform target;
        private float hpositionduration = 1f;
        private float hpositioninterval = 0.1f;
        private Queue<Vector3> historicalspeed;
        public Vector3 AverageVelocity
        {
            get {
                Vector3 a = Vector3.zero;
                foreach(Vector3 v in historicalspeed)
                {
                    a += v;
                }
                a.y = 0;
                return a/historicalspeed.Count;   
            }
        }
        private float lastPositiontime;
        private int maxquesize;
        public bool useprediction;
        public float movementpredictionThreshold = 0.33f;
        public float movementprodictiontime = 3f;
        public chase(Transform transform,bool x)
        {
            this.transform = transform;
            useprediction = x;
            maxquesize = Mathf.CeilToInt(1f / hpositioninterval * hpositionduration);
            historicalspeed=new Queue<Vector3>(maxquesize);
            Agent = transform.GetComponent<NavMeshAgent>();
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target != null)
            {
                this.target=target;
                if (target.tag == "freeze")
                {
                    this.target = null;
                    ClearData("target");
                    transform.GetComponent<Animator>().SetBool("Walking", false);
                    state = NodeState.FAILURE;
                    return state;
                }
                if (lastPositiontime + hpositioninterval <= Time.time)
                {
                    if (historicalspeed.Count == maxquesize)
                    {
                        historicalspeed.Dequeue();
                    }
                }
                historicalspeed.Enqueue(target.GetComponent<NavMeshAgent>().velocity);
                lastPositiontime = Time.time;



                if (Vector3.Distance(transform.position, target.position) > 1f)
                {
                    Debug.Log($"chase {target.name}");

                    if (!useprediction)
                    {

                        transform.GetComponent<Animator>().SetBool("Walking", true);
                        transform.GetComponent<NavMeshAgent>().SetDestination(target.position);
                    }
                    else
                    {
                        float timetoplayer = Vector3.Distance(target.position, transform.position) / transform.GetComponent<NavMeshAgent>().speed;
                        if (timetoplayer > movementprodictiontime)
                        {
                            timetoplayer=movementprodictiontime;
                        }
                        //Debug.Log("everage: "+AverageVelocity);
                        Vector3 targetposition = target.position + AverageVelocity * timetoplayer;
                        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.GetComponent<Vision>().viewRadius);

                        for (int i = 0; i < colliders.Length; i++)
                        {
                            if (colliders[i].tag == "chaster")
                            {
                                if (Vector3.Distance(colliders[i].transform.position, target.position) < Vector3.Distance(transform.position, target.position))
                                {
                                    targetposition = target.position - AverageVelocity * timetoplayer;
                                    break;
                                }
                                
                                
                            }
                        }
                        
                        Vector3 directionToTarget=(targetposition - transform.position).normalized;
                        Vector3 diractionToPlayer=(target.position-transform.position).normalized;
                        float dot= Vector3.Dot(diractionToPlayer,directionToTarget);
                        if (dot < movementpredictionThreshold)
                        {
                            targetposition = target.position;
                        }
                        transform.GetComponent<Animator>().SetBool("Walking", true);
                        transform.GetComponent<NavMeshAgent>().SetDestination(targetposition);
                    }
                }
                else
                {
                    transform.GetComponent<Animator>().SetBool("Walking", false);
                }
                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                this.target = null;
                return NodeState.FAILURE;
            }
           
        }
    }
}