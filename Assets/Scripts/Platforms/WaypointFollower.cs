using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour {

    public enum PATHTYPE { REVERSE, LOOP };
    public PATHTYPE pathtype;
    private bool bReverse;

    public Waypoint[] arWaypoint;
    public int iCurWaypoint;

    public GameObject goFollower;
    public NonPhysicsMoveable nonphysicsmoveable;
    public Rigidbody2D rbFollower;

    public bool bMoving;

    private float fTimeToReach;
    private float fCurTime;
    private Vector3 v3StartPos;

    public void Start() {
        if(arWaypoint.Length <= 1) {
            Debug.LogError("ERROR! - Can't have 1 or fewer waypoints");
        }
        if(arWaypoint[iCurWaypoint].transform.localPosition != goFollower.transform.localPosition) {
            Debug.LogError("ERROR! - Starting Waypoint should be at the same location as the Platform");
        }

        nonphysicsmoveable = goFollower.GetComponent<NonPhysicsMoveable>();
        rbFollower = goFollower.GetComponent<Rigidbody2D>();
    }

    public void SetNextWaypoint(){

        v3StartPos = goFollower.transform.position;
        
        switch (pathtype) {
            case PATHTYPE.LOOP:
                if(iCurWaypoint == arWaypoint.Length - 1) {
                    iCurWaypoint = 0;
                } else {
                    iCurWaypoint++;
                }
                break;
            case PATHTYPE.REVERSE:
                if (bReverse) {
                    if(iCurWaypoint == 0) {
                        bReverse = false;
                        iCurWaypoint = 1;
                    } else {
                        iCurWaypoint--;
                    }
                } else {
                    if(iCurWaypoint == arWaypoint.Length - 1) {
                        bReverse = true;
                        iCurWaypoint = arWaypoint.Length - 2;
                    } else {
                        iCurWaypoint++;
                    }
                }
                break;
        }

        if (bReverse) {
            fTimeToReach = arWaypoint[iCurWaypoint].fTimeToReachReverse;
        } else {
            fTimeToReach = arWaypoint[iCurWaypoint].fTimeToReach;
        }
    }

    public Vector2 GetFollowingVelocity() {
        return (arWaypoint[iCurWaypoint].transform.position - v3StartPos) / fTimeToReach;
    }


    public void FixedUpdate() {

        if (goFollower == null) return;

        fCurTime += Time.deltaTime;

        if (bMoving) {


            /* A force application version
              if (fCurTime >= fTimeToReach) {
                //Once we've reached the next destination waypoint, then we are no longer moving and should wait

                bMoving = false;
                fCurTime = 0f;

                Debug.Log("Reached location");
                
            }else if (fCurTime < 0.10f * fTimeToReach) {
                //Begin to accelerate
                Debug.Log("Accelerating");
                rbPlatform.AddForce(5 * Vector2.up * rbPlatform.mass);

            } else if (fCurTime > 0.9f * fTimeToReach) {
                //Begin to decellerate
                Debug.Log("Decellerating");
                rbPlatform.AddForce(5 * Vector2.down * rbPlatform.mass);

            } else {
                //Just keep moving at the same pace
                Debug.Log("Maintaining");
            }
            */

            /*Velocity setting approach
             * 
             * if (fCurTime >= fTimeToReach) {
                //Once we've reached the next destination waypoint, then we are no longer moving and should wait

                bMoving = false;
                fCurTime = 0f;

                PullAttachedWebs();

                rbPlatform.velocity = Vector2.zero;
                

            } else {

                rbPlatform.velocity = (arWaypoint[iCurWaypoint].transform.position - v3StartPos) / fTimeToReach;

                
            }*/
            if (fCurTime >= fTimeToReach) {

                bMoving = false;
                fCurTime = 0f;

                //If we're a platform (maybe generalize to webbable surface?), then we need to impart
                //  force to anything attached by webs since we're stopping moving
                PlatformMoveable platformMoveable = goFollower.GetComponent<PlatformMoveable>();
                if(platformMoveable != null) {
                    platformMoveable.PullAttachedWebs();
                }

                rbFollower.velocity = Vector2.zero;
                nonphysicsmoveable.SetMovingForce(Vector3.zero);
            } else {


            }



        } else {
            //Don't need to move at all

            //rbPlatform.velocity = Vector2.zero;

            if (fCurTime >= arWaypoint[iCurWaypoint].fTimeStay) {
                //If we've stayed long enough, then start moving to the next waypoint
                bMoving = true;
                fCurTime = 0f;
                SetNextWaypoint();

                nonphysicsmoveable.SetMovingForce((arWaypoint[iCurWaypoint].transform.position - v3StartPos) / fTimeToReach);
                rbFollower.AddForce(rbFollower.mass * nonphysicsmoveable.v3MovingForce, ForceMode2D.Impulse);
            }
        }
    }

    

}
