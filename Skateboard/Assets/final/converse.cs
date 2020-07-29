using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class converse :  MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // Start is called before the first frame update
    [SerializeField]
    RectTransform m_converse;
    RectTransform m_converse_joystick;

    Transform m_trCube;
    float m_fRadius;
    float m_fSpeed = 5.0f;
    float m_fSqr = 0f;
 
    Vector3 m_vecMove;
 
    Vector2 m_vecNormal;
 
    bool m_aTouch = false;
 
 
    void Start()
    {
        m_converse = GetComponent<RectTransform>();
         m_converse_joystick = GetComponent<RectTransform>();

        m_trCube = GameObject.Find("Cube").transform;
 
    }
 
    void Update()
    {
        if (m_aTouch)
        {
            m_trCube.position += m_vecMove;
        }
            
    }
 
    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(0f, vecTouch.y - m_converse.position.y);
 
        
        // vec값을 m_fRadius 이상이 되지 않도록 합니다.
        vec = Vector2.ClampMagnitude(vec, 1f);
        m_converse_joystick.localPosition = vec;
 
        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
 
        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;
 
        // m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime * fSqr, 0f, 0f);
                m_vecMove = new Vector3(0f , 0f, vecTouch.y - m_converse.position.y);

        // m_trCube.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_aTouch = true;
    }
 
    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_aTouch = true;
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        // 원래 위치로 되돌립니다.
        m_converse_joystick.localPosition = Vector2.zero;
        m_aTouch = false;
    }

}
