using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

namespace ClientLibrary
{
    public class GunCtrl : MonoBehaviourPun, IPunObservable
    {
        public enum State
        {
            Ready,
            Empty,
            Reloading
        }

        public State gunState { get; set; }

        public int ammoRemain = 100;    // 남은 전체 탄알
        public int magCapacity = 25;    // 탄창 용량
        public int magAmmo;    // 현재 탄창에 남아 있는 탄알

        public float timeBetFire = 0.12f;   // 탄알 발사 간격
        public float reloadTime = 1.8f;     // 재장전 소요 시간
        public float lastFireTime;     // 총을 마지막으로 발사한 시점

        //public BulletCtrl bullet;
        public GameObject bullet;
        private Button reloadBt;
        private Button fireBt;
        private Text ammoText;

        //public float timeBetweenShots;
        //private float shotCounter;

        public Transform firePoint;
        //public Transform firePointRotation;
        public Animator animator;

        private bool debugCheck;
        private void OnEnable()
        {
            debugCheck = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().debugCheck;
            if(debugCheck == false)
            {
                if (!photonView.IsMine)
                {
                    return;
                }
            }
            
            reloadBt = GameObject.Find("/Canvas/Reload").GetComponent<Button>();
            fireBt = GameObject.Find("/Canvas/Fire").GetComponent<Button>();
            ammoText = GameObject.Find("/Canvas/Ammo").GetComponent<Text>();
            animator = FindObjectOfType<Animator>();
            magAmmo = magCapacity;
            ammoText.text = magAmmo + " / " + ammoRemain;
            gunState = State.Ready;

            reloadBt.onClick.AddListener(() => Reload());
            fireBt.onClick.AddListener(() => Fire());
        }

        void FixedUpdate()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        void Fire()
        {
            if(gunState == State.Ready && Time.time >= lastFireTime + timeBetFire)
            {
                lastFireTime = Time.time;

                if (magAmmo <= 0)
                {
                    gunState = State.Empty;
                }
                else
                {
                    animator.SetTrigger("Shot");
                    if (debugCheck == false) {
                        photonView.RPC("Shot", RpcTarget.All);
                    }
                    else {
                        Shot();
                    }
                    magAmmo--;
                    ammoText.text = magAmmo + " / " + ammoRemain;
                }
            }
        }

        [PunRPC]
        void Shot()
        {
                Vector3 bulletVector = new Vector3(firePoint.position.x, 1.5f, firePoint.position.z);
                Quaternion bulletQuaternion = new Quaternion(0, firePoint.rotation.y, 0, firePoint.rotation.w);
                GameObject newBullet = PhotonNetwork.Instantiate(bullet.name, bulletVector, bulletQuaternion);
        }

        [PunRPC]
        public void AddAmmo(int ammo)
        {
            ammoRemain += ammo;
            ammoText.text = magAmmo + " / " + ammoRemain;
        }

        public bool Reload()
        {
            if(gunState == State.Reloading || magAmmo >= magCapacity)
            {
                Debug.Log(gunState);
                return false;
            }

            StartCoroutine(ReloadRoutine());
            return true;
        }

        private IEnumerator ReloadRoutine()
        {
            gunState = State.Reloading;
            animator.SetTrigger("Reload");

            yield return new WaitForSeconds(reloadTime);

            int ammoToFill = magCapacity - magAmmo;

            if(ammoRemain < ammoToFill)
            {
                ammoToFill = ammoRemain;
            }

            magAmmo += ammoToFill;
            ammoRemain -= ammoToFill;
            ammoText.text = magAmmo + " / " + ammoRemain;

            gunState = State.Ready;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // 로컬 오브젝트라면 쓰기 부분이 실행됨
            if (stream.IsWriting)
            {
                // 남은 탄약수를 네트워크를 통해 보내기
                stream.SendNext(ammoRemain);
                // 탄창의 탄약수를 네트워크를 통해 보내기
                stream.SendNext(magAmmo);
                // 현재 총의 상태를 네트워크를 통해 보내기
                stream.SendNext(gunState);
            }
            else
            {
                // 리모트 오브젝트라면 읽기 부분이 실행됨
                // 남은 탄약수를 네트워크를 통해 받기
                ammoRemain = (int)stream.ReceiveNext();
                // 탄창의 탄약수를 네트워크를 통해 받기
                magAmmo = (int)stream.ReceiveNext();
                // 현재 총의 상태를 네트워크를 통해 받기
                gunState = (State)stream.ReceiveNext();
            }
        }
    }
}