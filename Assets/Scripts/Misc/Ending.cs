using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {

	public static bool exists = false;

	private OTSprite _blackOv;

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

	// Use this for initialization
	void Start () {

		DialogUI.destroyDialog();

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

		exists = true;

		settleTexts();
	}



	private void settleTexts()
	{
		if (_Player.langChosen == PlayerSim.langList.fr)
		{
			_txtExplanation.text = "[ALLTEXTBROKEN]Bravo, les personnes de la soirée ont voté pour toi\n "
					+ "lors de l'éléction de la Dancing Queen. Mais quel est\n " 
					+ "le coût de ce succès ? As-tu correspondu à\n "
					+ "ce qu'on attendait de toi ? Est-ce que tu tes forcée\n "
					+ "à parler aux gens pour gagner leur sympathie ?\n "
					+ "Gagner, c'est un peu surfait. \n";

			_txtExplanation.text = "[ALLTEXTBROKEN]La soirée touche à sa fin, vous n'avez pas été élue Dancing Queen ce soir... \n" 
					+ "Mais quelle importance ? Est-ce si important d'être la reine, \n" 
					+ "Celle qu'on idole et qu'on porte haut dans son estime ?\n " 
					+ "Le regard des autres est-il si important ? \n" 
					+ "A vous de voir... Peut-être n'avez-vous pas \n" 
					+ "perdu... ? \n" ;

			_txtNumberVotes.text = "X personnes sur Y ont voté pour vous.";

			_txtGroupBoys.text = "Vous avez rencontré le groupe d'amis de Boris et vous avez approuvé le rejet de Stéphane." ;
			_txtGroupBoys.text = "Vous avez rencontré le groupe d'amis de Boris et vous avez désapprouvé le rejet de Stéphane. ";
			_txtGroupBoys.text = "Vous n'avez pas rencontré le groupe d'amis de Boris." ;

			_txtGroupGirls.text = "Vous avez rencontré le groupe d'amis de Chloé et Vanessa et vous avez approuvé leur homophobie.";
			_txtGroupGirls.text = "Vous avez rencontré le groupe d'amis de Chloé et Vanessa et vous avez avez desapprouvé leur homophobie.";
			_txtGroupGirls.text = "Vous n'avez pas rencontré le groupe d'amis de Chloé et Vanessa.";

			_txtCorrupted.text = "Vous avez rencontré Bob, le maire de la ville, et l'avez convaincu de truquer les votes.";
			_txtCorrupted.text = "Vous avez rencontré Bob, le maire de la ville, et ne l'avez pas convaincu de truquer les votes.";
			_txtCorrupted.text = "Vous n'avez pas rencontré Bob, le maire de la ville.";
			
			_txtBastien.text = "Vous avez aidé Bastien à se rapprocher de Thomas.";
			_txtBastien.text = "Vous avez refusé d'aider Bastien à se rapprocher de Thomas.";
			
			_txtRaphael.text = "Vous avez fait rompre Claire et Raphaël.";
			_txtRaphael.text = "Claire et Raphaël sont toujours ensembles.";
			
			_txtThomas.text = "Vous avez incité Thomas à droguer une fille pour sortir avec elle.";
				
			_txtChoices.text = "Ce sont les choix que vous avez ou n'avez pas fait.";

		}
		else
		{
			_txtExplanation.text = "[ALLTEXTBROKEN]Bravo, les personnes de la soirée ont voté pour toi\n "
				+ " lors de l'éléction de la Dancing Queen. Mais quel est\n " 
					+ " le coût de ce succès ? As-tu correspondu à\n "
					+ " ce qu'on attendait de toi ? Est-ce que tu tes forcée\n "
					+ " à parler aux gens pour gagner leur sympathie ?\n "
					+ " Gagner, c'est un peu surfait. \n";
			
			_txtExplanation.text = "[ALLTEXTBROKEN]La soirée touche à sa fin, vous n'avez pas été élue Dancing Queen ce soir... \n" 
				+ " Mais quelle importance ? Est-ce si important d'être la reine, \n" 
					+ " Celle qu'on idole et qu'on porte haut dans son estime ?\n " 
					+ " Le regard des autres est-il si important ? \n" 
					+ " A vous de voir... Peut-être n'avez-vous pas \n" 
					+ " perdu... ? \n" ;
			
			_txtNumberVotes.text = "X personnes sur Y ont voté pour vous.";
			
			_txtGroupBoys.text = "Vous avez rencontré le groupe d'amis de Boris et vous avez approuvé le rejet de Stéphane." ;
			_txtGroupBoys.text = "Vous avez rencontré le groupe d'amis de Boris et vous avez désapprouvé le rejet de Stéphane. ";
			_txtGroupBoys.text = "Vous n'avez pas rencontré le groupe d'amis de Boris." ;
			
			_txtGroupGirls.text = "Vous avez rencontré le groupe d'amis de Chloé et Vanessa et vous avez approuvé leur homophobie.";
			_txtGroupGirls.text = "Vous avez rencontré le groupe d'amis de Chloé et Vanessa et vous avez avez desapprouvé leur homophobie.";
			_txtGroupGirls.text = "Vous n'avez pas rencontré le groupe d'amis de Chloé et Vanessa.";
			
			_txtCorrupted.text = "Vous avez rencontré Bob, le maire de la ville, et l'avez convaincu de truquer les votes.";
			_txtCorrupted.text = "Vous avez rencontré Bob, le maire de la ville, et ne l'avez pas convaincu de truquer les votes.";
			_txtCorrupted.text = "Vous n'avez pas rencontré Bob, le maire de la ville.";
			
			_txtBastien.text = "Vous avez aidé Bastien à se rapprocher de Thomas.";
			_txtBastien.text = "Vous avez refusé d'aider Bastien à se rapprocher de Thomas.";
			
			_txtRaphael.text = "Vous avez fait rompre Claire et Raphaël.";
			_txtRaphael.text = "Claire et Raphaël sont toujours ensembles.";
			
			_txtThomas.text = "Vous avez incité Thomas à droguer une fille pour sortir avec elle.";
			
			_txtChoices.text = "Ce sont les choix que vous avez ou n'avez pas fait.";
		}
	}
}
