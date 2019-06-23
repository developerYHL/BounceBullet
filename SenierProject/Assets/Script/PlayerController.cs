using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
namespace ClientLibrary
{
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviourPun
    {
        public bool useJoystic = false;
        //public Button tempButton;

        public float moveSpeed;
        private Rigidbody myRigidbody;
        public Joystick joystick;
        public GunCtrl gun;

        private Vector3 moveInput;
        private Vector3 moveVelocity;

        private Camera mainCamra;

        //public Transform rightGunBone;
        //public Transform leftGunBone;
        //public Arsenal[] arsenal;
        public bool testCheck = false;

        private Animator animator;
        public Transform gunPivot;
        public Transform leftHandMount;
        public Transform rightHandMount;
        //public GunCtrl theGun;

        void Awake() {
            animator = GetComponent<Animator>();
            if (!photonView.IsMine)
            {
                return;
            }
            //temp Btn 생성
            //Button dada = Instantiate(tempButton, GameObject.Find("Canvas").transform);
            myRigidbody = GetComponent<Rigidbody>();
            mainCamra = FindObjectOfType<Camera>();
            joystick = GameObject.Find("/Canvas/JoystickPanel/Fixed Joystick").GetComponent<Joystick>();
            //if (arsenal.Length > 0)
            //    SetArsenal(arsenal[0].name);
        }
        private void Start() {
            //animator.SetBool("Aiming", false);
            //SetArsenal("Rifle");
        }

        private void Update() {

            if (!photonView.IsMine && testCheck)
            {
                return;
            }
            JoysticMove();

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            /*if (useJoystic) {
                JoysticMove();
            }
            else {
                KeybordMove();
            }*/
#endif

        }

        private void FixedUpdate() {
            //myRigidbody.velocity = moveVelocity;
        }

        //public void SetArsenal(string name) {
        //    foreach (Arsenal hand in arsenal) {
        //        if (hand.name == name) {
        //            if (rightGunBone.childCount > 0)
        //                Destroy(rightGunBone.GetChild(0).gameObject);
        //            if (leftGunBone.childCount > 0)
        //                Destroy(leftGunBone.GetChild(0).gameObject);
        //            if (hand.rightGun != null) {
        //                GameObject newRightGun = PhotonNetwork.Instantiate(gun.name, Vector3.zero, Quaternion.identity);
        //                newRightGun.transform.parent = rightGunBone;
        //                newRightGun.transform.localPosition = Vector3.zero;
        //                newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //                theGun = newRightGun.GetComponent<GunCtrl>();
        //                theGun.firePoint = newRightGun.transform;
        //                print(newRightGun);
        //            }
        //            if (hand.leftGun != null) {
        //                GameObject newLeftGun = Instantiate(hand.leftGun);
        //                newLeftGun.transform.parent = leftGunBone;
        //                newLeftGun.transform.localPosition = Vector3.zero;
        //                newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //                theGun = newLeftGun.GetComponent<GunCtrl>();
        //            }
        //            animator.runtimeAnimatorController = hand.controller;

        //            return;
        //        }
        //    }
        //}

        //[System.Serializable]
        //public struct Arsenal
        //{
        //    public string name;
        //    public GameObject rightGun;
        //    public GameObject leftGun;
        //    public RuntimeAnimatorController controller;
        //}

        private void JoysticMove() {
            Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

            if (moveVector != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveVector);
                //transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
                myRigidbody.MovePosition(transform.position + moveVector * moveSpeed * Time.deltaTime);
            }
            /*else {
                animator.SetFloat("Speed_f", 0.0f);
            }*/
            animator.SetFloat("Move", Mathf.Abs(joystick.Horizontal) + Mathf.Abs(joystick.Vertical));
        }

        private void KeybordMove() {

            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.z) > 0) {
                animator.SetFloat("Speed_f", 0.25f);
            }
            else {
                animator.SetFloat("Speed_f", 0.0f);
            }
            moveVelocity = moveInput * moveSpeed;
            Ray cameraRay = mainCamra.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (moveVelocity != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveVelocity);
                transform.Translate(moveVelocity * moveSpeed * Time.deltaTime, Space.World);
                animator.SetFloat("Speed_f", 0.25f);
            }

            if (groundPlane.Raycast(cameraRay, out rayLength)) {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                //transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }

            if (Input.GetMouseButtonDown(0)) {
                //animator.SetBool("Aiming", true);
                //animator.SetTrigger("Attack");
            }
            if (Input.GetMouseButtonUp(0)) {
                //animator.SetBool("Aiming", false);
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {
            // 총의 기준점 gunPivot을 3D 모델의 오른쪽 팔꿈치 위치로 이동
            gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

            // IK를 사용하여 왼손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

            // IK를 사용하여 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춘다
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
        }
    }
}