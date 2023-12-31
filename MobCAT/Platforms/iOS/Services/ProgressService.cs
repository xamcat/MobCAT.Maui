﻿using System;
using MobCAT.Platforms.iOS.Views;
using MobCAT.Services;
using UIKit;

namespace MobCAT.Platforms.iOS.Services
{
    public class ProgressService : IProgressService
    {
        private ProgressView _progressView;

        /// <inheritdoc />
        public void DisplayProgress(string title = null)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var vc = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }

                if (_progressView == null)
                    _progressView = new ProgressView(vc.View.Bounds, title);
                else
                    _progressView.StartAnimating();

                _progressView.Show(vc.View);
            });
        }

        /// <inheritdoc />
        public void HideProgress()
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _progressView?.Hide();
            });
        }
    }
}

