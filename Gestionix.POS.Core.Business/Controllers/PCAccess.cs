namespace Gestionix.POS.Core.Controllers
{
    public class PCAccess
    {
        #region Ctors
        public PCAccess()
        {                        
        }
        #endregion

        #region Public Methods
        public bool Authenticate(string username, string password)
        {
            //TODO:
            return false;
        }

        public void LogIn(string username, string password)
        {
            //TODO:
        }

        public void OpenCashRegister()
        {
            //TOOD:
        }

        public void CloseCashRegister()
        {
            //TOOD:
        }
        #endregion

        #region Helpers
        private void DownloadInitalUserInfo()
        {
            //TOOD:
        }

        /// <summary>
        /// Temporary. DownloadInitalUserInfo must be download all users information for this POS.
        /// </summary>
        private void DownloadAdditionalUserInfo()
        {
            //TOOD:
        }
        #endregion
    }
}
