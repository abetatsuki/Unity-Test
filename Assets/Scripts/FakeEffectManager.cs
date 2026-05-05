using UnityEngine;

public class FakeEffectManager : EffectManager
{
    public override void PlayEffect(Vector3 position)
    {
        Debug.Log(position.ToString());
        //  base.PlayEffect(position);
    }
}