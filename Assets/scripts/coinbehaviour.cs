using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinbehaviour : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "chaster")
        {
            
            Gamestat.chaserscore++;
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if(collision.collider.name == "player")
        {
            Gamestat.playerscore++;
            Destroy(gameObject.transform.parent.gameObject);
        }
        
    }
}
