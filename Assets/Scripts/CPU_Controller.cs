/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CPU_Controller : MonoBehaviour
{
	private SpriteRenderer sprite;
	private Animator anim;
	private enum MovementState {idleDown, idleSide, idleUp, walkDown, walkSide, walkUp}
	MovementState state = MovementState.idleDown;
	private Vector3 sprite_direction;
	
    public GameObject portal;
    public GameObject right_side;
    public GameObject left_side;
    public GameObject top_side;
    public GameObject bottom_side;
    public GameObject center_collider;
	
	private bool right_is_open;
	private bool left_is_open;
	private bool top_is_open;
	private bool bottom_is_open;
	private bool center_is_clear;
	
	private Vector3 new_velocity;
	
    public GameObject CPU;
	private Rigidbody2D rb; 
	public int CPU_speed = 3;
	private Vector3 dir_to_portal;
	private Vector2 priority_dir;
	
	// Start is called before the first frame update
    void Start()
    {
		sprite = CPU.GetComponent<SpriteRenderer>();
		anim = CPU.GetComponent<Animator>();
		rb = CPU.GetComponent<Rigidbody2D>();
		rb.freezeRotation = true;
		center_collider.GetComponent<maze_route_detector>().is_open = true;
        // belongs in CPU controller
        dir_to_portal = (portal.transform.position-CPU.transform.position);
		dir_to_portal.x = -1*dir_to_portal.x/Mathf.Abs(dir_to_portal.x);
		dir_to_portal.y = dir_to_portal.y/Mathf.Abs(dir_to_portal.y);
		Debug.Log("Prio" + dir_to_portal);
		sprite_direction = new Vector3(0,dir_to_portal.y,0);
		new_velocity = sprite_direction * CPU_speed;
		rb.velocity = new_velocity;
		//Vector2 priority_dir = new Vector2(dir_to_portal.x*-90, ((dir_to_portal.y-1)*-90)); // y = -1 --> 180  y= 1 --> 0		// x = -1 --> 90   x = 1 --> -90
    }

    // Update is called once per frame
    void Update()
    {
     // check raycasting (if enemy is nearby in front of you by 10 meters then fire projectile (make sure to wait  or recheck that its a different enemy before shooting again)
		//check_move();
		check_animation();
		dir_to_portal = (portal.transform.position-CPU.transform.position);
		dir_to_portal.x = -1*dir_to_portal.x/Mathf.Abs(dir_to_portal.x);
		dir_to_portal.y = dir_to_portal.y/Mathf.Abs(dir_to_portal.y);
	}
	
	void FixedUpdate()
	{
		//rb.velocity = CPU.transform.up * CPU_speed;
	}
	
	/* void OnTriggerEnter2D(Collider2D collider2D)
	{
		center_is_clear = center_collider.GetComponent<maze_route_detector>().is_open;
		if(center_is_clear == false)
		{
			CPU.transform.Translate(new Vector3 (-rb.velocity.x/50, -rb.velocity.y/50,0));
			force_move();
			//center_collider.GetComponent<maze_route_detector>().is_open = true;
		}
	}
	
	void OnCollisionStay2D(Collision2D collision)
	{
		Debug.Log("HOLDING");
	}
	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Forced Main");
		force_move();
		CPU.transform.Translate(new Vector3(-rb.velocity.x/50, -rb.velocity.y/50,0));
	}
	
	void OnTriggerExit2D(Collider2D collider2D)
	{
		check_move();
		//Debug.Log(collider2D);
	}
	
	void check_move()
	{ 
		right_is_open = right_side.GetComponent<maze_route_detector>().is_open;
		left_is_open = left_side.GetComponent<maze_route_detector>().is_open;
		top_is_open = top_side.GetComponent<maze_route_detector>().is_open;
		bottom_is_open = bottom_side.GetComponent<maze_route_detector>().is_open;
		
		if(right_is_open == true && dir_to_portal.x == 1 && rb.velocity.x >= 0)
		{
				new_velocity.y = 0;
				new_velocity.x = CPU_speed;
		}
		else if (left_is_open == true && dir_to_portal.x == -1 && rb.velocity.x <= 0)
		{
			new_velocity.y = 0;
			new_velocity.x = -CPU_speed;
		}
		else if (top_is_open == true && dir_to_portal.y == 1 && rb.velocity.y >= 0)
		{
			new_velocity.y = CPU_speed;
			new_velocity.x = 0;
		}
		else if (bottom_is_open == true && dir_to_portal.y == -1 && rb.velocity.y <= 0)
		{
			new_velocity.y = -CPU_speed;
			new_velocity.x = 0;
		}
		rb.velocity = new_velocity;
		Debug.Log("Checked " + rb.velocity);
	}
	
	void force_move()
	{
		right_is_open = right_side.GetComponent<maze_route_detector>().is_open;
		left_is_open = left_side.GetComponent<maze_route_detector>().is_open;
		top_is_open = top_side.GetComponent<maze_route_detector>().is_open;
		bottom_is_open = bottom_side.GetComponent<maze_route_detector>().is_open;
		
		if((Convert.ToInt32(right_is_open) + Convert.ToInt32(left_is_open) + Convert.ToInt32(bottom_is_open) + Convert.ToInt32(top_is_open)) == 3)
		{
			new_velocity.x = -rb.velocity.x; 
			new_velocity.y = -rb.velocity.y;
		}
		else 
		{
			if(right_is_open == true)
			{
					new_velocity.y = 0;
					new_velocity.x = CPU_speed;
			}
			else if (left_is_open == true)
			{
				new_velocity.y = 0;
				new_velocity.x = -CPU_speed;
			}
			else if (top_is_open == true)
			{
				new_velocity.y = CPU_speed;
				new_velocity.x = 0;
			}
			else if (bottom_is_open == true)
			{
				new_velocity.y = -CPU_speed;
				new_velocity.x = 0;
			}
		}
		rb.velocity = new_velocity;
		Debug.Log("Force " + rb.velocity);
	}
	
	void check_animation()
	{
		if (rb.velocity.y > 0) { state = MovementState.walkUp; sprite.flipX = false; sprite_direction = new Vector3(0, 1, 0); }
		if(rb.velocity.y < 0) { state = MovementState.walkDown; sprite.flipX = false; sprite_direction = new Vector3(0, -1, 0); }
		if(rb.velocity.x < 0) { state = MovementState.walkSide; sprite.flipX = false; sprite_direction = new Vector3(-1, 0, 0); }
		if(rb.velocity.x > 0) { state = MovementState.walkSide; sprite.flipX = true; sprite_direction = new Vector3(1, 0, 0); }
		if(rb.velocity.y == 0 && rb.velocity.x == 0) { 
			if(state == MovementState.walkUp) {state = MovementState.idleUp; sprite.flipX = false; sprite_direction = new Vector3(0, 1, 0); }
			if(state == MovementState.walkDown) {state = MovementState.idleDown; sprite.flipX = false; sprite_direction = new Vector3(0, -1, 0); }
			if(state == MovementState.walkSide && sprite.flipX == false) {state = MovementState.idleSide; sprite_direction = new Vector3(-1, 0, 0); }
			if(state == MovementState.walkSide && sprite.flipX == true) {state = MovementState.idleSide; sprite.flipX = true; sprite_direction = new Vector3(1, 0, 0); }
		}
		anim.SetInteger("state", (int) state);	
	}
}
 */