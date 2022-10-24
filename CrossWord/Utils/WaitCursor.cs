using System;
using System.Windows.Input;

namespace CrossWord
{
    public interface IWaitCursor : IDisposable
    {
        
    }

    public class WaitCursor : IWaitCursor
    {
        private Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}
