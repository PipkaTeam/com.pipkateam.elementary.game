using System;
using UnityEngine;

namespace Elementary.Game
{
    public class OnlineManager : Singleton<OnlineManager>
    {
        public event Action OnRewardedVideoFinishedEvent;

        protected override void Start()
        {
            base.Start();
        }

        public virtual void ShowRewardedVideo()
        {

        }

        protected void OnRewardedVideoFinished()
        {
            OnRewardedVideoFinishedEvent?.Invoke();
        }
    }
}