using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public float duration = 10;
	public RectTransform startRef;
	public RectTransform endRef;
	public AudioClip music;

	RectTransform credits;
	float lerpCounter;

	// Use this for initialization
	void Start () {
        lerpCounter = 0;
		credits = transform.Find("Credits").GetComponent<RectTransform>();
		credits.localPosition = startRef.localPosition;
		SoundController.instance.PlayLoop(music, transform.GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {
		lerpCounter += Time.deltaTime/duration;
		credits.localPosition = Vector3.Lerp(startRef.localPosition, endRef.localPosition, lerpCounter);
		if(lerpCounter > 1)
			ReturnTitle();
	}

	public void ReturnTitle(){
		Application.LoadLevel("Title");
	}
}
