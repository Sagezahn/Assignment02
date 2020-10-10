using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControllerScript : MonoBehaviour
{

    // public CharacterController controller;

    public float speed = 2.0f;

    // public float turnSmoothTime = 0.1f;

    // public Transform cam;

    public Animator animator;

    // it is the ground of the playerbody

    // private float turnSmoothVelocity;

    private Vector3 mouseClickedPosition;

    private RaycastHit raycastHit;

    private Vector3 storedClickedPosition;

    // Start is called before the first frame update
    void Start()
    {
        storedClickedPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // float horizontal = Input.GetAxis("Horizontal");
        // float vertical = Input.GetAxis("Vertical");

        // Vector3 direction = new Vector3(horizontal, 0f, vertical);

        // if (direction.magnitude > 0.1f) {

        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +cam.eulerAngles.y;
        //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        //    animator.SetBool("Moving", true);

        //    controller.Move(moveDir.normalized * speed * Time.deltaTime);
        // } else {
        //    animator.SetBool("Moving", false);
        // }
        // if (Input.GetKeyDown(KeyCode.LeftShift)) {
        //    animator.SetBool("Running", true);
        //    speed = 5f;
        // } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
        //    animator.SetBool("Running", false);
        //    speed = 2f;
        // }
        /**
         * When click the left mouse, get the current position
         * Use the current Position to take a raycastHit to make sure it clicked the ground
         * TODO this method need to be fixed because the third person camera could not work correctly.
         *
         * Another thinking:
         * Auto find path
         * Store the click position, check current position and click position
         * if they do not have same x, z position, player should move to the click position contiune.
         * if the have same x,z position, set the click position null, and stop moving
         * if player does not move finish but player has a new clicked position, the player should move to the new position
         * 
         */
        if (Input.GetMouseButton(0)) {
            // get the current screen position
            Vector3 currentScreenPosition = Input.mousePosition;

            // declear a ray to screen position 
            Ray groundCheckRay = Camera.main.ScreenPointToRay(currentScreenPosition);

            // if the ray touch to the ground
            if (Physics.Raycast(groundCheckRay, out raycastHit)) {
                GameObject currentBlockedObject = raycastHit.collider.gameObject;
                if (currentBlockedObject.tag == "Ground")
                {
                    // the ray touched to the ground, so we could get the position that mouse clicked
                    mouseClickedPosition = raycastHit.point;
                    // if (storedClickedPosition == Vector3.zero) {
                    //     storedClickedPosition = new Vector3(mouseClickedPosition.x, mouseClickedPosition.y, mouseClickedPosition.z);
                    // }
                    // compare the position
                    // TODO
                    transform.LookAt(mouseClickedPosition);
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    animator.SetBool("Moving", true);
                }
            }
        } else {
            animator.SetBool("Moving", false);
        }
    }
}
