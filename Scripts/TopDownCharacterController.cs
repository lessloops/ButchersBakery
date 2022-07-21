using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public Vector2 lastMotionVector;
    public float speed;
    public bool flipAnimations;
    private Animator animator;
    private Collider2D interactionCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        interactionCollider = transform.Find("InteractionDetector").GetComponent<Collider2D>();
    }


    private void Update()
    {
        if (GameStateController.instance.currentState != "Live")
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        Vector2 dir;

        if (GameStateController.instance.usingGamepad)
        {
            dir = CheckGamepadInput();
        }
        else
        {
            dir = CheckKeyboardMouseInput();
        }
        
        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        if (dir.x != 0 || dir.y != 0)
        {
            lastMotionVector = dir;
            interactionCollider.offset = dir;
        }

        GetComponent<Rigidbody2D>().velocity = speed * dir;
    }

    Vector2 CheckGamepadInput()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetAxis("Horizontal") < 0)
        {
            if (flipAnimations)
                animator.SetInteger("Direction", 2);
            else
                animator.SetInteger("Direction", 3);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (flipAnimations)
                animator.SetInteger("Direction", 3);
            else
                animator.SetInteger("Direction", 2);
        }


        if (Input.GetAxis("Vertical") > 0)
        {
            if (flipAnimations)
                animator.SetInteger("Direction", 0);
            else
                animator.SetInteger("Direction", 1);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            if (flipAnimations)
                animator.SetInteger("Direction", 1);
            else
                animator.SetInteger("Direction", 0);
        }

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        return direction;
    }

    Vector2 CheckKeyboardMouseInput()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1;

            if (flipAnimations)
                animator.SetInteger("Direction", 2);
            else
                animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;

            if (flipAnimations)
                animator.SetInteger("Direction", 3);
            else
                animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;

            if (flipAnimations)
                animator.SetInteger("Direction", 0);
            else
                animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;

            if (flipAnimations)
                animator.SetInteger("Direction", 1);
            else
                animator.SetInteger("Direction", 0);
        }

        return direction;
    }
}
