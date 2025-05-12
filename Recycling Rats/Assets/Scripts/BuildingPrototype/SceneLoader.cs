using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    public GameObject Car;
    public Rigidbody cockpit;
    public GameObject Enemy;
    public Button startGameButton;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => BuildSceneNetworkState.Instance != null);

        BuildSceneNetworkState.Instance.clientIsReady.OnValueChanged += (oldVal, newVal) =>
        {
            if (newVal)
            {
                startGameButton.gameObject.SetActive(true);
            }
        };
    }

    public void OnHostStartGame()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        }
    }

    public void loadScene()
    {
        Car = GameObject.FindGameObjectWithTag("Left Car");
        if (Car == null)
        {
            Car = GameObject.FindGameObjectWithTag("Right Car");
        }

        cockpit = GameObject.FindGameObjectWithTag("Cockpit").GetComponent<Rigidbody>();

        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        GameObject enemyCarOnePrefab = Resources.Load<GameObject>("Enemy Car One");
        GameObject enemyCarTwoPrefab = Resources.Load<GameObject>("Enemy Car Two");
        GameObject enemyCarThreePrefab = Resources.Load<GameObject>("Enemy Car Three");

        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        cockpit.useGravity = true;
        cockpit.isKinematic = false;
        CarMovement.movingAllowed = true;

        if (Car.tag == "Left Car")
        {
            Car.transform.SetPositionAndRotation(new Vector3(-10f, 4f, 0), transform.rotation);
        }
        if (Car.tag == "Right Car")
        {
            Car.transform.SetPositionAndRotation(new Vector3(10f, 4f, 0), transform.rotation);
        }

        Car.AddComponent<CarMovement>();
        Car.AddComponent<HealthManager>();

        SceneManager.MoveGameObjectToScene(Car, SceneManager.GetSceneByName(SceneName));
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
