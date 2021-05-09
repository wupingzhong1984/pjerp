﻿using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class LeaseReturnDetailsLogService: EntityRepositoryBase<DbContext, LHLeaseReturnDetails_Log>
    {
        public LeaseReturnDetailsLogService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public LeaseReturnDetailsLogService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}