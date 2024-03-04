using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {

    public int nLevel;

    public Text txtLabel;
    public GameObject goStarPanel;

    public int nStarsEarned {
       get { return PlayerPrefs.GetInt(ContScenes.Get().IndexToSceneName(nLevel) + LevelType.sSavedStarsEarnedSuffix); }
    }

    public Star[] arStars;


    public void Awake() {
        
    }

    public void Start() {
        DisplayLabel();

        if (IsUnlocked() == false) {
            gameObject.SetActive(false);
        } else {
            DisplayStars();
        }
    }

    public bool IsUnlocked() {
        //Replace this with checking if the level has been unlocked yet
        return true;
    }

    public void DisplayLabel() {
        txtLabel.text = ContScenes.Get().IndexToSceneName(nLevel);
    }

    public void DisplayStars() {

        if (nStarsEarned == 4) {
            //Then make each star golden
            foreach (Star star in arStars) {
                star.SetGold();
            }
        } else {
            //Otherwise we don't have gold stars
            //Hide each star we haven't earned yet
            for (int i = nStarsEarned; i < arStars.Length; i++) {
                arStars[i].HideStar();
            }
        }

        foreach (Star star in arStars) {
            star.PauseAnimation();
        }
    }

    public void OnClick() {
        ContScenes.Get().LoadScene(nLevel);
    }


}
