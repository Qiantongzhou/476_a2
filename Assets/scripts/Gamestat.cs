using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gamestat
{
    public static bool gamestart=false;
    public static List<freezedplayer> freezedplayers = new List<freezedplayer>();
    public static int numberofevaders=10;
    public static int numberofchasers=1;
    public static bool coingen=false;
    public static float gametime = 300;
    public static int numberofcoin=10;
    public static int playerscore=0;
    public static int chaserscore=0;
    public static int freeze = 0;
    public static int save = 0;
    public static float timetolose = 50;
}
public class freezedplayer
{
    public Vector3 position;
    public bool isretriving;

    public freezedplayer(Vector3 position, bool isretriving)
    {
        this.position = position;
        this.isretriving = isretriving;
    }
}