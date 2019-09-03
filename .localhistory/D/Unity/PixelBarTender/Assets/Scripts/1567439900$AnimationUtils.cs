using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationUtils {

    private async Task AnimateDialogue(Transform t)
    {
        t.localScale = t.localScale * 1.05f;
        await Task.Delay(50);
        t.localScale = t.localScale * 1.05f;
        await Task.Delay(50);
        t.localScale = t.localScale * 1.05f;
        await Task.Delay(50);
        t.localScale = t.localScale * 0.95f;
        await Task.Delay(50);
        t.localScale = t.localScale * 0.95f;
        await Task.Delay(50);
        t.localScale = t.localScale * 0.95f;
    }
}
