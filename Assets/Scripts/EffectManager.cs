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
    public virtual void PlayEffect(Vector3 position)
    {
        _effect.Stop();
        _effect.transform.position = position;
        _effect.Play();
    }
}
