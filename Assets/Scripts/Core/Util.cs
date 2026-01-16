using System;
using System.Collections;
using UnityEngine;

public static class Util
{
    public static IEnumerator Timeout(Action callback, float duration)
    {
        yield return new WaitForSeconds(duration);
        callback();
    }

    public static IEnumerator Periodic(Action callback, float duration)
    {
        var wait = new WaitForSeconds(duration);
        while (true)
        {
            callback();
            yield return wait;
        }
    }

    public static int CountInLayerMask(LayerMask mask)
    {
        int count = 0;
        foreach (var t in UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
        {
            if (((1 << t.gameObject.layer) & mask.value) != 0) count++;
        }
        return count;
    }
}
