using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour 
{
	public float speed;
	public CharacterController controller;
	private Vector3 position;

	public AnimationClip run;
	public AnimationClip idle;

	public static bool attack;
	public static bool die;

	public static Vector3 cursorPosition;

	public static bool busy;

	// Use this for initialization
	void Start () 
	{
		position = transform.position;
		busy = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!busy)
		{
			locateCursor();
			if(!attack&&!die)
			{
				if(Input.GetMouseButton(0))
				{
					//Locate where the player clicked on the terrain
					locatePosition();
				}

				//Move the player to the position
				moveToPosition();
			}
			else
			{
			}
		}
	}

	void locatePosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 1000))
		{
			if(hit.collider.tag!="Player"&&hit.collider.tag!="Enemy")
			{
				position = hit.point;
			}
		}
	}

	void locateCursor()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if(Physics.Raycast(ray, out hit, 1000))
		{
			cursorPosition = hit.point;
		}
	}

	void moveToPosition()
	{
		//Game Object is moving
		if(Vector3.Distance(transform.position, position)>1)
		{
			Quaternion newRotation = Quaternion.LookRotation(position-transform.position, Vector3.forward);

			newRotation.x = 0f;
			newRotation.z = 0f;

			transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10);
			controller.SimpleMove(transform.forward * speed);

			animation.CrossFade(run.name);
		}
		//Game Object is not moving
		else
		{
			animation.CrossFade(idle.name);
		}
	}

}
