using Gestionix.POS.Core.Data;
using Gestionix.POS.Core.Data.Access;
using System;
using System.Security.Cryptography;

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
            try
            {
                DBAccess Db = new DBAccess();                
                PMUser User = Db.GetUser(username, Functions.CreateMD5(password));

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
