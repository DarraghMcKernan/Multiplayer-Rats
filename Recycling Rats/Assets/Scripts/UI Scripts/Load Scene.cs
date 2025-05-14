using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoadScene : MonoBehaviour
{
    public Button myButton;

    private void Start()
    {
        myButton.onClick.AddListener(StartGamePressed);
    }

    void StartGamePressed()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("Battle Scene", LoadSceneMode.Single);
        }
    }

}
