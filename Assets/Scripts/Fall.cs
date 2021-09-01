using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    [SerializeField] private AudioSource DeathSound;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            DeathSound.Play();
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.DoubleJumpAllow = false;
            PermanentUI.perm.lives -= 1;
             if(PermanentUI.perm.lives >= 1){
                PermanentUI.perm.Invoke("Reset", 1f);
                Invoke("Restart", 1f);
            } else {
                //Game Over
                PermanentUI.perm.Reset();
                player.DeathState();
                PermanentUI.perm.gameOverUI.SetActive(true);
            }
        }
    }

    private void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
