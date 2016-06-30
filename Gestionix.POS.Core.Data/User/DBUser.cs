using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace Gestionix.POS.Core.Data.User
{
    public class DBUser
    {
        #region Ctors
        public DBUser() { }
        #endregion

        #region Public Methods   
        public PMUser GetUser(string username, string md5password)
        {
            try
            {
                using (Entities DBEntities = new Entities())
                {
                    IQueryable<PMUser> Result = DBEntities.PMUsers.Where(u => u.Email == username && u.Password == md5password);

                    if (Result.Count() > 0)
                        return Result.First();
                }
            }
            catch (Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }

            return new PMUser();
        }

        public bool AreThereUsers()
        {
            bool Response = false;

            try
            {
                using (Entities DBEntities = new Entities())
                {
                    Response = DBEntities.PMUsers.Count() == 0 ? false:true;
                }
            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }

            return Response;
        }
        #endregion
    }
}
