﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstrainAxes : MonoBehaviour
{
	/// <summary>
	/// Normal of the plane to constrain movement to. Use Rox axes and the appropriate Unity axes will be constrained
	/// </summary>
	public RigidbodyConstraints constraint;
	public string tooltip = "Constrain...";
	[System.NonSerialized]
	public Color color;
	[System.NonSerialized]
	public bool active;

	Outline outline;


	void Awake ()
	{
		color = GetComponent<Image> ().color;
		outline = GetComponent<Outline> ();
		outline.enabled = false;
		active = false;
	}

	void Update ()
	{
		if ( QuadMotor.ActiveController != null )
		{
			Rigidbody rb = QuadMotor.ActiveController.rb;
			RigidbodyConstraints rbc = rb.constraints;

			active = outline.enabled = ( rbc & constraint ) != 0;
		}
	}

	public void OnClick ()
	{
		if ( QuadMotor.ActiveController != null )
		{
			Rigidbody rb = QuadMotor.ActiveController.rb;
			RigidbodyConstraints rbc = rb.constraints;

			if ( active )
			{
				rbc ^= constraint;
//				outline.enabled = false;
//				active = false;

			} else
			{
				rbc |= constraint;
//				outline.enabled = true;
//				active = true;
			}
			rb.constraints = rbc;
			QuadMotor.ActiveController.UpdateConstraints ();

			QuadMotor.ActiveController.TriggerReset ();
			QuadMotor.ActiveController.ApplyMotorForce ( Vector3.zero );
			QuadMotor.ActiveController.ApplyMotorTorque ( Vector3.zero );
		}
	}

	public void OnMouseEnter ()
	{
		if ( ConstrainTooltip.instance != null )
			ConstrainTooltip.instance.Show ( this );
	}

	public void OnMouseExit ()
	{
		if ( ConstrainTooltip.instance != null )
			ConstrainTooltip.instance.Hide ();
	}
}