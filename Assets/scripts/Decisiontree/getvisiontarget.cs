using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTree
{
    public class getvisiontarget :Node
    {
        private Transform _transform;
        private Animator _animator;
        public bool none=true;

        public getvisiontarget(Transform transform)
        {
            _transform = transform;
            _animator = transform.GetComponent<Animator>();
            CoroutineRunner.Instance.StartCoroutine(cleardtarget());
        }
        IEnumerator cleardtarget()
        {
            while (true)
            {
                object t = GetData("target");
                if (t != null)
                {
                    Debug.Log($"{_transform.name} lose target");
                    ClearData("target");
                    _animator.SetBool("Walking", false);
                    none = true;
                    _transform.GetComponent<NavMeshAgent>().SetDestination(_transform.position);
                }
                yield return new WaitForSeconds(UnityEngine.Random.value*5f+5f);
            }
        }
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                List<Transform> targets=_transform.GetComponent<Vision>().targets;
                
                if (targets.Count > 0)
                {   none=false;
                    parent.parent.SetData("target", targets[0].transform);
                    //_animator.SetBool("Walking", true);
                    Debug.Log($"{_transform.name} get {targets[0].transform}");
                    state = NodeState.SUCCESS;
                    return state;
                }
                
                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS;
            return state;
        }
    }
}