using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Audio;

public class PlaceBlockCtrl : MonoBehaviourPun {
    public int hp =3;
    public GameObject explosionPrefab;
    public GameObject hitPrefab;
    public AudioClip hitSound;

    public AudioSource audioSource;
    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void Hit()
    {
        hp--;
        PhotonNetwork.Instantiate(hitPrefab.name, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(hitSound);
        if (hp < 0)
        {
            PhotonNetwork.Instantiate(explosionPrefab.name, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

