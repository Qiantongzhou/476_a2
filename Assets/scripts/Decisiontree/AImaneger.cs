using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AImaneger : MonoBehaviour
{
    EvaderAI evader;
    ChaserAI chaser;
    public bool ischaser;
    public LayerMask workinglayer;
    [Range(-1f, 1f)]
    public float actionsenstivity;
    public GameObject freezeboom;
    public GameObject questioning;
    public GameObject anger;
    public GameObject chaseralert;
    public GameObject evaderalert;

    private void Start()
    {
        transform.AddComponent<EvaderAI>();
        evader = transform.GetComponent<EvaderAI>();
        transform.AddComponent<ChaserAI>();
        chaser = transform.GetComponent<ChaserAI>();
        evader.HidableLayers = workinglayer;
        evader.Hidesenstivity= actionsenstivity;
        evader.freezeboom= freezeboom;
        evader.altert = evaderalert;


        chaser.chaseLayers = workinglayer;
        chaser.anger = anger;
        chaser.questioning = questioning;
        chaser.chasesentivity = actionsenstivity;
        chaser.alert = chaseralert;

        if (ischaser)
        {
            evader.enabled = false;
        }
        else
        {
            chaser.enabled = false;
        }
    }

    public void settochaser()
    {
        chaser.enabled = true;
        evader.enabled = false;
    }
    public void settodisable()
    {
        chaser.enabled = false;
        evader.enabled = false;
    }
    public void settoevader()
    {
        evader.enabled = true;
        chaser.enabled = false;
    }
}
