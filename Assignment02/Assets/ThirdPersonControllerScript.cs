using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControllerScript : MonoBehaviour
{

    public float speed = 2.0f;

    public Animator animator;

    private RaycastHit raycastHit;

    private Vector3 storedClickedPosition;

    private float turnSmooth = 25f;

    private string GROUND = "Ground";

    // In combat statut
    public string PLAYER_STATUTS_INCOMBAT = "Combat";

    // Not in combat statut
    public string PLAYER_STATUTS_NORMAL = "Normal";

    // Not in combat walking
    public string MOVING_FUNCTION_NORMAL_WALK = "Moving";

    public string MOVING_FUNCTION_RUNNING = "Running";

    // check the direction which player to destination
    public float DESTINATION_DIRECTION = 1f;

    // check the direction which player to enemy
    public float ENEMY_DIRECTION = 2F;

    // Start is called before the first frame update
    void Start()
    {
        storedClickedPosition = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
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
                // if mouse clicked to the ground, player should be move to this position
                if (currentBlockedObject.tag == GROUND) {
                    Vector3 mouseClickedPosition = raycastHit.point;
                    StoreDestinationPosition(mouseClickedPosition);
                    TakeSmoothRotation();
                    Moving2Destination();
                }
                /*
                 *  TODO
                 *  if mouse clicked to the enemy, player should move to the enemy.
                 *  if player near to the enemy less than 2f, player should not to move, player just could attack and defend
                 *  when player near to enemy less than 2f, player should be in combat and change the animations
                 */    
            }
        } else {
            if (hasStoredPosition()) {
                Moving2Destination();
            } else {
                animator.SetBool(MOVING_FUNCTION_NORMAL_WALK, false);
            }
        }
        RunOrWalk();
        // check player whether near the destination position less than 1f;
        if (isArrived2DestinationPositionByCustomDistance(DESTINATION_DIRECTION)) {
            RemoveDestinationPosition();
        }
    }

    /*
     *  Instead of the LookAt function, 
     *  this function will help player turn to new destination smoothly
     */
    private void TakeSmoothRotation() {
        
        Quaternion oldRotation = transform.rotation;
        transform.LookAt(storedClickedPosition);
        Quaternion newRotation = transform.rotation;
        transform.rotation = oldRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, turnSmooth * Time.deltaTime);
    }

    // Checking the new destination or had
    private bool hasStoredPosition() {
        
        if (storedClickedPosition != Vector3.zero) {
            return true;
        }
        return false;
    }

    // if push left shift, player should run
    private void RunOrWalk() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
           animator.SetBool(MOVING_FUNCTION_RUNNING, true);
           speed = 5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
           animator.SetBool(MOVING_FUNCTION_RUNNING, false);
           speed = 2f;
        }
    }

    // moving function
    private void Moving2Destination() {

        // transform.LookAt(storedClickedPosition);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        animator.SetBool(MOVING_FUNCTION_NORMAL_WALK, true);
    }

    

    // check the current player whether arrived to the destination position
    private bool isArrived2DestinationPositionByCustomDistance(float customDistance) {

        // should use approximate position because the step will not follow the position, it will have 0-1 error
        // if (transform.position.x == storedClickedPosition.x && transform.position.z == storedClickedPosition.z) {
        //     return true;
        // }
        // this is new function to check, use distance to check the position
        float destinationDistance = Vector3.Distance(transform.position, storedClickedPosition);
        if (destinationDistance < customDistance) {
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
