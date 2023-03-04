using System;
using _Projects._Scripts.Core;
using UnityEngine;

namespace _Projects._Scripts.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioClip clickSFX;
        public AudioClip destroySFX;

        private AudioSource _audioSource;


        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySFX(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
}