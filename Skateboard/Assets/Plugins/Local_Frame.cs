using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class Action
{
    public const int Ali = 1;
    public const int Sprint = 2;
    public const int FlipFlap = 3;
    public const int UpSideDown = 4;
    public const float skill_duration = 0.75f;
    public const int num_turn = 2;
}

internal static class Region
{
    public const int Menu = 0;
    public const int Screen = 1;
    public const int Action = 2;
}

internal static class Constants_
{
    public const float decrease_speed = 0.0005f;
    public const float decrease_speed_when_turn = 0.0002f;
    public const float increase_speed = 0.005f;
    public const float degree = 20f;
    public const int constraint_on_zoom_Operation = 50;
    public const float constraint_on_skill_Operation = 0.5f; //  Ali 동작간에, clear를 막기 위함.
}


public class Local_Frame : MonoBehaviour
{
    // Class : Class Here, : means derivation.

    // context 간에, script 간에 주고받을 변수
    public static bool start; // to start game actually when we put two fingers on Screen.
    public static float score;

    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_joystick;
    [SerializeField] private GameObject playable_game_object;

    public static Animator bandit_animator;
    public static float elapsed_time = 0f;


    GameObject sprint_point;
    GameObject sprint_panel;
    GameObject targetObject;
    GameObject mainCamera;
    GameObject board_image;
    public static GameObject board_game_object;
    private static GameObject bandit_game_object;

    Vector3 initialCameraPosition;

    private float velocity;

    // Region 나누기 위해서 사용
    public static Rect TopMost;
    public static Rect upper;
    public static Rect lower;
    public static int skill;
    public static bool aliable;

    // Action Control
    public static Rect sprint_region;
    Vector2 primary_point; public int primary_point_finger_id;
    Vector2 secondary_point; public int secondary_point_finger_id;
    Vector2 mid_point;

    Rigidbody playable_rigid_body;
    List<Hashtable> touches;

    float distance_start;
    float distance_earlier;
    Vector2 old_position;
    float distance_changed;
    bool ali;
    bool sprint;
    bool flipflap;
    bool upsidedown;

    private float slope;
    private bool point_set_needed;

    void OnGUI()
    {
        GUI.skin.box.fontSize = 100;
        GUI.Box(new Rect(0, 0, 400, 400), ((int)score).ToString());

        // GUI 에 표시되는 화면 y값 조정
        Rect Top_Most_on_GUI = new Rect(TopMost.x, Screen.height - TopMost.yMax, TopMost.width, TopMost.height);
        Rect Upper_on_GUI = new Rect(upper.x, Screen.height - upper.yMax, upper.width, upper.height);
        Rect Lower_on_GUI = new Rect(lower.x, Screen.height - lower.yMax, lower.width, lower.height);


        // 중간에 터치영역 보이도록 나누어준 UI
        GUI.Box(new Rect(Upper_on_GUI.x, Upper_on_GUI.y, Upper_on_GUI.width, 10), "Game Screen");
        GUI.Box(new Rect(Lower_on_GUI.x, Lower_on_GUI.y, Lower_on_GUI.width, 10), "Lower");

    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = 0f;

        point_set_needed = true;
        skill = 0;
        score = 0f;
        slope = 0f;
        aliable = false;
        lower = new Rect(0f, 0f, Screen.width, Screen.height * 0.5f);
        upper = new Rect(0f, lower.yMax, Screen.width, Screen.height * 0.4f);
        TopMost = new Rect(0f, upper.yMax, Screen.width, Screen.height * 0.1f);

        playable_game_object = GameObject.Find("Playable");
        playable_rigid_body = GameObject.Find("Playable").GetComponent<Rigidbody>();
        targetObject = GameObject.Find("Playable");
        mainCamera = GameObject.Find("MainCamera");
        board_game_object = GameObject.Find("skateboard");
        bandit_game_object = GameObject.Find("bandit");
        bandit_animator = bandit_game_object.GetComponent<Animator>();

        initialCameraPosition = mainCamera.GetComponent<Transform>().localPosition;

        sprint_panel = GameObject.Find("sprint_panel");
        sprint_point = GameObject.Find("sprint_point");
        board_image = GameObject.Find("board_image");
        board_image.SetActive(false);
        playable_rigid_body.centerOfMass = new Vector3(0f, -0.05f, 0f);
        start = false;

        touches = new List<Hashtable>();
        touches.Add(new Hashtable());
        touches.Add(new Hashtable());
        touches.Add(new Hashtable());

        distance_start = 0f;
        distance_earlier = 0f;
        distance_changed = 0f;

    }


