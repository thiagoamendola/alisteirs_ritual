using UnityEngine;
using System.Collections;

public class ToggleButton : MonoBehaviour {
	public Sprite normal1Sprite;
	public Sprite normal2Sprite;
	Sprite currentSprite;
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
		currentSprite = normal1Sprite;
		if(normal1Sprite == null)
			normal1Sprite = GetComponent<SpriteRenderer>().sprite;
		if(overSprite == null)
			overSprite = normal1Sprite;
		if(pressedSprite == null)
			pressedSprite = normal1Sprite;
    }
	
	void Start () {
		time = 0;

		if(clickFX == null)
			clickFX = (AudioClip)Resources.Load("Botoes", typeof(AudioClip));
		
		GetComponent<SpriteRenderer>().sprite = currentSprite;

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
				GetComponent<SpriteRenderer>().sprite = currentSprite;
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

			//Trocar normals
			if(currentSprite == normal1Sprite){
				if(overSprite == normal1Sprite)
					overSprite = normal2Sprite;
				if(pressedSprite == normal1Sprite)
					pressedSprite = normal2Sprite;
				currentSprite = normal2Sprite;
			}else{
				if(overSprite == normal2Sprite)
					overSprite = normal1Sprite;
				if(pressedSprite == normal2Sprite)
					pressedSprite = normal1Sprite;
				currentSprite = normal1Sprite;
			}

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
				GetComponent<SpriteRenderer>().sprite = currentSprite;
	        }else{
				GetComponent<SpriteRenderer>().sprite = overSprite;
	        }
		}
    }
    
    void OnMouseUp(){
		if(enabled){
			pressed = false;
			GetComponent<SpriteRenderer>().sprite = currentSprite;
		}
    }

    public void SetActive1(){
		if(overSprite == normal2Sprite)
			overSprite = normal1Sprite;
		if(pressedSprite == normal2Sprite)
			pressedSprite = normal1Sprite;
		currentSprite = normal1Sprite;
		GetComponent<SpriteRenderer>().sprite = currentSprite;
    }

    public void SetActive2(){
		if(overSprite == normal1Sprite)
			overSprite = normal2Sprite;
		if(pressedSprite == normal1Sprite)
			pressedSprite = normal2Sprite;
		currentSprite = normal2Sprite;
		GetComponent<SpriteRenderer>().sprite = currentSprite;
    }
    
    public void UpdateSprites(Sprite normal1, Sprite normal2, Sprite over, Sprite pressed){
    	//Update normal1
    	if(normal1 != null)
			normal1Sprite = normal1;

    	//Update normal2
    	if(normal2 != null)
			normal2Sprite = normal2;

		//Update over
		if(over != null)
			overSprite = over;
		else if(normal1 != null)
			overSprite = normal1;

		//Update pressed
		if(pressed != null)
			pressedSprite = pressed;
		else if(normal1 != null)
			pressedSprite = normal1;
    }

    public Sprite[] GetSprites(){
    	Sprite[] sprites = new Sprite[4];
    	sprites[0] = normal1Sprite;
    	sprites[1] = normal2Sprite;
    	sprites[2] = overSprite;
    	sprites[3] = pressedSprite;
    	return sprites;
    }   
}
