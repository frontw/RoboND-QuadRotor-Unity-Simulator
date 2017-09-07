﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleQuadController : MonoBehaviour
{
	public Vector3 LastTargetPoint { get { return lastTargetPoint; } }

	public static SimpleQuadController ActiveController;
	public Transform chassis;
	public Transform camTransform;
	public QuadMotor controller;
	public FollowCamera followCam;
	public PathFollower pather;
	public GimbalCamera gimbal;
	public TargetFollower follower;

	public float moveSpeed = 10;
	public float thrustForce = 25;
	public float maxTilt = 22.5f;
	public float tiltSpeed = 22.5f;
	public float turnSpeed = 90;
	public StateController stateController;

	Rigidbody rb;
	float tiltX;
	float tiltZ;

	public bool localInput;
	Vector3 lastTargetPoint;
	
	void Awake ()
	{
		ActiveController = this;
		rb = GetComponent<Rigidbody> ();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		if ( controller == null )
			controller = GetComponent<QuadMotor> ();
		if ( followCam == null )
			followCam = camTransform.GetComponent<FollowCamera> ();

		stateController.SetState ( "Patrol" );
	}

	void LateUpdate ()
	{
		if ( Input.GetKeyDown ( KeyCode.F12 ) )
		{
			if ( stateController.IsCurrentStateName ( "Local" ) )
				stateController.RevertState ();
			else
				stateController.SetState ( "Local" );
				
		}
	}

/*	void LateUpdate ()
	{
		if ( Input.GetKeyDown ( KeyCode.F12 ) )
		{
			active = !active;
			if ( active )
			{
				controller.UseGravity = false;
				controller.rb.isKinematic = true;
				controller.rb.isKinematic = false;
				controller.rb.freezeRotation = true;
			} else
				controller.rb.freezeRotation = false;
		}

		if ( Input.GetKeyDown ( KeyCode.R ) )
		{
			controller.ResetOrientation ();
			followCam.ChangePoseType ( CameraPoseType.Iso );
		}

		if ( Input.GetKeyDown ( KeyCode.G ) )
		{
			controller.UseGravity = !controller.UseGravity;
		}

		if ( !active )
			return;
		
		Vector3 input = new Vector3 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Thrust" ), Input.GetAxis ( "Vertical" ) );

		Vector3 inputVelo = new Vector3 ( input.x * moveSpeed, input.y * thrustForce, input.z * moveSpeed );
//		Vector3 forwardVelocity = Vector3.forward * input.z * moveSpeed;
//		Vector3 sidewaysVelocity = Vector3.right * input.x * moveSpeed;
//		Vector3 upVelocity = Vector3.up * input.y * thrustForce;
//		Vector3 inputVelo = forwardVelocity + sidewaysVelocity + upVelocity;

		Vector3 forward = transform.forward;
//		Vector3 forward = transform.forward - transform.right;
		forward.y = 0;
		Quaternion rot = Quaternion.LookRotation ( forward.normalized, Vector3.up );

//		rb.AddRelativeForce ( chassis.rotation * inputVelo * Time.deltaTime, ForceMode.VelocityChange );
		rb.velocity = rot * inputVelo;
//		transform.Rotate ( Vector3.up * input.x * thrustForce * Time.deltaTime, Space.World );

//		float x = input.z / 2 + input.x / 2;
//		float z = input.z / 2 - input.x / 2;
		Vector3 euler = transform.localEulerAngles;
		euler.x = maxTilt * input.z;
		euler.z = maxTilt * -input.x;
		transform.localEulerAngles = euler;

		float yaw = Input.GetAxis ( "Yaw" );
		if ( yaw != 0 )
		{
			transform.Rotate ( Vector3.up * yaw * turnSpeed * Time.deltaTime, Space.World );
			camTransform.Rotate ( Vector3.up * yaw * turnSpeed * Time.deltaTime, Space.World );
		}

//		if ( Input.GetKeyDown ( KeyCode.P ) )
//		{
//			PathPlanner.AddNode ( controller.Position, controller.Rotation );
//		}
//		if ( Input.GetKeyDown ( KeyCode.O ) )
//		{
//			controller.ResetOrientation ();
//			pather.SetPath ( new Pathing.Path ( PathPlanner.GetPath () ) );
//			PathPlanner.Clear ( false ); // clear the path but keep the visualization
//		}
//		if ( Input.GetKeyDown ( KeyCode.I ) )
//		{
//			PathPlanner.Clear ();
//		}
	}*/

	void OnGUI ()
	{
		GUI.backgroundColor = localInput ? Color.green : Color.red;
//		GUI.contentColor = Color.white;
		Rect r = new Rect ( 10, Screen.height - 100, 60, 25 );
		if ( GUI.Button ( r, "Input " + ( localInput ? "on" : "off" ) ) )
		{
			localInput = !localInput;
		}
	}

	public void OnTargetDetected (Vector3 point)
	{
		lastTargetPoint = point;
		stateController.SetState ( "Follow" );
	}

	public void OnTargetLost ()
	{
		stateController.SetState ( "Patrol" );
	}
}