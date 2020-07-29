using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class POLICE : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float Speed;
    private Rigidbody myRigid;

    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
     
            Move();
            if(Input.GetKeyDown("r")){
                UiController.Instance.EndGame();

            }


    }
    private void Move(){
        float _moveDirX= Input.GetAxisRaw("Horizontal");
        float _moveDirZ= Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right*_moveDirX;
        Vector3 _moveVertical = transform.forward*_moveDirZ;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized*Speed;
        myRigid.MovePosition(transform.position+ _velocity*Time.deltaTime);        
    }
        private void OnCollisionEnter(Collision collision)

    {

        // 이 컴포넌트가 부착된 게임 오브젝트의 콜라이더와 충돌한 게임 오브젝트 가져오기


        // 특정 컴포넌트 가져오기
    
        if( collision.gameObject.CompareTag("Inspector")){
            UiController.Instance.EndGame();
        }
        if( collision.gameObject.CompareTag("Supreme")){
            UiController.Instance.NextStage();
        }


    }


}
