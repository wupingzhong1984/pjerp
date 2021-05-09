﻿using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据审批过程
    /// </summary>
    public class BillFlowServie : EntityRepositoryBase<DbContext, LHBillFlow>
    {
        public BillFlowServie(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public BillFlowServie()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}