using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControllerScript : MonoBehaviour
{

    public float speed = 2.0f;

    public Animator animator;

    private RaycastHit raycastHit;

    private Vector3 storedClickedPosition;

    private string GROUND = "Ground";

    // Start is called before the first frame update
    void Start()
    {
        storedClickedPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Another thinking:
         * Auto find path
         * Store the click position, check current position and click position
         * if they do not have same x, z position, player should move to the click position contiune.
         * if the have same x,z position, set the click position null, and stop moving
         * if player does not move finish but player has a new clicked position, 
         *   the player should move to the new position 
         *   
         *   
         *  Auto find path function
         *
         *  different situations has different choices
         *  1.if mouse clicked the ground, store the position to storedClickedPosition and player move to this position.
         *      during this moving, if player does not move to the expectant position without new click, 
         *      player should move to the expectant position go on
         *  2.if mouse clicked the ground and player has not move to the expectant position, 
         *      store the new position and player should move to the new position
         *  3.if player has moved to the expectant position, should remove the stored position and stop moving
         */

        if (Input.GetMouseButton(0)) {
            
            // get current mouse screen position
            Vector3 currentScreenPosition = Input.mousePosition;

            // declear a ray to screen position
            Ray groundCheckRay = Camera.main.ScreenPointToRay(currentScreenPosition);

             // if the ray touch to the ground
            if (Physics.Raycast(groundCheckRay, out raycastHit)) {
                GameObject currentBlockedObject = raycastHit.collider.gameObject;
                if (currentBlockedObject.tag == GROUND) {
                    Vector3 mouseClickedPosition = raycastHit.point;
                    StoreDestinationPosition(mouseClickedPosition);
                    Moving2Destination(mouseClickedPosition);
                }   
            }
        } else {
            if (storedClickedPosition != Vector3.zero) {
                Moving2Destination(storedClickedPosition);
            } else {
                animator.SetBool("Moving", false);
            }
        }
        RunOrWalk();
        if (isArrived2DestinationPosition()) {
            RemoveDestinationPosition();
        }
    }

    // if push left shift, player should run
    private void RunOrWalk() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
           animator.SetBool("Running", true);
           speed = 5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
           animator.SetBool("Running", false);
           speed = 2f;
        }
    }

    // moving function
    private void Moving2Destination(Vector3 destinationPosition) {

        transform.LookAt(destinationPosition);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        animator.SetBool("Moving", true);
    }

    // check the current player whether arrived to the destination position
    private bool isArrived2DestinationPosition() {

        // should use approximate position because the step will not follow the position, it will have 0-1 error
        // if (transform.position.x == storedClickedPosition.x && transform.position.z == storedClickedPosition.z) {
        //     return true;
        // }
        // use distance to check the position
        float destinationDistance = Vector3.Distance(transform.position, storedClickedPosition);
        if (destinationDistance < 1f) {
            return true;
        }
        return false;
    }

    // store the destination position
    private void StoreDestinationPosition(Vector3 destinationPosition) {

        storedClickedPosition = new Vector3(destinationPosition.x, destinationPosition.y, destinationPosition.z);
    }

    // remove the stored position
    private void RemoveDestinationPosition() {

        storedClickedPosition = Vector3.zero;
    }
}
