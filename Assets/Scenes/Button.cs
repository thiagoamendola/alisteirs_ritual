using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public Sprite normalSprite;
	public Sprite overSprite;
	public Sprite pressedSprite;

	public GameObject objectOwner;
    public string scriptName;
	public string functionName;
	public bool enabled = true;
	public bool temporaryBlockOnClick = false;
	public AudioClip clickFX;
	
	float time = 0;
	AudioSource AS;
	bool pressed;
	
	//Fazer uma booleana "pressed"
	void Awake(){
		if(normalSprite == null)
			normalSprite = GetComponent<SpriteRenderer>().sprite;
		if(overSprite == null)
			overSprite = normalSprite;
		if(pressedSprite == null)
			pressedSprite = normalSprite;
    }
	
	void Start () {
		time = 0;

		if(clickFX == null)
			clickFX = (AudioClip)Resources.Load("Botoes", typeof(AudioClip));
		
		GetComponent<SpriteRenderer>().sprite = normalSprite;

		AS = null;
		AS = GetComponent<AudioSource>();
		if(AS == null){
			gameObject.AddComponent<AudioSource>();
			AS = GetComponent<AudioSource>();
			AS.playOnAwake = false;
			AS.loop = false;
			AS.clip = clickFX;
		}
		pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(temporaryBlockOnClick && pressed){
			if(time > 5){
				enabled = true;
				GetComponent<SpriteRenderer>().sprite = normalSprite;
				time = 0;
				pressed = false;
            }else{
				time += Time.deltaTime;
			}
		}
		//Debug.Log(enabled);
	}
    
	void OnMouseDown(){
		if(enabled){
			//Apertou
			GetComponent<SpriteRenderer>().sprite = pressedSprite;
		}	
	}

	void OnMouseUpAsButton(){
		if(enabled){
			pressed = true;
			if(scriptName.Length != 0){
				MonoBehaviour temp = (MonoBehaviour) objectOwner.GetComponent(scriptName);
				if(functionName != null){
					temp.Invoke(functionName, 0);
				}
				
				GetComponent<AudioSource>().PlayOneShot(clickFX);
				
				if(temporaryBlockOnClick){
					enabled = false;
					time = 0;
				}
			}
			
		}
	}

	void OnMouseEnter(){
		if(!pressed && enabled){
			GetComponent<SpriteRenderer>().sprite = overSprite;
        }
	}

	void OnMouseExit(){
		if(enabled){
			if(!pressed){
				GetComponent<SpriteRenderer>().sprite = normalSprite;
	        }else{
				GetComponent<SpriteRenderer>().sprite = overSprite;
	        }
		}
    }
    
    void OnMouseUp(){
		if(enabled){
			pressed = false;
			GetComponent<SpriteRenderer>().sprite = normalSprite;
		}
    }
    
    public void UpdateSprites(Sprite normal, Sprite over, Sprite pressed){
    	//Update normal
    	if(normal != null)
			normalSprite = normal;

		//Update over
		if(over != null)
			overSprite = over;
		else if(normal != null)
			overSprite = normal;

		//Update pressed
		if(pressed != null)
			pressedSprite = pressed;
		else if(normal != null)
			pressedSprite = normal;
    }

    public Sprite[] GetSprites(){
    	Sprite[] sprites = new Sprite[3];
    	sprites[0] = normalSprite;
    	sprites[1] = overSprite;
    	sprites[2] = pressedSprite;
    	return sprites;
    }   
}
