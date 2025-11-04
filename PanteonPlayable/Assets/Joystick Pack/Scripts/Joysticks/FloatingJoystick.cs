using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isInputEnable) return;

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        CloseBackground();
        base.OnPointerUp(eventData);
    }

    protected override void OnDeactivateInput()
    {
        base.OnDeactivateInput();
        CloseBackground();
    }

    private void CloseBackground()
    {
        background.gameObject.SetActive(false);
    }
}