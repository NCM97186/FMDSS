using FMDSS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FMDSS.Controllers
{
    public class GisController : ApiController
    {
        // GET api/gis
        
        GisServices oModel = new GisServices();

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        
        public List<DepoList> GetDepoList()
        {
            return oModel.GetDepoList();
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<NURSERYList> NurseriesList()
        {
            return oModel.NurseriesList();
        }


        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ProductList> ProductList()
        {
            return oModel.ProductList();
        }

      //  [System.Web.Http.AcceptVerbs("GET", "POST")]
        //public List<GetDepotWiseAvailableForestProduce> GetDepotListBySSOID(string SSOID)
        //{
        //   return oModel.GetDepotWiseAvailableForestProduce(REG_CODE, Circle_Code, DIV_Code, RANGE_CODE, Depo_id);
            
        //}



        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<GetDepotWiseAvailableForestProduce> GetDepotWiseAvailableForestProduce(string REG_CODE, string Circle_Code, string DIV_Code, string RANGE_CODE, string Depo_id)
        {
            return oModel.GetDepotWiseAvailableForestProduce(REG_CODE, Circle_Code, DIV_Code, RANGE_CODE, Depo_id);
            //return Models.GetDepotWiseAvailableForestProduce()
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<GetDepotWiseAvailableForestProduce> GetNurseryWiseAvailableForestProduce(string Dist_CODE, string Block_Code, string GP_Code, string Vill_Code, string Nursery_id)
        {
            return oModel.GetNurseryWiseAvailableForestProduce(Dist_CODE, Block_Code, GP_Code, Vill_Code, Nursery_id);
           
        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ProgressEntry> GetProgress(string MicroPlan_Code, string Vill_Code, string Dist_Id)
        {
            return oModel.GetProgress(MicroPlan_Code, Vill_Code, Dist_Id);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<ProgressEntryDistDivRange> GetProgressAdminDistDivVillage(string Dist_Code, string Div_Code, string Vill_Code)
        {
            return oModel.GetProgressAdminDistDivVillageWise(Dist_Code, Div_Code, Vill_Code);
        }

        public List<ProgressEntryDistDivRange> GetProgressForestCirDivRange(string CIRCLE_CODE, string Div_Code, string Range_Code)
        {
            return oModel.GetProgressForestCirDivRangeWise(CIRCLE_CODE, Div_Code, Range_Code);
        }





         
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<permissionWithKML> GetpermissionWithKML(string SSOID, string KML)
        {
            return oModel.GetpermissionWithKMLs(SSOID, KML);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<RegistrationOfOffense> GetRegistrationOfOffense(string CIRCLE_CODE, string DIV_CODE, string RANGE_CODE)
        {
            return oModel.GetRegistrationOfOffenses(CIRCLE_CODE, DIV_CODE, RANGE_CODE);
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<GetDepotWiseAvailableForestProduce> GetDepotWiseAvailableForestProduceBySSOID(string SSOID, string reqID)
        {
            return oModel.GetDepotWiseAvailableForestProducebySSOID(SSOID, reqID);

        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        public List<NurseryWiseAvailableForestProduce> GetNurseryWiseAvailableForestProduceBySSOID(string SSOID, string reqID)
        {
            return oModel.GetNurseryWiseAvailableForestProducebySSOID(SSOID, reqID);

        }


        //[System.Web.Http.AcceptVerbs("GET", "POST")]
        //public List<ProgressEntry> GetProgress(string MicroPlan_Code, string Vill_Code, string Dist_Id)
        //{
        //    return oModel.GetProgress(MicroPlan_Code, Vill_Code, Dist_Id);
        //}
        // GET api/gis/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/gis
        public void Post([FromBody]string value)
        {
        }

        // PUT api/gis/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/gis/5
        public void Delete(int id)
        {
        }
    }
}
