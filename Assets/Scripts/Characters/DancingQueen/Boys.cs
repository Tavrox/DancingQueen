using UnityEngine;
using System.Collections;

public class Boys : CharSim 
{
	public bool disapprovedStephane = false;
	public bool met = false;

	public void TriggerDialog ()
	{
		if (DialogUI.exists != true)
		{
			GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
			dialEvent.GetComponent<DialogEvent>().setupEvent(this,"3001");
		}
	}
}
