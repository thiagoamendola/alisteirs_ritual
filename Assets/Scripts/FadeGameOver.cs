using UnityEngine;
using System.Collections;

public class FadeGameOver : MonoBehaviour {

    public Transform gameover_t;
    private SpriteRenderer gameover;

    private bool didFinish;
    private bool forceExit;

	// Use this for initialization
	void Start () {
	    gameover = gameover_t.GetComponent<SpriteRenderer>();
        gameover.color = new Color(1f,1f,1f,1f);
        forceExit = false;
        didFinish = false;

        StartCoroutine(spriteFadeOut(this.gameover, 4.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator spriteFadeOut(SpriteRenderer spr, float time = 1.0f) {
		didFinish = false;

		yield return new WaitForSeconds(1);

        while (spr.color.a != 0f) {
            float a;

			a = spr.color.a - Time.deltaTime / time;
            if (a < 0f)
                a = 0f;

			spr.color = new Color(1f, 1f, 1f, a);

			if (this.forceExit) {
				spr.color = new Color(1f, 1f, 1f, 0f);
				break;
			}

            yield return null;
        }

        didFinish = true;
    }
}
