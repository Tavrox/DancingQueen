using UnityEngine;
using System.Collections;

public class Boys : CharSim 
{


	public void TriggerDialog ()
	{
		if (DialogUI.exists != true)
		{
			Boys GO = GameObject.FindGameObjectWithTag("Boys").GetComponent<Boys>();
			DialogUI.createDialog(this, "3001");
			IngameUI.destroyIngameUI();
			print ("Boys dialog triggered");
		}
	}
}
