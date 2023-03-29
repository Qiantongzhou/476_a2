using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

namespace BehaviorTree
{
    public class chaservalidatetarget : Node
    {
        float questiontime = 5f;
        float delay = 1f;
        bool delaycheck = true;
        bool questioning = true;
        bool havetargetbefore = false;
        public GameObject vfx;
        public GameObject vfxtarget;
        Transform transform;
        public chaservalidatetarget(Transform t,GameObject vfx)
        {
            
            transform = t;
            this.vfx=vfx;
           
        }
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                if (havetargetbefore)
                {
                    if (delaycheck)
                    {
                        delay -= Time.deltaTime;
                        if (delay < 0f)
                        {
                            delaycheck = false;
                            delay = 1f;
                        }

                        state = NodeState.RUNNING;
                        return state;
                    }
                    if (questioning)
                    {
                        transform.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                        transform.GetComponent<Animator>().SetBool("Walking", false);
                        genvfx();
                        questiontime -= Time.deltaTime;
                        if (questiontime < 0f)
                        {
                            questioning = false;
                            questiontime = 5f;
                            destoryvfx();
                            havetargetbefore = false;
                        }
                        state = NodeState.RUNNING;
                        return state;


                    }
                    state = NodeState.FAILURE;
                    return state;

                }
                else
                {
                    
                    delaycheck = true;
                    questioning = true;
                }
                state = NodeState.FAILURE;
                return state;
            }
            destoryvfx();
            havetargetbefore = true;
            state = NodeState.SUCCESS;
            return state;
        }
        public void genvfx()
        {
            if (vfxtarget == null)
            {
                vfxtarget = new GameObject();
                
                vfxtarget =MonoBehaviour.Instantiate(vfx, transform);
                vfxtarget.transform.position = vfxtarget.transform.position + new Vector3(0, 7f, 0);
                vfxtarget.transform.localScale *= 3;

            }
        }
        public void destoryvfx()
        {
            if (vfxtarget != null)
            {
                Object.Destroy(vfxtarget);
            }
        }
    }
}