﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotNetUtils.Forms
{
    /// <summary>
    ///     Hooks into the <see cref="Control.WndProc"/> message handler to allow classes outside of a <see cref="Control"/>
    ///     to receive and suppress native window messages.
    /// </summary>
    /// <seealso cref="http://stackoverflow.com/a/3539204/467582"/>
    /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.windows.forms.nativewindow(v=vs.100).aspx"/>
    public class WndProcHook : NativeWindow
    {
        private readonly Control _control;

        /// <summary>
        ///     Invoked whenever the hooked control's <see cref="Control.WndProc"/> method is called.
        /// </summary>
        public event WndProcEventHandler2 GotWndProc;

        /// <summary>
        ///     Constructs a new <see cref="WndProcHook"/> instance that listens for <see cref="Control.WndProc"/> messages
        ///     sent to the given <paramref name="control"/>.
        /// </summary>
        /// <param name="control">
        ///     A control to listen to for window messages.
        /// </param>
        public WndProcHook(Control control)
        {
            _control = control;

            if (_control == null)
                return;

            _control.HandleCreated += HandleCreated;
            _control.HandleDestroyed += HandleDestroyed;

            if (_control.IsHandleCreated)
                HijackHandle();
        }

        protected override void WndProc(ref Message m)
        {
            var args = new HandledEventArgs();

            if (GotWndProc != null)
                GotWndProc(ref m, args);

            if (args.Handled)
                return;

            base.WndProc(ref m);
        }

        private void HandleCreated(object sender, EventArgs args)
        {
            HijackHandle();
        }

        private void HandleDestroyed(object sender, EventArgs eventArgs)
        {
            ReleaseHandle();
        }

        private void HijackHandle()
        {
            AssignHandle(_control.Handle);
        }
    }

    /// <summary>
    ///     Invoked whenever the hooked control's <see cref="Control.WndProc"/> method is called.
    /// </summary>
    /// <param name="m">
    ///     Native window message.
    /// </param>
    /// <param name="args">
    ///     Event data.
    /// </param>
    public delegate void WndProcEventHandler2(ref Message m, HandledEventArgs args);
}
