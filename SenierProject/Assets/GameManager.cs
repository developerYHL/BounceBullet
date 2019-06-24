using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClientLibrary
{
    public class GameManager : MonoBehaviour
    {

        private void Update()
        {
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
#endif
        }
    }
}

