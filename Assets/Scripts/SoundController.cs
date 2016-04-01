using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	//Singleton
	public static SoundController instance = null;
	AudioSource musicSource;
	AudioSource efxSource;

	public float lowPitchRange = .95f;              //The lowest a sound effect will be randomly pitched.
	public float highPitchRange = 1.05f;            //The highest a sound effect will be randomly pitched.

	public bool mute = false;
	bool updateMute;

	AudioClip delayedClip;
	AudioSource delayedSource;
	float delayedInterval;

/*

	====== MANUAL DE INICIALIZAÇÂO =======
	- Criar objeto vazio de nome "(&)SoundController"
	- Resetar Transform
	- Colocar AudioSource
	- Atrelar SoundController a ele

	=> Acesso é possível em qualquer classe por meio de:
		SoundController.instance.oquequiser;
	=> Para sons individuais, não é necessário fornecer AudioSource
	=> Para sons em loop, é necessário fornecer AudioSource

*/
	void Awake(){
		//Singleton
		if(instance == null)
			instance = this;
		else if(this != instance)
				Destroy(gameObject);
		DontDestroyOnLoad(this);

		updateMute = true;

		efxSource = GetComponent<AudioSource>();
	}
	
	
	void Update () {
		//Aplicar valor de mute em caso de inicio ou troca de cena
		if(updateMute){
			updateMute = false;
			ApplyMuteValue();
		}
	}


	//Toggle mute value
	public void ToggleMute(){
		mute = !mute;	
		ApplyMuteValue();
	}
	void ApplyMuteValue(){
		foreach( AudioSource a in Object.FindObjectsOfType<AudioSource>())
			a.mute = mute;
	}


	//Play one sound 
	public void PlaySingle(AudioClip clip){
		PlaySingle(clip, efxSource);
    }
    public void PlaySingle(AudioClip clip, AudioSource source){//OVERLOAD
		source.PlayOneShot(clip);
	}


	public void PlayDelayedSingle(AudioClip clip, float f){
		PlayDelayedSingle(clip, f, efxSource);	
	}
	public void PlayDelayedSingle(AudioClip clip, float f, AudioSource source){//OVERLOAD
		delayedClip = clip;
		delayedSource = source;
		delayedInterval = f;
		StartCoroutine("PD");
	}
	IEnumerator PD(){
		yield return new WaitForSeconds(delayedInterval);
		delayedSource.PlayOneShot(delayedClip);
	}


	//Play random sound of a list with random pitching
	public void RandomizeSfx(AudioClip clip){
		RandomizeSfx(new AudioClip[]{clip}, efxSource);
    }
    public void RandomizeSfx(AudioClip[] clips){
		RandomizeSfx(clips, efxSource);
	}
	public void RandomizeSfx(AudioClip[] clips, AudioSource source){
		int randomIndex = Random.Range(0, clips.Length); //Randomiza os efeitos sonoros
		float randomPitch = Random.Range(lowPitchRange, highPitchRange); //randomiza o pitching, pra nao fica parecido

		source.pitch = randomPitch;
		source.PlayOneShot(clips[randomIndex]);
	}


	//Play sound loop
	public void PlayLoop(AudioClip clip, AudioSource source){
		source.loop = true;
		source.clip = clip;
		source.Play();
	}
	//Stop playing sound
	public void StopLoop(AudioSource source){
		if(source.isPlaying){
			source.Stop();
		}
	}

	public void Stop(){
		StopLoop(GetComponent<AudioSource>());
	}

	//Play music with intro
	public void PlayMusicWithIntro(AudioClip intro, AudioClip loop, AudioSource source){
		source.loop = false;
		source.clip = intro;
		source.Play();
		StartCoroutine(PlayLoop( loop, source, intro.length));
	}
	IEnumerator PlayLoop(AudioClip loop, AudioSource source, float time){
		yield return new WaitForSeconds(time);
		source.loop = true;
		source.clip = loop;
		source.Play();
	}

	
	void OnLevelWasLoaded(int level){
		updateMute = true;
	}
}
