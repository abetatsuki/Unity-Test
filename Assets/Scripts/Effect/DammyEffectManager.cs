using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DammyEffectManager : IEffectManager
{
    public async UniTask PlayEffect(Vector3 position, CancellationToken token)
    {
        Debug.Log("Dammy Effect");
    }

}
