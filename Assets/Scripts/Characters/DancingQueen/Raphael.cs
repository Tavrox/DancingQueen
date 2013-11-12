using UnityEngine;
using System.Collections;

public class Raphael : CharSim 
{
	private bool coupleClaire = true;
	private bool kissedPlayer = false;
	public int neededSympathyForKiss = 50;
	
	public void GiveBeer()
	{
		
	}
	
	public void tryKiss()
	{
		if (sympathy_score >= neededSympathyForKiss)
		{
			kissPlayer();
		}
		else
		{
			rejectPlayer();	
		}
	}
	private void kissPlayer()
	{
		kissedPlayer = true;
	}
	private void rejectPlayer()
	{
		kissedPlayer = false;
	}
	private void checkSlowMusic()
	{
		
	}
			
	public void Update()
	{
		OTDebug.Message("______DEBUG______RAPHAEL_____");
		OTDebug.Message("Sympathy Score :" + sympathy_score);
		OTDebug.Message("Couple Claire :" + coupleClaire);
		OTDebug.Message("Raphael kissed player :" + kissedPlayer);
		OTDebug.Message("______END_DEBUG______RAPHAEL_____");
		
	}
}
