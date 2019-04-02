using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    public static ObjectManager instance;
    public GameObject bulletPrefab;
    List<GameObject> bulletGroup = new List<GameObject>();
    Transform bulletHolder;
    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start () {
        CreateBullets(0);
    }
	
    void CreateBullets(int bulletGroupCount) {
        //불렛그룹을 만듬
        bulletHolder = new GameObject("bulletGroup").transform;

        bulletHolder.position = new Vector3(0, GameObject.Find("Player").transform.position.y, 0);
        //불렛 그룹 만큼 반복
        for (int i = 0; i<bulletGroupCount; i++) {


            //불렛 생성 , GameObject.Find("Player").transform.position, Quaternion.identity
            GameObject bullets = Instantiate(bulletPrefab) as GameObject;

            //불렛 홀더에 저장
            bullets.transform.SetParent(GameObject.Find("bulletGroup").transform);

            //bullets에 자식 불렛 리스트 저장
            bullets.SendMessage("SetBulletList");

            print(bullets.transform.childCount);

            //불렛 비활성화
            bullets.SetActive(false);

            //불렛 그룹 리스트에 추가
            bulletGroup.Add(bullets);
        }
            //bulletGroups.Add(bulletHolder as GameObject);

    }

    public GameObject GetBullet(Vector3 pos) {
        GameObject reqBullet = null;
        for(int i = 0; i< bulletGroup.Count; i++) {
            if(bulletGroup[i].activeSelf == false) {
                reqBullet = bulletGroup[i];
                break;
            }
        }

        if (reqBullet == null) {
            GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity) as GameObject;
            bulletGroup.Add(newBullet);
            newBullet.transform.SetParent(GameObject.Find("bulletGroup").transform);
            reqBullet = newBullet;
        }
        

        reqBullet.SetActive(true);
        reqBullet.transform.position = pos;
        return reqBullet;
    }

    public void ClearBullets() {
        for(int i = 0; i< bulletGroup.Count; i++) {
            bulletGroup[i].SetActive(false);
        }
    }
}
