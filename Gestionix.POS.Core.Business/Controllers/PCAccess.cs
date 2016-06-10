using Gestionix.POS.Core.Data;
using Gestionix.POS.Core.Data.Access;
using System;
using System.Security.Cryptography;

namespace Gestionix.POS.Core.Controllers
{
    public class PCAccess
    {
        #region Fields
        private DBAccess _dbaccess;
        #endregion

        #region Ctors
        public PCAccess()
        {
            _dbaccess = new DBAccess();
        }
        #endregion

        #region Public Methods
        public bool Authenticate(string username, string password)
        {
            try
            {                               
                PMUser User = _dbaccess.GetUser(username, Functions.CreateMD5(password));

                if(!String.IsNullOrEmpty(User.User1))
                    return true;
            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }

            return false;
        }

        public void LogIn(string username, string password)
        {
            if (IsFirstUser())
                DownloadInitalUserInfo(username, password);
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
        private bool IsFirstUser()
        {
            try
            {
                return !_dbaccess.AreThereUsers();
            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }

            return true;
        }

        private void DownloadInitalUserInfo(string username, string password)
        {
            try
            {

            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }
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
