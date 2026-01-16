using UnityEngine;

public class FinalLevelManager : MonoBehaviour
{
    [SerializeField] private LayerMask monsterMask;
    [SerializeField] private WinOverlay winOverlay;

    void FixedUpdate()
    {
        if (Util.CountInLayerMask(monsterMask) == 0)
        {
            winOverlay.Display();
            Destroy(this);
        }
    }
}
