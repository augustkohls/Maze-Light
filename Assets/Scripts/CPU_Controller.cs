using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_Controller : MonoBehaviour
{
    public GameObject portal;
    public GameObject right_side;
    public GameObject left_side;
    public GameObject front_side;
	
	private bool right_is_open;
	private bool left_is_open;
	private bool front_is_open;
	
    public GameObject CPU;
	private Rigidbody2D rb; 
	public int CPU_speed = 3;
	private Vector3 dir_to_portal;
	private Vector2 priority_dir;
	// Start is called before the first frame update
    void Start()
    {
		front_side.GetComponent<maze_route_detector>().is_open = true;
		rb = CPU.GetComponent<Rigidbody2D>();
		rb.velocity = CPU.transform.up * CPU_speed;
		rb.freezeRotation = true;
        // belongs in CPU controller
        dir_to_portal = (portal.transform.position-CPU.transform.position);
		dir_to_portal.x = dir_to_portal.x/Mathf.Abs(dir_to_portal.x);
		dir_to_portal.y = dir_to_portal.y/Mathf.Abs(dir_to_portal.y);
		Vector2 priority_dir = new Vector2(dir_to_portal.x*-90, ((dir_to_portal.y-1)*-90)); // y = -1 --> 180  y= 1 --> 0		// x = -1 --> 90   x = 1 --> -90
    }

    // Update is called once per frame
    void Update()
    {
     // check raycasting (if enemy is nearby in front of you by 10 meters then fire projectile (make sure to wait  or recheck that its a different enemy before shooting again)
		check_move();
	}
	
	void FixedUpdate()
	{
		rb.velocity = CPU.transform.up * CPU_speed;
	}
	
	void OnTriggerEnter2D(Collider2D collider2D)
	{
		//check_move();
		
	}
	
	void OnTriggerExit2D(Collider2D collider2D)
	{
		//check_move();
	}
	
	void check_move()
	{
		right_is_open = right_side.GetComponent<maze_route_detector>().is_open;
		left_is_open = left_side.GetComponent<maze_route_detector>().is_open;
		front_is_open = front_side.GetComponent<maze_route_detector>().is_open;
		if(right_is_open == true)
		{
			if((CPU.transform.localEulerAngles.z + -90 == priority_dir.x || CPU.transform.localEulerAngles.z + -90 == priority_dir.y) || (front_is_open == false && left_is_open == false))
			{
				CPU.transform.Rotate(0,0,-90);
			}
		}
		else if (left_is_open == true)
		{
			if(CPU.transform.localEulerAngles.z + 90 == priority_dir.x || CPU.transform.localEulerAngles.z + 90 == priority_dir.y || front_is_open == false)
			{
				CPU.transform.Rotate(0,0,90);
			}
		}
		else if (left_is_open == false && right_is_open == false && front_is_open == false)
		{
			Debug.Log("closed");
			CPU.transform.Rotate(0,0,180);
		}
	}
}
