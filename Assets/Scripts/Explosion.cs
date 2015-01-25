using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour 
{
	public Texture[] frames;
	public int framesPerSecond = 10;
	public float speed = .8f;
	int counter = 0;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator>();
//		anim.StopPlayback();
		anim.speed = speed;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		counter++;
		if(counter > 50)
			Destroy(gameObject);
//		int index = (counter * framesPerSecond ) % frames.Length;
//		renderer.material.mainTexture = frames[index];
//		counter++;

	}
}
