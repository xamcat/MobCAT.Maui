﻿using System;

namespace MobCAT.Platforms.Android
{
    public interface IApplication
    {
        void OnCreate();
        void OnTerminate();
        void OnActivityCreated(Activity activity, Bundle savedInstanceState);
        void OnActivityDestroyed(Activity activity);
        void OnActivityPaused(Activity activity);
        void OnActivityResumed(Activity activity);
        void OnActivitySaveInstanceState(Activity activity, Bundle outState);
        void OnActivityStarted(Activity activity);
        void OnActivityStopped(Activity activity);
    }
}

