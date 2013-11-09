using UnityEngine;
using System.Collections;

public class CharacterAnims : MonoBehaviour 
{
	public enum animDef
	{
		None,
		WalkLeft, WalkRight,
		RopeLeft, RopeRight,
		Climb, ClimbStop,
		StandLeft, StandRight,
		HangLeft, HangRight,
		FallLeft, FallRight ,
		ShootLeft, ShootRight,
		CrounchLeft, CrounchRight,
		AttackLeft, AttackRight,
	}
	
	public Transform spriteParent;
	public OTAnimatingSprite animSprite;
	public OTAnimation anim;
	
	private animDef currentAnim;
	private Character _character;
	private Player _player;
	
	private bool animPlaying = false;
	
	// Use this for initialization
	void Start () 
	{
		_character 	= GetComponent<Character>();
		_player 	= GetComponent<Player>();
	}
	void Update() 
	{
		animSprite.looping = true;
		// Order of action matters, they need to have priorities. //
		Run();
		Walk();
		Crounch();
		Jump();
		Attack();
		Hurt();
		Fall();
		Paused();
	}
	private void Run()
	{
		if(_character.isRight && _character.grounded)
		{
			currentAnim = animDef.WalkRight;
			animSprite.Play("run");
			NormalScaleSprite();;
		}
		if(_character.isLeft && _character.grounded)
		{
			currentAnim = animDef.WalkLeft;
			animSprite.Play("run");
			InvertSprite();
		}
	}
	private void Walk()
	{
		
	}
	private void Stand()
	{	
		if(!_character.isLeft && _character.grounded == true && currentAnim != animDef.StandLeft && _character.facingDir == Character.facing.Left && animPlaying == false)
		{
			currentAnim = animDef.StandLeft;
			animSprite.Play("stand"); // stand left
			InvertSprite();
		}
		if(!_character.isRight && _character.grounded && currentAnim != animDef.StandRight && _character.facingDir == Character.facing.Right && animPlaying == false)
		{
			currentAnim = animDef.StandRight;
			animSprite.Play("stand"); // stand left
			NormalScaleSprite();
		}
	}
	private void Crounch()
	{
		if (_character.isCrounch == true)
		{
			currentAnim = animDef.CrounchLeft;
			animSprite.Play("crounch");
		}
	}
	private void Jump()
	{
		if(_character.grounded == false && currentAnim != animDef.FallLeft && _character.facingDir == Character.facing.Left)
		{
			currentAnim = animDef.FallLeft;
			animSprite.Play("jump"); // fall left
			InvertSprite();
		}
		if(_character.grounded == false && currentAnim != animDef.FallRight && _character.facingDir == Character.facing.Right)
		{
			currentAnim = animDef.FallRight;
			animSprite.Play("jump"); // fall right
			NormalScaleSprite();
		}
	}
	private void Attack()
	{
		if (_player.shootingKnife == true  && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animSprite.Play("throw_knife");
			InvertSprite();
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
		if (_player.shootingKnife == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			currentAnim = animDef.ShootRight;
			animSprite.Play("throw_knife");
			NormalScaleSprite();
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[3]) ) );
		}
	}
	private void Hurt()
	{
		//ENEMIES SPECIFIC ANIMS
		if (_character.isShot == true && _character.facingDir == Character.facing.Left)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			InvertSprite();
		}
		if (_character.isShot == true && _character.facingDir == Character.facing.Right)
		{
			animPlaying = true;
			animSprite.Play("hurt");
			StartCoroutine( WaitAndCallback( anim.GetDuration(anim.framesets[2]) ) );
			NormalScaleSprite();
		}
	}
	private void Fall()
	{
		
	}
	private void Paused()
	{
		if (_player.paused == true)
		{
			currentAnim = animDef.None;
			animSprite.looping = false;
		}
	}
	
	private void AnimationFinished()
	{
	    animPlaying = false;
	}
	private void InvertSprite()
	{
		spriteParent.localScale = new Vector3(-1,1,1);
	}
	private void NormalScaleSprite()
	{
		spriteParent.localScale = new Vector3(1,1,1);
	}
	IEnumerator WaitAndCallback(float waitTime)
	{
	    yield return new WaitForSeconds(waitTime);
	    AnimationFinished();
	}
}
