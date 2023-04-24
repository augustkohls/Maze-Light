using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CPU_Controller_Easy_Mode : MonoBehaviour
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
	
	public bool[] path_is_options = new bool[4]; 
	
	private bool right_is_open;
	private bool left_is_open;
	private bool top_is_open;
	private bool bottom_is_open;
	public int num_open_paths;
	public int rand_path_choice;
	public int chosen_path;
	
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
        dir_to_portal = (portal.transform.position-CPU.transform.position);
		dir_to_portal.y = dir_to_portal.y/Mathf.Abs(dir_to_portal.y);
		sprite_direction = new Vector3(0,dir_to_portal.y,0);
		new_velocity = sprite_direction * CPU_speed;
		rb.velocity = new_velocity;
    }

    // Update is called once per frame
    void Update()
    {
		check_animation();
		rb.velocity = new_velocity;
	}
	
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Collision");
		right_is_open = right_side.GetComponent<maze_route_detector>().is_open;
		left_is_open = left_side.GetComponent<maze_route_detector>().is_open;
		top_is_open = top_side.GetComponent<maze_route_detector>().is_open;
		bottom_is_open = bottom_side.GetComponent<maze_route_detector>().is_open;
		if((Convert.ToInt32(right_is_open) + Convert.ToInt32(left_is_open) + Convert.ToInt32(bottom_is_open) + Convert.ToInt32(top_is_open)) == 1)
		{
			if((new_velocity.x > 0 && right_is_open == false) || (new_velocity.x < 0 && left_is_open == false) || (new_velocity.y > 0 && top_is_open == false) || (new_velocity.y < 0 && bottom_is_open == false))
			{
				Debug.Log("Collision with change");
				new_velocity = -1*new_velocity; 
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D collider2D)
	{
		check_move();
	}
	
	void check_move()
	{ 
		chosen_path = -1;
		right_is_open = right_side.GetComponent<maze_route_detector>().is_open;
		left_is_open = left_side.GetComponent<maze_route_detector>().is_open;
		top_is_open = top_side.GetComponent<maze_route_detector>().is_open;
		bottom_is_open = bottom_side.GetComponent<maze_route_detector>().is_open;
		
		//prevent turning around
		/* if(new_velocity.x < 0) {right_is_open = false;};
		if(new_velocity.x > 0) {left_is_open = false;};
		if(new_velocity.y < 0) {top_is_open = false;};
		if(new_velocity.y > 0) {bottom_is_open = false;}; */

		path_is_options[0] = right_is_open;  
		path_is_options[1] = left_is_open; 
		path_is_options[2] = top_is_open;
		path_is_options[3] = bottom_is_open;
		
		num_open_paths = Convert.ToInt32(right_is_open) + Convert.ToInt32(left_is_open) +  Convert.ToInt32(top_is_open) + Convert.ToInt32(bottom_is_open);
		
		//Debug.Log("Path Options " + path_is_options[0] + path_is_options[1] + path_is_options[2] + path_is_options[3] );
		for (int i = 0; i < 4; i++)
		{
			if(path_is_options[i]) //the path option is open
			{
				rand_path_choice = UnityEngine.Random.Range(1,num_open_paths+1);
				Debug.Log("Random Path " + rand_path_choice + "Path is " + path_is_options[i]+1);
				if(rand_path_choice == 1) // we randomly chose this path to the right
				{
					chosen_path = i;
					break;
				}
				else
				{
					num_open_paths = num_open_paths -1;
				}
			}	
		}
		//Debug.Log("Path Chosen " + chosen_path);
		if (chosen_path == 0) {new_velocity.y = 0; new_velocity.x = CPU_speed;}
		if (chosen_path == 1) {new_velocity.y = 0; new_velocity.x = -CPU_speed;}
		if (chosen_path == 2) {new_velocity.y = CPU_speed; new_velocity.x = 0;}
		if (chosen_path == 3) {new_velocity.y = -CPU_speed; new_velocity.x = 0;}
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
