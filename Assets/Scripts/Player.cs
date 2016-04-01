using UnityEngine;
using System.Collections;

public class Player : IsometricObject {

	public int lifes = 3;
	public AudioClip damageSound;

	GameInterface gameInterface;
	GameObject spriteScaler;
	Animator animator;
	ParticleSystem soulGather;

	void Awake(){
		
	}

	// Use this for initialization
	void Start () {
		base.active = true;
		spriteScaler = transform.GetComponentInChildren<SpriteRenderer>().transform.parent.gameObject;
		animator = transform.GetComponentInChildren<Animator>();
		gameInterface = GameObject.Find("GameInterface").GetComponent<GameInterface>();
		soulGather = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if(base.active){
			if(col.gameObject.tag == "Enemy"){
				col.GetComponent<Enemy>().Kill();
			}
		}
	}

	
	public void Hit(){
		animator.SetTrigger("Hit");
	}

	public void StealStart(){
		animator.SetBool("Steal", true);
	}

	public void StealEnd(){
		animator.SetBool("Steal", false);
	}

	public void StealSuccess(){
		StealEnd();
		soulGather.Play();
	}

	public void ChangeSide(bool EhDir){
		if(EhDir){
			spriteScaler.transform.localScale = new Vector3(-1,1,1);
		}else{
			spriteScaler.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void ReceiveDamage(){
		if(!gameInterface.wingame){
			lifes--;
			gameInterface.UpdateLife(lifes);
			SoundController.instance.PlaySingle(damageSound);
			if(lifes <= 0){
				Die();
			}else{
				animator.SetTrigger("Damage");
			}
		}
	}

	void Die(){
		animator.SetTrigger("Death");
		base.active = false;
		StartCoroutine("EndGame");
	}



	IEnumerator EndGame(){
 		yield return new WaitForSeconds(1.3f);
 		gameInterface.FinishGame();
 		//Application.LoadLevel(0);	
 	}
}
