using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerUIPopUpManager : MonoBehaviour
{
    [Header("Message Pop Up")]
    [SerializeField] GameObject _messagePopUp;
    [SerializeField] TextMeshProUGUI _messagePopUpText;

    [Header("You Died Pop Up")]
    [SerializeField] GameObject _youDiedPopUpGameObject;
    [SerializeField] TextMeshProUGUI _youDiedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI _youDiedPopUpText;
    [SerializeField] CanvasGroup _youDiedPopUpCanvasGroup;

    [Header("Boss Defeated Pop Up")]
    [SerializeField] GameObject _bossDefeatedPopUpGameObject;
    [SerializeField] TextMeshProUGUI _bossDefeatedPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI _bossDefeatedPopUpText;
    [SerializeField] CanvasGroup _bossDefeatedPopUpCanvasGroup;

    [Header("Rest Spot Activated Pop Up")]
    [SerializeField] GameObject _RestSpotPopUpGameObject;
    [SerializeField] TextMeshProUGUI _RestSpotPopUpBackgroundText;
    [SerializeField] TextMeshProUGUI _RestSpotdPopUpText;
    [SerializeField] CanvasGroup _RestSpotPopUpCanvasGroup;

    public void CloseAllPopUpWindows() {

        _messagePopUp.SetActive(false);

        PlayerUIManager.Instance.PopUpWindowIsOpen = false;
    }

    public void SendMessagePopUp(string messageText) {
        PlayerUIManager.Instance.PopUpWindowIsOpen = true;
        _messagePopUpText.text = ": " + messageText;
        _messagePopUp.SetActive(true);
    }

    public void SendYouDiedPopUp() {

        // TODO: Add Some Post Process

        _youDiedPopUpGameObject.SetActive(true);
        _youDiedPopUpBackgroundText.characterSpacing = 0;

        StartCoroutine(StretchPopUpTextOverTime(_youDiedPopUpBackgroundText, 10, 10));

        StartCoroutine(FadeInPopUpOverTime(_youDiedPopUpCanvasGroup, 5));

        StartCoroutine(WaitThenFadeOutPopUpOverTime(_youDiedPopUpCanvasGroup, 2, 5));
    }

    public void SendBossDefeatedPopUp(string bossDefeatedMessage) {

        // TODO: Add Some Post Process

        _bossDefeatedPopUpText.text = bossDefeatedMessage;
        _bossDefeatedPopUpBackgroundText.text = bossDefeatedMessage;

        _bossDefeatedPopUpGameObject.SetActive(true);
        _bossDefeatedPopUpBackgroundText.characterSpacing = 0;

        StartCoroutine(StretchPopUpTextOverTime(_bossDefeatedPopUpBackgroundText, 10, 10));

        StartCoroutine(FadeInPopUpOverTime(_bossDefeatedPopUpCanvasGroup, 5));

        StartCoroutine(WaitThenFadeOutPopUpOverTime(_bossDefeatedPopUpCanvasGroup, 2, 5));
    }

    public void SendRestSpotActivatedPopUp() {

        // TODO: Add Some Post Process

        _RestSpotPopUpGameObject.SetActive(true);
        _RestSpotPopUpBackgroundText.characterSpacing = 0;

        StartCoroutine(StretchPopUpTextOverTime(_RestSpotPopUpBackgroundText, 10, 10));

        StartCoroutine(FadeInPopUpOverTime(_RestSpotPopUpCanvasGroup, 3));

        StartCoroutine(WaitThenFadeOutPopUpOverTime(_RestSpotPopUpCanvasGroup, 2, 3));
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
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1, duration * Time.deltaTime);
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
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0, duration * Time.deltaTime);
                yield return null;
            }
        }

        canvas.alpha = 0f;

        yield return null;
    }
}
