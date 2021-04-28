using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


[RequireComponent(typeof(Image))]
public class Button : MonoBehaviour,
    IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler
{
    [System.Serializable]
    private struct ButtonSprites
    {
        public Sprite normal, hovered, pressed, disabled;
    }

    private enum ButtonState
    {
        None = 0,
        Normal, Hovered, Pressed,
    }

    [SerializeField]
    private  AudioEmitterUI emitter;

    [SerializeField]
    private Image image;

    [SerializeField]
    private UnityEvent onClick, onPress, onHover, onUnhover;

    [SerializeField]
    private ButtonSprites sprites;

    private ButtonState state;

    bool isDisabled;
    
    private void Start()
    {
        image.alphaHitTestMinimumThreshold = 0.5f;
    }

    public void SetDisable(bool? disable, bool toggle = false)
    {
        isDisabled = toggle ? !isDisabled : (bool)disable;
        image.sprite = sprites.disabled;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDisabled)
            return;

        onClick.Invoke();

        emitter["click"].Start();
        SetState(ButtonState.Hovered);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isDisabled)
            return;

        onPress.Invoke();

        emitter["press"].Start();
        SetState(ButtonState.Pressed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state == ButtonState.Pressed || isDisabled)
            return;

        onHover.Invoke();

        emitter["hover"].Start();
        SetState(ButtonState.Hovered);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (state == ButtonState.Pressed || isDisabled)
            return;

        onUnhover.Invoke();

        emitter["unhover"].Start();
        SetState(ButtonState.Normal);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SetState(ButtonState.Normal);
    }

    private void SetState(ButtonState newState)
    {
        state = newState;

        if (isDisabled)
            return;

        if (state == ButtonState.Normal)
            image.sprite = sprites.normal;
        else if(state == ButtonState.Hovered)
            image.sprite = sprites.hovered;
        else if (state == ButtonState.Pressed)
            image.sprite = sprites.pressed;
    }
}