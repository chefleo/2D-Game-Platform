using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public AudioSource PauseSound;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(PermanentUI.perm.lives > 0){
            if(Input.GetKeyDown(KeyCode.Escape)  || Input.GetKeyDown(KeyCode.P)){
                PauseSound.Play();
                if(GameIsPaused){
                    Resume();
                } else {
                    Pause();
                }
            } 
        }     
    }


    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void RestartLevel(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PermanentUI.perm.lives = PermanentUI.perm.Reloadlives;
        PermanentUI.perm.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit(){
        Application.Quit();
    }
}
