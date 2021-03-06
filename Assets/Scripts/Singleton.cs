using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;
    
    public static T Get()
    {
        if (_instance == null)
            Debug.LogWarning($"{ typeof(T).Name } has not been loaded - make sure ControllerScene is working.");
        
        return _instance;
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T) this;
        }
    }
}