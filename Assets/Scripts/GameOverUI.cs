using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public AudioClip gameOverSound;

    private AudioManager theAM;

    
    private void Update()
    {
        theAM = FindObjectOfType<AudioManager>();  

        if(gameOverSound != null){
            theAM.ChangeBGM(gameOverSound);
        }  
    }

    public void Quit (){
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
        PermanentUI.perm.gameOverUI.SetActive(false);
    }
}
