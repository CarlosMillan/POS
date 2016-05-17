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
        /// <summary>
        /// Authenticate and Login user.
        /// </summary>
        /// <param name="username">Registered Email is used like username.</param>
        /// <returns></returns>
        public bool LogIn(string username, string password)
        {
            //TODO: LocalAuthenticate(username, password)
            //TODO: RemoteAuthenticate(user, password)
            //TODO: DownloadUserInfo();
            return false;
        }

        public bool LogOut()
        {
            return true;
        }

        public bool Authenticate(string username, string password)
        {
            return false;
        }
        #endregion

        #region Helpers
        #endregion
    }
}
