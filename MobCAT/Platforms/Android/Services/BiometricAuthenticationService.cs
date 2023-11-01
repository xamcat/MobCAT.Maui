﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AndroidX.Core.Content;
using AndroidX.Core.Hardware.Fingerprint;
using MobCAT.Models;
using MobCAT.Services.Abstractions;
using Permission = Android.Content.PM.Permission;
using Android;
using System.Diagnostics;
using Android.Content;
using MobCAT.Platforms.Android.Views;

namespace MobCAT.Platforms.Android.Services
{
    public class BiometricAuthenticationService : IBiometricAuthenticationService
    {
        private bool _hasEvaluatedBiometricType;
        private BiometricType _biometricType;
        private readonly string _alertTitle;
        private readonly Type _fingerprintFragmentType;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Microsoft.MobCat.Core.Droid.Services.BiometricAuthenticationService"/> class.
        /// </summary>
        /// <param name="alertTitle">Alert title.</param>
        public BiometricAuthenticationService(string alertTitle = null)
        {
            _alertTitle = alertTitle ?? "Sign in";
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Microsoft.MobCat.Core.Droid.Services.BiometricAuthenticationService"/> class.
        /// </summary>
        /// <param name="fingerprintFragmentType">Custom Fingerprint fragment type.</param>
        public BiometricAuthenticationService(Type fingerprintFragmentType)
            : this("Sign in")
        {
            _fingerprintFragmentType = fingerprintFragmentType;
        }

        /// <inheritdoc />
        public Task<AuthenticationResult> AuthenticateAsync(string alertMessage = null)
        {
            Permission permissionResult = ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.UseFingerprint);

            var tcs = new TaskCompletionSource<AuthenticationResult>();

            if (permissionResult == Permission.Granted)
            {
                if (AvailableBiometricType == BiometricType.None)
                {
                    Debug.WriteLine("[BiometricAuthenticationService] Authentication not available on this device");
                    tcs.TrySetResult(new AuthenticationResult(false, "Authentication not available"));
                }
                else
                {
                    if (_fingerprintFragmentType != null)
                    {
                        var fragment = Activator.CreateInstance(_fingerprintFragmentType, tcs);
                        if (fragment is FingerprintManagerFragment customFragment)
                        {
                            MainApplication.CurrentActivity.RunOnUiThread(() => { customFragment.Show(MainApplication.CurrentActivity.FragmentManager, "dialog"); });
                        }
                        else
                        {
                            throw new Exception("Specified FingerprintManagerFragment does not inherit from FingerprintManagerFragment");
                        }
                    }
                    else
                    {
                        var fingerPrintDialog = new FingerprintManagerFragment(_alertTitle, alertMessage ?? "Please scan fingerprint", tcs);
                        MainApplication.CurrentActivity.RunOnUiThread(() => { fingerPrintDialog.Show(MainApplication.CurrentActivity.FragmentManager, "dialog"); });
                    }
                }
            }
            else
            {
                tcs.TrySetResult(new AuthenticationResult(false, "UseFingerprint Permission has not been granted."));
            }

            return tcs.Task;
        }

        /// <inheritdoc />
        public BiometricType AvailableBiometricType
        {
            get
            {
                if (_hasEvaluatedBiometricType) return _biometricType;

                var fingerprintManager = FingerprintManagerCompat.From(Application.Context);

                if (!fingerprintManager.IsHardwareDetected)
                {
                    Debug.WriteLine("This device does not have a fingerprint scanner.");
                    _biometricType = BiometricType.None;
                    _hasEvaluatedBiometricType = true;
                    return _biometricType;
                }

                KeyguardManager keyguardManager = (KeyguardManager)Application.Context.GetSystemService(Context.KeyguardService);
                if (!keyguardManager.IsKeyguardSecure)
                {
                    Debug.WriteLine("Secure lock screen hasn\'t been set up. Goto Settings -> Security to set up a keyguard.");
                    _biometricType = BiometricType.None;
                    _hasEvaluatedBiometricType = true;
                    return _biometricType;
                }


                if (!fingerprintManager.HasEnrolledFingerprints)
                {
                    Debug.WriteLine("Register a fingerprint at Settings -> Security -> Fingerprint.");
                    _biometricType = BiometricType.None;
                    _hasEvaluatedBiometricType = true;
                    return _biometricType;
                }

                Debug.WriteLine("Device is ready for Fingerprint Scanning");
                _biometricType = BiometricType.Fingerprint;
                _hasEvaluatedBiometricType = true;
                return _biometricType;
            }
        }
    }
}

