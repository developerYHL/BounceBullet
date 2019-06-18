﻿using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ServerManager : MonoBehaviourPunCallbacks, IPunObservable {

    // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    public static ServerManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<ServerManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static ServerManager m_instance; // 싱글톤이 할당될 static 변수
    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹
    public Transform[] playerSpawn;

    public override void OnEnable()
    {
        base.OnEnable();

        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    // 게임 시작과 동시에 플레이어가 될 게임 오브젝트를 생성
    private void Start()
    {

    }

    private void StartGame()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        player.GetComponent<PhotonTransformView>().enabled = false;
        player.transform.position = playerSpawn[player.GetPhotonView().OwnerActorNr - 1].position;
        player.GetComponent<PhotonTransformView>().enabled = true;
    }

    // 주기적으로 자동 실행되는, 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
        {
            // 네트워크를 통해 score 값을 보내기
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨
            // 네트워크를 통해 score 값 받기
        }
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }
}
