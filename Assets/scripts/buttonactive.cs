using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonactive : MonoBehaviour
{
    public GameObject vcam1;
    public GameObject vcam2;

    public void setf()
    {
        vcam1.SetActive(false);
        vcam2.SetActive(true);
    }
    public void sett()
    {
        vcam1.SetActive(true);
        vcam2.SetActive(false);
    }
}
