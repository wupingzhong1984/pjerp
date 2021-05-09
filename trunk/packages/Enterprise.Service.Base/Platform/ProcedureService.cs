using System.Data.Entity;
using System.Data.Objects;
using Enterprise.Framework.DataBase;
using Enterprise.Framework.EntityRepository;

namespace Enterprise.Service.Base.Platform
{
    /// <summary>
    /// 存储过程调用
    /// </summary>
    public class ProcedureService : EntityRepositoryBase<DbContext, base_sequence>
    {
        public ProcedureService(DbContext context)
        {
            base.Context = context;
            IsOwnContext = false;
        }

        public ProcedureService()
        {
            base.Context = DbContextHelper.CreateDbContextByEdmxName("HNLH_BASE");
            IsOwnContext = true;
        }

        /// <summary>
        /// 取数据表所占库大小
        /// </summary>
        /// <returns></returns>
        public ObjectResult<proc_GetTableSize_Result> GetTableSize()
        {
            return base.ObjContext.ExecuteFunction<proc_GetTableSize_Result>
                ("proc_GetTableSize", new ObjectParameter[0]);
        }

        /// <summary>
        /// 取所有表信息
        /// </summary>
        /// <returns></returns>
        public ObjectResult<proc_GetTables_Result> GetTables()
        {
            return base.ObjContext.ExecuteFunction<proc_GetTables_Result>
                ("proc_GetTables", new ObjectParameter[0]);
        }

        /// <summary>
        /// 取表所有字段信息
        /// </summary>
        /// <returns></returns>
        public ObjectResult<proc_GetTableStructure_Result> GetTableStructure(string tablename)
        {
            var parameter = new ObjectParameter("tablename", tablename);
            return base.ObjContext.ExecuteFunction<proc_GetTableStructure_Result>
                ("proc_GetTableStructure", parameter);
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        public void BackDatabase()
        {
            base.ObjContext.ExecuteFunction
                ("proc_BackDatabase", new ObjectParameter[0]);
        }
    }
}