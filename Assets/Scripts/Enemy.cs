using UnityEngine;
using System.Collections;

public class Enemy : IsometricObject {

	public enum EnemyStates {Pursuing, ChokeStunned, SoulStunned, Attacking, Dying}

	public float speed = 1;
	public float life = 3;
	public float soulResistance=1;
	public float stunDuration = .03f;

	public AudioClip damageSound;
	public AudioClip deathSound;

	SpriteRenderer spriteRend;
	Animator animator;

	public EnemyStates state;
	Vector3 target;
	Vector3 dir;
	Vector3 vel;
	float stunTime;
	float soulAbsortion;
	int animState;
	int animState2;

	// Use this for initialization
	void Awake(){
		
	}

	void Start () {
		base.active = true;
		state = EnemyStates.Pursuing;
		spriteRend = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();
		animator.SetInteger("Dir",0);
		animator.SetInteger("Action",0);
		animState2 = 0;
		//Debug.Log(transform.position);

		//Rotate sprite
		float hip = Mathf.Sqrt(Mathf.Pow(vel.y,2) + Mathf.Pow(vel.x,2));
		if(transform.position.z<0){
			if(transform.position.x/hip >= .5f){
				animState = 5;				
				//Debug.Log("DIR 5");
			}else if(transform.position.x/hip <= -.5f){
				animState = 1;				
				//Debug.Log("DIR 1");
			}else{
				animState = 0;				
				//Debug.Log("DIR 0");
			}	
		}else{
			if(transform.position.x/hip >= .5f){
				animState = 4;				
				//Debug.Log("DIRB 4");
			}else if(transform.position.x/hip <= -.5f){
				animState = 2;				
				//Debug.Log("DIRB 2");
			}else{
				animState = 3;
				//Debug.Log("DIRB 3");
			}	
		}
		animator.SetInteger("Dir",animState);
	}
	
	// Update is called once per frame
	void Update () {
		if(base.active){
			animator.SetInteger("Dir",animState);
			animator.SetInteger("Action",animState2);
			switch(state){
				case EnemyStates.Pursuing:
					//Perseguir player
					target = new Vector3(0,0,0);
					dir = target - transform.position;
					dir = dir.normalized;
		
					vel = dir * speed * Time.deltaTime;
		
					transform.Translate(vel);


					break;
				case EnemyStates.ChokeStunned:		
					//Aplicar stun temporario e voltar a perseguir
					stunTime += Time.deltaTime;
					if(stunTime >= stunDuration){
						state = EnemyStates.Pursuing;
						animState2 = 0;
						spriteRend.color = new Color(1f,1f,1f,1f);
					}
					break;
			}
		}
	}

	public void Choke(){
		state = EnemyStates.ChokeStunned;
		
		spriteRend.color = new Color(.9f, 1f, 0f, 1f);
		life--;
		stunTime = 0;
		if(life<=0){
			Die();
		}else{
			SoundController.instance.PlaySingle(damageSound);
			animState2 = 1;
		}
	}

	public void StealSoul(){
		if(state != EnemyStates.SoulStunned){
			animState2 = 3;
			state = EnemyStates.SoulStunned;
			spriteRend.color = new Color(.19f,.73f,1f,1f);
		}

	}

	public void ReleaseSoul(){
		state = EnemyStates.Pursuing;
		animState2 = 0;
		spriteRend.color = new Color(1f,1f,1f,1f);
	}

	public void Kill(){
		state = EnemyStates.Attacking;
		animState2 = 4;
		StartCoroutine("Attack");
	}

	IEnumerator Attack(){
		yield return new WaitForSeconds(.7f);
		if(base.active){
			GameObject.Find("Mage").GetComponent<Player>().ReceiveDamage();
			Die();
		}
	}

	public void Die(){
		Debug.Log("Morrendo");
		if(base.active)
			SoundController.instance.PlaySingle(deathSound);
		base.active = false;
		state = EnemyStates.Dying;
		animState2 = 2;
		Destroy(gameObject, 0.5f);
	}	


}
