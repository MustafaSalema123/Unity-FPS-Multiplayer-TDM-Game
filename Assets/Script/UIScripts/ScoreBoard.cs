using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
 public static ScoreBoard Instance;


  //  public GameObject Scorepanel;
    public Text blueTeamText;
    public Text redTeamText;


    public Text LastresultShowText;

    public int redTeamScore;
    public int blueTeamScore;

    public GameObject endPanel;
    public Text blueTeamEndText;
    public Text redTeamEndText;
    PhotonView view;

    [Header("timer")]

    public Text timerText;
    public float timeRemaining = 600f;

    public Action<bool> gameEndAction;
    private void Awake()
    {
        endPanel.SetActive(false);

        view = GetComponent<PhotonView>();
        Instance = this;
    }

    private void Update()
    {
        if(timeRemaining > 0) 
        {
            timeRemaining -= Time.deltaTime;
            UpateTimerText();
        }
        else 
        {
            gameEndAction?.Invoke(false);

            GameManager.instance.isGameEnd = true;
            GameManager.instance.isPlayGame = false;
            view.RPC("UpdateScore", RpcTarget.All, blueTeamScore, redTeamScore);
        timeRemaining = 0;

            //return;
        }
    }
    private void UpateTimerText() 
    {
    TimeSpan timeSpan = TimeSpan.FromSeconds(timeRemaining);
        timerText.text = string.Format("{0:D2}:{1:D2}" , timeSpan.Minutes, timeSpan.Seconds);
    }
    public void PlayerDiedTeamVise(int playerTeam) 
    {
        if(playerTeam == 1) 
        {
            blueTeamScore++;
        }
        if (playerTeam == 2)
        {
            redTeamScore++;
        }
        view.RPC("UpdateScore", RpcTarget.All, blueTeamScore, redTeamScore);
    }

    [PunRPC]
    void UpdateScore(int blueScore, int redScore)
    {
        blueTeamScore = blueScore;
        redTeamScore = redScore;

        blueTeamText.text= blueScore.ToString();
        redTeamText.text= redScore.ToString();


        CheckWinnerAndLoser();

    }
    void CheckWinnerAndLoser()
    {
        if (blueTeamScore >= 40 || redTeamScore >= 40 || timeRemaining <= 0)
        {
            if (blueTeamScore > redTeamScore)
            {
                if (view.IsMine)
                {
                    LastresultShowText.text = "LOSER!";
                }
                else
                {
                    LastresultShowText.text = "VICTORY!";
                }
            }
            else if (redTeamScore > blueTeamScore)
            {
                if (view.IsMine)
                {
                    LastresultShowText.text = "VICTORY!";
                }
                else
                {
                    LastresultShowText.text = "LOSER!";
                }
            }
            else
            {
                LastresultShowText.text = "It's a tie!";
            }
            blueTeamEndText.text = redTeamScore.ToString();
             redTeamEndText.text = blueTeamScore.ToString() ;

    Cursor.lockState = CursorLockMode.None;
            //Scorepanel.SetActive(false);
            endPanel.SetActive(true);
        }
    }

}
