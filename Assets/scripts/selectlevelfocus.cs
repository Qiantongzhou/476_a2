using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectlevelfocus : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1F;
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
