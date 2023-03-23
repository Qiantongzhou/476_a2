using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIagent : MonoBehaviour
{
    
    public float maxSpeed;
    public float maxDegreesDelta;
    public float wonderrange;
    public float wondertime;
    public float chastingTime;
    public bool lockY = true;
    public bool debug;
    private Maze Gameenginemaze;
    private Animator animator;
    public enum EBehaviorType { Chaster, Evader }
    public EBehaviorType behaviorType;

    public Transform trackedTarget;
    public Vector3 targetPosition;
    private int currentindex;
    List<AstarNode> path;
    public GameObject mouseclickaction;
    //GameObject mouseclicking;
    float Timer=0;
    bool ischasting = false;
    bool collid =false;
    private bool collisionStayed;
    private float collisionTime;
    
    public Vector3 Velocity { get;  set; }
    public Vector3 TargetPosition
    {
        get => trackedTarget != null ? trackedTarget.position : targetPosition;
    }
    public Vector3 TargetForward
    {
        get => trackedTarget != null ? trackedTarget.forward : Vector3.forward;
    }
    public Vector3 TargetVelocity
    {
        get
        {
            Vector3 v = Vector3.zero;
            if(trackedTarget != null)
            {
                AIagent targetAgent=trackedTarget.GetComponent<AIagent>();
                if(targetAgent != null)
                {
                    v = targetAgent.Velocity;
                }
            }
            return v;
        }
    }
    public void TrackTarget(Transform target)
    {
        trackedTarget = target;
    }
    public  void UnTrackTarget()
    {
        trackedTarget = null;
    }
    IEnumerator vfx(Vector3 mp)
    {
        GameObject temp=Instantiate(mouseclickaction, mp, Quaternion.identity);
        temp.transform.localScale = Vector3.one*5f;
        yield return new WaitForSeconds(15f);
        print("destoryed");
        
        DestroyImmediate(temp);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Gameenginemaze = GameObject.Find("GameEngine").GetComponent<Maze>();
        
        StartCoroutine("clearTargetWithDelay", 5);

        StartCoroutine(ResetRotationCoroutine());

    }
    private void Update()
    {

        
        if (transform.tag != "freeze")
        {
            if (collisionStayed)
            {
                collisionTime += Time.deltaTime;
            }
            if (collid && collisionStayed && collisionTime >= 1.0f)
            {
                StartCoroutine(RotateInDifferentDirection());
            }
            else
            {
                getvisiontarget();
                if (trackedTarget != null)
                {
                    print(transform.name + " has target: " + trackedTarget.name);
                }
                
                if (debug)
                {
                    Debug.DrawRay(transform.position, Velocity, Color.red);
                }
                Timer += Time.deltaTime;

                if (behaviorType == EBehaviorType.Chaster)
                {

                    GameObject coin = GameObject.Find("WarningBolt(Clone)");
                    if (coin!=null&&Timer>=chastingTime)
                    {
                        print("chasting coin");
                        
                        targetPosition = coin.transform.position;  
                        Gameenginemaze.Astarsearch.maze.getXY(targetPosition, out int x, out int y);
                        //StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                        Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);

                        path = Gameenginemaze.Astarsearch.Find(p, j, x, y);
                        currentindex = 0;
                        Timer = 0;

                    }else


                    if (coin == null&&ischasting && Timer >= chastingTime)
                    {
                        
                        print("chasting");
                        targetPosition = trackedTarget.position;
                        Gameenginemaze.Astarsearch.maze.getXY(trackedTarget.position, out int x, out int y);
                        //StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                        Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);
                        
                        path = Gameenginemaze.Astarsearch.Find(p, j, x, y);
                        currentindex = 0;
                        Timer = 0;

                    }
                    else
                    if (coin == null&&!ischasting && (targetPosition == Vector3.zero || Timer > wondertime || path == null))
                    {
                        
                        Vector3 mp = new Vector3(Random.value * wonderrange, Random.value * wonderrange, Random.value * wonderrange);


                        Gameenginemaze.Astarsearch.maze.getXY(mp, out int x, out int y);
                        if (x < Maze.mazegen.GetLength(0) && y < Maze.mazegen.GetLength(0))
                        {
                           // StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                            Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);


                            path = Gameenginemaze.Astarsearch.Find(p, j, x, y);
                            currentindex = 0;
                            Timer = 0;
                        }
                    }
                   
                }
                if (behaviorType == EBehaviorType.Evader)
                {

                    if (ischasting && Timer >= chastingTime)
                    {

                       
                        gameObject.GetComponent<Arrive>().arrive = false;
                        gameObject.GetComponent<Flee>().flee = true;
                        currentindex = 0;
                        Timer = 0; print("fleeing");
                        path = null;



                    }
                    else
                    if (Gamestat.freezedplayers.Count > 0 && Timer > wondertime)
                    {
                        
                        gameObject.GetComponent<Flee>().flee = false;
                        gameObject.GetComponent<Arrive>().arrive = true;
                        targetPosition = Gamestat.freezedplayers[0].position;
                        path = null;
                        Gameenginemaze.Astarsearch.maze.getXY(Gamestat.freezedplayers[0].position, out int x, out int y);
                        if (x < Maze.mazegen.GetLength(0) && y < Maze.mazegen.GetLength(0))
                        {
                            //StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                            Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);


                            path = Gameenginemaze.Astarsearch.Find(p, j, x, y);
                            currentindex = 0;
                            Timer = 0;
                        }

                    }
                    else
                    if (!ischasting && (targetPosition == Vector3.zero || Timer > wondertime || path == null))
                    {
                       
                        Vector3 mp = new Vector3(Random.value * wonderrange, Random.value * wonderrange, Random.value * wonderrange);


                        Gameenginemaze.Astarsearch.maze.getXY(mp, out int x, out int y);
                        if (x < Maze.mazegen.GetLength(0) && y < Maze.mazegen.GetLength(0))
                        {
                           // StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                            Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);


                            path = Gameenginemaze.Astarsearch.Find(p, j, x, y);
                            currentindex = 0;
                            Timer = 0;
                        }
                    }


                }





                if (path != null)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(new Vector3(path[i].x, 0, path[i].y) * 10f + new Vector3(0, 1, 0), new Vector3(path[i + 1].x, 0, path[i + 1].y) * 10f + new Vector3(0, 1, 0), Color.green, 4);
                    }
                }
                if (path != null)
                {
                    gameObject.GetComponent<Flee>().flee = false;
                    gameObject.GetComponent<Arrive>().arrive = true;
                    targetPosition = new Vector3(path[currentindex].x, 0, path[currentindex].y) * 10f;
                    if (Vector3.Distance(transform.position, targetPosition) > 2f)
                    {

                        //print("Moving to Position :" + targetPosition.ToString());

                    }
                    else
                    {
                        currentindex++;
                        if (currentindex >= path.Count)
                        {
                            currentindex = 0;
                            path = null;
                        }
                    }

                }
                else
                {
                  //  mouseclicking = null;
                    gameObject.GetComponent<Arrive>().arrive = false;
                }
                getKinematicAvg(out Vector3 kinematicavg, out Quaternion rotation);
                Velocity = kinematicavg.normalized * maxSpeed;
                transform.position += Velocity * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * maxDegreesDelta);

                animator.SetBool("walk", Velocity.magnitude > 0);
                animator.SetBool("run", Velocity.magnitude > maxSpeed / 2);
            }
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("run", false);
        }
        
    }

    private void getvisiontarget()
    {
        if (trackedTarget != null)
        {
            
        }
        else
        {


            if (gameObject.GetComponent<Vision>().targets.Count>0)
            {

                ischasting = true;
                trackedTarget = gameObject.GetComponent<Vision>().targets[0];
                Timer = chastingTime;
               
            }

            

         

        }
    }
    IEnumerator clearTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (gameObject.GetComponent<Vision>().targets.Count == 0)
            {
                print("clear");
                ischasting = false;
                trackedTarget = null;
            }
        }
    }

    private void getKinematicAvg(out Vector3 kv, out Quaternion rota)
    {
        kv = Vector3.zero;
        Vector3 euleravg= Vector3.zero;
        AIMovement[] m = GetComponents<AIMovement>();
        int count = 0;
        foreach(AIMovement t in m)
        {
            kv += t.getkinematic(this).Linear;
            euleravg+=t.getkinematic(this).angular.eulerAngles;
            ++count;
        }
        if(count > 0)
        {
            kv/= count;
            euleravg/= count;
            rota=Quaternion.Euler(euleravg);
        }
        else
        {
            kv = Velocity;
            rota=transform.rotation;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "maze" || collision.collider.tag == "freeze" || collision.collider.tag == "chaser" || collision.collider.tag == "evader")
        {
            collid = true;
        }
        if (ischasting)
        {
            
            if (collision.collider.tag == "maze")
            {
                
                while (path == null)
                {
                    Vector3 mp = new Vector3(Random.value * wonderrange, Random.value * wonderrange, Random.value * wonderrange);


                    Gameenginemaze.Astarsearch.maze.getXY(mp, out int x, out int y);
                    if (x < Maze.mazegen.GetLength(0) && y < Maze.mazegen.GetLength(0))
                    {
                        //StartCoroutine(vfx(new Vector3(x * 10, 0, y * 10)));
                        Gameenginemaze.Astarsearch.maze.getXY(transform.position, out int p, out int j);


                        path = Gameenginemaze.Astarsearch.Find(p, j, x, y);
                        currentindex = 0;
                        Timer = 0;
                    }
                }
            }
        }
    }
    void OnCollisionStay(Collision collision)
    {
        // Set the collision stayed flag to true
        collisionStayed = true;
    }
    void OnCollisionExit(Collision collision)
    {
        // Reset the collision occurred and collision stayed flags and the collision time
        collid = false;
        collisionStayed = false;
        collisionTime = 0.0f;
    }
    IEnumerator ResetRotationCoroutine()
    {
        while (true)
        {
            // Wait for the reset interval
            yield return new WaitForSeconds(5);

            // Reset the rotation of the transform around the X and Z axes
            Quaternion targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y,0 );
            while (transform.rotation != targetRotation)
            {
                transform.rotation = targetRotation;
                yield return null;
            }
        }
    }
    IEnumerator RotateInDifferentDirection()
    {
        // Rotate the object for the specified duration
        float elapsedTime = 0.0f;
        while (elapsedTime < 1)
        {
            // Calculate a random rotation around the Y-axis
            float yRotation = Random.Range(0.0f, 360.0f);
            Quaternion randomRotation = Quaternion.Euler(0, yRotation, 0);

            // Rotate the object towards the random rotation
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, randomRotation, 5 * Time.deltaTime);
            transform.rotation = newRotation;

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Reset the collision occurred flag
        collid = false;
    }
}
