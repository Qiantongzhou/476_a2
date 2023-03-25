using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    // Start is called before the first frame update
    public string countdownFormat = "Time remaining: {0:00}:{1:00}";
    private float startTime;
    public bool debug;
    bool started=false;
    bool gameover=false;
    public GameObject winningdisplay;
    public GameObject losedisplay;
    bool lose=false;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamestat.gamestart)
        {
            if (!debug)
            {
                if (!started)
                {
                    startTime = Time.time;
                    started = true;
                }
                float timeRemaining = Gamestat.gametime - (Time.time - startTime);
                TMP_Text[] j = GameObject.Find("Canvas").GetComponent<Canvas>().GetComponentsInChildren<TMP_Text>();
                j[0].text = string.Format(countdownFormat, Mathf.FloorToInt(timeRemaining / 60), Mathf.FloorToInt(timeRemaining % 60));
                j[1].text = Gamestat.playerscore.ToString() + "/" + Gamestat.numberofcoin;
                j[2].text = Gamestat.chaserscore.ToString() + "/" + Gamestat.numberofcoin;
                j[3].text = Gamestat.save.ToString();
                j[4].text = Gamestat.freeze.ToString();

                if (!lose)
                {
                    if (Gamestat.chaserscore > Gamestat.numberofcoin / 2)
                    {
                        Instantiate(losedisplay, GameObject.Find("Canvas").transform);
                        Time.timeScale = 0;
                        lose = true;
                    }
                    if (GameObject.Find("Player").transform.GetChild(0).tag == "chaster")
                    {
                        Instantiate(losedisplay, GameObject.Find("Canvas").transform);
                        Time.timeScale = 0;
                        lose = true;
                    }
                }

                if (timeRemaining < 0 && !gameover)
                {
                    gameover = true;
                    playerwin();
                    Time.timeScale = 0;
                }



            }
        }
    }
    public void playerwin()
    {
        Instantiate(winningdisplay, GameObject.Find("Canvas").transform);
        

    }





}
