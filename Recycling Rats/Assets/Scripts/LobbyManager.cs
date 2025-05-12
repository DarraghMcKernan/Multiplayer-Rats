using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button startGameButton;
    public Image player1Indicator;
    public Image player2Indicator;

    private void Start()
    {
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
        startGameButton.onClick.AddListener(StartGame);

        startGameButton.gameObject.SetActive(false);
        player1Indicator.color = Color.red;
        player2Indicator.color = Color.red;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    void OnClientConnected(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        int connectedCount = NetworkManager.Singleton.ConnectedClients.Count;

        if (connectedCount >= 1)
            player1Indicator.color = Color.green;

        if (connectedCount >= 2)
            player2Indicator.color = Color.green;

        if (NetworkManager.Singleton.IsHost && connectedCount == 2)
            startGameButton.gameObject.SetActive(true);
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Host started");
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Client attempting to connect");
    }

    void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("BuildScene", LoadSceneMode.Single);
        }
    }
}
