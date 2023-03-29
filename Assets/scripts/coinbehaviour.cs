using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinbehaviour : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(takeme());
    }
    IEnumerator takeme()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            Gamestat.coinstotake += 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "chaster")
        {
            
            Gamestat.chaserscore++;
            StopCoroutine(takeme());
            Destroy(gameObject.transform.parent.gameObject);
        }
        else if(collision.collider.name == "player" || collision.collider.name == "evader")
        {
            Gamestat.playerscore++;
            StopCoroutine(takeme());
            Destroy(gameObject.transform.parent.gameObject);
        }
        
    }
}
