using System;
using TMPro;
using UnityEngine;

namespace GDPanda.BanterForge
{
    public class DialogueAudio : MonoBehaviour
    {
        private DialogueAudioHandler _audioHandler;
        
        private void Start()
        {
            _audioHandler = GetComponent<DialogueAudioHandler>();
            if(_audioHandler == null)
                throw new Exception("No DialogueAudioHandler found on GameObject");
            
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

        private void CharacterRevealAudio(char character)
        {
            _audioHandler.PlayCharacterRevealAudio(character);
        }
        
        private void OnEmotionChange(Emotion emotion)
        {
            var currentSpeaker = DialogueManager.GetInstance().CurrentSpeaker;
            if (!currentSpeaker)
                return;
            
            _audioHandler.SetAudioParamaterByName("Character", currentSpeaker.VoiceType);
        
            var emotionString = emotion.ToString();
            
            emotionString = char.ToUpper(emotionString[0]) + emotionString.Substring(1);
            
            if (emotionString.Contains("Suprised"))
                emotionString = "Surprised";
            
            _audioHandler.PlayOnEmotionChangedAudio(emotionString);
        }
    }
}