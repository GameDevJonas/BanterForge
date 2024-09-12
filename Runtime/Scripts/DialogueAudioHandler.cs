using UnityEngine;

namespace GDPanda.BanterForge
{
    public abstract class DialogueAudioHandler : MonoBehaviour
    {
        private bool ContainsSpecialLetter(char letter)
        {
            return letter is ' ' or ',' or '=' or '*' or '!' or ':' or ')' or '.';
        }

        public virtual void PlayCharacterRevealAudio(char character)
        {
            string charEvent = "event:/Dialogue/" + character;
            if (ContainsSpecialLetter(character))
                charEvent = "event:/Dialogue/space";
            
            //Run audio event
        }

        public virtual void PlayOnEmotionChangedAudio(string emotion)
        {
            string emotionEvent = "event:/SetMood/Set" + emotion;
            
            //Run audio event
        }
        
        public abstract void SetAudioParamaterByName(string character, int currentSpeakerVoiceType);
        
        public abstract void PlayOnPopUpAudio();
        
        public abstract void PlayOnExitAudio();
        
        public abstract void PlayOnUiButtonAudio();
    }
}