    // Update is called once per frame
    void Update()
    {
        playable_game_object.transform.Translate(0, 0, velocity);
        velocity -= Constants_.decrease_speed;
        if (velocity < 0f)
            velocity = 0f;
        score += Time.deltaTime;
        elapsed_time += Time.deltaTime;

        // play time

        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            int fingerId = t.fingerId;
            int region = Get_region(t); // 0 Menu 1 Screen 2 Action
            switch (t.phase)
            {
                case TouchPhase.Began:
                    if (touches[region].ContainsKey(fingerId))
                        Debug.Log("Double Contain Error. Not Removed Properly?");
                    else
                        touches[region].Add(fingerId, t);
                    break;
                case TouchPhase.Ended:
                    if (touches[region].ContainsKey(fingerId))
                        touches[region].Remove(fingerId);
                    else
                        Debug.Log("Double Remove Error. Not Added Properly?");
                    break;
                case TouchPhase.Moved:
                    if (touches[region].ContainsKey(fingerId))
                        touches[region][fingerId] = t;
                    else
                        Debug.Log("Cannot find corresponding touch point. Not Added Properly?");
                    break;
            }
            ++i;
        }

        if (touches[Region.Menu].Count > 1)
            touches[Region.Menu].Clear();
        if (touches[Region.Screen].Count > 2)
            touches[Region.Screen].Clear();
        if (touches[Region.Action].Count > 2)
            touches[Region.Action].Clear();

        // screen zoom out : only if two touch is detected
        if (touches[Region.Screen].Count == 2)
        {
            // Debug.Log("touch count 2 on screen");
            // need previous information 
            // actually we need only distance
            UnityEngine.Vector2[] touch_point = new UnityEngine.Vector2[2];
            i = 0;
            foreach (DictionaryEntry d in touches[Region.Screen])
            {
                touch_point[i++] = new UnityEngine.Vector2(((Touch)d.Value).position.x, ((Touch)d.Value).position.y);
            }

            // update distance
            distance_changed = UnityEngine.Vector2.Distance(touch_point[0], touch_point[1]); // always changed if two touch point is given

            // if distance_earlier not set, set.
            if (distance_earlier == 0)
            {
                Debug.Log(distance_changed);
                distance_start = distance_changed;
                distance_earlier = distance_changed;
                return;
            }

            // scale 해주는건 좀 시간소모가 심할 것 같다.
            // 그래도 함수 이름은 그대로 scale을 사용해주겠다.
            scale(distance_changed / distance_earlier);
        }
        else if (touches[Region.Screen].Count == 1)
        {
            // actually, only one
            Vector2 current_position = Vector2.zero;
            foreach (DictionaryEntry d in touches[Region.Screen])
                current_position = new Vector2(((Touch)d.Value).position.x, ((Touch)d.Value).position.y);

            if (current_position.x < Screen.width * 0.5f)
                mainCamera.GetComponent<Transform>().Rotate(new Vector3(0, 0.05f * Time.deltaTime, 0));
            else
                mainCamera.GetComponent<Transform>().Rotate(new Vector3(0, -0.05f * Time.deltaTime, 0));
        }
        else
        {
            // 0 or 1 or at least 3 touch
            // 두개일때만 동작을 시작하고 끝맺을 것.
            distance_start = 0;
            distance_earlier = 0;
            old_position = Vector2.zero;
        }

