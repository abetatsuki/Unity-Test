using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Init()
    {
        Debug.Log("Hello Singleton");
    }
}
