using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed =10;
    public float count = 0;
    public Vector3 dir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count+=Time.deltaTime;
        if(count > 1)
        {
            Destroy(gameObject,0.1f);
        }
       transform.position+=dir*Time.deltaTime*speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "chaster")
        {
            Destroy(gameObject,0.1f);
        }
    }
}
