using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class freezetag : MonoBehaviour
{
    private Rigidbody characterRigidbody;
    bool targetfreezed=false;
    public freezedplayer player;
    public float Converttime=5;
    float timenow = 0;
    public GameObject textprefab;
    GameObject countdowntext;
    public GameObject freezeeffect;
    private void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        timenow = Converttime;
    }
    private void Update()
    {
        if (targetfreezed)
        {
            
            FreezeCharacter();
            gameObject.layer = 8;
            gameObject.tag = "freeze";
            transform.position = player.position;
            countdown();
        }
    }
  
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "evader"|| gameObject.tag == "freeze")
        {
            if (collision.collider.tag == "chaster")
            {
                if (!targetfreezed)
                {
                    print("an evader get freezed");
                    if (gameObject.GetComponent<AIagent>() != null)
                    {
                        gameObject.GetComponent<AIagent>().trackedTarget = null;
                    }
                    Gamestat.freeze++;
                    GameObject.Find("GameEngine").GetComponent<Maze>().Astarsearch.maze.getXY(transform.position, out int p, out int j);
                    player = new freezedplayer(new Vector3(p * 10, 0, j * 10), false);
                    Gamestat.freezedplayers.Add(player);
                    targetfreezed = true;
                    if (freezeeffect != null)
                    {
                        StartCoroutine(freezeeffectactive());
                    }
                    countdowntext = Instantiate(textprefab, GameObject.Find("Canvas").transform.GetChild(2).GetChild(0).gameObject.transform);
                    countdowntext.transform.SetAsFirstSibling();
                }
            }
            else
            if (collision.collider.tag == "evader")
            {
                if (targetfreezed)
                {
                    Gamestat.freezedplayers.Remove(player);
                    Gamestat.save++;
                    gameObject.layer = 6;
                    gameObject.tag = "evader";
                    targetfreezed = false;
                    resettime();
                    countdowntext.SetActive(false);
                }
            }
        }
    }
    public void resettime()
    {
        Converttime = timenow;

    }
    public void countdown()
    {
       Converttime-= Time.deltaTime;
        countdowntext.GetComponentInChildren<TMP_Text>().text=transform.name+" Timeleft:"+Mathf.FloorToInt( Converttime).ToString();
        if (Converttime <= 0)
        {
            targetfreezed = false;
            countdowntext.SetActive(false);
            print("convert to chaser");
            convert();
        }
    }
    public void convert()
    {
        gameObject.tag = "chaster";
        gameObject.layer = 7;
        if(gameObject.GetComponent<AIagent>()!=null)
        {
            gameObject.GetComponent<AIagent>().behaviorType = AIagent.EBehaviorType.Chaster;
            gameObject.GetComponent<AIagent>().targetPosition = Vector3.zero;
            gameObject.GetComponent<visialeffect>().changetochaster();
        }

    }
    IEnumerator freezeeffectactive()
    {
        GameObject temp = Instantiate(freezeeffect, transform);
        temp.transform.localScale = Vector3.one * 6f;
        temp.transform.position += Vector3.up ;
        yield return new WaitForSeconds(2);
        Destroy(temp); 
    }

    private void FreezeCharacter()
    {
        characterRigidbody.velocity = Vector3.zero;
        characterRigidbody.angularVelocity = Vector3.zero;
    }
}
