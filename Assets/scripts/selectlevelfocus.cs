using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectlevelfocus : MonoBehaviour
{
    public GameObject vcam1;
    public GameObject vcam2;
    private void Start()
    {
        Time.timeScale = 1F;
    }
    public void setf()
    {
        if (vcam1 != null)
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            vcam1.SetActive(false);
            vcam2.SetActive(true);
        }
    }
    public void sett()
    {
        if (vcam1 != null)
        {
            transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
            vcam1.SetActive(true);
            vcam2.SetActive(false);
        }
    }
    public void SelectNormal()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        Gamestat.numberofcoin = 10;
    }
    public void Selectservive()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        Gamestat.numberofcoin = 5;
    }

    public void SelectEasy()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        Gamestat.gametime= 300;
    }
    public void SelectHard()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        Gamestat.gametime = 600;
    }
    public void SelectImpossible()
    {
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        Gamestat.gametime = 900;
    }

}
