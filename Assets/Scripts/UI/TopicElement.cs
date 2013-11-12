using UnityEngine;
using System.Collections;


[System.Serializable]
public class TopicElement : MonoBehaviour {

	public enum Characters{ David, Megan};
   public enum AvatarPos{ left, right};
   public Characters Character;
   public AvatarPos CharacterPosition;
   public Texture2D CharacterPic;
   public string DialogueText;
   public GUIStyle DialogueTextStyle;
   public float TextPlayBackSpeed;
   public AudioClip PlayBackSoundFile;
}