        // Actions
        switch (touches[Region.Action].Count)
        {

            case 0:
                // 0 touch
                board_image.SetActive(false);
                sprint_panel.SetActive(false);
                point_set_needed = true;
                if (slope != 0f)
                {
                    board_image.transform.RotateAround(new Vector3(0, 0, -1), (float)Math.Atan(slope));
                    slope = 0f;
                }

                // Ali 동작을 실행하는 동안 clear 되는 것을 막아준다.
                if (skill == 1 && elapsed_time < Action.skill_duration)
                    break;

                // Already cleared
                if (primary_point.Equals(Vector2.zero) || secondary_point.Equals(Vector2.zero))
                    break;

                // clear
                Debug.Log("Clear");
                skill = 0;
                ali = false;
                bandit_animator.SetBool("Ali", false);
                bandit_animator.SetBool("Sprint", false);
                bandit_animator.SetBool("UpSideDown", false);
                bandit_animator.SetBool("FlipFlap", false);
                sprint = false;
                primary_point = Vector2.zero;
                secondary_point = Vector2.zero;
                break;

            case 1:

                if (primary_point == Vector2.zero
                    && secondary_point == Vector2.zero)
                {
                    return;
                    skill = 1;



                }

                // cool time
                if (skill == 1)
                    break;

                // primary가 떼진경우, Ali 인지 확인한다
                // secondary가 떼진경우, Sprint 인지 확인한다.
                foreach (DictionaryEntry d in touches[Region.Action])
                {
                    int finger_Id = (int)d.Key;
                    if (finger_Id == primary_point_finger_id)
                    {
                        // Sprint
                        Debug.Log("Sprint");
                        elapsed_time = 0f;
                        skill = 1; sprint = true;
                        sprint_panel.SetActive(true);
                    }
                    else if (finger_Id == secondary_point_finger_id)
                    {
                        // Ali
                        skill = 1;
                        elapsed_time = 0f;
                        if (aliable)
                        {
                            Debug.Log("aliable : true -> false");
                            aliable = false;
                            ali = true;
                            sprint_panel.SetActive(false);
                        }
                        else
                        {
                            Debug.Log("Ali command but not aliable");
                        }
                    }
                }

                break;
            case 2:

                if (skill == 1)
                {
                    if (ali)
                    {
                        // 이부분은 여러번 불리는데..
                        if (elapsed_time < Action.skill_duration)
                        {
                            // 아직 스킬 하는 중이면,
                            bandit_animator.SetBool("Ali", true);
                            playable_game_object.transform.Translate(0, 0.13f, 0);
                            action(Action.Ali);
                        }
                        else
                        {
                            elapsed_time = 0;
                            bandit_animator.SetBool("Ali", false);
                            skill = 0;
                        }
                    }
                    else if (sprint)
                    {
                        if (elapsed_time < Action.skill_duration)
                        {
                            bandit_animator.SetBool("Sprint", true);
                            velocity += Constants_.increase_speed;
                            action(Action.Sprint);
                        }
                        else
                        {
                            elapsed_time = 0;
                            bandit_animator.SetBool("Sprint", false);
                            skill = 0;
                        }
                    }
                    return;
                }

                // Game Start
                // start : static public int variable
                if (!start)
                    start = true;


                // 현재 터치 정보를 가져온다
                Vector2[] touch_point = new Vector2[3];
                i = 0;
                foreach (DictionaryEntry d in touches[Region.Action])
                {
                    touch_point[i++] = new Vector2(((Touch)d.Value).position.x, ((Touch)d.Value).position.y);
                }

                if (touch_point[0].x < touch_point[1].x)
                {
                    Vector2 temp = touch_point[0];
                    touch_point[0] = touch_point[1];
                    touch_point[1] = temp;
                    // 이제 touch_point[0] 는 오른쪽 터치 정보를 저장하고
                    // touch_point[1] 은 왼쪽 터치 정보를 저장하고 있다.
                }
                touch_point[2] = (touch_point[0] + touch_point[1]) * 0.5f;



                // primary, secondary 포인트 설정
                if (point_set_needed)
                {
                    primary_point = touch_point[0];
                    secondary_point = touch_point[1];
                    for (i = 0; i < Input.touchCount; i++)
                    {
                        // Finger Id 기록
                        Touch t = Input.GetTouch(i);
                        if (touch_point[0] == t.position)
                        {
                            primary_point_finger_id = t.fingerId;
                        }
                        if (touch_point[1] == t.position)
                        {
                            secondary_point_finger_id = t.fingerId;
                        }
                    }

                    mid_point = (primary_point + secondary_point) * 0.5f;
                    board_image.SetActive(true);
                    board_image.transform.position = mid_point;

                    // 기울기 갱신
                    board_image.transform.RotateAround(new Vector3(0, 0, -1), (float)Math.Atan(slope));
                    slope = (float)Math.Atan((primary_point.y - secondary_point.y) / (primary_point.x - secondary_point.x));
                    board_image.transform.RotateAround(new Vector3(0, 0, 1), (float)Math.Atan(slope));

                    point_set_needed = false;
                }




                // 패널은 안보이는게 맞지. turn 하는데.
                sprint_panel.SetActive(false);

                if (Vector2.Distance(mid_point, touch_point[2]) < 30f)
                {
                    Debug.Log("immediate return too close");
                    return;
                }

                float distance = Vector2.Distance(touch_point[0], touch_point[1]);

                if (distance < 300f)
                {
                    Debug.Log("SPRINT명령인데, left 되지 않게 리턴.");
                    return;
                }

                // 턴
                if (touch_point[2].x < mid_point.x)
                {
                    playable_game_object.transform.Rotate(0, -0.7f, 0);
                    playable_game_object.transform.Translate(0, 0, 0.03f);
                    velocity -= Constants_.decrease_speed_when_turn;
                    Debug.Log("turn left");
                }
                else
                {
                    Debug.Log("turn right");
                    playable_game_object.transform.Rotate(0, 0.7f, 0);
                    playable_game_object.transform.Translate(0, 0, 0.03f);
                    velocity -= Constants_.decrease_speed_when_turn;
                }

                break;

        }
    }

    private void scale(float x)
    {
        // Linear Scale
        if (x > 1f)
            mainCamera.transform.Translate(initialCameraPosition.magnitude * (-0.001f) * initialCameraPosition, targetObject.transform);
        else
            mainCamera.transform.Translate(initialCameraPosition.magnitude * (+0.001f) * initialCameraPosition, targetObject.transform);
    }

    private int Get_region(Touch t)
    {
        if (TopMost.Contains(t.position))
            return Region.Menu;
        else if (upper.Contains(t.position))
            return Region.Screen;
        else if (lower.Contains(t.position))
            return Region.Action;
        else
            return -1;
    }
    private static void action(int current_action)
    {
        // 각각 액션에 대해서, 보드가 할 행동을 여기서 바꿔주면 된다.
        switch (current_action)
        {
            case Action.Ali:

                if (elapsed_time * 4 < Action.skill_duration)
                    board_game_object.transform.Rotate(360 * Time.deltaTime * Action.num_turn * 0.1f, 0f, 0f);
                else if (elapsed_time * 4 < Action.skill_duration * 3)
                    board_game_object.transform.Rotate(-360 * Time.deltaTime * Action.num_turn * 0.1f, 0f, 0f);
                else
                    board_game_object.transform.Rotate(360 * Time.deltaTime * Action.num_turn * 0.1f, 0f, 0f);
                break;

            case Action.FlipFlap:

                board_game_object.transform.Rotate(0f, 0f, 360 * Time.deltaTime * Action.num_turn);
                break;

            case Action.Sprint:
                // No action
                
                break;
            case Action.UpSideDown:
                if (elapsed_time * 4 < Action.skill_duration)
                    bandit_game_object.transform.Rotate(0f, 360 * Time.deltaTime * Action.num_turn * 0.1f, 0f);
                else if (elapsed_time * 4 > 3 * Action.skill_duration)
                    bandit_game_object.transform.Rotate(0f, -360 * Time.deltaTime * Action.num_turn * 0.1f, 0f, 0f);
                break;

        }
    }
}
