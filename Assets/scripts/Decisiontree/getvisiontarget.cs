using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    public class getvisiontarget :Node
    {
        private Transform _transform;
        private Animator _animator;

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
                Debug.Log($"{_transform.name} lose target");
                ClearData("target");
                yield return new WaitForSeconds(10f);
            }
        }
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                List<Transform> targets=_transform.GetComponent<Vision>().targets;
                
                if (targets.Count > 0)
                {
                    parent.parent.SetData("target", targets[0].transform);
                    _animator.SetBool("Walking", true);
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