using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;   

public abstract class LevelType : Singleton<LevelType> {

    
    public List<Collectable> lstCollectables;
    public int nCollected;
    public float fTimeElapsed;

    public float fCollectedCompletion;
    public float fTimeCompletion;

    public Subject subCollected;

    public Text txtCollectables;
    public Text txtTime;

    public GameMenu gameMenu;
    public Player plyr;

    public ScreenTransition screenTransition;

    public float fDelayBeforeFinishMenu;
    public float fDelayBeforeShowingStars;
    public float fDelayBeforeAddingStars;

    public enum StateLevel { INTRO, READY, ACTIVE, ENDING, SPAWNINGMENU, SHOWINGSTARS, ADDINGSTARS, COMPLETE }
    public StateLevel curStateLevel;

    public enum StartType { STANDARD, RESTART, FASTRESTART };
    public static StartType curStartType;

    public struct CompletionInfo {
        public int nCollectedFlies;
        public int nTotalFlies;
        public string sCollectedDisplay;

        public float fTimeUsed;
        public string sTimeDisplay;
    }

    public CompletionInfo completionInfo;

    public Subject subStartActive;

    const string sSavedCompleteSuffix = "-Complete";
    public bool bSavedComplete {
        get {
            return PlayerPrefs.GetString(ContScenes.Get().sCurScene + sSavedCompleteSuffix) == "Complete";
        }
        set {
            if (value) {
                PlayerPrefs.SetString(ContScenes.Get().sCurScene + sSavedCompleteSuffix, "Complete");
            } else {
                PlayerPrefs.SetString(ContScenes.Get().sCurScene + sSavedCompleteSuffix, "Incomplete");
            }
        }

    }

    public const string sSavedCollectedSuffix = "-Collected";
    public int nSavedCollected {
        get {
            return PlayerPrefs.GetInt(ContScenes.Get().sCurScene + sSavedCollectedSuffix);
        }
        set {
            PlayerPrefs.SetInt(ContScenes.Get().sCurScene + sSavedCollectedSuffix, value);
        }
    }

    public const string sSavedTimeSuffix = "-Time";
    public float fSavedTime {
        get {
            return PlayerPrefs.GetFloat(ContScenes.Get().sCurScene + sSavedTimeSuffix);
        }
        set {
            PlayerPrefs.SetFloat(ContScenes.Get().sCurScene + sSavedTimeSuffix, value);
        }
    }

    public const string sSavedStarsEarnedSuffix = "-Stars";
    public int nSavedStarsEarned {
        get {
            return PlayerPrefs.GetInt(ContScenes.Get().sCurScene + sSavedStarsEarnedSuffix);
        }
        set {
            PlayerPrefs.SetInt(ContScenes.Get().sCurScene + sSavedStarsEarnedSuffix, value);
        }
    }


    public LevelType() {
        subCollected = new Subject();
        subStartActive = new Subject();
    }

    public override void Init() {

        plyr = FindObjectOfType<Player>();

        lstCollectables = FindObjectsOfType<Collectable>().ToList<Collectable>();

        TransitionState(StateLevel.INTRO);

        UpdateTextTime();
        UpdateTextCollectables();
        
    }

    public void OnStartingMovement() {
        if (curStateLevel != StateLevel.READY) return;
        TransitionState(StateLevel.ACTIVE);
    }

    public virtual void Update() {

        if (curStateLevel == StateLevel.ACTIVE) {
            fTimeElapsed += Time.deltaTime;

            if (CheckLevelFinished()) {
                TransitionState(StateLevel.ENDING);
            }

            UpdateTextTime();
        }

        
    }

    public void Collected(Collectable col) {

        col.bCollected = true;
        nCollected++;

        lstCollectables.Remove(col);

        OnCollected(col);

        UpdateTextCollectables();
        subCollected.NotifyObs(col);
    }

    public virtual void OnCollected(Collectable col) {

    }

    public abstract bool CheckLevelFinished();

    public virtual void UpdateTextCollectables() {
        txtCollectables.text = "Flies: " + nCollected;
    }

    //For debugging purposes
    public void ClearStars() {
        Debug.Log("Clearing Stars");

        nSavedStarsEarned = 0;
    }

    public void NextState() {
        TransitionState(curStateLevel + 1);
    }

    public void TransitionState(StateLevel newState) {
        curStateLevel = newState;

        switch (curStateLevel) {
            case StateLevel.INTRO:
                if (curStartType == StartType.RESTART || curStartType == StartType.FASTRESTART) {
                    //If we restarted the level, then skip the intro animation, and do a small screen shake instead

                    if(curStartType == StartType.FASTRESTART) Camera.main.GetComponent<ContCamera>().ScreenShake();

                    curStartType = StartType.STANDARD;

                    TransitionState(StateLevel.READY);
                    return;
                }
                //Enable the transition object
                screenTransition.enabled = true;
                //Do the intro animation
                screenTransition.OpenTransition();
                //Lock the character's movement
                plyr.LockMovement();
                //Move to the next state once the transition time is done
                Invoke("NextState", screenTransition.fTransitionTime);
                break;

            case StateLevel.READY:
                //Unlocked the character's movement
                plyr.UnlockMovement();
                break;

            case StateLevel.ACTIVE:
                //Ensure the level timer starts at 0
                fTimeElapsed = 0f;
                //TODO:: Start Music

                subStartActive.NotifyObs();
                break;

            case StateLevel.ENDING:
                //Save completion information
                fTimeCompletion = fTimeElapsed;
                fCollectedCompletion = nCollected;
                SetCompletionInfo();

                //Lock character movement
                plyr.LockMovement();

                Invoke("NextState", fDelayBeforeFinishMenu);
                break;

            case StateLevel.SPAWNINGMENU:
                gameMenu.DisplayMenu(completionInfo);

                Invoke("NextState", fDelayBeforeShowingStars);
                break;

            case StateLevel.SHOWINGSTARS:
                gameMenu.HandleStars(completionInfo);
                gameMenu.HandleHighScores();

                PlayerPrefs.Save();

                Invoke("NextState", fDelayBeforeAddingStars);
                break;

            case StateLevel.ADDINGSTARS:
                Debug.Log("Still have to implement adding stars");

                break;

            case StateLevel.COMPLETE:
                Debug.Log("Nothing to do in the complete state");

                break;


        }

    }

    public abstract void UpdateTextTime();

    public abstract void SetCompletionInfo();

    public abstract bool CheckPersonalHighScore();
    public abstract void SavePersonalHighScore();

    public abstract void SaveGlobalHighScore();

    public abstract int GetStarsEarned(CompletionInfo completeInfo);
}
