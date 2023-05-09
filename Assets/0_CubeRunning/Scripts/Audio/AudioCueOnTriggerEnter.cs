using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Class to activate AudioCues when a GameObject (i.e. the Player) enters the trigger Collider on this same GameObject.
    /// This component is mostly used for testing purposes.
    /// </summary>
    [RequireComponent(typeof(AudioCue))]
    public class AudioCueOnTriggerEnter : MonoBehaviour
    {
        [SerializeField] private string tagToDetect = "Player";

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(tagToDetect))
                GetComponent<AudioCue>().PlayAudioCue();
        }
    }
}