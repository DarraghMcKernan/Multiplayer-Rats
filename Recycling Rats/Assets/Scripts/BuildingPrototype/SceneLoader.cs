using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    public GameObject Car;
    public Rigidbody[] cockpit;
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
            foreach (Rigidbody rb in cockpit)
            {
                if (rb != null)
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                }
            }

            CarMovement.movingAllowed = true;

            if (Car.tag == "Left Car")
            {
                Car.transform.SetPositionAndRotation(new Vector3(-10f, 4f, 0), transform.rotation);
            }
            if (Enemy.tag == "Right Car")
            {
                Enemy.transform.SetPositionAndRotation(new Vector3(10f, 4f, 0), transform.rotation);
            }

            Car.AddComponent<CarMovement>();
            Car.AddComponent<HealthManager>();
            Enemy.AddComponent<CarMovement>();
            Enemy.AddComponent<HealthManager>();

            CarInstanceStore.hostCar = GameObject.FindWithTag("Left Car");
            CarInstanceStore.clientCar = GameObject.FindWithTag("Right Car");

            DontDestroyOnLoad(CarInstanceStore.hostCar);
            DontDestroyOnLoad(CarInstanceStore.clientCar);

            NetworkManager.Singleton.SceneManager.LoadScene("Battle Scene", LoadSceneMode.Single);
        }
    }

    public void loadScene()
    {
        Car = GameObject.FindGameObjectWithTag("Left Car");
        Enemy = GameObject.FindGameObjectWithTag("Right Car");

        GameObject[] cockpitObjects = GameObject.FindGameObjectsWithTag("Cockpit");
        cockpit = new Rigidbody[cockpitObjects.Length];
        for (int i = 0; i < cockpitObjects.Length; i++)
        {
            cockpit[i] = cockpitObjects[i].GetComponent<Rigidbody>();
        }

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

        foreach (Rigidbody rb in cockpit)
        {
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        CarMovement.movingAllowed = true;

        if (Car.tag == "Left Car")
        {
            Car.transform.SetPositionAndRotation(new Vector3(-10f, 4f, 0), transform.rotation);
        }
        if (Enemy.tag == "Right Car")
        {
            Enemy.transform.SetPositionAndRotation(new Vector3(10f, 4f, 0), transform.rotation);
        }

        Car.AddComponent<CarMovement>();
        Car.AddComponent<HealthManager>();
        Enemy.AddComponent<CarMovement>();
        Enemy.AddComponent<HealthManager>();

        //Enemy.transform.SetPositionAndRotation(new Vector3(10f, 4f, 0f), transform.rotation);

        SceneManager.MoveGameObjectToScene(Enemy, SceneManager.GetSceneByName(SceneName));

        SceneManager.MoveGameObjectToScene(Car, SceneManager.GetSceneByName(SceneName));

        SceneManager.UnloadSceneAsync(currentScene);
    }
}
