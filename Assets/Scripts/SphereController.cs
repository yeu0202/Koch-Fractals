using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    private Rigidbody rb;

    public float speed = 6f;
    public float jumpForce = 7f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public string stopKey = "x";
    public string respawnKey = "r";

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            // transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.AddForce(moveDir.normalized * speed * Time.deltaTime);
            // controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // stop player from moving
        if(Input.GetKeyDown(stopKey)){
            rb.velocity = Vector3.zero;
        }

        // to move player back to the origin position
        if(Input.GetKeyDown(respawnKey)){
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(0f, 0f, 0f);
        }
    }
}
