using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncreaseNumberCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator IncreaseNumberCoroutine()
    {
        while (true)
        {
            // Wait for 20 seconds
            yield return new WaitForSeconds(20.0f);

            // Increment the number
            Gamestat.coingen = true;

            // Output the new number to the console
            Debug.Log("coin appeared");
        }
    }
}
