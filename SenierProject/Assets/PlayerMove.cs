using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMove : MonoBehaviour {

    [System.Serializable]
    public class PlayerAnim
    {
        public AnimationClip idle;
        public AnimationClip runForward;
        public AnimationClip runBackward;
        public AnimationClip runRight;
        public AnimationClip runLeft;
    }

    public Image hpBar;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    public PlayerAnim playerAnim;
    float h, v;
    float moveSpeed = 7.0f;
    float rotateSpeed = 200.0f;
    Transform tr;
    Animation anim;
    float initHp = 100;
    float currHp;
    	
	void Start () {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        currHp = initHp;
	}
		
	void Update () {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir =   // 방향!
            (Vector3.forward * v) + (Vector3.right * h);
        transform.Translate(moveDir.normalized *
            moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * rotateSpeed *
            Time.deltaTime * Input.GetAxis("Mouse X"));        

        if(v>=0.1f) {
            anim.CrossFade(playerAnim.runForward.name, 0.3f);
        } else if(v<=-0.1f) {
            anim.CrossFade(playerAnim.runBackward.name, 0.3f);
        } else if(h>=0.1f) {
            anim.CrossFade(playerAnim.runRight.name, 0.3f);
        } else if (h<=-0.1f) {
            anim.CrossFade(playerAnim.runLeft.name, 0.3f);
        } else {
            anim.CrossFade(playerAnim.idle.name, 0.3f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "PUNCH") {
            currHp -= 10.0f;
            hpBar.fillAmount = currHp / initHp;
            if(currHp <= 0.0f) {
                //GameController.instance.isGameOver = true;
                OnPlayerDie();
            }
        }
    }

    /*
    void PlayerDie() {
        GameObject[] monsters =
           GameObject.FindGameObjectsWithTag("MONSTER");
        foreach(GameObject monster in monsters) {
            monster.SendMessage("OnPlayerDie",
                SendMessageOptions.DontRequireReceiver);
        }
    }
    */
}
