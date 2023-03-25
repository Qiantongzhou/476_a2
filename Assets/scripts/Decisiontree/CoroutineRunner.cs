using UnityEngine;
using System.Collections;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject coroutineRunnerObject = new GameObject("CoroutineRunner");
                _instance = coroutineRunnerObject.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(coroutineRunnerObject);
            }
            return _instance;
        }
    }

    public Coroutine Run(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void Stop(Coroutine coroutine)
    {
        StopCoroutine(coroutine);
    }
}

