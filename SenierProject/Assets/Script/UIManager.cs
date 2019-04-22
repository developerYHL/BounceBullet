using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClientLibrary
{

    public class UIManager : MonoBehaviour
    {
        bool useJoystic;
        private void Start() {
             useJoystic = GameObject.Find("/Sci-Fi_Soldier(Clone)").GetComponent<PlayerController>().useJoystic;
        }
        public void UseJoysticBtn() {
            if(GameObject.Find("/Sci-Fi_Soldier(Clone)").GetComponent<PlayerController>().useJoystic == true) {
                GameObject.Find("/Sci-Fi_Soldier(Clone)").GetComponent<PlayerController>().useJoystic = false;
            }else
                GameObject.Find("/Sci-Fi_Soldier(Clone)").GetComponent<PlayerController>().useJoystic = true;

        }
    }
}