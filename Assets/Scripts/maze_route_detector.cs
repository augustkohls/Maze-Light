using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze_route_detector : MonoBehaviour
{
    public bool is_open;

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
            is_open = true;

        
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
            is_open = false;
	}
}
