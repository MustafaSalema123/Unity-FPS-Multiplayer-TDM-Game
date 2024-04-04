using Photon.Pun;

using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager instance;

    public bool isMenuOpened = false;

    public GameObject menuUi;
    public bool isPlayGame = false;
    public bool isGameEnd = false;
    public CanvasGroup scoreLeaderBoardUi;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameEnd)
            return;
        
        isMenuOpened = Input.GetKeyDown(KeyCode.Escape) ? !isMenuOpened : isMenuOpened;
        menuUi.gameObject.SetActive(isMenuOpened );
        if (isMenuOpened) 
        {

            ScoreBoard.Instance.gameEndAction?.Invoke(false);
            isPlayGame = false;
            Cursor.lockState = CursorLockMode.None;   
        }
        else 
        {

            ScoreBoard.Instance.gameEndAction?.Invoke(true);
            isPlayGame = true;
            Cursor.lockState = CursorLockMode.Locked;   
        }



       // isscoreLeaderBoardUiOpened = Input.GetKeyDown(KeyCode.Tab) ? !isscoreLeaderBoardUiOpened : isscoreLeaderBoardUiOpened;
       // scoreLeaderBoardUi.gameObject.SetActive(isscoreLeaderBoardUiOpened);
        if (Input.GetKey(KeyCode.Tab))
        {

            ScoreBoard.Instance.gameEndAction?.Invoke(false);
            Cursor.lockState = CursorLockMode.None;
            scoreLeaderBoardUi.alpha= 1;
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {

            ScoreBoard.Instance.gameEndAction?.Invoke(true);
            Cursor.lockState = CursorLockMode.Locked;
            scoreLeaderBoardUi.alpha= 0;
        }
     

    }

    public void LeaveGame() 
    {
        PhotonNetwork.LoadLevel(0);
        //MenuManager.instance.OpenMenu("TitleMenu");
    }

    public void QuitGame() 
    {
    
    Application.Quit();
    }


    public void RestartGame()
    {

       //Debug.Log("Restart Level ");
        photonView.RPC("RPC_RestartGame", RpcTarget.All);   
    }

    [PunRPC]
    void RPC_RestartGame()
    {
        // Reset game state (e.g., player scores, positions)

        // Reload the scene
        PhotonNetwork.LoadLevel(PhotonNetwork.CurrentRoom.Name);
    }
}
