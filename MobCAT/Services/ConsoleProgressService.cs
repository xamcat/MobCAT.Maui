using System;
using System.Diagnostics;

namespace MobCAT.Services
{
    public class ConsoleProgressService : IProgressService
    {
        /// <inheritdoc />
        public void DisplayProgress(string title = null)
        {
            Debug.WriteLine($"[Progress] Displaying progress - with title {title}");
        }

        /// <inheritdoc />
        public void HideProgress()
        {
            Debug.WriteLine($"[Progress] Hiding progress");
        }
    }
}

