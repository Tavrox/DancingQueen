using UnityEngine;
using System.Collections;

public class Boys : CharSim 
{
	public void TriggerDialog ()
	{
		if (DialogUI.exists != true)
		{
			GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
			dialEvent.GetComponent<DialogEvent>().setupEvent(this,"3001");
		}
	}
}
