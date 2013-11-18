using UnityEngine;
using System.Collections;

public class Boys : CharSim 
{

	public void Update()
	{

	}
	public void TriggerDialog ()
	{
		if (DialogUI.exists != true)
		{
			Boys GO = GameObject.FindGameObjectWithTag("Boys").GetComponent<Boys>();
			GO.dialToTrigger = "1001";
			DialogUI.createDialog(GO);
			IngameUI.destroyIngameUI();
		}
	}
}
