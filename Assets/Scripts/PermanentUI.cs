using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PermanentUI : MonoBehaviour
{
    public int cherries;
    public int gem;
    public int lives = 3;
    public int Reloadlives = 3;
    public int Reloadcherries;
    public int Reloadgem;
    public int health = 5;
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI heartsText;
    public Text healthAmount;
    [SerializeField] public GameObject gameOverUI;

    public string name = "SampleScene";
    public bool isChange = false;

    public static PermanentUI perm;

    private void Start(){

        DontDestroyOnLoad(gameObject);

        if(!perm){
            perm = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void Reset(){
        if(SceneManager.GetActiveScene().name == name){
            // if is not the first level
            if(isChange == true){
                cherries = Reloadcherries;
                gem = Reloadgem;
            } else {
                cherries = 0;
                gem = 0;
            }    
        }
        health = 5;
        cherryText.text = cherries.ToString();
        gemText.text = gem.ToString();
        if(lives >= 1){
            heartsText.text = lives.ToString();
        } else {
            lives = 0;
            heartsText.text = lives.ToString();
        }
    }

}
