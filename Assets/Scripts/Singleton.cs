using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;
    
    public static T Get()
    {
        if (_instance == null)
            CreateInstance();
        
        return _instance;
    }

    private static void CreateInstance()
    {
        System.Type singletonType = typeof(T);
        GameObject singletonObject = new GameObject(singletonType.ToString(), singletonType);
        _instance = singletonObject.GetComponent<T>();
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