using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCanvasCtl : MonoBehaviour {
    private float scaleX;
    private float scaleY;
	// Use this for initialization
	void Start () {
        scaleX = gameObject.transform.parent.transform.localScale.x;
        scaleY = gameObject.transform.parent.transform.localScale.y;

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void ChangeCanvsScale() {
        if (gameObject.transform.parent.transform.localScale.x < 0)
            gameObject.transform.localScale = new Vector3(-scaleX, scaleY, 0);
        else
            gameObject.transform.localScale = new Vector3(scaleX, scaleY, 0);
    }
}
