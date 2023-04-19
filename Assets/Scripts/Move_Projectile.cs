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
		Collider2D head_collider = GetComponent<CircleCollider2D>();
		Collider2D arms_collider = GetComponent<BoxCollider2D>();
		spawn_timer = Time.time;
		rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * projectile_speed;
    }
	//check for any collisions and destory the game object
	private void FixedUpdate()
    {
		if(Time.time - spawn_timer > 1)
		{
			//Destroy(projectile);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		Debug.Log("EXIT");
		float coll_dist_left = Vector3.Distance(other.ClosestPoint(projectile.transform.position), leftside.position);
		float coll_dist_right = Vector3.Distance(other.ClosestPoint(projectile.transform.position), rightside.position);
		if(coll_dist_left < coll_dist_right)
		{
			to_rotate = -90;
		}
		else 
		{
			to_rotate = 90;
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		transform.Rotate(0,0,to_rotate);
		Debug.Log(to_rotate);
		rb.velocity = transform.up * projectile_speed;
		to_rotate = 180;
	}
	// we can add on collision effects
}