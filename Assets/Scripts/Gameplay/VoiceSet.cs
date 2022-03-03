using UnityEngine;

namespace Gameplay.Player
{
    [CreateAssetMenu]
    public class VoiceSet : ScriptableObject
    {
        public AudioClip[] AngryAttack;
        public AudioClip[] AngryGreeting;
        public AudioClip[] AngryHurt;
        public AudioClip[] AngryIdle;
        public AudioClip[] HappyAttack;
        public AudioClip[] HappyGreeting;
        public AudioClip[] HappyHurt;
        public AudioClip[] HappyIdle;
        public AudioClip[] AnnoyedAttack;
        public AudioClip[] AnnoyedGreeting;
        public AudioClip[] AnnoyedHurt;
        public AudioClip[] AnnoyedIdle;

        public AudioClip[] GetSet(int mood, Character.SpeechQueue queue)
        {
            if (mood <= 0) {
                if (queue == Character.SpeechQueue.Attack) {
                    return HappyAttack;
                }
                if (queue == Character.SpeechQueue.Idle) {
                    return HappyIdle;
                }
                if (queue == Character.SpeechQueue.Greeting) {
                    return HappyGreeting;
                }
                if (queue == Character.SpeechQueue.Hurt) {
                    return HappyHurt;
                }
            } else if (mood == 1) {
                if (queue == Character.SpeechQueue.Attack) {
                    return AnnoyedAttack;
                }
                if (queue == Character.SpeechQueue.Idle) {
                    return AnnoyedIdle;
                }
                if (queue == Character.SpeechQueue.Greeting) {
                    return AnnoyedGreeting;
                }
                if (queue == Character.SpeechQueue.Hurt) {
                    return AnnoyedHurt;
                }
            } else {
                if (queue == Character.SpeechQueue.Attack) {
                    return AngryAttack;
                }
                if (queue == Character.SpeechQueue.Idle) {
                    return AngryIdle;
                }
                if (queue == Character.SpeechQueue.Greeting) {
                    return AngryGreeting;
                }
                if (queue == Character.SpeechQueue.Hurt) {
                    return AngryHurt;
                }
            }
            return null;
        }
    }

}