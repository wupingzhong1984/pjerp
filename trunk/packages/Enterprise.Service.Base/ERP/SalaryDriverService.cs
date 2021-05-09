using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Enterprise.Service.Base.ERP
{
    public class SalaryDriverService : EntityRepositoryBase<DbContext, LHSalaryDriver>
    {
        public SalaryDriverService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SalaryDriverService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}
