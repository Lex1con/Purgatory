using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainScript : MonoBehaviour
{
    [Header("UI")]

    public GameObject restartBtn;
    public Text ScoreText;
    public Text StatusText;
    public Text HealthText;

    private GameObject player;
    public GameObject StatusBox;

    public int score;
    public bool playerAlive;
    



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        player = GameObject.Find("Player");
        CheckPlatform();
        setUpCanvasUI();
        checkForHighScore();
        ScoreText.text = "Score: 0";
        StatusText.text = "";
        HealthText.text = "Health: "+ player.GetComponent<PlayerCombat>().maxHealth;
        playerAlive = true;
        score = 0;
    }

    public void setUpCanvasUI(){
        restartBtn = GameObject.Find("RestartBtn");
        restartBtn.SetActive(false); 
        StatusBox = GameObject.Find("StatusBox");
        StatusBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerAlive){
            StatusText.text = "You Died!";
            StatusBox.SetActive(false);

            Time.timeScale = 0;
            restartBtn.SetActive(true);
        }
        ScoreText.text = "Score: "+score;
        
    }

    public void checkForHighScore(){
        if(PlayerPrefs.HasKey("highscore") == false){
            PlayerPrefs.SetInt("highscore", 0);
        }
    }

    public void CheckPlatform(){
        #if UNITY_EDITOR
            Debug.Log("Unity Editor");
        #endif

        #if UNITY_ANDROID
            Debug.Log("Unity Android");
        #endif

        #if UNITY_STANDALONE
            Debug.Log("Unity Standalone");
        #endif
    }

    public void Restart() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
    
}
