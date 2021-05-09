using System;
using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    public class SalaryProducerSetService: EntityRepositoryBase<DbContext, LHSalaryProducerSet>
    {
        public SalaryProducerSetService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public SalaryProducerSetService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}