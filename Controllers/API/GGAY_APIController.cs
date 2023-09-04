using FMDSS.CustomModels.Models;
using FMDSS.Models.ForestProduction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMDSS.Controllers.API
{
    public class GGAY_APIController : ApiController
    {
		#region API FOR GGAY
		[HttpGet]
		public DataSetResponse GetNurseryStockGGAY(string NURSERY_CODE)
		{
			//System.Web.HttpContext.Current.Session["UserId"] = UserID;			
			//System.Web.HttpContext.Current.Session["SSOId"] = SSOId;

			DataSetResponse response = new DataSetResponse();
			InventoryManagement objInvntry = new InventoryManagement();
			try
			{
				DataSet dsProduces = new DataSet();
				dsProduces = objInvntry.GetNurseryStockGGAY(NURSERY_CODE);

				response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

			}
			catch (Exception ex)
			{
				response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
			}
			return response;
		}
		[HttpPost]
		public DataSetResponse UpdateNurseryStockGGAY(GGAY_Details ggayObj)
		{
		
			DataSetResponse response = new DataSetResponse();
			InventoryManagement objInvntry = new InventoryManagement();
			try
			{
				DataSet dsProduces = new DataSet();
				dsProduces = objInvntry.UpdateNurseryStockGGAY(ggayObj.Nursery_Code, ggayObj.SSO_ID, ggayObj.ProductOutList);

				response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

			}
			catch (Exception ex)
			{
				response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
			}
			return response;
		}
        
        [HttpGet]
        public DataSetResponse GetNurseryMasterDataGGAY(string NURSERY_CODE="", string DIV_CODE = "")
        {
            DataSetResponse response = new DataSetResponse();
            InventoryManagement objInvntry = new InventoryManagement();
            try
            {
                DataSet dsProduces = new DataSet();
                dsProduces = objInvntry.GetNurseryMasterGGAY(NURSERY_CODE, DIV_CODE);

                response = new DataSetResponse { Status = ResponseStatus.Success, Message = "Success", Data = dsProduces };

            }
            catch (Exception ex)
            {
                response = new DataSetResponse { Status = ResponseStatus.Failed, Message = "Error Occurs", ErrorDescription = ex.Message };
            }
            return response;
        }
        #endregion
    }
}
