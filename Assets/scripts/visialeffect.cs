using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visialeffect : MonoBehaviour
{
    public GameObject[] behaviours;
    public GameObject[] action;
    GameObject currentbehaviour;
    GameObject currentaction;
    private void Start()
    {
        if (gameObject.tag == "chaster")
        {
            currentbehaviour = Instantiate(behaviours[0], transform);
            currentbehaviour.transform.localScale = Vector3.one * 6f;
            currentbehaviour.transform.position += Vector3.up * 0.2f;

            currentaction = Instantiate(action[0], transform);
            currentaction.transform.localScale = Vector3.one * 10f;
        }else
        if(gameObject.tag == "evader")
        {
            currentbehaviour = Instantiate(behaviours[1], transform);
            currentbehaviour.transform.localScale = Vector3.one * 6f;
            currentbehaviour.transform.position += Vector3.up * 0.2f;

            currentaction = Instantiate(action[1], transform);
            currentaction.transform.localScale = Vector3.one * 10f;
        }
    }

    public void changetochaster()
    {
        Destroy(currentbehaviour);
        Destroy(currentaction);
        

        GameObject currentbehaviours = Instantiate(behaviours[0], transform);
        currentbehaviours.transform.localScale = Vector3.one * 6f;
        currentbehaviours.transform.position += Vector3.up * 0.2f;

        GameObject currentactions = Instantiate(action[0], transform);
        currentactions.transform.localScale = Vector3.one * 10f;
    }


}
