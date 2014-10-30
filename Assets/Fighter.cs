using UnityEngine;
using System.Collections;

public class Fighter : MonoBehaviour 
{
	public CharacterController controller;
	public GameObject opponent;

	public AnimationClip attack;
	public AnimationClip dieClip;

	public int maxHealth;
	public int health;
	public int damage;

	private double impactLength;

	public double impactTime;
	public bool impacted;
	public bool inAction;

	public float range;

	bool started;
	bool ended;

	public float combatEscapeTime;

	public float countDown;

	public bool specialAttack;

	// Use this for initialization
	void Start () 
	{
		health = maxHealth;
		impactLength = (animation[attack.name].length*impactTime);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space) && !specialAttack)
		{
			inAction = true;
		}

		if(inAction)
		{
			if(attackFunction(0, 1, KeyCode.Space, null, 0, true))
			{
				
			}
			else
			{
				inAction = false;
			}
		}

		die ();
	}

	public bool attackFunction(int stunSeconds, double scaledDamage, KeyCode key, GameObject particleEffect, int projectile, bool opponenBased)
	{
		if(opponenBased)
		{
			if(Input.GetKey(key)&&inRange())
			{
				animation.Play(attack.name);
				ClickToMove.attack = true;
				
				if(opponent!=null)
				{
					transform.LookAt(opponent.transform.position);
				}
			}
		}
		else
		{
			if(Input.GetKey(key))
			{
				animation.Play(attack.name);
				ClickToMove.attack = true;
				transform.LookAt(ClickToMove.cursorPosition);
			}
		}
		
		if(animation[attack.name].time>0.9*animation[attack.name].length)
		{
			ClickToMove.attack = false;
			impacted = false;
			if(specialAttack)
			{
				specialAttack = false;
			}
			return false;
		}
		impact(stunSeconds, scaledDamage, particleEffect, projectile, opponenBased);
		return true;
	}


	public void resetAttackFunction()
	{
		ClickToMove.attack = false;
		impacted = false;
		animation.Stop(attack.name);
	}

	void impact(int stunSeconds, double scaledDamage, GameObject particleEffect, int projectile, bool opponenBased)
	{
		if((!opponenBased || opponent!=null)&&animation.IsPlaying(attack.name)&&!impacted)
		{
			if((animation[attack.name].time)>impactLength&&(animation[attack.name].time<0.9*animation[attack.name].length))
			{
				countDown = combatEscapeTime + 2;
				CancelInvoke("combatEscapeCountDown");
				InvokeRepeating("combatEscapeCountDown", 0, 1);
				if(opponenBased)
				{
					opponent.GetComponent<Mob>().getHit((damage*scaledDamage));
					opponent.GetComponent<Mob>().getStun(stunSeconds);
				}
				//send out spherers
				Quaternion rot = transform.rotation;
				rot.x = 0f;
				rot.z = 0f;

				if(projectile>0)
				{
					Instantiate(Resources.Load("Projectile"), new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), rot);
				}
				//Play the particle effect
				if(particleEffect!=null)
				{
					Instantiate(particleEffect, new Vector3(opponent.transform.position.x, opponent.transform.position.y + 1.5f, opponent.transform.position.z), Quaternion.identity);
				}
					impacted = true;
			}
		}
	}

	void combatEscapeCountDown()
	{
		countDown = countDown - 1;
		if(countDown == 0)
		{
			CancelInvoke("combatEscapeCountDown");
		}
	}

	public void getHit(int damage)
	{
		health = health - damage;
		if (health < 0) 
		{
			health = 0;
		}
	}

	bool inRange()
	{
		if (Vector3.Distance (opponent.transform.position, transform.position) <= range) 
		{
			return true;
		} 
		else
		{
			return false;
		}
	}

	//If dead returns true or else returns false
	public bool isDead()
	{
		if (health == 0) 
		{
			return true;
		} 
		else
		{
			return false;
		}
	}

	void die()
	{
		if (isDead ()&&!ended)
		{
			if(!started)
			{
				ClickToMove.die = true;
				animation.Play(dieClip.name);
				started = true;
			}

			if(started&&!animation.IsPlaying(dieClip.name))
			{
				//What ever you want to do
				Debug.Log("You have died");
				health = 100;

				ended = true;
				started = false;
				ClickToMove.die =false;

			}
		}
	}
}
