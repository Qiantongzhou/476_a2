using EpicToonFX;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class gotocoinchaser : Node
    {
        Transform transform;
        Vector3 lastposition=Vector3.zero;
        GameObject coin;
        GameObject vfx;
        GameObject vfxtarget;
        bool Iamgoingtogetcion=false;
        bool canwonder = true;
        public gotocoinchaser(Transform transform,GameObject face)
        {
            this.transform = transform;
            this.vfx = face;
        }
        public override NodeState Evaluate()
        {
            if (transform.GetComponent<Vision>().coins.Count > 0)
            {
                if (transform.GetComponent<Vision>().coins[0] != null)
                {
                    lastposition = Vector3.zero;
                    coin = null;
                    Iamgoingtogetcion = false;
                    transform.GetComponent<Animator>().SetBool("Walking", true);
                    transform.GetComponent<NavMeshAgent>().SetDestination(transform.GetComponent<Vision>().coins[0].transform.position);
                    state = NodeState.RUNNING;
                    return state;
                }
            }
                Debug.Log("gotocoin");
            if (lastposition == Vector3.zero) {
                coin = GameObject.Find("WarningBolt(Clone)"); 
            }
            if (coin != null) {
                if (Gamestat.coinstotake > 0)
                {
                    Gamestat.coinstotake -=1;
                    Iamgoingtogetcion = true;
                    
                }
                lastposition = coin.transform.position;
            }
            
                if (lastposition != Vector3.zero)
                {
                if (Iamgoingtogetcion)
                {
                    transform.GetComponent<Animator>().SetBool("Walking", true);
                    transform.GetComponent<NavMeshAgent>().SetDestination(lastposition);
                    if (Vector3.Distance(lastposition, transform.position) < 4f)
                    {
                        if (transform.GetComponent<Vision>().coins.Count > 0)
                        {
                            state = NodeState.RUNNING;
                            return state;
                        }
                        else
                        {
                            Iamgoingtogetcion = false;
                            CoroutineRunner.Instance.Run(wonder());
                            lastposition = Vector3.zero;
                            coin = null;
                            state = NodeState.FAILURE;
                            return state;
                        }
                    }
                    state = NodeState.RUNNING;
                    return state;
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
        IEnumerator wonder()
        {
            if (canwonder)
            {
                genvfx();
                yield return new WaitForSeconds(2f);
                destoryvfx();
            }
        }
        public void genvfx()
        {
            if (vfxtarget == null)
            {
                vfxtarget = new GameObject();

                vfxtarget = MonoBehaviour.Instantiate(vfx, transform);
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