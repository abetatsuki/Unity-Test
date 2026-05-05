
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Init()
    {

    }

    private void Update()
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

                ServiceLocator<IEffectManager>.Instance.PlayEffect(hit.point);
            }
        }
    }
}
