using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    [SerializeField]
    RectTransform m_rectBack;
    RectTransform m_rectJoystick;
 
    Transform m_trCube;
    float m_fRadius;
    float m_fSpeed = 5.0f;
    float m_fSqr = 0f;
 
    Vector3 m_vecMove;
 
    Vector2 m_vecNormal;
 
    bool m_bTouch = false;
 
 
    void Start()
    {
        m_rectBack = transform.Find("boardbackground").GetComponent<RectTransform>();
        m_rectJoystick = transform.Find("boardbackground/boardjoystick").GetComponent<RectTransform>();
 
        m_trCube = GameObject.Find("Skater").transform;
 
        // JoystickBackground의 반지름입니다.
        m_fRadius = m_rectBack.rect.width * 0.1f;
    }
 
    void Update()
    {
        if (m_bTouch)
        {
            m_trCube.position += m_vecMove;
        }
            
    }
 
    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - m_rectBack.position.x, 0f);
 
        
        // vec값을 m_fRadius 이상이 되지 않도록 합니다.
        vec = Vector2.ClampMagnitude(vec, 10f);
        m_rectJoystick.localPosition = vec;
 
        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        float fSqr = (m_rectBack.position - m_rectJoystick.position).sqrMagnitude / (m_fRadius * m_fRadius);
 
        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;
 
        // m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime * fSqr, 0f, 0f);
                m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime , 0f, 0f);

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
