using Gestionix.POS.Core.Business.Sync;
using Gestionix.POS.Core.Data;
using Gestionix.POS.Core.Data.User;
using System;
using System.Security.Cryptography;

namespace Gestionix.POS.Core.Controllers
{
    public class PCAccess
    {
        #region Fields
        private DBUser _dbuser;
        #endregion

        #region Ctors
        public PCAccess()
        {
            _dbuser = new DBUser();            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Se ejecuta cualquier instrucción SQL para que se inicialize el Entity Framewoark (EF), de lo contrario al hacer uso de la primera consulta a base de datos se tarda mucho. (Cold Query)
        /// </summary>
        public void InitializeEF()
        {
            try
            {                
                PMUser User = _dbuser.GetUser(String.Empty, String.Empty);
            }
            catch (Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }            
        }

        public bool Authenticate(string username, string password)
        {
            try
            {                               
                PMUser User = _dbuser.GetUser(username, Functions.CreateMD5(password));

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
                return !_dbuser.AreThereUsers();
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
                SyncManager SManager = new SyncManager();
                SManager.DownlodUserInfo(username, password);
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
