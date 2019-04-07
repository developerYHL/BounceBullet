using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {
    int activeCount = 0;
    public List<GameObject> bullets = new List<GameObject>();
    float time;
    public float continuousTime = 1.5f;

    private void OnEnable() {
        time = 0;
        gameObject.BroadcastMessage("CalculateTarget", true);
    }

    private void Update() {
        //time += Time.deltaTime;
        //if (time > continuousTime) {
        //    gameObject.SetActive(false);
        //}
        
    }
    public void ActiveCount() {
        //print("activeCount : " + activeCount);
        if (activeCount < transform.childCount - 1)
            activeCount++;
        else {
            print(activeCount);
            activeCount = 0;
            Invoke("ActiveFalse", 1.6f);
            
        }
    }

    public void ActiveFalse() {
        gameObject.SetActive(false);
    }
    public void SetBulletList() {
        
        int iCount = transform.childCount;
        for (int i = 0; i < iCount; i++) {

            Transform trChild = transform.GetChild(i);
            bullets.Add(trChild.gameObject);
            //trChild.gameObject.SetActive(true);
        }
    }
}
