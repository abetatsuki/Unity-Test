using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }
    public static bool IsValid() => Instance != null;

    [System.Obsolete("Use Instance instead.")]
    public static T Instace => Instance;

    protected virtual bool DestroyTargetGameObject => false;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
            Instance.Init();
            return;
        }

        if (ReferenceEquals(Instance, this))
        {
            return;
        }

        if (DestroyTargetGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if (ReferenceEquals(Instance, this))
        {
            Instance = null;
        }

        OnRelease();
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnRelease()
    {
        Debug.Log("Release");
    }
}
