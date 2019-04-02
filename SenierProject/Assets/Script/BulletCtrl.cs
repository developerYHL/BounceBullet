using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {
    int activeCount = 0;
    public List<GameObject> bullets = new List<GameObject>();
    float time;
    float continuousTime = 10.0f;

    private void OnEnable() {
        time = 0;
        gameObject.BroadcastMessage("CalculateTarget", true);
    }

    private void Update() {
        time += Time.deltaTime;
        if (time > continuousTime) {
            gameObject.SetActive(false);
        }
        
    }
    //public void ActiveCount() {
    //    //print("activeCount : " + activeCount);
    //    if (activeCount < transform.childCount-1)
    //        activeCount++;
    //    else {
    //        print(activeCount);
    //        activeCount = 0;
            
    //    }
    //}

    public void SetBulletList() {
        
        int iCount = transform.childCount;
        for (int i = 0; i < iCount; i++) {

            Transform trChild = transform.GetChild(i);
            bullets.Add(trChild.gameObject);
            //trChild.gameObject.SetActive(true);
        }
    }
}
