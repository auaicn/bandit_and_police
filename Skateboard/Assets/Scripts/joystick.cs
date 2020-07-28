using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{





    //해야할일, 1.현재는 JUMP가 누르면 계속 위로 올라감, 이거를 함수로 만들어야 할듯?(ex 한번 누르면 1초간 점프
    //2. 멀티터치가 완벽하게 구현 안댐..
    //3. 에니메이션이랑 연결
    //4. 게임 맵 구성





    // Start is called before the first frame update
    [SerializeField]
    RectTransform m_rectBack;
    RectTransform m_rectJoystick;

    Transform m_trCube;
    float m_fRadius;
    float m_fSpeed = 5.0f;
    float m_fSqr = 0f;
    Vector3 centerofmass;

    Vector3 m_vecMove;
    Vector3 m_vecMove2;

    Vector3 m_vecMove3;
    Vector3 zero; 
    Vector2 m_vecNormal;
    Rigidbody myrigid;
    bool m_bTouch = false;


    void Start()
    {
        m_rectBack = transform.Find("deck").GetComponent<RectTransform>();
        m_rectJoystick = transform.Find("deck/deck_bg").GetComponent<RectTransform>();
        //m_button = transform.Find("button").GetComponent<RectTransform>();
        myrigid = GameObject.Find("Playable").GetComponent<Rigidbody>();
        m_trCube = GameObject.Find("Playable").transform;

        // JoystickBackground의 반지름입니다.
        m_fRadius = m_rectBack.rect.width * 0.1f;
        zero = new Vector3(0, 0, 0);
        centerofmass = new Vector3(0, -0.05f, 0);
        myrigid.centerOfMass = centerofmass;

    }

    void Update()
    {

        if (Input.touchCount != 0)
        {
            UnityEngine.Debug.Log("터치 개수: " + Input.touchCount);
            UnityEngine.Debug.Log(Input.GetTouch(0));
        }
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);
            if (t1.position.x<= Screen.width / 3 && t2.position.x > Screen.width / 3 && t2.position.x <= Screen.width / 3 * 2)
            {
                // 한손 왼쪽, 한손 중간에 있으면
                // Z 포지션 상관없이 오른쪽으로 힘을 준다.
                myrigid.AddRelativeForce(m_vecMove2);

            }
            else if (t2.position.x <= Screen.width / 3 && t1.position.x > Screen.width / 3 && t1.position.x <= Screen.width / 3 * 2)
            {
                myrigid.AddRelativeForce(m_vecMove2);

            }
            else if (t1.position.x > Screen.width / 3 && t1.position.x <= Screen.width / 3 * 2 && t2.position.x > Screen.width / 3 && t2.position.x <= Screen.width / 3 * 2)
            {
                if (m_bTouch)
                {
                    // m_trCube.Rotate(0, Vector3.magnitude(m_vecMove), 0);
                    if (m_vecMove[0] < 0)
                        m_trCube.Rotate(0, -m_vecMove.magnitude * 5, 0);
                    else
                        m_trCube.Rotate(0, m_vecMove.magnitude * 5, 0);
                    // m_trCube.position += m_vecMove;
                }
            }

        }
        else if(Input.touchCount == 1) 
        {
            Touch t = Input.GetTouch(0);

            if (t.position.x > Screen.width / 3 * 2)
            {
                myrigid.AddRelativeForce(m_vecMove3);

            }
        }
    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - m_rectBack.position.x, 0f);
        // UnityEngine.Debug.Log(vec[0]);
        //점프
        m_vecMove3 = new Vector3(0, 100,0);
        //m_vecMove2 = new Vector3(0, 0, 50);

        // vec값을 m_fRadius 이상이 되지 않도록 합니다.
        // vec = Vector2.ClampMagnitude(vec, 10f);
        vec = Vector2.ClampMagnitude(vec, 20f);
        m_rectJoystick.localPosition = vec;

        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        float fSqr = (m_rectBack.position - m_rectJoystick.position).sqrMagnitude / (m_fRadius * m_fRadius);

        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;

        // m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime * fSqr, 0f, 0f);
        m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime, 0f, 0f);
        //m_vecmove2 = new vector3( 0f, 0f, m_fspeed * time.deltatime*1000);
        m_vecMove2 = new Vector3(0, 0,100);

        // m_trCube.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 원래 위치로 되돌립니다.
        m_rectJoystick.localPosition = Vector2.zero;
        m_bTouch = false;
    }

}