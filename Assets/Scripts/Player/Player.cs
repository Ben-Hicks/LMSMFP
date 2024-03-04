using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Singleton<Player> {


    [Header("Configurable")]
    public float fMaxRotateVelocity = 800f;
    public float fCollisionDepth = 0.04f;
    public float fFriction;
    public float fMoveSpeed;
    public float fMaxMoveVelocity;
    public float fJumpAccel;
    public float fFastFall;
    public List<GameObject> lstGroundColliderCheckers;

    

    //TODO - refactor this to be an external class owned by the player
    public struct CollisionInfo {
        public bool bFloor;
        public bool bLeftWall;
        public bool bRightWall;
        public bool bCeiling;
    }

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public CollisionInfo oldCollision;
    public CollisionInfo curCollision;

    [Header("Properties")]
    public float fTorqueToAdd;
    public float fxForceToAdd;
    public float fyForceToAdd;

    public bool bMovementLocked;
    [HideInInspector]
    public bool bStartedMoving;

    [HideInInspector]
    public ContLaunchShooter contLaunchShooter;
    [HideInInspector]
    public ContSwingShooter contSwingShooter;
    [HideInInspector]
    public ContDashing contDashing;
    [HideInInspector]
    public ContJumping contJumping;
    [HideInInspector]
    public ContFriction contFriction;
    [HideInInspector]
    public ContInput contInput;
    [HideInInspector]
    public ContWallStick contWallStick;
    

    public Subject subPlayerDied;
    public Subject subOnFirstMovementInput;

	// Use this for initialization
	public override void Init() {
        subPlayerDied = new Subject();
        oldCollision = new CollisionInfo();
        curCollision = new CollisionInfo();

        contInput = GetComponent<ContInput>();
        contLaunchShooter = GetComponent<ContLaunchShooter>();
        contSwingShooter = GetComponent<ContSwingShooter>();
        contDashing = GetComponent<ContDashing>();
        contJumping = GetComponent<ContJumping>();
        contFriction = GetComponent<ContFriction>();
        contWallStick = GetComponent<ContWallStick>();
        

        rb = GetComponent<Rigidbody2D>();

        GetComponent<DistanceJoint2D>().connectedBody = rb;
    }

    // Update is called once per frame
    void Update() {
        
        contInput.UpdateInput();

        oldCollision = curCollision;
        curCollision = UpdateCollisions();

        ProcessInput();
    }


    public bool IsColliding(Vector2 v2Direction, string sLayer) {

        bool bTouching = false;

        //For each of our collision detectors, check if there is the tagged layer in the direction off of that detector point
        foreach (GameObject go in lstGroundColliderCheckers) {
            
            int nLayerPlatforms = 1 << LayerMask.NameToLayer(sLayer);
            Debug.DrawRay(go.transform.position, v2Direction * fCollisionDepth, Color.green);
            if (Physics2D.Raycast(go.transform.position, v2Direction, fCollisionDepth, nLayerPlatforms)) {
                //Debug.Log("Found a collision with collider at position " + go.transform.position);
                Debug.DrawRay(go.transform.position, v2Direction * fCollisionDepth, Color.red);
                bTouching = true;
            }
        }

        return bTouching;
    }

    public void PrintCollision(CollisionInfo collisionInfo) {
        Debug.Log("Touching Ceiling: " + collisionInfo.bCeiling);
        Debug.Log("Touching Left Wall: " + collisionInfo.bLeftWall);

        Debug.Log("Touching Right Wall: " + collisionInfo.bRightWall);
        Debug.Log("Touching Floor: " + collisionInfo.bFloor);
    }

    public CollisionInfo UpdateCollisions() {

        CollisionInfo newCollision = new CollisionInfo();

        newCollision.bFloor = IsColliding(Vector2.down, "Floor");
        newCollision.bLeftWall = IsColliding(Vector2.left, "Wall");
        newCollision.bCeiling = IsColliding(Vector2.up, "Ceiling");
        newCollision.bRightWall = IsColliding(Vector2.right, "Wall");

        //PrintCollision(newCollision);

        return newCollision;
    }

    public void LockMovement() {
        bMovementLocked = true;
    }

    public void UnlockMovement() {
        bMovementLocked = false;
    }

    public void OnMovementInput() {

        if (bStartedMoving == false && LevelType.Get() != null) {
            LevelType.Get().OnStartingMovement();
        }
    }

    void ProcessInput() {
        fxForceToAdd = 0f;
        fyForceToAdd = 0f;
        fTorqueToAdd = 0f;


        contWallStick.HandleWallStick();
        contSwingShooter.HandleSwingInput();
        contLaunchShooter.HandleLaunchInput();
        contDashing.HandleDashInput();

        //Horizontal Movement

        if (bMovementLocked == false) {
            if (contInput.bMoveRight) {
                fxForceToAdd += fMoveSpeed;

                OnMovementInput();
            }
            if (contInput.bMoveLeft) {
                fxForceToAdd -= fMoveSpeed;

                OnMovementInput();
            }
        }

        contJumping.HandleJumpInput();
        
        //Modify Gravity Scale if holding Down
        if (bMovementLocked == false) {
            if (contInput.bFastFall && GetComponent<ContDashing>().curDashDirection == ContDashing.DashDirection.NONE) {
                GetComponent<Rigidbody2D>().gravityScale = fFastFall;
            } else if (GetComponent<ContDashing>().curDashDirection == ContDashing.DashDirection.NONE) {
                GetComponent<Rigidbody2D>().gravityScale = 1f;
            }
        }

        contFriction.LimitMaxVelocity();
        contFriction.ApplyFriction();


        //Adding velocity
        rb.AddForce(new Vector2(fxForceToAdd, fyForceToAdd));

        //Debug.Log("Current speed X is " + rb.velocity.x + " and we're adding an x force of " + fxForceToAdd);

        //Adding rotation
        rb.AddTorque(fTorqueToAdd);

    }


    public void OnHazardHit(Hazard harard) {
        Debug.Log("We hit " + harard.gameObject.name);

        //Stop the character from moving
        LockMovement();

        //Change the sprite of the character to the dead sprite
        GetComponent<ContExpressions>().stmachExpressions.stateCur.OnDeath();

        //Chang ethe layer of the player so that they won't be able to do stuff like collecting flies
        this.gameObject.layer =  LayerMask.NameToLayer("PlayerDead");

        subPlayerDied.NotifyObs();
    }
}