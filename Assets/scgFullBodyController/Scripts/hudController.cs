
using UnityEngine;
using UnityEngine.UI;

    public class hudController : MonoBehaviour
    {
    public GameObject healthObject;
        public Text uiBullets;
        public GameObject crosshair;

    private void OnEnable()
    {
        ScoreBoard.Instance.gameEndAction += EnabledisHud;

    }

   
    void EnabledisHud(bool a )
    {

        healthObject.SetActive(a);
        crosshair.SetActive(a);
        uiBullets.gameObject.SetActive(a);
    }

    private void OnDisable()
    {
        ScoreBoard.Instance.gameEndAction -= EnabledisHud;
    }
}

