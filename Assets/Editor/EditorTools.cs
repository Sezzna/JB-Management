using UnityEngine;
using System.Collections;
using UnityEditor;

public class EditorTools : MonoBehaviour {
    [MenuItem("Tools/Clean PlayerPrefs")]
    private static void CleanPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }


}
