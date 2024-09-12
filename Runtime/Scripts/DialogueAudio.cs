using System;
using TMPro;
using UnityEngine;

namespace GDPanda.BanterForge
{
    public class DialogueAudio : MonoBehaviour
    {
        private void Start()
        {
            DialoguePanel.OnCharacterRevealCallback -= CharacterRevealAudio;
            DialoguePanel.OnCharacterRevealCallback += CharacterRevealAudio;

            DialogueManager.OnEmotionChange -= OnEmotionChange;
            DialogueManager.OnEmotionChange += OnEmotionChange;
        }

        private void OnDisable()
        {
            DialoguePanel.OnCharacterRevealCallback -= CharacterRevealAudio;
            DialogueManager.OnEmotionChange -= OnEmotionChange;
        }

        private void OnEnable()
        {
            // Lookup();
        }

        private void CharacterRevealAudio(char character)
        {
            string charEvent = "event:/Dialogue/" + character;
            if (ContainsSpecialLetter(character))
                charEvent = "event:/Dialogue/space";

#if FMOD
            FMODUnity.RuntimeManager.PlayOneShot(charEvent);
#endif
        }
        
        private bool ContainsSpecialLetter(char letter)
        {
            return letter is ' ' or ',' or '=' or '*' or '!' or ':' or ')' or '.';
        }
        
        private void OnEmotionChange(Emotion emotion)
        {
            var currentSpeaker = DialogueManager.GetInstance().CurrentSpeaker;
            if (!currentSpeaker)
                return;
            
#if FMOD
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Character", currentSpeaker.VoiceType);
#endif
        
            var emotionString = emotion.ToString();
            
            emotionString = char.ToUpper(emotionString[0]) + emotionString.Substring(1);
            
            if (emotionString.Contains("Suprised"))
                emotionString = "Surprised";
            
#if FMOD
            string emotionEvent = "event:/SetMood/Set" + emotionString;
            FMODUnity.RuntimeManager.PlayOneShot(emotionEvent);
#endif
        }
        
#if FMOD
        private FMOD.RESULT Lookup()
       {
           FMOD.RESULT result = RuntimeManager.StudioSystem.getParameterDescriptionByName(_musStateParam, out _musStateParameter);
           return result;
       }
        
         [SerializeField] 
         private StudioEventEmitter _popupAudio, _exitAudio, _uiButtonAudio;

         public void EnterAudio()
         {
             _popupAudio.Play();
         }

         public void ExitAudio()
         {
             _exitAudio.Play();
         }

         public void UiButtonAudio()
         {
             _uiButtonAudio.Play();
         }
#endif
    }
}