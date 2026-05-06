using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FakeEffectManager : EffectManager
{
    public async override UniTask PlayEffect(Vector3 position, CancellationToken token)
    {
        Debug.Log(position.ToString());
    }
}