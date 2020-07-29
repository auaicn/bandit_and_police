using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeGameScene_toNormal(){
        SceneManager.LoadScene("final/Normalmode");
    }
    public void ChangeGameScene_toDownHill(){
        SceneManager.LoadScene("final/SelectCharacter");
    }
    public void ChangeGameScene_toDownHill_real()
    {
        SceneManager.LoadScene("final/DownHillMode");
    }

    public void ChangeGameScene_totest(){
        SceneManager.LoadScene("final/NopainNogain");
    }

}
