using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClientLibrary
{
    public class GameManager : MonoBehaviour
    {

        private void Awake() {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
    }
}

