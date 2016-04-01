using UnityEngine;
using System.Collections;

public class MagicHandler : MonoBehaviour {

	float CLICKMAXTIME = .2f;
	float TOUCHSKIN = 1f;

	public GameObject Soul;
	public GameObject Thunder;
	Player player;

	public AudioClip soulStealSound;

	float SoulFactor = .25f;
	float SoulMin = .45f;

	public bool active;
	
	bool mouseHold;
	float mouseHoldTime;
	GameObject interactObject;
	float soulAbsortion;
	GameObject soulInstance;
	Vector3 initTouch;

	
	
	void Start () {
		active = true;
		initTouch = new Vector3(0, 0, 0);
		player = transform.parent.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if(active){
			//EVENT - MOUSE DOWN
			if (Input.GetMouseButtonDown(0)){
				//Debug.Log("Inicio");
				//Initiate mouseHold
		  		mouseHold = true;
		  		mouseHoldTime = 0;  

		  		//Raycast to find interacted object
		        RaycastHit hit;	  
		        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    	if (Physics.Raycast(ray, out hit)){
					//Debug.Log(hit.collider.gameObject.name);
					interactObject = hit.collider.gameObject;
					initTouch = hit.point;

				}

			//EVENT - MOUSE HOLD
		 	}else if (Input.GetMouseButton(0) && mouseHold){
		 		//Debug.Log("Segurando");
		 		//Apply deltaTime to mouseHold
		 		mouseHoldTime += Time.deltaTime;

		 		//Check if touch moved away from the original point
		 		RaycastHit hit;	  
		        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		    	if (Physics.Raycast(ray, out hit)){
					if(Mathf.Abs(Vector3.Distance(hit.point,initTouch)) > TOUCHSKIN){
						mouseHold = false;
						//End of hold
						ReleaseSoul(interactObject);
					}
				}

				//Check if it was a hold
		 		if(mouseHoldTime > CLICKMAXTIME && mouseHold){
		 			//Holdou!		 	
		 			//Check which relevant object is being hold
					if(interactObject != null){		 			
						if(interactObject.tag == "Enemy"){
			 				//Steal it's soul!!!
			 				StealSoul(interactObject);
			 			}
		 			}	
		 		}

		 	//Event - MOUSE UP
		 	}else if (Input.GetMouseButtonUp(0)){
		 		//End mouseHold

		 		//Check if it was a click
		 		if(mouseHoldTime <= CLICKMAXTIME){
		 			//Clicked!
		 			//Check which relevant object was clicked
		 			if(interactObject != null){	
			 			if(interactObject.tag == "Enemy" && interactObject.GetComponent<Enemy>().active){
			 				//Choke it!
			 				Choke(interactObject);
			 			}
		 			}
		 		}else if(mouseHold){
		 			//End of hold
					ReleaseSoul(interactObject);
		 		}

		 		mouseHold = false;    
		 	}

		 	//Mudar direcao
//		 	Debug.Log(initTouch.x);
		 	if(initTouch.x < 0){
		 		player.ChangeSide(false);
	 		}else{
				player.ChangeSide(true);
	 		}

	 	}
	}

	void Choke(GameObject enemy){
		Enemy enScript = enemy.GetComponent<Enemy>();
		enScript.Choke();
		player.Hit();
		GameObject thunder = Instantiate(Thunder, enemy.transform.position, Quaternion.identity) as GameObject;
		//SoundController.instance.PlayDelayedSingle(chokeSound, .2f);
		thunder.GetComponent<AudioSource>().mute = SoundController.instance.mute;
		Destroy(thunder, .2f);
	}

	void StealSoul(GameObject enemy){
		Enemy enScript = enemy.GetComponent<Enemy>();
		

		if(enScript.active){
			if(enScript.state != Enemy.EnemyStates.SoulStunned){
				SoundController.instance.PlayLoop(soulStealSound, transform.GetComponent<AudioSource>());
				player.StealStart();
				enScript.StealSoul();
				soulAbsortion = 0;
				soulInstance = Instantiate(Soul, enemy.transform.position, Quaternion.identity) as GameObject;
			}else{
				soulAbsortion += Time.deltaTime;
			}
			
			float soulTotal = enScript.life * enScript.soulResistance * SoulFactor + SoulMin;
			float soulAbsPercent = soulAbsortion/soulTotal;
			//Debug.Log(soulAbsPercent + " wow");
	
			//Ver se já absorveu tudo
			if(soulAbsPercent >= 1){
				//SUgou!
				//Debug.Log("Terminou");
				SoundController.instance.StopLoop(transform.GetComponent<AudioSource>());
				Destroy(soulInstance);
				enScript.Die();
				mouseHold = false;
				player.StealSuccess();
				//Ganhar alma
				GameObject.Find("Pentagrama").GetComponent<Pentagram>().GatherSoul();
			}else{
				//Keep Sugando. Fazer Lerp da alma
				Vector3 target = new Vector3(0,0,0);
				soulInstance.transform.position = Vector3.Lerp(enemy.transform.position, target ,soulAbsPercent);
			}
	
			//Debug.Log(enScript.state);
		}


	}

	void ReleaseSoul(GameObject enemy){
		if(enemy != null){
			Enemy enScript = enemy.GetComponent<Enemy>();
			if(enScript != null)
				enScript.ReleaseSoul();
		}
		SoundController.instance.StopLoop(transform.GetComponent<AudioSource>());
		mouseHold = false;		
		player.StealEnd();
		if(soulInstance != null){
			Destroy(soulInstance);
		}

	}
}
