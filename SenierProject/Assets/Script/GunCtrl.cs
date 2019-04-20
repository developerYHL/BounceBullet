using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour {
    public bool isFireing;

    public BulletCtrl bullet;
    public float bulletSpeed;

    public float timeBetweenShots;
    private float shotCounter;

    public Transform firePoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isFireing) {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0) {
                shotCounter = timeBetweenShots;
                firePoint.rotation = new Quaternion(0, firePoint.rotation.y, firePoint.rotation.z, firePoint.rotation.w);
                BulletCtrl newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as BulletCtrl;
                newBullet.speed = bulletSpeed;
            }
        }
        else {
            shotCounter = 0;
        }
	}
}
