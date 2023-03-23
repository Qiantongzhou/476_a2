using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evader : MonoBehaviour
{
    public GameObject[] behaviours;
    public GameObject[] action;
    GameObject currentbehaviour;
    GameObject currentaction;
    private void Start()
    {
        currentbehaviour = Instantiate(behaviours[0], transform);
        currentbehaviour.transform.localScale = Vector3.one * 6f;
        currentbehaviour.transform.position += Vector3.up * 0.2f;

        currentaction = Instantiate(action[0], transform);
        currentaction.transform.localScale = Vector3.one * 10f;

    }

    private void Update()
    {

    }
}
