﻿using System;
using MobCAT.Models;

namespace MobCAT.Services.Abstractions
{
    public interface IBiometricAuthenticationService
    {
        /// <summary>
        /// Authenticate with biometrics.
        /// </summary>
        /// <returns>Task with AuthenticationResult containing if success and error message, if any.</returns>
        /// <param name="alertMessage">Alert message.</param>
        Task<AuthenticationResult> AuthenticateAsync(string alertMessage = null);

        /// <summary>
        /// Gets the type of the available biometric authentication.
        /// </summary>
        BiometricType AvailableBiometricType { get; }
    }
}

