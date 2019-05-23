using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakableObject : MonoBehaviour {
    public int hp = 3;

    private void Start()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("DD" + collision);
        if(collision.transform.tag == "Bullet")
        {
            if (hp > 0)
            {
                hp -= 1;
                print(hp);
            }
            else
                DestroyObject();
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
