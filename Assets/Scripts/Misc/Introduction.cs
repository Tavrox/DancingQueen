using UnityEngine;
using System.Collections;

public class Introduction : MonoBehaviour {


	private OTSprite _logo;
	private OTSprite _overlay;
	private OTSprite _title;
	private OTSprite _en;
	private OTSprite _fr;
	private GUIText _intro;
	private GUIText _credits;
	private GUIText _creditsAdd;
	private GUIText _lgChoose;
	private GUIText watchTxt;
	private GameObject _enObject;
	private GameObject _frObject;
	private LevelManager _LM;
	private PlayerSim _Player;
	public Color colWatch;
	public bool debug;

	// Use this for initialization
	void Start () {

		DialogUI.exists = true;

		_LM = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_logo = GameObject.Find("Introduction/Logo4Edges").GetComponent<OTSprite>();
		_overlay = GameObject.Find("Introduction/BlackOverlay").GetComponent<OTSprite>();
		_title = GameObject.Find("Introduction/GameTitle").GetComponent<OTSprite>();
		_en = GameObject.Find("Introduction/langEN").GetComponentInChildren<OTSprite>();
		_enObject = GameObject.Find("Introduction/langEN");
		_fr = GameObject.Find("Introduction/langFR").GetComponentInChildren<OTSprite>();
		_frObject = GameObject.Find("Introduction/langFR");
		_intro = GameObject.Find("Introduction/IntroExplanation").GetComponent<GUIText>();
		_credits = GameObject.Find("Introduction/Credits4Edges").GetComponent<GUIText>();
		_creditsAdd = GameObject.Find("Introduction/CreditsSidekick").GetComponent<GUIText>();
		_lgChoose = GameObject.Find("Introduction/ChooseLg").GetComponent<GUIText>();
		watchTxt = GameObject.Find("WatchDisplay/Timer").GetComponent<GUIText>();

		if (debug != true)
		{
			watchTxt.color = Color.clear;
		}
		else
		{
			DialogUI.exists = false;
			Destroy(gameObject);
		}
	}

	private void fadeEverything(OTTween _tween)
	{
		OTTween _watchOut = new OTTween(watchTxt, 2.5f).Tween("color", colWatch).Wait(8f);
		OTTween _introOut = new OTTween(_intro, 2.5f).Tween("color", Color.clear).Wait(8f);
		OTTween _titleOut = new OTTween(_title, 2.5f).Tween("alpha", 0f).Wait(8f);
		OTTween _overlayIn = new OTTween(_overlay, 2.5f).Tween("alpha", 0f).Wait(8f);
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
		OTTween _creditsIn = new OTTween(_credits, 2.5f).Tween("color", Color.clear).Wait(5f);
		OTTween _creditsAddIn = new OTTween(_creditsAdd, 2.5f).Tween("color", Color.clear).Wait(5f);
		OTTween _logoOut = new OTTween(_logo, 2.5f).Tween("alpha", 0f);
		_logoOut.OnFinish(revealIntro);
	}
	private void revealIntro(OTTween _tween)
	{
		OTTween _introOut = new OTTween(_intro, 2.5f).Tween("color", Color.white).Wait(2f);
		OTTween _titleOut = new OTTween(_title, 2.5f).Tween("alpha", 1f).Wait(2f);
		_titleOut.OnFinish(fadeEverything);
	}

	public void revealCredits()
	{
		setupTexts();
		
		OTTween _enOut = new OTTween(_en, 2.5f).Tween("alpha", 0f);
		OTTween _frOut = new OTTween(_fr, 2.5f).Tween("alpha", 0f);
		OTTween _chooseOut = new OTTween(_lgChoose, 2.5f).Tween("color", Color.clear);

		OTTween _logoOut = new OTTween(_logo, 2.5f).Tween("alpha", 1f).Wait(3f);
		_logoOut.OnFinish(revealCredits);
	}

	private void destroyLangs()
	{
		Destroy(_enObject.gameObject);
		Destroy(_frObject.gameObject);
	}

	private void setupTexts()
	{

	}

}
