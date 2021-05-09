using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Enterprise.Service.Base.ERP
{
   public  class WaterSpaceService : EntityRepositoryBase<DbContext, LHWaterSpace>
    {
        public WaterSpaceService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public WaterSpaceService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}
