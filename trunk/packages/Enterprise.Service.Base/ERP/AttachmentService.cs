using System.Data.Entity;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.ERP
{
    /// <summary>
    ///     单据审批过程
    /// </summary>
    public  class AttachmentService: EntityRepositoryBase<DbContext, LHAttachment>
    {
        public AttachmentService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public AttachmentService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }
    }
}