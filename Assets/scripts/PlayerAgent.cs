
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public float maxSpeed;
    public float maxDegreesDelta;
    public float multipler=1;
    public bool lockY = true;
    public bool debug;
    float horizontal = 0;
    float vertical = 0;
    private Maze Gameenginemaze;
    private Animator animator;
    List<AstarNode> path;
    List<AstarNode> path2;
    public GameObject[] effects;
    private GameObject currentObject;
    private GameObject currentObject2;
    public int currentObjectID;
    private float delay = 0;
    private float delay2 = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        Gameenginemaze = GameObject.Find("GameEngine").GetComponent<Maze>();
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        // MOVE
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        if ((!Mathf.Approximately(vertical, 0.0f) || !Mathf.Approximately(horizontal, 0.0f)))
        {

            Vector3 direction = new Vector3(0.0f, 0.0f, vertical);
            direction = Vector3.ClampMagnitude(direction, 1.0f);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                multipler = 2;
                // transform.Translate(direction * maxSpeed * multipler * Time.deltaTime);
                
                transform.RotateAround(gameObject.transform.position, Vector3.up, horizontal * maxDegreesDelta * Time.deltaTime);
                
                animator.SetBool("run", true);
            }
            else {
               // transform.Translate(direction * maxSpeed * multipler * Time.deltaTime);
                transform.RotateAround(gameObject.transform.position, Vector3.up, horizontal * maxDegreesDelta * Time.deltaTime);
                animator.SetBool("Walking", true);
                animator.SetBool("run", false); ;
            }



        }
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("run", false);
        }
        if (vertical < 0.0f)
        {
            animator.SetFloat("animedirection", -1.0f);
        }
        else
        {
            animator.SetFloat("animedirection", 1.0f);
        }

        pathtoplayer();
        pathtocoin();

    }
    public void pathtocoin()
    {
        GameObject coin = GameObject.Find("WarningBolt(Clone)");
        if (coin !=null)
        {

            Gameenginemaze.Astarsearch.maze.getXY(coin.transform.position, out int x, out int y);
            if (x < Maze.mazegen.GetLength(0) && y < Maze.mazegen.GetLength(0))
            {
                //StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);


                path2 = Gameenginemaze.Astarsearch.Find(p, j, x, y);

            }
        }
        else
        {
            path2 = null;
            Destroy(currentObject2);
        }

        if (path2 != null)
        {
            delay2 += Time.deltaTime;
            if (delay2 > 10f)
            {
                Destroy(currentObject2);
                currentObject2 = new GameObject();
                delay2 = 0;
            }
            if (delay2 == 0)
            {
                currentObject2 = Instantiate(effects[1], transform.position, Quaternion.identity);
                Spline spline = currentObject2.GetComponent<Spline>();
                for (int i = 0; i < path2.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path2[i].x, 0, path2[i].y) * 10f + new Vector3(0, 1, 0), new Vector3(path2[i + 1].x, 0, path2[i + 1].y) * 10f + new Vector3(0, 1, 0), Color.green, 4);
                    spline.AddNode(new Vector3(path2[i].x, 0, path2[i].y) * 10f + new Vector3(0, 1, 0), Vector3.forward);
                }
            }

        }
    }
    public void pathtoplayer()
    {
        if (Gamestat.freezedplayers.Count > 0)
        {

            Gameenginemaze.Astarsearch.maze.getXY(Gamestat.freezedplayers[0].position, out int x, out int y);
            if (x < Maze.mazegen.GetLength(0) && y < Maze.mazegen.GetLength(0))
            {
                //StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);


                path = Gameenginemaze.Astarsearch.Find(p, j, x, y);

            }
        }
        else
        {
            Destroy(currentObject);
            path = null;
        }

        if (path != null)
        {
            delay += Time.deltaTime;
            if (delay > 10f)
            {
                Destroy(currentObject);
                currentObject = new GameObject();
                delay = 0;
            }
            if (delay == 0)
            {
                currentObject = Instantiate(effects[currentObjectID], transform.position, Quaternion.identity);
                Spline spline = currentObject.GetComponent<Spline>();
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, 0, path[i].y) * 10f + new Vector3(0, 1, 0), new Vector3(path[i + 1].x, 0, path[i + 1].y) * 10f + new Vector3(0, 1, 0), Color.green, 4);
                    spline.AddNode(new Vector3(path[i].x, 0, path[i].y) * 10f + new Vector3(0, 1, 0), Vector3.forward);
                }
            }

        }
    }
}
