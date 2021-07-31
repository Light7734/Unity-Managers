using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GameTip
{
    public string content;
    public float duration;
}

class LoadingScreenManager : MonoBehaviour
{
    [Header("Progress")]

    [SerializeField] private Image progressBarImageMask;
    [SerializeField] private TextMeshProUGUI progressBarPercent;


    [Header("Game Tips")]

    [SerializeField] private Image tipTimerBG;
    [SerializeField] private Image tipTimerMask;
    [SerializeField] private Image tipTimerFill;
    [SerializeField] private TextMeshProUGUI tipText;

    [Range(0.1f, 1.1f)]
    [SerializeField] private float tipFadeIn = .2f, tipFadeOut = .4f;

    // #todo: load tips from a file
    // #todo: add level specific tips
    [SerializeField] private List<GameTip> tips = new List<GameTip> { };

    private bool isChangingTip = false;
    private float tipTimer = 0f;
    private float currentTipDuration = 0f;

    private List<AsyncOperation> loadingOperations = new List<AsyncOperation>{};

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
        if (!loadingOperations.IsEmpty())
        {
            float operationsProgress = 0f;
            foreach (var operation in loadingOperations)
                operationsProgress += operation.progress;

            operationsProgress = operationsProgress / loadingOperations.Count;

            progressBarImageMask.fillAmount = operationsProgress;
            progressBarPercent.text = (operationsProgress * 100f).ToString() + '%';
        }

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
        tipTimerMask.fillAmount = 1.0f;

        tipTimerFill.color = Color.black;

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

    public void SetOperations(List<AsyncOperation> operations)
    {
        loadingOperations = operations;
    }

}
