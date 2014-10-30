using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour 
{
	public float speed;
	public float range;

	public CharacterController controller;
	public Transform player;
	public LevelSystem playerLevel;
	private Fighter opponent;
	
	public AnimationClip attackClip;
	public AnimationClip run;
	public AnimationClip idle;
	public AnimationClip die;

	public double impactTime = 0.36;

	public int maxHealth;
	public int  health;
	public int damage;

	private bool impacted;

	private int stunTime;

	// Use this for initialization
	void Start () 
	{
		health = maxHealth;
		opponent = player.GetComponent<Fighter> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isDead ())
		{
			if(stunTime<=0)
			{
				if (!inRange ()) 
				{
					chase ();
				}
				else 
				{
					animation.Play(attackClip.name);
					attack();

					if(animation[attackClip.name].time>0.9*animation[attackClip.name].length)
					{
						impacted = false;
					}
				}
			}
			else
			{

			}
		}
		else
		{
			dieMethod();
		}
	}

	void attack()
	{
		if (animation [attackClip.name].time > animation [attackClip.name].length * impactTime&&!impacted&&animation[attackClip.name].time<0.9*animation[attackClip.name].length) 
		{
			opponent.getHit(damage);
			impacted = true;
		}
	}

	bool inRange()
	{
		if(Vector3.Distance(transform.position, player.position)<range)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void getHit(double damage)
	{
		health = health - (int)damage;

		if(health<0)
		{
			health = 0;
		}
	}

	public void getStun(int seconds)
	{
		CancelInvoke("stunCountDown");
		stunTime += seconds +1;
		InvokeRepeating("stunCountDown", 0f, 1f);
	}

	void stunCountDown()
	{
		Debug.Log(stunTime);
		stunTime = stunTime - 1;

		if(stunTime==0)
		{
			CancelInvoke("stunCountDown");
		}
	}


	void chase()
	{
		transform.LookAt(player.position);
		controller.SimpleMove(transform.forward*speed);
		animation.CrossFade(run.name);
	}

	void dieMethod()
	{
		animation.Play (die.name);

		if(animation[die.name].time>animation[die.name].length*0.9)
		{
			playerLevel.exp = 	playerLevel.exp + 100;
			Destroy(gameObject);
		}
	}

	bool isDead()
	{
		if (health <= 0)
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	void OnMouseOver()
	{
		player.GetComponent<Fighter>().opponent = gameObject;
	}
}
