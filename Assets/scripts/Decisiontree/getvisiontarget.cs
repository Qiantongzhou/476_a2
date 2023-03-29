
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace BehaviorTree
{
    public class getvisiontarget :Node
    {
        private Transform transform;
        private Animator _animator;
        public bool none=true;
        public GameObject vfx;
        public GameObject vfxtarget;
        bool cangenvfx = true;
        public getvisiontarget(Transform transform, GameObject alert)
        {
            this.transform = transform;
            _animator = transform.GetComponent<Animator>();
            CoroutineRunner.Instance.StartCoroutine(cleardtarget());
            this.vfx = alert;
            CoroutineRunner.Instance.StartCoroutine(checktarget());
        }
        IEnumerator cleardtarget()
        {
            while (true)
            {
                object t = GetData("target");
                if (t != null)
                {
                    Debug.Log($"{transform.name} lose target");
                    ClearData("target");
                    _animator.SetBool("Walking", false);
                    none = true;
                    transform.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                }
                yield return new WaitForSeconds(Random.value*1.5f+9f);
            }
        }
        IEnumerator checktarget()
        {
            while (true)
            {
                object t = GetData("target");
                if (t != null)
                {
                }
                else
                {
                    cangenvfx=true;
                }
                    yield return new WaitForSeconds(2f);
            }
        }
        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if (t == null)
            {
                List<Transform> targets=transform.GetComponent<Vision>().targets;
                
                if (targets.Count > 0)
                {   none=false;
                    Transform closest = null;
                    float closestDistance = Mathf.Infinity;
                    vfxonce();
                    foreach (Transform enemy in targets)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.position);

                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closest = enemy;
                        }
                    }
                    parent.parent.SetData("target", closest.transform);
                    //_animator.SetBool("Walking", true);
                    Debug.Log($"{transform.name} get {closest.transform}");
                    state = NodeState.SUCCESS;
                    return state;
                }
                
                
            }

            state = NodeState.SUCCESS;
            return state;
        }

        public void vfxonce()
        {
            if (cangenvfx)
            {
                CoroutineRunner.Instance.Run(alter());
                cangenvfx = false;
            }
        }
        IEnumerator alter()
        {
            genvfx();
            yield return new WaitForSeconds(2f);
            destoryvfx();
        }

        public void genvfx()
        {
            if (vfxtarget == null)
            {
                vfxtarget = new GameObject();

                vfxtarget = Object.Instantiate(vfx, transform);
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