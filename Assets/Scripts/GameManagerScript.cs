using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {
    public Text scoreText;
    public Text ballsText;
    public Text levelText;
    public Text highscoreText;
    
    public GameObject playerPrefab;
    public GameObject ballPrefab;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelGameOver;
    public GameObject panelLevelCompleted;

    public GameObject[] levels;

    public static GameManagerScript Instance { get; private set; }

    public bool isSwitchingState;
    public bool ballOuttaBounds = false;

    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }; 
    State currentState;
    GameObject currentBall;
    GameObject currentLevel;
    float ballSidewaysSeconds = 0;

    Rigidbody ballRb;
    BallScript ballScript;

    // ======== GETTERS AND SETTERS  ======== //

    private int score;

    public int Score {
        get { return score; }
        set { score = value; 
            scoreText.text = "Score: " + score; }
    }

    private int level;

    public int Level {
        get { return level; }
        set { level = value; 
            levelText.text = "Level: " + (level + 1); }
    }

    private int balls;

    public int Balls {
        get { return balls; }
        set { balls = value; 
            ballsText.text = "Balls: " + balls; }
    }

    // ======== UNITY FUNCTIONS ======== //

    void Start() {
        GetComponent<AudioSource>().Play();
        Instance = this;
        SwitchState(State.MENU);
    }

    void Update() {
        StateUpdate();
        BallSidewaysChecker();
    }

    // ======== UI ======== //

    public void PlayClicked() {
        SwitchState(State.INIT);
    }

    // ======== STATE HANDLERS ======== //

    public void SwitchState(State newState, float delay = 0) {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay) {
        isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        currentState = newState;
        BeginState(newState);
        isSwitchingState = false;
    }

    void BeginState(State newState) {
        switch(newState) {
            case State.MENU:
                Cursor.visible = true;
                highscoreText.text = "High score: " + PlayerPrefs.GetInt("highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(currentBall);
                Destroy(currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if(level >= levels.Length) {
                    SwitchState(State.GAMEOVER);
                } else {
                    currentLevel = Instantiate(levels[Level]);
                    Debug.Log("Instantiated level");
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if(score > PlayerPrefs.GetInt("highscore")) {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                panelGameOver.SetActive(true);
                break;
        }
    }

    void StateUpdate() {
        switch(currentState) {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if(currentBall == null) {
                    if(Balls > 0) {
                        currentBall = Instantiate(ballPrefab);
                        ballRb = currentBall.GetComponent<Rigidbody>();
                        ballScript = currentBall.GetComponent<BallScript>();
                    } else {
                        SwitchState(State.GAMEOVER);
                    }
                }
                
                if(currentBall != null && currentLevel.transform.childCount == 0 && !isSwitchingState) {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if(Input.anyKeyDown) {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    void EndState() {
        switch(currentState) {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }

    void BallSidewaysChecker() {
        if(currentState == State.PLAY && currentBall != null) {
            if((ballRb.velocity.y <= 3.5 && ballRb.velocity.y >= -3.5)) {
                ballSidewaysSeconds += Time.deltaTime;
                if(ballSidewaysSeconds > 3) {
                    GameObject.Find("SidewaysText").GetComponent<Text>().enabled = true;
                    if(Input.GetKeyDown(KeyCode.Space)) {
                        ballRb.velocity = new Vector3(ballRb.velocity.x, -ballScript.speed, 0);
                    }
                }
            } else {
                ballSidewaysSeconds = 0;
                GameObject.Find("SidewaysText").GetComponent<Text>().enabled = false;            
            }
        }
    }
}