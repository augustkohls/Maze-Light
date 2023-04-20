using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player_Controller : MonoBehaviour
{
	private SpriteRenderer sprite;
	private Animator anim;
	private enum MovementState {idleDown, idleSide, idleUp, walkDown, walkSide, walkUp}
	MovementState state = MovementState.idleDown;
	private Vector3 sprite_direction = new Vector3(0,-1,0);

	public float speed = 120;
	public float rotationSpeed = 1000;
	
    private Rigidbody2D rb;
	private Quaternion toRotation;
	

	private float movementX;
    private float movementY;

	public GameObject projectile;
	
	public float ranged_attack_cooldown = 1f;
	private float next_time_ranged_attack_possible = 0.0f;
	
	public int attack_radius = 3;
	public int ammo;
	public GameObject bullets_parent;

	//private Animation anim;


	// Start is called before the first frame update
	void Start()
    {
		ammo = 3;
        rb = GetComponent<Rigidbody2D>();
		rb.freezeRotation = true;
		sprite = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponentInChildren<Animator>();
		
    }

	void Update(){
		// check if input matches the projectile input and that the projectile is not on cooldown. Prevents spaming
		if(Mouse.current.rightButton.wasPressedThisFrame && Time.time > next_time_ranged_attack_possible){
			next_time_ranged_attack_possible = Time.time + ranged_attack_cooldown;
			shoot_projectile();
		}
		check_animation();
	}
	
    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
	
	
    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(movementX, movementY);
		
		movement.Normalize();
		if(movementY == 0) { rb.velocity = new Vector3(rb.velocity.x, 0, 0); ;}
		if(movementX == 0) { rb.velocity = new Vector3(0,rb.velocity.y, 0); }
		rb.AddForce(movement * speed);
	    if (movement == new Vector2(1.0f,0.0f) || movement == new Vector2(-1.0f,0.0f) || movement == new Vector2(0.0f,1.0f) || movement == new Vector2(0.0f,-1.0f))//(movement != Vector2.zero && movement != new Vector2(1.0f,1.0f) && movement != new Vector2(-1.0f,-1.0f))
		{
			toRotation = Quaternion.LookRotation(transform.forward, movement);

		}
    }

	// make this a public function so that when we get the bot code working we can use it
	public void shoot_projectile()
	{
		if (ammo != 0) {
			Destroy(bullets_parent.transform.GetChild(ammo-1).gameObject);
			ammo = ammo - 1;
			float spawnDistance = 1.5f;
			Vector3 playerPos = transform.position;
			Quaternion playerRotation = toRotation;
			Vector3 spawnPos = playerPos + sprite_direction * spawnDistance;
			var my_projectile = Instantiate(projectile, spawnPos, playerRotation);
			//set the collison box of the particle to be wide and on all sides
			// give the projectile the same tag as the parent
			my_projectile.tag = gameObject.tag;
			my_projectile.GetComponent<Move_Projectile>().projectile_owner = gameObject.name;
		}
	}


	private void check_animation()
	{
		if (rb.velocity.y > 0) { state = MovementState.walkUp; sprite.flipX = false; sprite_direction = new Vector3(0, 1, 0); }
		if(rb.velocity.y < 0) { state = MovementState.walkDown; sprite.flipX = false; sprite_direction = new Vector3(0, -1, 0); }
		if(rb.velocity.x < 0) { state = MovementState.walkSide; sprite.flipX = false; sprite_direction = new Vector3(-1, 0, 0); }
		if(rb.velocity.x > 0) { state = MovementState.walkSide; sprite.flipX = true; sprite_direction = new Vector3(1, 0, 0); }
		if(rb.velocity.y == 0 && rb.velocity.x == 0) { 
			if(toRotation.eulerAngles.z == 0) {state = MovementState.idleUp; sprite.flipX = false; sprite_direction = new Vector3(0, 1, 0); }
			if(toRotation.eulerAngles.z == 180) {state = MovementState.idleDown; sprite.flipX = false; sprite_direction = new Vector3(0, -1, 0); }
			if(toRotation.eulerAngles.z == 90) {state = MovementState.idleSide; sprite.flipX = false; sprite_direction = new Vector3(-1, 0, 0); }
			if(toRotation.eulerAngles.z == 270) {state = MovementState.idleSide; sprite.flipX = true; sprite_direction = new Vector3(1, 0, 0); }
		}
		anim.SetInteger("state", (int) state);	
	}
}