using UnityEngine;
using System.Collections;

public class Claire : CharSim 
{
	public bool talkedAboutFlirting = false;
	public bool talkedAboutKissing = false;
	public bool talkedAboutSlow = false;

	public void TriggerDialogClaireMusic()
	{
		if (DialogUI.exists != true)
		{
			DialogUI.createDialog(this, "14010");
			IngameUI.destroyIngameUI();
		}
	}
}
