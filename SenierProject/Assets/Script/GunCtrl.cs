using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ClientLibrary
{
    public class GunCtrl : MonoBehaviourPun
    {
        public bool isFireing;

        //public BulletCtrl bullet;
        public GameObject bullet;
        public float bulletSpeed;

        public float timeBetweenShots;
        private float shotCounter;

        public Transform firePoint;
        public Transform firePointRotation;

        [PunRPC]
        void Shoot()
        {
            if (isFireing)
            {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    shotCounter = timeBetweenShots;
                    //firePointRotation.eulerAngles = new Vector3(0, firePoint.eulerAngles.y, firePoint.eulerAngles.z);
                    /*BulletCtrl newBullet = Instantiate(bullet,
                        new Vector3(firePoint.position.x, 0.75f, firePoint.position.z),
                        new Quaternion(0, firePoint.rotation.y, 0, firePoint.rotation.w)) as BulletCtrl;
                    newBullet.speed = bulletSpeed;*/
                    Vector3 bulletVector = new Vector3(firePoint.position.x, 0.75f, firePoint.position.z);
                    Quaternion bulletQuaternion = new Quaternion(0, firePoint.rotation.y, 0, firePoint.rotation.w);
                    GameObject newBullet = PhotonNetwork.Instantiate(bullet.name, bulletVector, bulletQuaternion);
                }
            }
            else
            {
                shotCounter = 0;
            }
        }

        // Update is called once per frame
        void Update() {
            if (!photonView.IsMine)
            {
                return;
            }
            photonView.RPC("Shoot", RpcTarget.All);
        }
    }
}