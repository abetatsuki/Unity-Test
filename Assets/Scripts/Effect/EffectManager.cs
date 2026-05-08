using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EffectManager : MonoBehaviour, IEffectManager
{
    [SerializeField]
    private ParticleSystem _effect = null;


    private void OnEnable()
    {
        ServiceLocator<IEffectManager>.Bind(this);
    }

    private void OnDisable()
    {
        ServiceLocator<IEffectManager>.UnBind(this);
    }
    public async virtual UniTask PlayEffect(Vector3 position, CancellationToken token)
    {
        _effect.Stop();
        _effect.transform.position = position;
        await UniTask.Delay(3000);
        _effect.Play();
    }
}
