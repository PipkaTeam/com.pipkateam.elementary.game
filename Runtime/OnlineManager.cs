using System;

namespace Elementary.Game
{
    public class OnlineManager : Singleton<OnlineManager>
    {
        public event Action OnRewardedVideoFinishedEvent;
        public event Action OnFullScreenVideoFinishedEvent;

        protected override void Start()
        {
            base.Start();
        }

        public virtual void ShowRewardedVideo()
        {

        }

        public void OnRewardedVideoFinished()
        {
            OnRewardedVideoFinishedEvent?.Invoke();
        }

        public virtual void ShowFullScreenVideo()
        {

        }

        public void OnFullScreenVideoFinished()
        {
            OnFullScreenVideoFinishedEvent?.Invoke();
        }
    }
}