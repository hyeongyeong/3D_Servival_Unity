using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    // ���� ����
    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // ���� ���θ� ���� ������Ʈ
    private CapsuleCollider capsuleCollider;

    // �ΰ���
    [SerializeField]
    private float lookSensitivity;


    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;

    // �ʿ� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private CrossHair theCrosshair;

    // ������ üũ ����
    private Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //theCamera = FindObjectOfType<Camera>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<CrossHair>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }
    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
        MoveCheck();
    }
    private void MoveCheck()
    {
        if(!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }    
    // �ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnimation(isCrouch);
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
        StartCoroutine(CrouchCoroutine());
    }
    //  �ε巯�� �ɱ� ���� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
        // yield return new WaitForSeconds(1f);
    }
    // �ɱ� �õ�
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    // ���� �õ�
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    // ���� �浹 Ȯ��
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.3f);
        theCrosshair.JumpingAnimation(!isGround);
    }
    // ���� ����
    private void Jump()
    {
        // ���� �� ���� ���� ����
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    private void CameraRotation()
    {
        // ���� ī�޶� ȸ��
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    private void CharacterRotation()
    {
        // �¿� ĳ���� ȸ��
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
        //Debug.Log(myRigid.rotation);
        //Debug.Log(myRigid.rotation.eulerAngles);
    }
}