using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class evadervalidatetarget : Node
    {
        Transform transform;
        GameObject vfx;
        float delay = 0.2f;

        public evadervalidatetarget(Transform t,GameObject gameObject)
        {
            vfx = gameObject;
            transform = t;
        }
        public override NodeState Evaluate()
        {
            
            Transform t = (Transform)GetData("target");
            if (t == null)
            {
                state = NodeState.FAILURE;
                return state;
                
            }
            if (Vector3.Distance(transform.position, t.position) < 10f)
            {
                delay-=Time.deltaTime;
                if (delay < 0f)
                {
                    
                    delay = 1f;

                }
                if (delay == 1)
                {   Vector3 dir=(t.position - transform.position);
                    GameObject temp=GameObject.Instantiate(vfx, transform.position, Quaternion.LookRotation(dir));
                    temp.GetComponent<projectile>().dir = dir;
                    temp.transform.localScale *= 3f;
                }
                
            }
            if (Vector3.Distance(transform.position, t.position) < 5f)
            {
                transform.GetComponent<NavMeshAgent>().ResetPath();
                transform.position += (transform.position - t.position) * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(transform.position - t.position), 1f);
                state = NodeState.RUNNING;
                return state;
            }
                state = NodeState.SUCCESS;
            return state;
        }
    }
}