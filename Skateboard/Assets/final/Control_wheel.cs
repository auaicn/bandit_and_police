using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_wheel : MonoBehaviour

{
    public Transform[] wheels;
    public float motorPower = 15.0f;
    public float maxTurn = 2.0f;

    float instantePower = 0.0f;
    float brake = 0.0f;
    float wheelTurn = 0.0f;
    Rigidbody myRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = this.gameObject.GetComponent<Rigidbody>();
        myRigidbody.centerOfMass=new Vector3(0,0,0);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        instantePower = Input.GetAxis("Vertical")*motorPower*Time.deltaTime;
        wheelTurn=Input.GetAxis("Horizontal")*maxTurn;
        brake=Input.GetKey("space")?myRigidbody.mass*0.1f :0.0f;

        getCollider(0).steerAngle = wheelTurn;
        getCollider(1).steerAngle = wheelTurn;

        wheels[0].localEulerAngles = new Vector3(wheels[0].localEulerAngles.x,getCollider(0).steerAngle - wheels[0].localEulerAngles.z,
        wheels[0].localEulerAngles.z);
        wheels[1].localEulerAngles = new Vector3(wheels[1].localEulerAngles.x,getCollider(1).steerAngle - wheels[1].localEulerAngles.z,
        wheels[1].localEulerAngles.z);

        wheels[0].Rotate(-getCollider(0).rpm/360*Time.deltaTime,0,0);
        wheels[1].Rotate(-getCollider(1).rpm/360*Time.deltaTime,0,0);
        wheels[2].Rotate(-getCollider(2).rpm/360*Time.deltaTime,0,0);
        wheels[3].Rotate(-getCollider(3).rpm/360*Time.deltaTime,0,0);

        if(brake > 0.0f){
            getCollider(0).brakeTorque = brake;
                        getCollider(1).brakeTorque = brake;
            getCollider(2).brakeTorque = brake;
            getCollider(3).brakeTorque = brake;
            getCollider(2).motorTorque = 0.0f;
            getCollider(3).motorTorque = 0.0f;

        }
        else{
            getCollider(0).brakeTorque = 0.0f;
            getCollider(1).brakeTorque = 0.0f;
            getCollider(2).brakeTorque = 0.0f;
            getCollider(3).brakeTorque = 0.0f;
            getCollider(2).motorTorque = instantePower;
            getCollider(3).motorTorque = instantePower;

        }


    }
    WheelCollider getCollider(int n){
        return wheels[n].gameObject.GetComponent<WheelCollider>();
    }
}
