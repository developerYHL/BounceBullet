using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CameraSetup : MonoBehaviourPun {
    public CinemachineVirtualCamera followcam;

    void Start () {
        if (photonView.IsMine)
        {
            followcam = FindObjectOfType<CinemachineVirtualCamera>();
            followcam.Follow = transform;
            //followcam.LookAt = transform;
        }
	}
}
