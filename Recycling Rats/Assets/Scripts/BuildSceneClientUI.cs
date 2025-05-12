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
        else
        {
            Debug.LogError("ReadyButton not assigned!");
        }
    }

    void OnClientReady()
    {
        if (!NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost)
            return;

        if (BuildSceneNetworkState.Instance != null)
        {
            BuildSceneNetworkState.Instance.SetClientReadyServerRpc(true);
            readyButton.interactable = false;
            Debug.Log("Client marked ready.");
        }
        else
        {
            Debug.LogWarning("BuildSceneNetworkState.Instance is null. This object only exists on the host.");
        }
    }
}
