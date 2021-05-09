using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Enterprise.Service.Base.ERP
{
   public class ItemPurchaseService : EntityRepositoryBase<DbContext, vm_ItemsPurchasePrice>
    {
        public ItemPurchaseService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ItemPurchaseService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}
