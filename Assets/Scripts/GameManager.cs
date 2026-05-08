
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Init()
    {

    }

    protected override void OnRelease()
    {

    }


    private async Task Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (!ServiceLocator<IEffectManager>.IsValid())
                {
                    ServiceLocator<IEffectManager>.Bind(new DammyEffectManager());
                }

                CancellationToken token = this.GetCancellationTokenOnDestroy();
                await UniTask.Delay(TimeSpan.FromSeconds(3f), cancellationToken: token);
                await ServiceLocator<IEffectManager>.Instance.PlayEffect(hit.point, token);
            }
        }
    }
}
