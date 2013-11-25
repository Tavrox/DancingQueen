using UnityEngine;
using System.Collections;

public class Girls : CharSim 
{
	public void TriggerDialogVanessa()
	{
		if (DialogUI.exists != true)
		{
			Girls GO = GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
			GO.dialToTrigger = "1001";
			DialogUI.createDialog(GO);
			IngameUI.destroyIngameUI();
		}
	}
	public void TriggerDialogChloe()
	{
		if (DialogUI.exists != true)
		{
			Girls GO = GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
			GO.dialToTrigger = "1010";
			DialogUI.createDialog(this);
			IngameUI.destroyIngameUI();
		}
	}
}
