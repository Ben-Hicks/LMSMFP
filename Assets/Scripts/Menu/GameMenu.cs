using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    public Text txtFinalCollected;
    public Text txtFinalTime;

    public Text txtBestCollected;
    public Text txtBestTime;

    public Star[] arStars;
    
    ContScenes contScenes;

    public void Start() {
        contScenes = ContScenes.Get();
    }

    public void DisplayMenu(LevelType.CompletionInfo completeInfo) {

        gameObject.SetActive(true);

        txtFinalCollected.text = completeInfo.sCollectedDisplay;
        txtFinalTime.text = completeInfo.sTimeDisplay;
    }

    public void HandleStars(LevelType.CompletionInfo completeInfo) {

        foreach (Star star in arStars) {
            star.gameObject.SetActive(true);
        }

        int nPreviousStars = LevelType.Get().nSavedStarsEarned;

        Debug.Log("Previously earned " + nPreviousStars);

        int nStarsEarnedThisRun = LevelType.Get().GetStarsEarned(completeInfo);

        //If we're maxed out our stars for this
        if(nPreviousStars == 4 || nStarsEarnedThisRun == 4) {
            //Then make each star golden
            Debug.Log("Earned gold");
            foreach(Star star in arStars) {
                star.SetGold();
            }

            if(nStarsEarnedThisRun > nPreviousStars) {
                //If we just got gold for the first time, then play the pop-up animation for each star
                Debug.Log("Earned gold for the first time");
                foreach (Star star in arStars) {
                    star.NewlyEarnedStar();
                }
            }
        } else {
            //Otherwise we don't have gold stars

            for (int i = 0; i < arStars.Length; i++) {
                if(nPreviousStars <= i && i < nStarsEarnedThisRun) {
                    Debug.Log("Earned star " + i + " for the first time");
                    //If we didn't have this star before, but we do now
                    arStars[i].NewlyEarnedStar();
                    GeneralManager.nSavedTotalStarsEarned++;
                } else if(nPreviousStars <= i && nStarsEarnedThisRun <= i) {
                    Debug.Log("We still haven't earned star " + i + " yet");
                    //If we haven't earned this star before, and we didn't this run either
                    arStars[i].HideStar();
                }
            }
        }

        if (nStarsEarnedThisRun > nPreviousStars) {

            Debug.Log("New stars earned! " + nStarsEarnedThisRun + ".   Saving progress!");

            LevelType.Get().nSavedStarsEarned = nStarsEarnedThisRun;
        }

        
        
    }

    void SetTextHighscores() {
        txtBestCollected.text = LevelType.Get().nSavedCollected.ToString();
        txtBestTime.text = LevelType.Get().fSavedTime.ToString("F2");
    }

    public void HandleHighScores() {

        if (LevelType.Get().CheckPersonalHighScore()) {
            //if we got a new highscore, then we should play some animation

            Debug.Log("Got a new highscore!");

            //Then save these scores
            LevelType.Get().SavePersonalHighScore();
            LevelType.Get().SaveGlobalHighScore();
        }

        SetTextHighscores();

    }

    public void ClickNextLevel() {
        LevelType.Get().screenTransition.CloseTransition();
        ContScenes.Get().Invoke("LoadNextScene", LevelType.Get().screenTransition.fTransitionTime);
    }
    public void ClickReplayLevel() {
        //LevelType.Get().screenTransition.CloseTransition();
        //ContScenes.Get().Invoke("LoadCurrentScene", LevelType.Get().screenTransition.fTransitionTime);
        LevelType.curStartType = LevelType.StartType.RESTART;
        ContScenes.Get().LoadCurrentScene();
    }
    public void ClickMainMenu() {
        LevelType.Get().screenTransition.CloseTransition();
        ContScenes.Get().Invoke("LoadLevelSelect", LevelType.Get().screenTransition.fTransitionTime);
    }

}
