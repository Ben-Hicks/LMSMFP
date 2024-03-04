using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContScenes : SingletonPersistent<ContScenes> {

    [Header("Configurable")]

    public int[] arnLevelsPerWorld;
    public int iLevelSelect = 0;

    [Header("Properties")]
    public int iCurWorld;
    public int iCurScene;
    public string sCurScene {
        get { return IndexToSceneName(iCurScene, iCurWorld); }
    }

    public override void Init() {

        InitCurSceneIndices();
    }

    public void Update() {

        if (Input.GetKeyDown(KeyCode.B)) {
            //Go back to the previous scene

            Debug.Assert(iCurScene >= 0);

            LoadPreviousScene();
        }else if (Input.GetAxisRaw("Restart Level") == 1 && iCurScene != 0) {
            //Reload the current scene (only if it's not a menu scene)

            LevelType.curStartType = LevelType.StartType.FASTRESTART;
            LoadCurrentScene();
            
        }else if (Input.GetKeyDown(KeyCode.M)) {
            //Move to the next scene

            Debug.Assert(iCurScene < arnLevelsPerWorld[iCurWorld]);

            LoadNextScene();
        }else if (Input.GetAxisRaw("Main Menu") == 1) {
            LoadLevelSelect();
        }
    }

    public void LoadHubWorld() {
        Debug.Log("Clicked load hub world");
        LoadScene(0, 0);
    }

    public void LoadLevelSelect() {
        LoadScene(iLevelSelect); 
    }
    public void LoadPreviousScene() {
        LoadScene(iCurScene - 1);
    }
    public void LoadCurrentScene() {
        Debug.Assert(LevelType.curStartType == LevelType.StartType.RESTART || LevelType.curStartType == LevelType.StartType.FASTRESTART);
        LoadScene(iCurScene);
    }
    public void LoadNextScene() {
        LoadScene(iCurScene + 1);
    }

    public string IndexToSceneName(int iScene, int iCurWorld) {
        return iCurWorld + "-" + iScene;
    }

    public string IndexToSceneName(int iScene) {
        InitCurSceneIndices(); //Just in case it hasn't been done yet so we know what curworld is
        return IndexToSceneName(iScene, iCurWorld);
    }

    public string IndexToSceneName() {
        InitCurSceneIndices(); //Just in case it hasn't been done yet so we know what curworld is
        return IndexToSceneName(iCurScene, iCurWorld);
    }

    void InitCurSceneIndices() {

        string sCurScene = SceneManager.GetActiveScene().name;

        string[] arSplit = sCurScene.Split('-');

        if (arSplit.Length != 2) {
            Debug.Log("ERROR - " + sCurScene + " is not of the form 'world#-level#'");
            return;
        }

        if (!System.Int32.TryParse(arSplit[0], out iCurWorld)) {
            Debug.Log("ERROR - " + sCurScene + " is not of the form 'world#-level#'");
        }
        if (!System.Int32.TryParse(arSplit[1], out iCurScene)) {
            Debug.Log("ERROR - " + sCurScene + " is not of the form 'world#-level#'");
        }

    }

    public void LoadScene(int iScene) {
        LoadScene(iScene, iCurWorld);
    }

    public void LoadScene(int iScene, int iWorld) {
        if(iWorld < 0 || iWorld > arnLevelsPerWorld.Length || iScene < 0 || iScene > arnLevelsPerWorld[iWorld]) {
            Debug.Log("World: " + iWorld + " Scene: " + iScene + " is not a valid scene - returning to main menu");
            LoadScene(0, 0);
            return;
        }

        iCurWorld = iWorld;
        iCurScene = iScene;
        Debug.Log("Loading " + sCurScene);

        SceneManager.LoadScene(sCurScene);
    }

}
