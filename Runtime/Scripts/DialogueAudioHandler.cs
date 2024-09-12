using UnityEngine;

namespace GDPanda.BanterForge
{
    public abstract class DialogueAudioHandler : MonoBehaviour
    {
        public bool ContainsSpecialLetter(char letter)
        {
            return letter is ' ' or ',' or '=' or '*' or '!' or ':' or ')' or '.';
        }

        public abstract void PlayCharacterRevealAudio(char character);

        public abstract void PlayOnEmotionChangedAudio(string emotion);
        
        public abstract void SetAudioParamaterByName(string character, int currentSpeakerVoiceType);
        
        public abstract void PlayOnPopUpAudio();
        
        public abstract void PlayOnExitAudio();
        
        public abstract void PlayOnUiButtonAudio();
    }
}