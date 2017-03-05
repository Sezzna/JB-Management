using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartSceneLogic : MonoBehaviour {

    void Start() {
        //处理版本验证等问题;

        //加载主场景;
        SceneManager.LoadScene("MainScene");
    }


}
