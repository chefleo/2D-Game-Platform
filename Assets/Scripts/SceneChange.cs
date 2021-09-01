using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    [SerializeField] private string sceneName;
    

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Player"){
            Save();
            PermanentUI.perm.isChange = true;
            SceneManager.LoadScene(sceneName);
        }
    }

    public void Save(){
        PermanentUI.perm.Reloadlives = PermanentUI.perm.lives;
        PermanentUI.perm.Reloadcherries = PermanentUI.perm.cherries;
        PermanentUI.perm.Reloadgem = PermanentUI.perm.gem;
        PermanentUI.perm.name = sceneName;
    }

}
