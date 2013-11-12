using UnityEngine;
using System.Collections;

public class Boris : CharSim 
{
	private bool BorisDestroyedDrug = false;
	
	public void DestroyDrug()
	{
		BorisDestroyedDrug = true;
	}
}
