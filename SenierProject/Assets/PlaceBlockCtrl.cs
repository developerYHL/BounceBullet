using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBlockCtrl : MonoBehaviour {
    public int hp =3;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void Hit()
    {
        hp--;
        print("block hp : " + hp);
        if (hp < 0)
        {
            print("block hpdasd : " + hp);
            Destroy(gameObject);
        }
    }
}
