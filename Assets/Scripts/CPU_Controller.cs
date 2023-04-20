using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // belongs in CPU controller
        //if tag == CPU locate the portal exit and create a directional vector towards it
        // this.transform.position - portal.transform.position
        // //Normalize this above vector
        // organize prioty direction array based on the above vector (ex if -1,-1 set priopity to down and left)
    }

    // Update is called once per frame
    void Update()
    {
     // check raycasting (if enemy is nearby in front of you by 10 meters then fire projectile (make sure to wait  or recheck that its a different enemy before shooting again)

    }
}

//maybe this belongs in CPU controller
// if tag == CPU allow the CPU to chose to move into the newly opened area
// calculate the new direction the CPU would end up moving
// if it is a prioity (example new direction is 180, which is down and prioity.y = -1) then move there
// if prioity.y = -1 && new rotation == 180 || prioity.y = 1 && new rotation == 0) || prioity.x == 1 && new rotation == -90 || prioity.x == -1 && new rotation == 90) 
// rb.rotate(new rotation)
