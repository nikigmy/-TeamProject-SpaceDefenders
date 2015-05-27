using System;
using System.Media;

namespace GameSounds
{
    public class Sounds
    {
        #region Fields

        private SoundPlayer menuSound = new SoundPlayer("menuSound.wav");
        private SoundPlayer menuSoundtrack = new SoundPlayer("menuMusic.wav");
        private SoundPlayer startGameSound = new SoundPlayer("startGameSound.wav");
        private SoundPlayer transitionSound = new SoundPlayer("transition.wav");
        private SoundPlayer explosionSound = new SoundPlayer("explosion.wav");
        private SoundPlayer shotSound = new SoundPlayer("Shot.wav");

        #endregion

        #region Constructors

        public Sounds()
        {
            using (menuSound)
            {
                menuSound.Load();
            }
            using (menuSoundtrack)
            {
                menuSoundtrack.Load();
            }
            using (startGameSound)
            {
                startGameSound.Load();
            }
            using (transitionSound)
            {
                transitionSound.Load();
            }
            using (explosionSound)
            {
                explosionSound.Load();
            }
            using (shotSound)
            {
                shotSound.Load();
            }
        }

        #endregion

        #region Methods

        public void PlayMenuSound()
        {
            this.menuSound.Play();
        }

        public void PlayMenuSoundtrack()
        {
            this.menuSoundtrack.PlayLooping();
        }

        public void StopMenuSoundtrack()
        {
            this.menuSoundtrack.Stop();
        }

        public void PlayStartGameSound()
        {
            this.startGameSound.Play();
        }
        public void PlayTransitionSound()
        {
            this.transitionSound.Play();
        }

        public void PlayExplosionSound()
        {
            this.explosionSound.Play();
        }

        public void PlayShotSound()
        {
            this.shotSound.Play();
        }


        #endregion
    }
}
