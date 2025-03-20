using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("You Died Pop Up")]
    [SerializeField] GameObject _youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI _youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI _youDiedPopUpText;
    [SerializeField] CanvasGroup _youDiedPopUpCanvasGroup;

    public void SendYouDiedPopUp() {

        // TODO: Add Some Post Process

        _youDiedPopUpGameObject.SetActive(true);
        _youDiedPopUpBackgroundText.characterSpacing = 0;

        StartCoroutine(StretchPopUpTextOverTime(_youDiedPopUpBackgroundText, 10, 10));

        StartCoroutine(FadeInPopUpOverTime(_youDiedPopUpCanvasGroup, 5));

        StartCoroutine(WaitThenFadeOutPopUpOverTime(_youDiedPopUpCanvasGroup, 2, 5));
    }

    IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount) {
        if (duration > 0f) {
            text.characterSpacing = 0;

            float timer = 0f;

            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                yield return null;
            }
        }

    }

    IEnumerator FadeInPopUpOverTime(CanvasGroup canvas, float duration) {
        if(duration > 0) {
            canvas.alpha = 0f;
            float timer = 0f;

            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * (Time.deltaTime / 10));
                yield return null;
            }
        }

        canvas.alpha = 1f;

        yield return null;
    }

    IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration, float delay) {

        if (duration > 0) {

            while (delay > 0f) {
                delay -= Time.deltaTime;
                yield return null;
            }

            canvas.alpha = 1f;
            float timer = 0f;

            yield return null;

            while (timer < duration) {
                timer += Time.deltaTime;
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * (Time.deltaTime / 5));
                yield return null;
            }
        }

        canvas.alpha = 0f;

        yield return null;
    }
}
