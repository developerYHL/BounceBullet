using UnityEngine;
using System.Collections;
using Photon.Pun;
namespace ClientLibrary
{
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviourPun
    {
        public float moveSpeed;
        private Rigidbody myRigidbody;
        public Joystick joystick;
        public GameObject gun;

        private Vector3 moveInput;
        private Vector3 moveVelocity;

        private Camera mainCamra;

        public Transform rightGunBone;
        public Transform leftGunBone;
        public Arsenal[] arsenal;
        public bool testCheck = false;

        private Animator animator;

        public GunCtrl theGun;

        void Awake() {
            myRigidbody = GetComponent<Rigidbody>();
            mainCamra = FindObjectOfType<Camera>();
            animator = GetComponent<Animator>();
            if (arsenal.Length > 0)
                SetArsenal(arsenal[0].name);
        }
        private void Start() {
            animator.SetBool("Aiming", false);
            SetArsenal("Rifle");
        }

        private void Update() {
            if (!photonView.IsMine && testCheck)
            {
                print("A");
                return;
            }

#if UNITY_STANDALONE || UNITY_WEBPLAYER

            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            if (Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.z) > 0 ) {
                animator.SetFloat("Speed", 0.5f);
            }
            else {
                animator.SetFloat("Speed", 0.0f);
            }
            moveVelocity = moveInput * moveSpeed;

            Ray cameraRay = mainCamra.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength)) {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }

            if (Input.GetMouseButtonDown(0)) {
                animator.SetBool("Aiming", true);
                //animator.SetTrigger("Attack");
                theGun.isFireing = true;
            }
            if (Input.GetMouseButtonUp(0)) {
                animator.SetBool("Aiming", false);
                theGun.isFireing = false;
            }
#endif
            //Move();
        }

        private void FixedUpdate() {
            myRigidbody.velocity = moveVelocity;
        }

        public void SetArsenal(string name) {
            foreach (Arsenal hand in arsenal) {
                if (hand.name == name) {
                    if (rightGunBone.childCount > 0)
                        Destroy(rightGunBone.GetChild(0).gameObject);
                    if (leftGunBone.childCount > 0)
                        Destroy(leftGunBone.GetChild(0).gameObject);
                    if (hand.rightGun != null) {
                        GameObject newRightGun = PhotonNetwork.Instantiate(gun.name, Vector3.zero, Quaternion.identity);
                        newRightGun.transform.parent = rightGunBone;
                        newRightGun.transform.localPosition = Vector3.zero;
                        newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        theGun = newRightGun.GetComponent<GunCtrl>();
                        theGun.firePoint = newRightGun.transform;
                        print(newRightGun);
                    }
                    if (hand.leftGun != null) {
                        GameObject newLeftGun = Instantiate(hand.leftGun);
                        newLeftGun.transform.parent = leftGunBone;
                        newLeftGun.transform.localPosition = Vector3.zero;
                        newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        theGun = newLeftGun.GetComponent<GunCtrl>();
                    }
                    animator.runtimeAnimatorController = hand.controller;

                    return;
                }
            }
        }

        [System.Serializable]
        public struct Arsenal
        {
            public string name;
            public GameObject rightGun;
            public GameObject leftGun;
            public RuntimeAnimatorController controller;
        }

        /*private void Move()
        {
            Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);

            if (moveVector != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveVector);
                transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
            }
        }*/
    }
}