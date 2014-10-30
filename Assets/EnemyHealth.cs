using UnityEngine;
using System.Collections;
//1280 * 800
public class EnemyHealth : MonoBehaviour 
{
	public Fighter player;
	public Texture2D frame;
	public Rect framePosition;

	public float horizontalDistance;
	public float verticalDistance;
	public float width;
	public float height;
	public Texture2D healthBar;
	public Rect healthBarPosition;

	public Mob target;
	public float healthPercentage;

	void Start () 
	{
	
	}
	
	void Update () 
	{
		if(player.opponent!=null)
		{
			target = player.opponent.GetComponent<Mob> ();
			healthPercentage = (float)target.health / (float)target.maxHealth;
		}
		else
		{
			target = null;
			healthPercentage = 0;
		}
	}

	void OnGUI()
	{
		if(target!=null&&player.countDown>0)
		{
			drawFrame ();
			drawBar ();
		}
	}

	void drawFrame()
	{
		framePosition.x = (Screen.width  - framePosition.width) / 2;
		framePosition.width = Screen.width * 0.39f;
		framePosition.height = Screen.height * 0.0625f;
		GUI.DrawTexture (framePosition, frame);
	}

	void drawBar()
	{
		healthBarPosition.x = framePosition.x + framePosition.width * horizontalDistance;
		healthBarPosition.y = framePosition.y + framePosition.height * verticalDistance;
		healthBarPosition.width = framePosition.width*width*healthPercentage;
		healthBarPosition.height = framePosition.height*height;

		GUI.DrawTexture (healthBarPosition, healthBar);
	}

}
