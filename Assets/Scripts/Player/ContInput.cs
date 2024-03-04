using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContInput : MonoBehaviour {

    [Header("Configurable")]


    [Header("Properties")]
    public bool bMoveLeft;
    public bool bMoveRight;
    public bool bMoveUp;
    public bool bMoveDown;
    public bool bFastFall;

    public bool bDashLeft;
    public bool bDashRight;

    public bool bJump;

    bool bShootSwingingWebLastFrame;
    public bool bShootSwingingWebHeld;
    public bool bShootSwingingWeb;

    bool bShootLaunchingWebLastFrame;
    public bool bShootLaunchingWebHeld;
    public bool bShootLaunchingWeb;

    public bool bRetract;
    public bool bExtend;

    public bool bRestartScene;

    public Vector2 v2MousePos;
    public Vector2 v2WebReticle;

    public void UpdateInput() {
        float fHoriz = Input.GetAxisRaw("Horizontal");
        float fVert = Input.GetAxisRaw("Vertical");

        bMoveLeft = fHoriz < 0;
        bMoveRight = fHoriz > 0;
        bMoveUp = fVert > 0;
        bMoveDown = fVert < 0;
        bFastFall = Input.GetAxisRaw("Fast Fall") == 1;

        bRetract = Input.GetAxisRaw("Extend/Retract") > 0;
        bExtend = Input.GetAxisRaw("Extend/Retract") < 0;

        bDashLeft = Input.GetAxisRaw("Dash Left") == 1;
        bDashRight = Input.GetAxisRaw("Dash Right") == 1;

        bJump = Input.GetAxis("Jump") == 1;

        bShootSwingingWebLastFrame = bShootSwingingWebHeld;
        bShootSwingingWebHeld = Input.GetAxisRaw("Shoot Swinging Web") > 0;
        bShootSwingingWeb = bShootSwingingWebHeld && (bShootSwingingWebLastFrame == false);

        bShootLaunchingWebLastFrame = bShootLaunchingWebHeld;
        bShootLaunchingWebHeld = Input.GetAxisRaw("Shoot Launching Web") > 0;
        bShootLaunchingWeb = bShootLaunchingWebHeld && (bShootLaunchingWebLastFrame == false);

        Vector3 v3RawMousePosition = Input.mousePosition;
        v3RawMousePosition.z = Mathf.Abs(Camera.main.transform.position.z);

        v2MousePos  = Camera.main.ScreenToWorldPoint(v3RawMousePosition);
        v2WebReticle = v2MousePos;

    }


}
