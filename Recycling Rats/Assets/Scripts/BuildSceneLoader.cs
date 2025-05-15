using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildSceneLoader : NetworkBehaviour
{
    public static BuildSceneLoader Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadHostBuildScene()
    {
        SceneManager.LoadScene("Building", LoadSceneMode.Single);
    }
}