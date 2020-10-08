using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonControllerScript : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 2.0f;

    public float turnSmoothTime = 0.1f;

    public Transform cam;

    public Animator animator;

    private float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 0.1f) {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            animator.SetBool("Moving", true);

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        } else {
            animator.SetBool("Moving", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            animator.SetBool("Running", true);
            speed = 5f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            animator.SetBool("Running", false);
            speed = 2f;
        }
    }
}
