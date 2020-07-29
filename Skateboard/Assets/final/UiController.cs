using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class UiController : MonoBehaviour
{
    public static UiController Instance = null;
    private static UiController instance;
    Button m_btn;
    public GameObject EndGameGo;
    public GameObject NextStageGO;
    public GameObject MenuGo;

public bool isPause = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void Awake(){
        Instance=this;
    }
    public void EndGame()
    {
        // StartCoroutine(EndGamePopUp());
        EndGameGo.SetActive(true);
        // SceneManager.LoadScene(2);
    

    }
    public void NextStage()
    {
        // StartCoroutine(EndGamePopUp());
        NextStageGO.SetActive(true);
        // SceneManager.LoadScene(2);
    

    }
    public void Menu()
    {
        isPause =!isPause;
        if(isPause){
            Time.timeScale=0f;
            MenuGo.SetActive(true);

        }
        else{
            MenuGo.SetActive(false);

            Time.timeScale=1f;
            
        }
        // SceneManager.LoadScene(2);
    }
    public void backbuttonclick(){
        isPause =!isPause;
        if(isPause){
            Time.timeScale=0f;
            MenuGo.SetActive(true);

        }
        else{
            MenuGo.SetActive(false);

            Time.timeScale=1f;
            
        }

    }
    public void restartbuttonclick(){
        SceneManager.LoadScene(1);
        Time.timeScale=1f;

    }
    public void Menubuttonclick(){
        SceneManager.LoadScene(2);

    }

    public void nextbuttonclick(){
        
            Time.timeScale=1f;

    }
    public void  Modebuttonclick(){
                SceneManager.LoadScene(0);

            Time.timeScale=1f;

    }

    IEnumerator EndGamePopUp()
    {

        EndGameGo.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene(1);
    }
}
