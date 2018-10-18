using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {

	SpriteRenderer BG2;
	float alphaParam = 0;
	public float alphaSpeed = 0.1f;
	public bool blink = true;
	public bool ending = false;
	public AudioClip titleMusic;
	public AudioClip clickSound;

	float timeHappyEnding;
	
	public void Start(){
		if(blink){
			BG2 = GameObject.Find("BG2").GetComponent<SpriteRenderer>();
			BG2.color = new Color(1f,1f,1f,0f);
		}
		alphaParam = 0;
		if(blink){
			SoundController.instance.PlayLoop(titleMusic, transform.GetComponent<AudioSource>());
			Debug.Log(SoundController.instance.mute);
			if(SoundController.instance.mute)
				GameObject.Find("btnMute").GetComponent<ToggleButton>().SetActive2();
		}else if(ending){
			SoundController.instance.PlaySingle(titleMusic);
			timeHappyEnding = 0;
			GameObject.Find("Button").GetComponent<Button>().enabled = false;
		}
	}

	public void Update(){
		if(blink){
			//Debug.Log(alphaParam);
			alphaParam += alphaSpeed * Time.deltaTime;
			Mathf.Max(0,Mathf.Min(alphaParam,1));
			BG2.color = Color.Lerp(new Color(1f,1f,1f,0f), Color.white, alphaParam);
			if((alphaSpeed > 0 && alphaParam >= 1) || (alphaSpeed < 0 && alphaParam <= 0))
				alphaSpeed *= -1;
		}else if(ending){
			if(timeHappyEnding > 5){
				GameObject.Find("Button").GetComponent<Button>().enabled = true;
			}else{
				timeHappyEnding += Time.deltaTime;
			}
		}
	}

	public void Mute () {
		SoundController.instance.ToggleMute();
		SoundController.instance.PlaySingle(clickSound);
	}


	public void StartGame () {
		SoundController.instance.PlaySingle(clickSound);
		GameObject.Find("Shadow").GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,.5f);
		Application.LoadLevel("Main");
	}

	public void BackToTitle () {
		SoundController.instance.Stop();
		Application.LoadLevel("Title");
	}

	public void GoToCred () {
		SoundController.instance.PlaySingle(clickSound);
		Application.LoadLevel("Credits");
	}

	public void GoToInstr () {
		SoundController.instance.PlaySingle(clickSound);
		Application.LoadLevel("Instr");
	}

	public void GoToSite(){
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.Gamux.alisteirsritual");
	}
}
