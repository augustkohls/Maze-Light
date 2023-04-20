using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Projectile : MonoBehaviour
{
	public float projectile_speed = 20;
	private float spawn_timer;
	private int to_rotate;
	public GameObject rightarm;
	public GameObject leftarm;
	public GameObject projectile;

	private Collider2D head_collider;
	
    private Rigidbody2D rb;
	public string projectile_owner = "player1";


	// Start is called before the first frame update
	void Start()
    {
		spawn_timer = Time.time;
		rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * projectile_speed;
    }
	//check for any collisions and destory the game object
	private void FixedUpdate()
    {
		if(Time.time - spawn_timer > 1)
		{
			Destroy(projectile);
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (leftarm.GetComponent<maze_route_detector>().left_is_open == true)
		{
			to_rotate = 90;
		}
		else if (rightarm.GetComponent<maze_route_detector>().right_is_open == true)
		{
			to_rotate = -90;
		}
		else
		{
			to_rotate = 180;
		}
		projectile.transform.Rotate(0,0, to_rotate);
		rb.velocity = transform.up * projectile_speed;
	}
	// we can add on collision effects
}