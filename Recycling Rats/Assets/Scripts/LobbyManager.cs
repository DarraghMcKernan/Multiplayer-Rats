using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LobbyUI : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button startGameButton;
    public Image player1Indicator;
    public Image player2Indicator;
    public TMP_InputField joinCodeInput;

    private void Start()
    {
        hostButton.onClick.AddListener(() => StartCoroutine(StartRelayHost()));
        joinButton.onClick.AddListener(StartRelayClient);
        startGameButton.onClick.AddListener(StartGame);

        startGameButton.gameObject.SetActive(false);
        player1Indicator.color = Color.red;
        player2Indicator.color = Color.red;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    IEnumerator StartRelayHost()
    {
        var task = RelayManager.Instance.CreateRelay();
        yield return new WaitUntil(() => task.IsCompleted);
        string joinCode = task.Result;
        Debug.Log("Share this join code with your friend: " + joinCode);
    }

    void StartRelayClient()
    {
        string code = joinCodeInput.text;
        RelayManager.Instance.JoinRelay(code);
    }

    void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count >= 1)
            player1Indicator.color = Color.green;

        if (NetworkManager.Singleton.ConnectedClientsList.Count >= 2)
            player2Indicator.color = Color.green;

        if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.ConnectedClientsList.Count == 2)
            startGameButton.gameObject.SetActive(true);
    }

    void StartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("BuildScene", LoadSceneMode.Single);
        }
    }
}
