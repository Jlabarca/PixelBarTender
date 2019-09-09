using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationUtils {

    public static async Task PushMeAnimation(Transform t, int times = 3)
    {
        for (int i = 0; i < times; i++)
        {
            t.localScale = t.localScale * 1.05f;
            await Task.Delay(50);
        }
        for (int i = 0; i < times; i++)
        {
            t.localScale = t.localScale * 0.95f;
            await Task.Delay(50);
        }
    }

    public static async Task BlinkAnimation(GameObject g, int times = 3)
    {
       for(int i = 0; i < times; i++)
        {
            g.SetActive(false);
            await Task.Delay(100);
            g.SetActive(true);
        }
    }
}
