using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
public interface IEffectManager
{
    public UniTask PlayEffect(Vector3 position, CancellationToken token);
}