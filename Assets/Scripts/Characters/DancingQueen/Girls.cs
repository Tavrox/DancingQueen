using UnityEngine;
using System.Collections;

public class Girls : CharSim 
{

	public bool met = false;
	public bool facedHomophobia = false;
	public bool approvedHomophobia = false;

	public void TriggerDialogVanessa()
	{
		if (DialogUI.exists != true)
		{
			GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
			dialEvent.GetComponent<DialogEvent>().setupEvent(this,"1010");
		}
	}
	public void TriggerDialogChloe()
	{
		if (DialogUI.exists != true)
		{
			GameObject dialEvent = Instantiate(Resources.Load("03UI/Event")) as GameObject;
			dialEvent.GetComponent<DialogEvent>().setupEvent(this,"1001");
		}
	}
}
