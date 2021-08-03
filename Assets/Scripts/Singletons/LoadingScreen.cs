using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class LoadingScreen : MonoBehaviour
{
    [System.Serializable]
    public struct Tip
    {
        public string content;
        public float duration;
    }

    [Header("Progress")]

    [SerializeField] private Image progressBarImageMask;
    [SerializeField] private TextMeshProUGUI progressBarPercent;

    [Header("Game Tips")]

    [SerializeField] private Image tipTimerBG;
    [SerializeField] private Image tipTimerMask;
    [SerializeField] private Image tipTimerFill;
    [SerializeField] private TextMeshProUGUI tipText;

    [Range(0.1f, 1.1f)]
    [SerializeField] private float tipFadeIn, tipFadeOut;

    // #todo: add level/progress related tips
    [SerializeField] private List<Tip> tips = new List<Tip> { };

    private bool isChangingTip = true;
    private float currentTipDuration = .1f;
    private float tipTimer;

    private List<AsyncOperation> loadingOperations = new List<AsyncOperation>{};

    private void Start()
    {
        tipText.text = "";
        ChangeGameTip();
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
                Tip tip = GetRandomTip();

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

    private Tip GetRandomTip()
    {
        // #todo: don't choose previously shown tips
        return tips[Random.Range(0, tips.Count - 1)];
    }

    public void SetOperations(List<AsyncOperation> operations)
    {
        loadingOperations = operations;
    }

}
