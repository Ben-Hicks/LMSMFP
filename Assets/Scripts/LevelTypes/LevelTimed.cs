using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimed : LevelType{

    [Header("Configurable")]
    public float fTimeLimit;
    public int nStartingFlies;
    public float fTimeBetweenNormalSpawns;

    public GameObject goFlyToSpawn;

    public float fMinSpawnX;
    public float fMinSpawnY;
    public float fMaxSpawnX;
    public float fMaxSpawnY;

    public float fDistSpawnPlatforms;
    public float fDistSpawnOtherFlies;
    public float fDistSpawnPlayer;

    public int[] arnStarThresholds;

    [Header("Properties")]
    public float fTimeSinceLastNormalSpawn;
    public int nLiveFlies;

    public float fTimeActuallyUsed;

    public override void Init() {
        base.Init();
        nLiveFlies = 0;

        for (int i=0; i<nStartingFlies; i++) {
            SpawnFly();
        }
    }


    public override void Update() {
        base.Update();

        fTimeSinceLastNormalSpawn += Time.deltaTime;
        if(fTimeSinceLastNormalSpawn >= fTimeBetweenNormalSpawns) {
            fTimeSinceLastNormalSpawn = 0f;
            SpawnFly();
        }

        
    }

    public override bool CheckLevelFinished() {
        return GetTimeRemaining() == 0f;
    }


    Vector2 DetermineSpawnCoords() {

        

        int nPlatformLayer = 1 << LayerMask.NameToLayer("Platforms");
        int nPlayerLayer = 1 << LayerMask.NameToLayer("Player");
        int nCollectablesLayer = 1 << LayerMask.NameToLayer("Collectable");

        for(int i=0; i<50; i++) { 
            Vector2 v2PotentialSpawn = new Vector2(Random.Range(fMinSpawnX, fMaxSpawnX), Random.Range(fMinSpawnY, fMaxSpawnY));

            if (Physics2D.OverlapCircle(v2PotentialSpawn, fDistSpawnPlatforms, nPlatformLayer)) {
                //Debug.Log(v2PotentialSpawn + " was too close to a platform");
                Debug.DrawLine(v2PotentialSpawn, v2PotentialSpawn + Vector2.left, Color.red, 10);
                continue;
            }else if (Physics2D.OverlapCircle(v2PotentialSpawn, fDistSpawnPlayer, nPlayerLayer)) {
                //Debug.Log(v2PotentialSpawn + " was too close to the player");
                Debug.DrawLine(v2PotentialSpawn, v2PotentialSpawn + Vector2.left, Color.black, 10);
                continue;
            } else if (Physics2D.OverlapCircle(v2PotentialSpawn, fDistSpawnOtherFlies, nCollectablesLayer)) {
                //Debug.Log(v2PotentialSpawn + " was too close to another fly");
                Debug.DrawLine(v2PotentialSpawn, v2PotentialSpawn + Vector2.left, Color.blue, 10);
                continue;
            } else {
                return v2PotentialSpawn;
            }
        }

        Debug.LogError("Could not find a suitable location to spawn a new fly");
        return Vector2.zero;

    }

    public float GetTimeRemaining() {
        return Mathf.Max(fTimeLimit - fTimeElapsed, 0f);
    }

    public void SpawnFly() {

        GameObject newFly = Instantiate(goFlyToSpawn, DetermineSpawnCoords(), Quaternion.identity);

        Debug.Assert(newFly.GetComponent<Collectable>());

        lstCollectables.Add(newFly.GetComponent<Collectable>());

        nLiveFlies++;
        
    }

    public override void OnCollected(Collectable col) {

        nLiveFlies--;

        if(nLiveFlies == 0) {
            SpawnFly();
        }

        fTimeActuallyUsed = fTimeElapsed;
    }

    public override void SetCompletionInfo() {

        completionInfo = new CompletionInfo() {
            fTimeUsed = fTimeActuallyUsed,
            nCollectedFlies = nCollected,
            nTotalFlies = nCollected + nLiveFlies,
            sCollectedDisplay = nCollected.ToString(),
            sTimeDisplay = fTimeActuallyUsed.ToString("F2")

        };
        
    }

    public override void UpdateTextTime() {
        txtTime.text = "Time: " + GetTimeRemaining().ToString("F2");
    }

    public override bool CheckPersonalHighScore() {

        if (bSavedComplete == false) {
            Debug.Log("First clear");
            return true;

        }else if (completionInfo.nCollectedFlies > nSavedCollected) {
            Debug.Log("Collected strictly more flies " + completionInfo.nCollectedFlies + " vs. " + nSavedCollected);
            return true;
        } else if (completionInfo.nCollectedFlies == nSavedCollected) {
            //Have to check more closely for tie breakers

            if(completionInfo.fTimeUsed < fSavedTime) {
                Debug.Log("Beat the previous time to collect " + completionInfo.nCollectedFlies + " with " + completionInfo.fTimeUsed + " vs. " + fSavedTime);
                return true;
            } else {
                Debug.Log("Got the same " + completionInfo.nCollectedFlies + " but " + completionInfo.fTimeUsed + " was slower than " + fSavedTime);
                return false;
            }
        } else {

            Debug.Log("New score " + completionInfo.nCollectedFlies + " didn't beat old score " + nSavedCollected);
            return false;
        }
    }

    public override void SavePersonalHighScore() {

        Debug.Assert(bSavedComplete == false || completionInfo.nCollectedFlies > nSavedCollected || (completionInfo.nCollectedFlies == nSavedCollected && completionInfo.fTimeUsed < fSavedTime));

        bSavedComplete = true;

        nSavedCollected = completionInfo.nCollectedFlies;
        fSavedTime = completionInfo.fTimeUsed;

    }

    public override void SaveGlobalHighScore() {

        Debug.LogError("Actually gotta implement global highscores at some point");

    }

    public override int GetStarsEarned(CompletionInfo completeInfo) {
        int iEarned = 0;
        for (; iEarned < arnStarThresholds.Length; iEarned++) {
            if (completeInfo.nCollectedFlies < arnStarThresholds[iEarned]) {
                break;
            }
        }

        return iEarned;
    }
}
