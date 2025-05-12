using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class BuildSceneClientUI : MonoBehaviour
{
    [Header("Assign the 'Ready' button in the Inspector")]
    public Button readyButton;

    void Start()
    {
        // Hook up the button press
        if (readyButton != null)
        {
            readyButton.onClick.AddListener(OnClientReady);
        }
        else
        {
            Debug.LogError("ReadyButton is not assigned in the inspector.");
        }
    }

    void OnClientReady()
    {
        // Ensure only the client sends the ready signal
        if (NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsHost)
        {
            BuildSceneNetworkState.Instance.SetClientReadyServerRpc(true);
            readyButton.interactable = false; // Disable button to prevent double sends
            Debug.Log("Client marked as ready.");
        }
    }
}
