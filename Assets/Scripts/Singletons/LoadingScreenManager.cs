using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct GameTip
{
    public string content;
    public float duration;
}

class LoadingScreenManager : MonoBehaviour
{
    [Header("LoadingProgress")]
    [SerializeField] private Image loadingProgressMask;
    [SerializeField] private TextMeshProUGUI loadingProgressPercentage;

    [Header("Tips")]
    [SerializeField] private Image tipTimerBG;
    [SerializeField] private Image tipTimerMask;
    [SerializeField] private Image tipTimerFill;
    [SerializeField] private TextMeshProUGUI tipText;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float tipFadeIn = .2f, tipFadeOut = .4f;

    [SerializeField] private List<GameTip> tips = new List<GameTip> { };

    bool isChangingTip = false;

    private float testVar = 0f;

    private float tipTimer = 0f;
    private float currentTipDuration = 0f;

    private void Start()
    {
        isChangingTip = true;
        tipTimerMask.fillAmount = 1.0f;

        GameTip tip = GetRandomTip();

        currentTipDuration = tip.duration;
        tipText.text = tip.content;

        LeanTween.value(0f, 1f, tipFadeIn)
            .setOnUpdate((float _value) =>
            {
                tipTimerFill.color = new Color(_value, _value, _value, _value);
                tipText.color = tipText.color.WithAlpha(_value);

            }).setOnComplete(() => { isChangingTip = false; });
    }

    private void Update()
    {
        testVar = loadingProgressMask.fillAmount += Time.deltaTime * 25f / 100.0f;
        loadingProgressPercentage.text = (Mathf.FloorToInt(testVar * 100.0f)).ToString() + '%';

        if (!isChangingTip)
        {
            tipTimer += Time.deltaTime;
            tipTimerMask.fillAmount = ((currentTipDuration - tipTimer) / currentTipDuration);
        }

        if (tipTimer >= currentTipDuration)
        {
            ChangeGameTip();
            tipTimer = 0f;
        }
    }

    private void ChangeGameTip()
    {
        isChangingTip = true;

        tipTimerFill.color = Color.black;
        
        tipTimerMask.fillAmount = 1.0f;

        LeanTween.value(1f, 0f, tipFadeOut)
            .setOnUpdate((float _value) =>
            {
                tipText.color = tipText.color.WithAlpha(_value);
            })
            .setOnComplete(() =>
            {
                GameTip tip = GetRandomTip();

                tipText.text = tip.content;
                currentTipDuration = tip.duration;

                LeanTween.value(0f, 1f, tipFadeIn).
                setOnUpdate((float _value) =>
                {
                    tipTimerFill.color = new Color(_value, _value, _value, _value);
                    tipText.color = tipText.color.WithAlpha(_value);
                    // tipText.color = tipText.color.WithAlpha(_value);
                })
                .setOnComplete(() =>
                {
                    isChangingTip = false;
                });
            });
    }

    private GameTip GetRandomTip()
    {
        // #todo: don't choose previously shown tips
        return tips[Random.Range(0, tips.Count - 1)];
    }

}
