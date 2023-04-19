using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Projectile : MonoBehaviour
{
	public float projectile_speed = 20;
	public GameObject projectile;
	public string projectile_owner = "player_1";
	private float spawn_timer;
	private int to_rotate;
	private bool collision_is_rotation;
	public Transform leftside;
	public Transform rightside;
	

	private Collider2D head_collider;
	
    private Rigidbody2D rb;
	
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
			//Lower intensity then die
			Destroy(projectile);
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		int to_rotate_projectile = maze_route_detector.to_rotate;
		transform.Rotate(0,0, to_rotate_projectile);
		rb.velocity = transform.up * projectile_speed;
	}
	// we can add on collision effects
}