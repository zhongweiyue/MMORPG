using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class SceneManager : MonoSingleton<SceneManager>
{
    UnityAction<float> onProgress = null;
    // Use this for initialization
    protected override void OnStart()
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;
        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
        if (User.Instance.currentCharacterObject)
        {
            if (User.Instance.currentCharacterObject.gameObject.GetComponent<NavMeshAgent>())
            {
                User.Instance.currentCharacterObject.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);
        if (NavManager.Instance.NavState) 
        {
            NavManager.Instance.LoadSceneComplete = true;
        }
        
        StartCoroutine(openNavMeshAgent());
        Debug.Log("LevelLoadCompleted:" + obj.progress);
    }

    IEnumerator openNavMeshAgent() 
    {
        yield return new WaitForSeconds(3f);
        if (User.Instance.currentCharacterObject)
        {
            if (User.Instance.currentCharacterObject.gameObject.GetComponent<NavMeshAgent>())
            {
                User.Instance.currentCharacterObject.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                if (NavManager.Instance.NavState && NavManager.Instance.LoadSceneComplete && NavManager.Instance.NavTargetNpc != 0)
                {
                    if (User.Instance.currentCharacterObject.gameObject.GetComponent<NavMeshAgent>().enabled)
                    {
                        User.Instance.currentCharacterObject.StartNav(NavManager.Instance.GetNpcPos(NavManager.Instance.NavTargetNpc));
                    }
                }
            }
        }
    }
}
