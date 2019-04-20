using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CameraSetup : MonoBehaviourPun {

	void Start () {
        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followcam = FindObjectOfType<CinemachineVirtualCamera>();
            followcam.Follow = transform;
            followcam.LookAt = transform;
        }
	}
}
