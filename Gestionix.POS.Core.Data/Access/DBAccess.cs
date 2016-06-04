using System;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace Gestionix.POS.Core.Data.Access
{
    public class DBAccess
    {
        //static readonly Func<Entities, IQueryable<PMUser>> s_compiledQuery1 =
        //                CompiledQuery.Compile<Entities, IQueryable<PMUser>>(ctx => (IQueryable<PMUser>)ctx.PMUsers.First());

        #region Ctors
        public DBAccess() { }
        #endregion

        #region Public Methods   
        public PMUser GetUser(string username, string md5password)
        {
            try
            {
                using (var DBEntities = new Entities())
                {                    
                    IQueryable<PMUser> Result = DBEntities.PMUsers.Where(u => u.Email == username && u.Password == md5password);                    

                    if (Result.Count() > 0)
                        return Result.First();
                }
            }
            catch(Exception ex)
            {
                ErrorsManager.SaveException(ex);
            }

            return new PMUser();
        }
        #endregion
    }
}
