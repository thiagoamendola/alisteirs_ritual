using UnityEngine;
using System.Collections;

public class Pentagram : MonoBehaviour {
	public int SOULSTOWIN=10;
	public int gatheredSouls;
	public AudioClip soulGatheredSound;

	SpriteRenderer pentaVerde;
	SpriteRenderer pentaVermelho;
	SpriteRenderer pentaGlow;
	GameInterface gameInterface;
	ParticleSystem pentagramParticles;

	bool ending;

	// Use this for initialization
	void Start () {
		pentaVerde = transform.Find("PentaVerde").GetComponent<SpriteRenderer>();
		pentaVermelho = transform.Find("PentaVermelho").GetComponent<SpriteRenderer>();
		pentaGlow = transform.Find("PentaGlow").GetComponent<SpriteRenderer>();
		pentagramParticles = GetComponentInChildren<ParticleSystem>();

		gameInterface = GameObject.Find("GameInterface").GetComponent<GameInterface>();

		gatheredSouls = 0;
		ending = false;

		//pentaVerde.color.a = 1;
		pentaVerde.color = new Color(pentaVerde.color.r,pentaVerde.color.b,pentaVerde.color.g,1f);
		pentaVermelho.color = new Color(pentaVermelho.color.r,pentaVermelho.color.b,pentaVermelho.color.g,0f);
		pentaGlow.color = new Color(pentaGlow.color.r,pentaGlow.color.b,pentaGlow.color.g,0f);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(gatheredSouls);
		//Debug.Log("oloko "+ (float)(gatheredSouls)/SOULSTOWIN);
		if(!ending){
			pentaVermelho.color = new Color(pentaVermelho.color.r,pentaVermelho.color.b,pentaVermelho.color.g, (float)(gatheredSouls)/SOULSTOWIN);
			pentaGlow.color = new Color(pentaGlow.color.r,pentaGlow.color.b,pentaGlow.color.g, gatheredSouls/(SOULSTOWIN*3f));
			if(gatheredSouls >= SOULSTOWIN){
				//yield
				Debug.Log("GANHOUOUUUUU");
				//Handheld.Vibrate();
				GameObject.Find("Cursor").gameObject.SetActive(false);
				ending = true;
				pentagramParticles.loop = true;
				pentagramParticles.Play();
				gameInterface.WinGame();
			}
		}
	}

	public void GatherSoul(){
		gatheredSouls++;
		//Handheld.Vibrate();
		pentagramParticles.Play();
		gameInterface.UpdateSouls((int)gatheredSouls);
		SoundController.instance.PlaySingle(soulGatheredSound);
	}


}


