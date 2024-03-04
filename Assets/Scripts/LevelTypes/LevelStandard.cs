using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStandard : LevelType {

    public float[] arfStarThresholds;

    public override bool CheckLevelFinished() {
        return nCollected == lstCollectables.Capacity;
    }


    public override void OnCollected(Collectable col) {
        
    }

    public override void SetCompletionInfo() {

        completionInfo = new CompletionInfo() {
            fTimeUsed = fTimeElapsed,
            nCollectedFlies = nCollected,
            nTotalFlies = nCollected,
            sCollectedDisplay = nCollected.ToString(),
            sTimeDisplay = fTimeElapsed.ToString("F2")

        };
    }

    public override void UpdateTextTime() {
        txtTime.text = "Time: " + fTimeElapsed.ToString("F2");
    }

    public override bool CheckPersonalHighScore() {

        if(bSavedComplete == false) {
            Debug.Log("First clear");
            return true;
        }else if (completionInfo.fTimeUsed < fSavedTime) {
            //Beat the previous score
            Debug.Log("New score " + completionInfo.fTimeUsed + " beat old score " + fSavedTime);
            return true;
        } else {
            //Didn't beat the previous highscore
            Debug.Log("New score " + completionInfo.fTimeUsed + " didn't beat old score " + fSavedTime);
            return false;
        }
    }

    public override void SavePersonalHighScore() {

        Debug.Assert(bSavedComplete == false || completionInfo.fTimeUsed < fSavedTime);

        bSavedComplete = true;

        fSavedTime = completionInfo.fTimeUsed;
        nSavedCollected = completionInfo.nCollectedFlies;

    }

    public override void SaveGlobalHighScore() {

        Debug.LogError("Actually gotta implement global highscores at some point");

    }

    public override int GetStarsEarned(CompletionInfo completeInfo) {
        int iEarned = 0;
        for (; iEarned < arfStarThresholds.Length; iEarned++) {
            if(completeInfo.fTimeUsed > arfStarThresholds[iEarned]) {
                break;
            }
        }

        return iEarned;
    }
}
