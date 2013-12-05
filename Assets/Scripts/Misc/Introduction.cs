using UnityEngine;
using System.Collections;

public class Introduction : MonoBehaviour {


	private OTSprite _logo;
	private OTSprite _overlay;
	private OTSprite _title;
	private GUIText _intro;
	private GUIText _credits;
	private GUIText _creditsAdd;
	private GUIText watchTxt;
	private LevelManager _LM;
	public bool debug;

	// Use this for initialization
	void Start () {

		DialogUI.exists = true;

		_LM = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_logo = GameObject.Find("Introduction/Logo4Edges").GetComponent<OTSprite>();
		_overlay = GameObject.Find("Introduction/BlackOverlay").GetComponent<OTSprite>();
		_title = GameObject.Find("Introduction/GameTitle").GetComponent<OTSprite>();
		_intro = GameObject.Find("Introduction/IntroExplanation").GetComponent<GUIText>();
		_credits = GameObject.Find("Introduction/Credits4Edges").GetComponent<GUIText>();
		_creditsAdd = GameObject.Find("Introduction/CreditsSidekick").GetComponent<GUIText>();
		watchTxt = GameObject.Find("WatchDisplay/Timer").GetComponent<GUIText>();

		if (debug != true)
		{
			watchTxt.color = Color.clear;
			OTTween _logoOut = new OTTween(_logo, 2.5f).Tween("alpha", 1f);
			_logoOut.OnFinish(revealCredits);
		}
		else
		{
			DialogUI.exists = false;
			Destroy(gameObject);
		}
	}

	private void fadeEverything(OTTween _tween)
	{
		OTTween _introOut = new OTTween(_intro, 2.5f).Tween("color", Color.clear).Wait(4f);
		OTTween _titleOut = new OTTween(_title, 2.5f).Tween("alpha", 0f).Wait(4f);
		OTTween _overlayIn = new OTTween(_overlay, 2.5f).Tween("alpha", 0f).Wait(4f);
		_overlayIn.OnFinish(endIntro);
	}
	private void endIntro(OTTween _tween)
	{
		DialogUI.exists = false;
		Destroy(gameObject);
	}
	private void revealCredits(OTTween _tween)
	{
		OTTween _creditsOut = new OTTween(_credits, 2.5f).Tween("color", Color.white);
		_creditsOut.OnFinish(revealAdditionalCredits);
	}
	private void revealAdditionalCredits(OTTween _tween)
	{
		OTTween _addCreditsOut = new OTTween(_creditsAdd, 2.5f).Tween("color", Color.white);
		_addCreditsOut.OnFinish(hideCredits);
	}
	private void hideCredits(OTTween _tween)
	{
		OTTween _creditsIn = new OTTween(_credits, 2.5f).Tween("color", Color.clear).Wait(3f);
		OTTween _creditsAddIn = new OTTween(_creditsAdd, 2.5f).Tween("color", Color.clear).Wait(3f);
		OTTween _logoOut = new OTTween(_logo, 2.5f).Tween("alpha", 0f);
		_logoOut.OnFinish(revealIntro);
	}
	private void revealIntro(OTTween _tween)
	{
		OTTween _introOut = new OTTween(_intro, 2.5f).Tween("color", Color.white).Wait(2f);
		OTTween _titleOut = new OTTween(_title, 2.5f).Tween("alpha", 1f).Wait(2f);
		_titleOut.OnFinish(fadeEverything);
	}

}
