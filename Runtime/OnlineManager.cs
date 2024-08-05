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
            OnRewardedVideoFinished();
        }

        public void OnRewardedVideoFinished()
        {
            OnRewardedVideoFinishedEvent?.Invoke();
        }

        public virtual void ShowFullScreenVideo()
        {
            OnFullScreenVideoFinished();
        }

        public void OnFullScreenVideoFinished()
        {
            OnFullScreenVideoFinishedEvent?.Invoke();
        }
    }
}