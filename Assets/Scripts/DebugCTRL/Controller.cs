using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody myRigidbody;


    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 playerDirection;
    private Vector3 MaskRota;

    private Animator myAnimator;

    public Transform MaskTransform;

    private bool b_CanMove = false;

    private int Xbox_One_Controller = 0;
    private int PS4_Controller = 0;

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;


    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        Invoke("endIntro", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = 1;
                Xbox_One_Controller = 0;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = 0;
                Xbox_One_Controller = 1;

            }
        }


        if (Xbox_One_Controller == 1)
        {
            PlayerManager.Instance.SetControlType(2);
            MaskRota = Vector3.right * Input.GetAxisRaw("H_RStickXBOX") + Vector3.forward * Input.GetAxisRaw("V_RStickXBOX");
            if (MaskRota.sqrMagnitude > 0.0f)
            {
                MaskTransform.rotation = Quaternion.LookRotation(MaskRota, Vector3.up);
            }
            if (b_CanMove)
            {
                moveInput = new Vector3(Input.GetAxisRaw("H_LStickXBOX"), (myRigidbody.velocity.y / moveSpeed), Input.GetAxisRaw("V_LStickXBOX"));
                moveVelocity = moveInput * moveSpeed;
                if (moveVelocity != new Vector3(0, (myRigidbody.velocity.y / moveSpeed), 0))
                {
                    myAnimator.SetBool("IsMoving", true);
                }
                else if (moveVelocity == new Vector3(0, (myRigidbody.velocity.y / moveSpeed), 0))
                {
                    myAnimator.SetBool("IsMoving", false);
                }

                playerDirection = Vector3.right * Input.GetAxisRaw("H_LStickXBOX") + Vector3.forward * Input.GetAxisRaw("V_LStickXBOX");
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                }
            }
        }
        else if (PS4_Controller == 1)
        {
            PlayerManager.Instance.SetControlType(1);
            MaskRota = Vector3.right * Input.GetAxisRaw("H_RStickPS4") + Vector3.forward * Input.GetAxisRaw("V_RStickPS4");
            if (MaskRota.sqrMagnitude > 0.0f)
            {
                MaskTransform.rotation = Quaternion.LookRotation(MaskRota, Vector3.up);
            }
            if (b_CanMove)
            {
                moveInput = new Vector3(Input.GetAxisRaw("H_LStickPS4"), (myRigidbody.velocity.y / moveSpeed), Input.GetAxisRaw("V_LStickPS4"));
                moveVelocity = moveInput * moveSpeed;
                if (moveVelocity != new Vector3(0, (myRigidbody.velocity.y / moveSpeed), 0))
                {
                    myAnimator.SetBool("IsMoving", true);
                }
                else if (moveVelocity == new Vector3(0, (myRigidbody.velocity.y / moveSpeed), 0))
                {
                    myAnimator.SetBool("IsMoving", false);
                }

                playerDirection = Vector3.right * Input.GetAxisRaw("H_LStickPS4") + Vector3.forward * Input.GetAxisRaw("V_LStickPS4");
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                }
            }
        }
        else
        {
            PlayerManager.Instance.SetControlType(0);
            MaskRota = Vector3.right * Input.GetAxisRaw("HorizontalViewPC") + Vector3.forward * Input.GetAxisRaw("VerticalViewPC");
            if (MaskRota.sqrMagnitude > 0.0f)
            {
                MaskTransform.rotation = Quaternion.LookRotation(MaskRota, Vector3.up);
            }
            if (b_CanMove)
            {
                moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), (myRigidbody.velocity.y / moveSpeed), Input.GetAxisRaw("Vertical"));
                moveVelocity = moveInput * moveSpeed;
                if (moveVelocity != new Vector3(0, (myRigidbody.velocity.y / moveSpeed), 0))
                {
                    myAnimator.SetBool("IsMoving", true);
                }
                else if (moveVelocity == new Vector3(0, (myRigidbody.velocity.y / moveSpeed), 0))
                {
                    myAnimator.SetBool("IsMoving", false);
                }

                playerDirection = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (b_CanMove)
            myRigidbody.velocity = moveVelocity;
        else if (!b_CanMove)
            myRigidbody.velocity = new Vector3(0, 0, 0);
    }

    public void SetPlayerMove(bool CanMove)
    {
        b_CanMove = CanMove;
    }

    public void endIntro()
    {
        b_CanMove = true;
    }


}

