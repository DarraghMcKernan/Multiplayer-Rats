using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string SceneName;
    public GameObject Car;
    public Rigidbody cockpit;
    public GameObject Enemy;

    void Start()
    {

    }

    public void loadScene()
    {
        Car = GameObject.FindGameObjectWithTag("Left Car");
        cockpit = GameObject.FindGameObjectWithTag("Cockpit").GetComponent<Rigidbody>();

        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // Spawn Enemy Car One prefab
        GameObject enemyCarOnePrefab = Resources.Load<GameObject>("Enemy Car One");
        GameObject enemyCarTwoPrefab = Resources.Load<GameObject>("Enemy Car Two");
        GameObject enemyCarThreePrefab = Resources.Load<GameObject>("Enemy Car Three");

        // Set the current Scene to be able to unload it later
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();

        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
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
        Car.AddComponent<CarMovement>();
        Car.AddComponent<HealthManager>();

        //GameObject Enemy = Instantiate(enemyCarOnePrefab, new Vector3(9.24f, 4.09f, 0), Quaternion.identity);

        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(Car, SceneManager.GetSceneByName(SceneName));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
