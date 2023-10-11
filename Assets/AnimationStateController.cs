using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    float Velocity = 0.0f;
    float acceleration = 0.1f;
    private bool forwardPressed;
    private bool SprintPressed;
    private int VelocityHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        VelocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        forwardPressed = Input.GetKey("w");
        SprintPressed = Input.GetKey("left shift");

        if (forwardPressed && Velocity < 1) 
        {
            Velocity += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && Velocity > 0)
        {
            Velocity -= Time.deltaTime * acceleration;
        }
        animator.SetFloat(VelocityHash, Velocity);
    }
}
