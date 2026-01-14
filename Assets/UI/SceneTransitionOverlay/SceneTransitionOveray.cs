using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SceneTransitionOveray : MonoBehaviour
{
    [SerializeField] private float effectDuration = 1f;

    private UIDocument UI;

    private VisualElement main;

    private float Alpha
    {
        get => main.style.backgroundColor.value.a;
        set => main.style.backgroundColor = new Color(main.style.backgroundColor.value.r, main.style.backgroundColor.value.g, main.style.backgroundColor.value.b, value);
    }

    void Awake()
    {
        UI = GetComponent<UIDocument>();
        main = UI.rootVisualElement.Query<VisualElement>();
    }

    public IEnumerator FadeIn()
    {
        return ChangeAlpha(0);
    }

    public IEnumerator FadeOut()
    {
        return ChangeAlpha(1);
    }

    private IEnumerator ChangeAlpha(float target)
    {
        float duration = 0;
        float start = Alpha;

        while (duration < effectDuration)
        {
            duration += Time.deltaTime;

            Alpha = Mathf.Lerp(start, target, duration / effectDuration);

            yield return null;
        }

        Alpha = target;
    }
}
