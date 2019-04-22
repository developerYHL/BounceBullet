using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClientLibrary
{
    public class GunCtrl : MonoBehaviour
    {
        public bool isFireing;

        public BulletCtrl bullet;
        public float bulletSpeed;

        public float timeBetweenShots;
        private float shotCounter;

        public Transform firePoint;
        public Transform firePointRotation;

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (isFireing) {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0) {
                    shotCounter = timeBetweenShots;
                    //firePointRotation.eulerAngles = new Vector3(0, firePoint.eulerAngles.y, firePoint.eulerAngles.z);
                    BulletCtrl newBullet = Instantiate(bullet,
                        new Vector3(firePoint.position.x, 0.75f, firePoint.position.z),
                        new Quaternion(0, firePoint.rotation.y, 0, firePoint.rotation.w)) as BulletCtrl;
                    newBullet.speed = bulletSpeed;
                }
            }
            else {
                shotCounter = 0;
            }
        }
    }
}