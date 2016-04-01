 using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameInterface : MonoBehaviour {

	Hud hud;
	PauseScreen pauseScreen;
	GameOverScreen gameOverScreen;

	public AudioClip gameMusic;
	public AudioClip endMusic;
	public AudioClip popSound;
	Player player;
	MagicHandler magicHandler;
	public Sprite[] hearts;

	ArrayList pausedObjs;
	bool reactivateMagicHandler;
	bool gameover = false;
	public bool wingame = false;

	void Start () {
		//Inicializar
		hud = new Hud();
		pauseScreen = new PauseScreen();
		gameOverScreen = new GameOverScreen();

		//Pegar referências
		hud.GetRefs(gameObject);
		pauseScreen.GetRefs(gameObject);
		gameOverScreen.GetRefs(gameObject);

		hud.hud.SetActive(true);
		pauseScreen.pauseScreen.SetActive(false);
		gameOverScreen.gameOverScreen.SetActive(false);

		SoundController.instance.PlayLoop(gameMusic, transform.GetComponent<AudioSource>());
	}
	
	void Update () {
		//hud.UpdateFrame(player, magicHandler, hearts);
		if(Input.GetKey(KeyCode.W)){
			WinGame();
		}
	}

	public void UpdateLife(int life){
		hud.UpdateLife(life, hearts);
	}

	public void UpdateSouls(int souls){
		hud.UpdateSouls(souls);
	}

	public void StartPause(){
		//Pausar tudo
		//Debug.Log("Tranquile");
		if(!gameover)
			SoundController.instance.PlaySingle(popSound);

		pausedObjs = new ArrayList();
		//Desligar cada IsometricObject ligado
		foreach(IsometricObject obj in GameObject.FindObjectsOfType<IsometricObject>() as IsometricObject[]){
			if(obj.active){
				//Desligar e armazena-lo
				pausedObjs.Add(obj);
				obj.active = false;
				obj.gameObject.GetComponentInChildren<Animator>().enabled = false;
			}
		}

		//Desligar MagicHandler, se estiver ligado
		MagicHandler mh = Object.FindObjectOfType<MagicHandler>();
		if(mh.active){
			//Desativa-lo e  marcar pra dps
			reactivateMagicHandler = true;
			mh.active = false;
		}else{
			reactivateMagicHandler = false;	
		}

		//Desligar Spawner
		Object.FindObjectOfType<EnemySpawner>().active = false;

		//Desligar botao de pause
		GameObject.Find("PauseButton").GetComponent<UnityEngine.UI.Button>().interactable = false;

		//Aparecer tela de pause
		pauseScreen.pauseScreen.SetActive(true);

	}

	public void EndPause(){
		//Debug.Log("Favorable");
		SoundController.instance.PlaySingle(popSound);

		//Religar cada IsometricObject desligado
		foreach(IsometricObject obj in pausedObjs){
			//Religar
			obj.active = true;
			obj.gameObject.GetComponentInChildren<Animator>().enabled = true;
		}

		//Religar MagicHandler, se estiver desligado pelo pause
		if(reactivateMagicHandler){
			Object.FindObjectOfType<MagicHandler>().active = true;
		}

		//Religar SPawner
		Object.FindObjectOfType<EnemySpawner>().active = true;

		//Ligar botao de pause
		GameObject.Find("PauseButton").GetComponent<UnityEngine.UI.Button>().interactable = true;

		//Desaparecer tela de pause
		pauseScreen.pauseScreen.SetActive(false);
	}



	public void EndSession(){
		SoundController.instance.PlaySingle(popSound);
		Application.LoadLevel("Title");
	}

	public void FinishGame(){
		gameover = true;
		StartPause();
		pauseScreen.pauseScreen.SetActive(false);
		hud.hud.SetActive(false);
		gameOverScreen.gameOverScreen.SetActive(true);
		SoundController.instance.StopLoop(transform.GetComponent<AudioSource>());
		SoundController.instance.PlaySingle(endMusic, transform.GetComponent<AudioSource>());
	}

	public void RestartSession(){
		SoundController.instance.PlaySingle(popSound);
		Application.LoadLevel("Main");
	}

	public void Mute () {
		SoundController.instance.ToggleMute();
		SoundController.instance.PlaySingle(popSound);
	}

	public void WinGame(){
		wingame = true;
		StartCoroutine("EndingScene");
	}

	IEnumerator EndingScene(){
		Debug.Log("ENTROOO");
		//Pegar refs
		hud.hud.SetActive(false);
		GameObject.FindObjectOfType<MagicHandler>().active = false;

		//Destruir inimigos
		GameObject.FindObjectOfType<EnemySpawner>().active = false;
		Enemy[] es = GameObject.FindObjectsOfType<Enemy>();
		for(int i=0; i<es.Length; i++){
			Debug.Log(es[i]);
			es[i].GetComponent<Enemy>().Die();
		}

		//Loop da magia finalizando
		//yield return new WaitForSeconds(3f);
		float fadeVel = .2f;
		UnityEngine.UI.Image fadein = transform.Find("FadeIn").GetComponent<UnityEngine.UI.Image>();
		fadein.color = new Color(1f,1f,1f,0f);
		fadein.gameObject.SetActive(true);
		SpriteRenderer menina = GameObject.Find("MENINA").GetComponent<SpriteRenderer>();
		while(fadein.color.a < 1){
			menina.color = new Color(1f,menina.color.g - (fadeVel * Time.deltaTime * 4f), menina.color.b - (fadeVel * Time.deltaTime * 4f));
			fadein.color = new Color(1f,1f,1f,fadein.color.a + (fadeVel * Time.deltaTime));
			yield return null;
		}


		//Passar de cena
		Application.LoadLevel("Win");
	}
}

class Hud{
	public GameObject hud;
	public GameObject bar;
	public UnityEngine.UI.Text txt;
	public UnityEngine.UI.Image img;

	// - Atualizar vida ////////////
	// - Contar almas sugadas //////////
	// - Triggar evento no botao de pause ///////////////

	public void GetRefs(GameObject master){
		hud = master.transform.Find("Hud").gameObject;
		bar = hud.transform.Find("Bar").gameObject;
		txt = bar.transform.Find("Text").GetComponent<UnityEngine.UI.Text>();
		img = hud.transform.Find("Heart").GetComponent<UnityEngine.UI.Image>();
		txt.text = "0";
	}

	public void UpdateLife(int life, Sprite[] hearts){
		if(life > 0){
			// Atualizar contador
			img.sprite = hearts[life-1];
		}else{
			//Apagar
			img.enabled = false;// = Color.clear;
		}
	}

	public void UpdateSouls(int souls){
		txt.text = souls.ToString();
	}

}

class PauseScreen{

	public GameObject pauseScreen;

	public void GetRefs(GameObject master){
		pauseScreen = master.transform.Find("PauseScreen").gameObject;
	}
}

class GameOverScreen{

	public GameObject gameOverScreen;

	public void GetRefs(GameObject master){
		gameOverScreen = master.transform.Find("GameOverScreen").gameObject;
	}
}