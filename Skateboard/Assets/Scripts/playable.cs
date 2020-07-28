using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class playable : MonoBehaviour
{
    GameObject bandit;
    GameObject skateboard;
    Rigidbody bandit_rigid_body;
    Rigidbody skateboard_rigid_body;


    // Start is called before the first frame update
    void Start()
    {
        bandit = GameObject.Find("bandit");
        skateboard = GameObject.Find("skateboard");
        bandit_rigid_body = bandit.GetComponent<Rigidbody>();
        skateboard_rigid_body = skateboard.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collison nowo aliable is true");
        Local_Frame.aliable = true;
        if (collision.collider.gameObject.tag.Equals("obstacle")) ;
    }

}
