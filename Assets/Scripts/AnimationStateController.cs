using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    float VelocityZ = 0.0f;
    float VelocityX = 0.0f;
    float acceleration = 5f;
    private bool forwardPressed;
    private bool BackwardsPressed;
    private bool LeftPressed;
    private bool RightPressed;
    private bool SprintPressed;
    private int VelocityZHash;
    private int VelocityXHash;
    private float TurnSensitivity = 5.0f;
    float CharacterRotationHor;
    [SerializeField] private GameObject CameraGO;
    Vector3 InitialCameraPos;
    private float CameraOffset = -1.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");

        InitialCameraPos = CameraGO.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        forwardPressed = Input.GetKey("w");
        BackwardsPressed = Input.GetKey("s");
        LeftPressed = Input.GetKey("a");
        RightPressed = Input.GetKey("d");
        SprintPressed = Input.GetKey("left shift");

        if (forwardPressed && VelocityZ < 1) 
        {
            VelocityZ += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && VelocityZ > 0)
        {
            VelocityZ -= Time.deltaTime * acceleration;
        }

        if (BackwardsPressed && VelocityZ > -1)
        {
            VelocityZ -= Time.deltaTime * acceleration;
        }

        if (!BackwardsPressed && VelocityZ < 0)
        {
            VelocityZ += Time.deltaTime * acceleration;
        }

        if (RightPressed && VelocityX < 1)
        {
            VelocityX += Time.deltaTime * acceleration;
        }

        if (!RightPressed && VelocityX > 0)
        {
            VelocityX -= Time.deltaTime * acceleration;
        }

        if (LeftPressed && VelocityX > -1)
        {
            VelocityX -= Time.deltaTime * acceleration;
        }

        if (!LeftPressed && VelocityX < 0)
        {
            VelocityX += Time.deltaTime * acceleration;
        }

        animator.SetFloat(VelocityZHash, VelocityZ);
        animator.SetFloat(VelocityXHash, VelocityX);

        CharacterRotationHor = TurnSensitivity * Input.GetAxis("Mouse X");
        transform.Rotate(0, CharacterRotationHor, 0);

       CameraGO.transform.localPosition = new Vector3(InitialCameraPos.x,InitialCameraPos.y,Mathf.Lerp(InitialCameraPos.z, CameraOffset, VelocityZ));
    }
}
