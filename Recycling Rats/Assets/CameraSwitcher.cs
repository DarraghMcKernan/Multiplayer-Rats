using Unity.Netcode;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject hostCamera;
    public GameObject clientCamera;

    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            hostCamera.SetActive(true);
            clientCamera.SetActive(false);
        }
        else
        {
            hostCamera.SetActive(false);
            clientCamera.SetActive(true);
        }
    }
}
