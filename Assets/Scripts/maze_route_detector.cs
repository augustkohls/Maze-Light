using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze_route_detector : MonoBehaviour
{
    public static int to_rotate;
    public static bool left_is_open;
    public static bool right_is_open;
       
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerExit2D(Collider2D other)
	{
        if (this.tag == "RightArm")
        {
            right_is_open = true;
            to_rotate = -90;
        }
        if (this.tag == "LeftArm")
        {
            left_is_open = true;
            to_rotate = 90;
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (this.tag == "RightArm")
        {
            right_is_open = false;
        }
        if (this.tag == "LeftArm")
        {
            left_is_open = false;
        }
        if(left_is_open == false && right_is_open == false)
        {
            to_rotate = 180;
        }
    }
}