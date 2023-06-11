using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Image foreground;
    Vector3 goal;

    /// <summary>
    /// Set the HP bar to @scale %. Currently assuming scale to only decrease
    /// </summary>
    /// <param name="scale"></param>
    public void SetHP(float scale)
    {
        Vector3 newScale;
        if (scale < goal.x)
        {
            newScale = foreground.rectTransform.localScale;
            newScale.x = scale;
            foreground.rectTransform.localScale = newScale;
        }
        else
        {

            newScale = background.rectTransform.localScale;
            newScale.x = scale;
            background.rectTransform.localScale = newScale;
        }

        goal = newScale;
    }

    private void Start()
    {
        goal = background.rectTransform.localScale;
    }

    private void Update()
    {
        background.rectTransform.localScale = Vector3.Lerp(background.rectTransform.localScale, goal, 0.05f);
        foreground.rectTransform.localScale = Vector3.Lerp(foreground.rectTransform.localScale, goal, 0.05f);
    }
}
