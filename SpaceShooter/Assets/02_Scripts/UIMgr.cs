using UnityEngine;
using UnityEngine.SceneManagement; //scene 부르기
using System.Collections;

//[ExecuteInEditMode] 두 씬을 아예 병합함
public class UIMgr : MonoBehaviour {

    /*void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 250, 50), "START"))
        {
            SceneManager.LoadScene("Level01");
            SceneManager.LoadScene("Play", LoadSceneMode.Additive);
        }
    }*/

    public void OnStartBtnClick() {
            SceneManager.LoadScene("Level01");
            SceneManager.LoadScene("Play", LoadSceneMode.Additive);
    }
}
