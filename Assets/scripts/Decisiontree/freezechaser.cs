using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class freezechaser : MonoBehaviour
{
    bool targetfreezed = false;
    public float freezetime = 5;
    float timenow = 0;
    public AImaneger aiscript;
    public GameObject freezeeffect;
    // Start is called before the first frame update
    void Start()
    {
        timenow=freezetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetfreezed)
        {
            countdown();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "boom")
        {
            if (!targetfreezed)
            {
                print("an chaser get freezed by boom");
                if (gameObject.GetComponent<AImaneger>() != null)
                {
                    aiscript=gameObject.GetComponent<AImaneger>();
                    aiscript.settodisable();
                }
                gameObject.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                gameObject.GetComponent<Animator>().SetBool("Walking", false);
                targetfreezed = true;
                if (freezeeffect != null)
                {
                    StartCoroutine(freezeeffectactive());
                }
                
            }
        }
    }
    public void countdown()
    {
        freezetime -= Time.deltaTime;
        if (freezetime <= 0)
        {
            targetfreezed = false;
            aiscript.settochaser();
            freezetime = timenow;
        }
    }
    IEnumerator freezeeffectactive()
    {
        GameObject temp = Instantiate(freezeeffect, transform);
        temp.transform.localScale = Vector3.one * 12f;
        temp.transform.position += new Vector3(0,3,0);
        yield return new WaitForSeconds(freezetime);
        Destroy(temp);
    }
}
