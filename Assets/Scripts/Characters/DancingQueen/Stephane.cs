using UnityEngine;
using System.Collections;

public class Stephane : CharSim 
{
	private int numberDrinks;
	private bool isDrunk = false;
	private bool isVomitting = false;
	public int neededDrinksForVomit = 5;
	public int neededDrinksForGoodState = 2;
	
	void Start()
	{
		
	}
	
	public void checkAlcohol()
	{
		if (numberDrinks > neededDrinksForGoodState)
		{
			isDrunk = true;	
		}
		if (numberDrinks > neededDrinksForVomit)
		{
			isVomitting = true;
		}
	}
}
