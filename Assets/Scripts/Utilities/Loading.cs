using Firebase;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider progressSlider;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        StartCoroutine(LoadScene(1));
    }

    IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            progressSlider.value = progress;

            yield return null;
        }
    }

    private IEnumerator Initialize()
    {
        while(GameData.instance == null)
        {
            yield return null;
        }

        DataManager.Verify();
    }
}
