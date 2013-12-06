using UnityEngine;
using System.Collections;

public class Yannick : CharSim 
{
	public bool hasSpokenOnceToPlayer;

	void Start()
	{
//		StartCoroutine( triggerTuto(4f) );
	}

	IEnumerator triggerTuto(float wait)
	{
		yield return new WaitForSeconds(wait);
		DialogUI.exists = false;
		DialogUI.createDialog(this);
		IngameUI.destroyIngameUI();
		GameObject go = GameObject.FindGameObjectWithTag("Yannick");
		go.GetComponent<Yannick>().tutoMode = false;
		go.GetComponent<Yannick>().hasSpokenOnceToPlayer = true;
		go = GameObject.FindGameObjectWithTag("Chloe");
		go.GetComponent<Chloe>().tutoMode = false;
		go = GameObject.FindGameObjectWithTag("Vanessa");
		go.GetComponent<Vanessa>().tutoMode = false;
	}
}
