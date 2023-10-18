using System;
using Android.App;
using MobCAT.Android.Views;
using MobCAT.Services;

namespace MobCAT.Android.Services
{
    public class ProgressService : IProgressService
    {
        private ProgressDialogFragment _progressDialog;

        /// <inheritdoc />
        public void DisplayProgress(string title = null)
        {
            if (_progressDialog != null)
                return;
            _progressDialog = new ProgressDialogFragment(MainApplication.CurrentActivity, title);

            MainApplication.CurrentActivity.RunOnUiThread(() =>
            {
                _progressDialog.Show(MainApplication.CurrentActivity.FragmentManager, "progress");
            });
        }

        /// <inheritdoc />
        public void HideProgress()
        {
            MainApplication.CurrentActivity.RunOnUiThread(() =>
            {
                _progressDialog?.Dismiss();
                _progressDialog = null;
            });
        }
    }
}

