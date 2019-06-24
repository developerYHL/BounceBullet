using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaceBlockCtrl : MonoBehaviourPun {
    public int hp =3;
    public GameObject explosionPrefab;
    public GameObject hitPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void Hit()
    {
        hp--;
        PhotonNetwork.Instantiate(hitPrefab.name, transform.position, Quaternion.identity);
        if (hp < 0)
        {
            PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
