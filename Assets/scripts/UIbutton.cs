using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIbutton : MonoBehaviour
{



    public void setdisappear()
    {
        transform.parent.gameObject.SetActive(false);
        Gamestat.gamestart = true;
    }
    public void setsceme()
    {
        SceneManager.LoadScene(0);
        Gamestat.chaserscore = 0;
        Gamestat.playerscore = 0;
        Gamestat.save = 0;
        Gamestat.gamestart = false;
        Gamestat.coingen = false;
        Gamestat.numberofcoin = 10;
        Gamestat.gametime = 300;
        Gamestat.freeze = 0;
        Gamestat.numberofevaders = 10;
        Gamestat.numberofchasers = 1;
    }
}
