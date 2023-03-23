
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

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
                animator.SetBool("walk", true);
                animator.SetBool("run", false); ;
            }



        }
        else
        {
            animator.SetBool("walk", false);
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
    }
}
