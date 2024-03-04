using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swatter : MonoBehaviour {

    
    public float fTimeBeforeRepositioning;
    public float fTimePositioning;
    public float fSwattingSpeed;


    public float fSpawnPositionY;
    public float fFinishPositionY;

    public Rigidbody2D rbSwatter;
    public GameObject goTarget;
    public GameObject goFinishMarker;

    public enum StSwatting { UNSTARTED, POSITIONING, SWATTING, FINISHING };
    public StSwatting curStSwatting;


    void Start() {
        goTarget = Player.Get().gameObject;

        fSpawnPositionY = rbSwatter.position.y;
        fFinishPositionY = goFinishMarker.transform.position.y;


        LevelType.Get().subStartActive.Subscribe(OnLevelActive);
    }


    public void PositionAboveTarget() {

        rbSwatter.MovePosition(new Vector3(goTarget.transform.position.x, fSpawnPositionY, 0));

    }

    public void AdvanceState() {

        switch (curStSwatting) {
            case StSwatting.UNSTARTED:
            case StSwatting.FINISHING:
                curStSwatting = StSwatting.POSITIONING;

                //Advance our state after this delay
                Invoke("AdvanceState", fTimePositioning);
                break;

            case StSwatting.POSITIONING:
                curStSwatting = StSwatting.SWATTING;

                //Make the swatter move downward
                rbSwatter.velocity = Vector2.down * fSwattingSpeed;
                break;

            case StSwatting.SWATTING:
                curStSwatting = StSwatting.FINISHING;

                //Cancel the swatter's velocity
                rbSwatter.velocity = Vector2.zero;

                //Advance our state after this delay
                Invoke("AdvanceState", fTimeBeforeRepositioning);
                break;
        }

    }

    public void FixedUpdate() {

        switch (curStSwatting) {
            case StSwatting.POSITIONING:
                PositionAboveTarget();
                break;

            case StSwatting.SWATTING:
                if (CheckIfFinishedSwatting()) {
                    //Once we've reached the bottom of the stage, then immediately switch our state;
                    AdvanceState();
                }
                break;
        }

    }

    public bool CheckIfFinishedSwatting() {
        return rbSwatter.position.y <= fFinishPositionY;
    }

    public void OnLevelActive(Object target, params object[] args) {
        Invoke("AdvanceState", fTimeBeforeRepositioning);
    }
    
}
