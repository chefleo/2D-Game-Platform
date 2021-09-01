using UnityEngine;
using UnityEngine.SceneManagement;

public class end : MonoBehaviour
{

    public void Quit (){
        Debug.Log("Application Quit");
        Application.Quit();
    }

    public void Retry (){
        PermanentUI.perm.lives = 3;
        PermanentUI.perm.Reloadcherries = 0;
        PermanentUI.perm.Reloadgem = 0;
        PermanentUI.perm.cherries = 0;
        PermanentUI.perm.gem = 0;
        PermanentUI.perm.name = "SampleScene";
        PermanentUI.perm.isChange = false;
        PermanentUI.perm.Reset();
        SceneManager.LoadScene("SampleScene");
    }
}

