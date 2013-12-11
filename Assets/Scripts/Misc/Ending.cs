using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

	public static bool exists = false;

	private OTSprite _blackOv;

	
	private Alex _Alex;
	private Bastien _Bastien;
	private Bob _Bob;
	private Boris _Boris;
	private Boys _Boys;
	private Charlie _Charlie;
	private Chloe _Chloe;
	private Christine _Christine;
	private Claire _Claire;
	private Didier _Didier;
	private Girls _Girls;
	private Manon _Manon;
	private Paul _Paul;
	private Raphael _Raphael;
	private Stephane _Stephane;
	private Thomas _Thomas;
	private Vanessa _Vanessa;
	private Yannick _Yannick;

	private GUIText _txtExplanation;
	private GUIText _txtNumberVotes;
	private GUIText _txtGroupBoys;
	private GUIText _txtGroupGirls;
	private GUIText _txtCorrupted;
	private GUIText _txtBastien;
	private GUIText _txtRaphael;
	private GUIText _txtThomas;
	private GUIText _txtChoices;
	private GUIText watchTxt;

	private LevelManager _LM;
	private PlayerSim _Player;
	private int nbVotes;

	// Use this for initialization
	void Start () {

		DialogUI.destroyDialog();

		_Alex 		= GameObject.FindGameObjectWithTag("Alex").GetComponent<Alex>();
		_Bob 		= GameObject.FindGameObjectWithTag("Bob").GetComponent<Bob>();
		_Boris 		= GameObject.FindGameObjectWithTag("Boris").GetComponent<Boris>();
		_Bastien 	= GameObject.FindGameObjectWithTag("Bastien").GetComponent<Bastien>();
		_Charlie	= GameObject.FindGameObjectWithTag("Charlie").GetComponent<Charlie>();
		_Boys		= GameObject.FindGameObjectWithTag("Boys").GetComponent<Boys>();
		_Chloe 		= GameObject.FindGameObjectWithTag("Chloe").GetComponent<Chloe>();
		_Christine	= GameObject.FindGameObjectWithTag("Christine").GetComponent<Christine>();
		_Claire 	= GameObject.FindGameObjectWithTag("Claire").GetComponent<Claire>();
		_Didier 	= GameObject.FindGameObjectWithTag("Didier").GetComponent<Didier>();
		_Manon 		= GameObject.FindGameObjectWithTag("Manon").GetComponent<Manon>();
		_Paul 		= GameObject.FindGameObjectWithTag("Paul").GetComponent<Paul>();
		_Raphael 	= GameObject.FindGameObjectWithTag("Raphael").GetComponent<Raphael>();
		_Stephane 	= GameObject.FindGameObjectWithTag("Stephane").GetComponent<Stephane>();
		_Thomas 	= GameObject.FindGameObjectWithTag("Thomas").GetComponent<Thomas>();
		_Vanessa 	= GameObject.FindGameObjectWithTag("Vanessa").GetComponent<Vanessa>();
		_Yannick 	= GameObject.FindGameObjectWithTag("Yannick").GetComponent<Yannick>();
		_Girls 		= GameObject.FindGameObjectWithTag("Girls").GetComponent<Girls>();
		_Player 	= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();
		_LM 		= GameObject.Find("Level Manager").GetComponent<LevelManager>();


		_LM = GameObject.Find("Level Manager").GetComponent<LevelManager>();
		_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSim>();

		_blackOv = GameObject.Find("Ending(Clone)/00BlackOverlay").GetComponent<OTSprite>();

		_txtExplanation = GameObject.Find("Ending(Clone)/01Expl").GetComponent<GUIText>();
		_txtNumberVotes = GameObject.Find("Ending(Clone)/02Voted").GetComponent<GUIText>();
		_txtGroupBoys = GameObject.Find("Ending(Clone)/03Boris").GetComponent<GUIText>();
		_txtGroupGirls = GameObject.Find("Ending(Clone)/04Girls").GetComponent<GUIText>();
		_txtCorrupted = GameObject.Find("Ending(Clone)/05Bob").GetComponent<GUIText>();
		_txtBastien = GameObject.Find("Ending(Clone)/06Bastien").GetComponent<GUIText>();
		_txtRaphael = GameObject.Find("Ending(Clone)/07Claire").GetComponent<GUIText>();
		_txtThomas = GameObject.Find("Ending(Clone)/08Thomas").GetComponent<GUIText>();
		_txtChoices = GameObject.Find("Ending(Clone)/09Choices").GetComponent<GUIText>();
		watchTxt = GameObject.Find("Timer").GetComponent<GUIText>();
		watchTxt.color = Color.clear;

		_txtExplanation.text = "";
		_txtNumberVotes.text = "";
		_txtGroupBoys.text = "";
		_txtGroupGirls.text = "";
		_txtCorrupted.text = "";
		_txtBastien.text = "";
		_txtRaphael.text = "";
		_txtThomas.text = "";
		_txtChoices.text = "";

		OTTween _overlayOutX = new OTTween(_blackOv, 4f).Tween("size" , new Vector2(20f, 15f));
		OTTween _overlayAlpha = new OTTween(_blackOv, 2f).Tween("alpha" , 1f);
		_overlayOutX.OnFinish(displayExpl);

		exists = true;
		DialogUI.exists = true;

		nbVotes = _LM.getVotes();

		settleTexts();
	}

	private void displayExpl(OTTween _tween)
	{

	}
	private void displayVotes(OTTween _tween)
	{
		
	}
	private void displayBoys(OTTween _tween)
	{
		
	}
	private void displayGirls(OTTween _tween)
	{
		
	}
	private void displayCorrupt(OTTween _tween)
	{
		
	}
	private void displayBastien(OTTween _tween)
	{
		
	}
	private void displayRaphael(OTTween _tween)
	{
		
	}
	private void displayThomas(OTTween _tween)
	{
		
	}
	private void displayChoices(OTTween _tween)
	{
		
	}

	private void settleTexts()
	{
		if (_Player.langChosen == PlayerSim.langList.fr)
		{
			if (nbVotes > _LM.stepVotesForWin)
			{
				_txtExplanation.text = 
						"Bravo, vous avez été élue Dancing Queen ce soir.\n"
						+ "Vous êtes sur le podium et tout le monde vous regarde.\n"
						+ "Ce regard a t-il guidé vos choix ?";
			}
			else
			{
				_txtExplanation.text =
						  "La soirée touche à sa fin, vous n'avez pas été élue Dancing Queen...\n" 
						+ "Mais quelle importance, ce n'est qu'un concours après tout. \n"
						+ "Le regard des autres est-il si précieux pour vous ? \n" 
						+ "De toute façon, la défaite est une notion bien relative.";
			}

			_txtNumberVotes.text = nbVotes + " personnes sur 15 ont voté pour vous.";

			
			if (_Boys.disapprovedStephane == false && _Boys.met == true)
			{
				_txtGroupBoys.text = "Vous avez rencontré le groupe d'amis de Boris et vous avez désapprouvé le rejet de Stéphane. ";
			}
			if (_Boys.disapprovedStephane == true && _Boys.met == true)
			{
				_txtGroupBoys.text = "Vous avez rencontré le groupe d'amis de Boris et vous avez approuvé le rejet de Stéphane." ;
			}
			if (_Boys.met == false)
			{
				_txtGroupBoys.text = "Vous n'avez pas rencontré le groupe d'amis de Boris." ;
			}

			if (_Girls.facedHomophobia == true && _Girls.approvedHomophobia == true)
			{
				_txtGroupGirls.text = "Vous avez rencontré le groupe d'amis de Chloé et Vanessa et vous avez approuvé leur homophobie.";
			}
			if (_Girls.facedHomophobia == true && _Girls.approvedHomophobia == false)
			{
				_txtGroupGirls.text = "Vous avez rencontré le groupe d'amis de Chloé et Vanessa et vous avez avez desapprouvé leur homophobie.";
			}
			if (_Girls.met == false)
			{
				_txtGroupGirls.text = "Vous n'avez pas rencontré le groupe d'amis de Chloé et Vanessa.";
			}

			if (_Bob.met == true && _Bob.convinced == true)
			{
				_txtCorrupted.text = "Vous avez rencontré Bob, le maire de la ville, et l'avez convaincu de truquer les votes.";
			}
			if (_Bob.met == true && _Bob.convinced == false)
			{
				_txtCorrupted.text = "Vous avez rencontré Bob, le maire de la ville, mais n'a pas été convaincu de votre comportement";
			}
			else if (_Bob.met == false && _Bob.convinced == false)
			{
				_txtCorrupted.text = "Vous n'avez pas rencontré Bob, le maire de la ville.";
			}

			if (_Bastien.succeedMission == true)
			{
				_txtBastien.text = "Vous avez aidé Bastien à se rapprocher de Thomas.";
			}
			if (_Bastien.refusedMission == true)
			{
				_txtBastien.text = "Vous n'avez pas aidé Bastien à se rapprocher de Thomas.";
			}

			
			if (_Raphael.coupleClaire == false)
			{
				_txtRaphael.text = "Vous avez fait rompre Claire et Raphaël.";
			}
			else if (_Raphael.coupleClaire == true)
			{
				_txtRaphael.text = "Claire et Raphaël sont toujours ensembles.";
			}


			if (_Paul.missionDone == true && _Thomas.missionClaireThomasDone == true)
			{
				_txtThomas.text = "Vous avez incité Thomas à droguer une fille pour sortir avec elle.";
			}
				
			_txtChoices.text = "Ce sont les choix que vous avez ou n'avez pas fait.";
		}
		// ENG LANGUAGE //
		else
		{
			if (nbVotes > _LM.stepVotesForWin )
			{
				_txtExplanation.text = 
						" You've been elected Dancing Queen tonight.\n"
						+ "You're on the podium and everyone looks at you.\n"
						+ "Did these looks guide your choices ? ";
			}
			else
			{
				_txtExplanation.text =
						" The evening ends and you haven't been elected Dancing Queen.\n" 
						+ "But does it matter ? It's only a contest after all. \n"
						+ "Are the looks of the other that important to you ?";
			}
			
			_txtNumberVotes.text = nbVotes + " people on 15 have voted for you.";
			
			if (_Boys.disapprovedStephane == true)
			{
				_txtGroupBoys.text = "You met the friends group of Boris and you approved the reject of Stephane." ;
			}
			if (_Boys.disapprovedStephane == false && _Boys.met == true)
			{
				_txtGroupBoys.text = "You met the friends group of Boris and you disapproved the reject of Stephane.";
			}
			if (_Boys.met == false)
			{
				_txtGroupBoys.text = "You didn't met the friends group of Boris." ;
			}
			
			if (_Girls.facedHomophobia == true && _Girls.approvedHomophobia == true)
			{
				_txtGroupGirls.text = "You met the friends group of Chloe and Vanessa and disapproved their homophobia.";
			}
			if (_Girls.facedHomophobia == true && _Girls.approvedHomophobia == false)
			{
				_txtGroupGirls.text = "You met the friends group of Chloe and Vanessa and disapproved their homophobia.";
			}
			if (_Girls.met == true)
			{
				_txtGroupGirls.text = "You didn't met the friends group of Chloe and Vanessa.";
			}
			
			if (_Bob.met == true && _Bob.convinced == true)
			{
				_txtCorrupted.text = "You met Bob, the mayor of the town, and have convinced him to scam the votes.";
			}
			if (_Bob.met == true && _Bob.convinced == false)
			{
				_txtCorrupted.text = "You met Bob, the mayor of the town, but wasn't convinced by your behaviour.";
			}
			else if (_Bob.met == false && _Bob.convinced == false)
			{
				_txtCorrupted.text = "You didn't met Bob, the mayor of the town.";
			}
			if (_Bastien.succeedMission == true)
			{
				_txtBastien.text = "You helped Bastien get closer to Thomas.";
			}
			if (_Bastien.refusedMission == true)
			{
				_txtBastien.text = "You refused to help Bastien to get closer to Thomas.";
			}

			if (_Raphael.coupleClaire == false)
			{
				_txtRaphael.text = "You made Claire and Raphael break up";
			}
			else if (_Raphael.coupleClaire == true)
			{
				_txtRaphael.text = "Claire and Raphael are still a couple.";
			}

			if (_Paul.missionDone == true)
			{
				_txtThomas.text = "You encouraged Thomas to drug a girl to date her.";
			}
			
			_txtChoices.text = "There are the choices you made or didn't make.";
		}
	}
}
