using UnityEngine;
using System.Collections;

public class AnimatedBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseOver()
	{
		print ("omg");
		OTAnimatingSprite anim = GetComponent<OTAnimatingSprite>();
		anim.Play("over");
		
	}
}
