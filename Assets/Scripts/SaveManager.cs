using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManager {


    public enum SaveType { COMPLETE, TIME, COLLECTED };

    static readonly string[] arsSuffixes = { "-Complete", "-Time", "-Collected" };

    public static string GetSavedInfo(SaveType saveType) {
        return GetSavedInfo(saveType, ContScenes.Get().iCurScene);
    }
    public static string GetSavedInfo(SaveType saveType, int nLevel) {
        return GetSavedInfo(saveType, nLevel, ContScenes.Get().iCurWorld);
    }
    public static string GetSavedInfo(SaveType saveType, int nLevel, int nWorld) {
        return PlayerPrefs.GetString(ContScenes.Get().IndexToSceneName(nLevel, nWorld) + arsSuffixes[(int)saveType]);
    }
    public static void SaveInfo(SaveType saveType, string sVal) {
        SaveInfo(saveType, sVal, ContScenes.Get().iCurScene);
    }
    public static void SaveInfo(SaveType saveType, string sVal, int nLevel) {
        SaveInfo(saveType, sVal, nLevel, ContScenes.Get().iCurWorld);
    }
    public static void SaveInfo(SaveType saveType, string sVal, int nLevel, int nWorld) {

        PlayerPrefs.SetString(ContScenes.Get().IndexToSceneName(nLevel, nWorld) + arsSuffixes[(int)saveType], sVal);

    }

}
