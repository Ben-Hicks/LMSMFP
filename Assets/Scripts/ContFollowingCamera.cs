using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContFollowingCamera : ContCamera {

    public GameObject goFocus;

    //Minimum Distance from the edge of the screen before the camera starts scrolling in that direction
    //Note: Distances are percentages
    public float fLeftDist;
    public float fRightDist;
    public float fTopDist;
    public float fBotDist;

    public float fMaxHorzEdge;
    public float fMinHorzEdge;

    public float fMaxVertEdge;
    public float fMinVertEdge;

    public float fMaxFocusSpeedX;
    public float fMaxFocusSpeedY;
    public float fMinFocusDistance; //Distance under which we stop moving the camera to further center the target

    public float fScrollSpeed;


    public Vector2 v2BotLeftBoundary {
        get {
            Vector3 v3RawBoundaryPosition = new Vector3(fLeftDist * Screen.width, fBotDist * Screen.height, Mathf.Abs(transform.position.z));

            return cam.ScreenToWorldPoint(v3RawBoundaryPosition);
        }
    }

    public Vector2 v2TopRightBoundary {
        get {
            Vector3 v3RawBoundaryPosition = new Vector3(Screen.width * (1 - fRightDist), Screen.height * (1 - fTopDist), Mathf.Abs(transform.position.z));

            return cam.ScreenToWorldPoint(v3RawBoundaryPosition);
        }
    }

    public Camera cam;



    void AdjustEdges() {

        Rigidbody2D rbFocus = goFocus.GetComponent<Rigidbody2D>();
        if(rbFocus == null) {
            //Then we don't expect the focused object to be moving, so just set have no edges
            fLeftDist = fRightDist = fTopDist = fBotDist = 0.5f;
        } else {

            //NOTE - If we end up not liking the snap-back thing that happens after a second of not moving, then address the case where
            // the velocity is really low but the edges are trying to shift slightly due to this low velocity - instead just ignore any velocity under a certain delta

            //Otherwise we'll have to adjust the bounds by looking at our focus's velocity
            //Get a percentage of the range (-fMaxFocusSpeed, fMaxFocusSpeed)
            float fMovementPercentageX = Mathf.Clamp01((rbFocus.velocity.x + fMaxFocusSpeedX)/ (2 * fMaxFocusSpeedX));
            float fMovementPercentageY = Mathf.Clamp01((rbFocus.velocity.y + fMaxFocusSpeedY) / (2 * fMaxFocusSpeedY));

            float fLeftDistDesired = Mathf.Lerp(fMaxHorzEdge, fMinHorzEdge, fMovementPercentageX);
            float fRightDistDesired = Mathf.Lerp(fMinHorzEdge, fMaxHorzEdge, fMovementPercentageX);

            float fBotDistDesired = Mathf.Lerp(fMaxVertEdge, fMinVertEdge, fMovementPercentageY);
            float fTopDistDesired = Mathf.Lerp(fMinVertEdge, fMaxVertEdge, fMovementPercentageY);

            fLeftDist = Mathf.Lerp(fLeftDist, fLeftDistDesired, fScrollSpeed * Time.deltaTime);
            fRightDist = Mathf.Lerp(fRightDist, fRightDistDesired, fScrollSpeed * Time.deltaTime);

            fBotDist = Mathf.Lerp(fBotDist, fBotDistDesired, fScrollSpeed * Time.deltaTime);
            fTopDist = Mathf.Lerp(fTopDist, fTopDistDesired, fScrollSpeed * Time.deltaTime);

        }

        

    }


    void MaintainFocus() {

        float fDesiredCameraX = 0f;
        float fDesiredCameraY = 0f;

        if(goFocus.transform.position.x < v2BotLeftBoundary.x) {
            //Debug.Log("Too far left");
            fDesiredCameraX = goFocus.transform.position.x - v2BotLeftBoundary.x;
        }
        if (goFocus.transform.position.x > v2TopRightBoundary.x) {
            //Debug.Log("Too far right");
            fDesiredCameraX = goFocus.transform.position.x - v2TopRightBoundary.x;
        }
        if (goFocus.transform.position.y < v2BotLeftBoundary.y) {
            //Debug.Log("Too far down");
            fDesiredCameraY = goFocus.transform.position.y - v2BotLeftBoundary.y;
        }
        if (goFocus.transform.position.y > v2TopRightBoundary.y) {
            //Debug.Log("Too far up");
            fDesiredCameraY = goFocus.transform.position.y - v2TopRightBoundary.y;
        }
        
        Vector3 v3DesiredCamera = new Vector3(transform.position.x + fDesiredCameraX, transform.position.y + fDesiredCameraY, transform.position.z);

        if (Vector3.Distance(v3DesiredCamera, transform.position) < fMinFocusDistance) return;

        transform.position = v3DesiredCamera;

    }

    void DrawBoundary() { 
        Debug.DrawLine(new Vector3(v2BotLeftBoundary.x, v2BotLeftBoundary.y), new Vector3(v2BotLeftBoundary.x, v2TopRightBoundary.y), Color.red);
        Debug.DrawLine(new Vector3(v2BotLeftBoundary.x, v2BotLeftBoundary.y), new Vector3(v2TopRightBoundary.x, v2BotLeftBoundary.y), Color.red);
        Debug.DrawLine(new Vector3(v2BotLeftBoundary.x, v2TopRightBoundary.y), new Vector3(v2TopRightBoundary.x, v2TopRightBoundary.y), Color.red);
        Debug.DrawLine(new Vector3(v2TopRightBoundary.x, v2TopRightBoundary.y), new Vector3(v2TopRightBoundary.x, v2BotLeftBoundary.y), Color.red);
    }

    private void LateUpdate() {
        AdjustEdges();
        DrawBoundary();
        MaintainFocus();
    }

}
