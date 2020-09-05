using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Loading : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(LoadScene(1));
    }

    IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(2);

        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
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
