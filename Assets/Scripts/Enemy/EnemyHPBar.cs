using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Image foreground;

    /// <summary>
    /// Set the HP bar to @scale %. Currently assuming scale to only decrease
    /// </summary>
    /// <param name="scale"></param>
    public void SetHP(float scale)
    {
        // Stop previous background animation, which is the only coroutine on the monobehavior
        StopAllCoroutines();

        Vector3 newScale = foreground.rectTransform.localScale;
        newScale.x = scale;
        foreground.rectTransform.localScale = newScale;

        StartCoroutine(AnimateBackground(newScale));
    }

    private IEnumerator AnimateBackground(Vector3 goal)
    {
        Vector3 startScale = background.rectTransform.localScale;
        while ((goal - startScale).magnitude > 0.01f)
        {
            startScale = background.rectTransform.localScale;

            background.rectTransform.localScale = Vector3.Lerp(startScale, goal, 0.01f);
            yield return null;
        }

        background.rectTransform.localScale = goal;
    }
}
