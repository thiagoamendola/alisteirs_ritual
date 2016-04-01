using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	public float speed = 1;
	public float startY;
	public float endY;
	public AudioClip music;

	RectTransform credits;

	// Use this for initialization
	void Start () {
		credits = transform.Find("Credits").GetComponent<RectTransform>();
		credits.position = new Vector3(credits.position.x, startY, credits.position.z);
		SoundController.instance.PlayLoop(music, transform.GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {
		credits.Translate((speed * Time.deltaTime) * Vector3.up);
		Debug.Log(credits.position.y);		
		if(credits.position.y > endY)
			ReturnTitle();
	}

	public void ReturnTitle(){
		Application.LoadLevel("Title");
	}
}
