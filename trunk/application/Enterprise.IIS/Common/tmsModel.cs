using Enterprise.Service.Base;
using System.Collections.Generic;

namespace Enterprise.IIS.Common
{
    public  class tmsModel
    {
        public List<LHPassCard> passCardList { get; set; }
        public List<LHPassCardDetails> passCardDetailsList { get; set; }


        public List<LHStockInDetails> stockInDetailsList { get; set; }
        public List<LHStockIn> stockInList { get; set; }

        public List<LHStockOutDetails> stockOutDetailsList { get; set; }
        public List<LHStockOut> stockOutList { get; set; }



        public List<LHDispatchCenter> dispatchCenterList { get; set; }


        public List<LHDispatchDetails> DispatchDetailsList { get; set; }
    }
}
