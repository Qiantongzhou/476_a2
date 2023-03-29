using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Vision : MonoBehaviour
{
    public float viewRadius;
    [Range(0f, 360f)]
    public float viewAngle;
    public float dynamicangle;
    public LayerMask evaderMask;
    public LayerMask chasterMask;
    public LayerMask coinmask;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [HideInInspector]
    public List<Transform> targets=new List<Transform>();

    [HideInInspector]
    public List<Transform> coins = new List<Transform>();

    private void Start()
    {
        StartCoroutine("FindTargetWithDelay",0.2);
       
        dynamicangle = viewAngle;
    }
    private void Update()
    {
        if (gameObject.GetComponent<AIagent>() != null)
        {
            float velocityMagnitude =Mathf.Abs( gameObject.GetComponent<AIagent>().Velocity.x)+Mathf.Abs( gameObject.GetComponent<AIagent>().Velocity.z);
           
            
            viewAngle = dynamicangle - dynamicangle / 15 * velocityMagnitude;
        }
        if (transform.tag == "chaster")
        {
            targetMask = evaderMask;
        }
        if(transform.tag == "evader")
        {
            targetMask = chasterMask;
        }
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds (delay);
            FindVisibleTargets();
            FindVisiblecoin();
        }
    }
    void FindVisiblecoin()
    {
        coins.Clear();
        Collider[] targetsinVision = Physics.OverlapSphere(transform.position, viewRadius, coinmask);
        for (int i = 0; i < targetsinVision.Length; i++)
        {
            Transform target = targetsinVision[i].transform;
            Vector3 dirToarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToarget) < viewAngle / 2)
            {
                float dsttotarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToarget, dsttotarget, obstacleMask))
                {
                    coins.Add(target);
                }
            }
        }
    }
    void FindVisibleTargets()
    {
        targets.Clear();
        Collider[] targetsinVision=Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for(int i = 0; i < targetsinVision.Length; i++)
        {
            Transform target = targetsinVision[i].transform;
            Vector3 dirToarget=(target.position-transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToarget) < viewAngle / 2)
            {
                float dsttotarget =Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToarget, dsttotarget, obstacleMask))
                {
                    targets.Add(target);
                }
            }
        }
    }
    public Vector3 DirFromAngle(float deg, bool isglobal)
    {
        if (!isglobal)
        {
            deg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(deg*Mathf.Deg2Rad),0,Mathf.Cos(deg*Mathf.Deg2Rad));
    }

}
