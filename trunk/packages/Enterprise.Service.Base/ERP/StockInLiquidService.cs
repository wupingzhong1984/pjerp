using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Enterprise.Service.Base.ERP
{
   public  class StockInLiquidService : EntityRepositoryBase<DbContext, LHStockInLiquid>
    {
        public StockInLiquidService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public StockInLiquidService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}
