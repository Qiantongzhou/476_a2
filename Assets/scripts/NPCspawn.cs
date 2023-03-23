using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCspawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] NPCs;
    List<GameObject> spawnList = new List<GameObject>();
    public GameObject coin;
    public enum npctype
    {
        chaser,
        evader
    }
    public npctype npctypes;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamestat.gamestart)
        {
            if (Gamestat.coingen)
            {
                if (coin != null&&GameObject.Find("WarningBolt(Clone)")==null)
                {
                    if (Random.value > 0.5)
                    {
                        Gamestat.coingen = false;
                        Instantiate(coin, transform.position, Quaternion.identity);
                    }
                }
            }
            if (npctypes == npctype.chaser)
            {
                if (Gamestat.numberofchasers > 0)
                {
                    spawnchasers();
                    Gamestat.numberofchasers--;
                }
            }
            if (npctypes == npctype.evader)
            {
                if (Gamestat.numberofevaders > 0)
                {
                    spawnevaders();
                    Gamestat.numberofevaders--;
                }
            }
        }
    }
    [ContextMenu("spawnchasers")]
    public void spawnchasers()
    {
        GameObject temp= Instantiate(NPCs[0]);
        temp.transform.position=transform.position;
        spawnList.Add(temp);
    }
    public void spawnevaders()
    {
        GameObject temp = Instantiate(NPCs[1]);
        temp.transform.position = transform.position;
        spawnList.Add(temp);
    }
    [ContextMenu("removechasers")]
    public void deletechasers()
    {
        foreach (GameObject temp in spawnList)
        {
            DestroyImmediate(temp);
        }
    }
}
