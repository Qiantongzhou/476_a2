using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree
{
    public class sharedinfo : Node
    {
        Transform transform;
        public sharedinfo(Transform t)
        {
            transform = t;
        }
        public override NodeState Evaluate()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, transform.GetComponent<Vision>().viewRadius * 2);

            if (colliders.Length>0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].tag == "chaster")
                    {
                        if (colliders[i] != null)
                        {
                            if (colliders[i].GetComponent<ChaserAI>() != null)
                            {
                                object t = colliders[i].GetComponent<ChaserAI>().root.GetData("target");
                                if (t != null)
                                {
                                    Transform target = (Transform)t;
                                    parent.parent.SetData("target", target.transform);
                                    //_animator.SetBool("Walking", true);
                                    Debug.Log($"{transform.name} get {target.transform}");
                                    state = NodeState.SUCCESS;
                                    return state;
                                }
                            }
                        }
                    }
                }
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}