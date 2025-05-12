using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class BuildSceneClientUI : MonoBehaviour
{
    public Button readyButton;

    void Start()
    {
        if (readyButton != null)
        {
            readyButton.onClick.AddListener(OnClientReady);
        }
    }

    void OnClientReady()
    {
        if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost)
        {
            var player = NetworkManager.Singleton.LocalClient.PlayerObject;
            if (player != null)
            {
                player.GetComponent<ClientReadyCommunicator>().SendReadyServerRpc();
                readyButton.interactable = false;
            }
            else
            {
                Debug.LogError("PlayerObject is null on client.");
            }
        }
    }
}
