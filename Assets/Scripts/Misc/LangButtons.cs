using UnityEngine;
using System.Collections;

public class LangButtons : MonoBehaviour {

	public enum lgList { fr, en};
	public lgList lg;
	private PlayerSim _Player;
	private Introduction _intro;
	private bool activated = false;

	// Use this for initialization
	void Start () {
		
		_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_intro = GameObject.Find("Introduction").GetComponent<Introduction>();
	
	}
	
	private void OnMouseDown()
	{
		if (lg == lgList.en && activated == false)
		{
			print ("clicked en");
			_Player.langChosen = PlayerSim.langList.en;
			activated = true;
			_intro.revealCredits();
		}
		if (lg == lgList.fr && activated == false)
		{
			print ("clicked fr");
			_Player.langChosen = PlayerSim.langList.fr;
			activated = true;
			_intro.revealCredits();
		}
	}
}
