using UnityEngine;
using System.Collections;

public class Vanessa : CharSim 
{
	public bool isSad = true;
	public bool knowsDance = false;

	public void TriggerDialogVanessa()
	{
		if (DialogUI.exists != true)
		{
			DialogUI.createDialog(this, "7009");
			IngameUI.destroyIngameUI();
		}
	}

}
