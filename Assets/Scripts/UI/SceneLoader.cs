using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour {

    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Text LoadProgressText;
    
    public void LoadScene(string SceneName)
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadAsync(SceneName));
    }

    IEnumerator LoadAsync(string SceneName)
    {
        AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(SceneName);
        while (!LoadOperation.isDone)
        {
            int progress = (int)((LoadOperation.progress/0.9f) * 100);
            LoadProgressText.text = progress.ToString() + "%";
            Debug.Log("Loading... " + progress + "%");
            yield return null;
        }
    }
}
