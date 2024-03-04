using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour {

    public RectTransform rectPanelTop;
    public RectTransform rectPanelLeft;
    public RectTransform rectPanelRight;
    public RectTransform rectPanelBottom;

    public float fTransitionTime;

    float fCurTransitionTime;

    public enum TransitionState { STILL, OPENING, CLOSING };
    TransitionState curTransitionState;


    public void OpenTransition() {
        curTransitionState = TransitionState.OPENING;
        fCurTransitionTime = 0f;
    }

    public void CloseTransition() {
        curTransitionState = TransitionState.CLOSING;
        fCurTransitionTime = 0f;
    }

    public float GetMaxWidth() {
        return (Screen.width / 2 + 2) / this.transform.lossyScale.x;
    }

    public float GetMaxHeight() {
        return (Screen.height / 2 + 2) / this.transform.lossyScale.y;
    }

    public void HandleTransition() {
        fCurTransitionTime += Time.deltaTime;
        float fProgress = fCurTransitionTime / fTransitionTime;

        switch (curTransitionState) {
            case TransitionState.STILL:
                return;
            case TransitionState.CLOSING:
                rectPanelTop.anchoredPosition = new Vector3(0, Mathf.Lerp(0, -GetMaxHeight(), fProgress));
                rectPanelLeft.anchoredPosition = new Vector3(Mathf.Lerp(0, GetMaxWidth(), fProgress), 0);
                rectPanelRight.anchoredPosition = new Vector3(Mathf.Lerp(0, -GetMaxWidth(), fProgress), 0);
                rectPanelBottom.anchoredPosition = new Vector3(0, Mathf.Lerp(0, GetMaxHeight(), fProgress));
                break;
            case TransitionState.OPENING:
                rectPanelTop.anchoredPosition = new Vector3(0, Mathf.Lerp(-GetMaxHeight(), 0, fProgress));
                rectPanelLeft.anchoredPosition = new Vector3(Mathf.Lerp(GetMaxWidth(), 0, fProgress), 0);
                rectPanelRight.anchoredPosition = new Vector3(Mathf.Lerp(-GetMaxWidth(), 0, fProgress), 0);
                rectPanelBottom.anchoredPosition = new Vector3(0, Mathf.Lerp(GetMaxHeight(), 0, fProgress));
                break;
        }

        //If we've reached 100% of the way through our animation, then we can stop
        if(fProgress >= 1f) {
            curTransitionState = TransitionState.STILL;
        }
    }

    void Update() {
        HandleTransition();
    }
}
