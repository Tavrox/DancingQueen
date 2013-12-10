using UnityEngine;
using System.Collections;

public class DialogEvent : MonoBehaviour {

	private LevelManager _LevMan;
	private PlayerSim _Player;
	public Color _col;

	private CharSim charToTrigger;
	private string idDialog;

	private OTSprite _overlay;
	private OTSprite _bubble;
	private GUIText _text;
	
	private float lifeTime = 3f;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine("Wait", lifeTime);

		_overlay = GameObject.Find("Event(Clone)/evOverlay/EvOvSprite").GetComponent<OTSprite>();
		_bubble = GameObject.Find("Event(Clone)/Bubble").GetComponent<OTSprite>();
		_text = GameObject.Find("Event(Clone)/EventNotifier").GetComponent<GUIText>();
		_LevMan = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_bubble.alpha = 0f;
		_text.color = Color.clear;

		IngameUI.destroyIngameUI();
		DialogUI.exists = true;
		_LevMan.eventHappening = true;

		_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		if (_Player.langChosen == PlayerSim.langList.en && _text.text != null && _Player != null)
		{
			_text.text = "Wait... Someone wants \n to tell you something !";
		}
		else
		{
			_text.text = "Attends... quelqu'un vient  \n  te parler !";
		}
		
		OTTween bubbleOut = new OTTween(_bubble, 1f).Tween("alpha", 1f);
		OTTween textOut = new OTTween(_text, 1f).Tween("color", _col);
	}

	public void setupEvent(CharSim _char, string _dialTrigger)
	{
		charToTrigger = _char;
		idDialog = _dialTrigger;
	}
	public void triggerDeath()
	{
		OTTween bubbleOut = new OTTween(_bubble, 1f).Tween("alpha", 0f);
		OTTween textOut = new OTTween(_text, 1f).Tween("color", Color.clear);
		DialogUI.createDialog(charToTrigger, idDialog);
		_overlay.alpha = 0;
		StartCoroutine("WaitDestroy", 5f);
	}

	public void destroyEvent()
	{
		_LevMan.eventHappening = false;
		Destroy (gameObject);
	}

	IEnumerator Wait(float life)
	{
		yield return new WaitForSeconds(life);
		triggerDeath();
	}
	IEnumerator WaitDestroy(float life)
	{
		yield return new WaitForSeconds(life);
		destroyEvent();
	}

}
