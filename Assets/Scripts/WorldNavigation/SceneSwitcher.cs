using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : PersistentMonoSingleton<SceneSwitcher>
{
    public void GoToDestination(Destination destination)
    {
        StartCoroutine(LoadScene(destination.SceneName));
    }


    private IEnumerator LoadScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            yield break;
        }

        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;
        
        while (!asyncOperation.isDone)
        {
            // unity stops at 0.9 to indicate that scene load is complete.
            if (asyncOperation.progress >= 0.9f)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                asyncOperation.allowSceneActivation = true;
                SceneManager.UnloadSceneAsync(activeScene);
            }

            yield return null;
        }
    }
    
    public struct Destination
    {
        public string SceneName;
        public string SpawnPoint;
    }
}
