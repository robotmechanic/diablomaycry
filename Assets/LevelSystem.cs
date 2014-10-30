using UnityEngine;
using System.Collections;

public class LevelSystem : MonoBehaviour 
{
	//We need 100 exp to ding up every level

	public int level;
	public int exp;
	public Fighter player;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		LevelUp ();
	}

	void LevelUp()
	{
		if(exp>=Mathf.Pow(level, 2) + 100)
		{
			exp = exp - (int)(Mathf.Pow(level, 2) + 100);
			level = level + 1;
			levelEffect();
		}
	}

	void levelEffect()
	{
		player.maxHealth = player.maxHealth + 100;
		player.damage = player.damage + 50;
	}
}
