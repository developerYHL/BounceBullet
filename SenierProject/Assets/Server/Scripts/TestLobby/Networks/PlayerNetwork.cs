using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNetwork : MonoBehaviourPunCallbacks {

    public static PlayerNetwork instance;
    public string playerName { get; private set; }
    private PhotonView photonView;
    private int playersInGame = 0;

    private void Awake()
    {
        instance = this;
        photonView = GetComponent<PhotonView>();

        playerName = "Distul#" + Random.Range(1000, 9999);

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Game")
        {
            if (PhotonNetwork.IsMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        playersInGame = 1;
        photonView.RPC("Rpc_LoadGameOthers", RpcTarget.Others);
    }

    private void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void Rpc_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if(playersInGame == PhotonNetwork.PlayerList.Length)
        {
            print("모든 플레이어가 들어옴");
        }
    }
}
