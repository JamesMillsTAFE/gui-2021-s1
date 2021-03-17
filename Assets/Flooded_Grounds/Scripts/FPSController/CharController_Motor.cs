using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController_Motor : MonoBehaviour 
{
	private bool active = false;

	public float speed = 10.0f;
	public float sensitivity = 30.0f;
	public float WaterHeight = 15.5f;
	CharacterController character;
	public GameObject cam;
	float moveFB, moveLR;
	float rotX, rotY;
	public bool webGLRightClickRotation = true;
	float gravity = -9.8f;


	public void Initialise()
	{
		//LockCursor ();
		active = true;
		SetCursorState(false);
		character = GetComponent<CharacterController> ();
		if (Application.isEditor) 
		{
			webGLRightClickRotation = false;
			sensitivity = sensitivity * 1.5f;
		}
	}

	/// <summary>
	/// Enables or disables the cursor and also changes its visiblity.
	/// </summary>
	/// <param name="_active">Whether or not the cursor should be enabled.</param>
	public void SetCursorState(bool _active)
    {
		cam.GetComponent<Camera>().enabled = !_active;
		Cursor.visible = _active;
		if(_active)
        {
			// The state of the cursor, Locked means it can't move from the centre of the screen
			// But mouse input still works
			Cursor.lockState = CursorLockMode.Locked;
        }
		else
        {
			Cursor.lockState = CursorLockMode.None;
        }
    }

	void CheckForWaterHeight()
	{
		if (transform.position.y < WaterHeight) 
		{
			gravity = 0f;			
		} 
		else 
		{
			gravity = -9.8f;
		}
	}

	void Update()
	{
		// If we aren't active, ignore this update
		if (!active)
		{
			SetCursorState(true);
			return;
		}

		moveFB = Input.GetAxis ("Horizontal") * speed;
		moveLR = Input.GetAxis ("Vertical") * speed;

		rotX = Input.GetAxis ("Mouse X") * sensitivity;
		rotY = Input.GetAxis ("Mouse Y") * sensitivity;

		//rotX = Input.GetKey (KeyCode.Joystick1Button4);
		//rotY = Input.GetKey (KeyCode.Joystick1Button5);

		CheckForWaterHeight ();


		Vector3 movement = new Vector3 (moveFB, gravity, moveLR);



		if (webGLRightClickRotation) 
		{
			if (Input.GetKey (KeyCode.Mouse0)) 
			{
				CameraRotation (cam, rotX, rotY);
			}
		} 
		else if (!webGLRightClickRotation) 
		{
			CameraRotation (cam, rotX, rotY);
		}

		movement = transform.rotation * movement;
		character.Move (movement * Time.deltaTime);
	}


	void CameraRotation(GameObject cam, float rotX, float rotY)
	{		
		transform.Rotate (0, rotX * Time.deltaTime, 0);
		cam.transform.Rotate (-rotY * Time.deltaTime, 0, 0);
	}




}
