using FMDSS.Models.Admin;
using FMDSS.Models.CitizenService.ProductionServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace FMDSS.Controllers.CitizenService.ProductionServices
{
    public class OnlinePurchasesController : BaseController
    {
        //
        // GET: /OnlinePurchases/
        Location location = new Location();
        List<SelectListItem> items = new List<SelectListItem>();
        
        List<InfoForm> item = new List<InfoForm>(); 
        public ActionResult OnlinePurchases()
        {
            //#region Division
            //DataTable dt = new DataTable();
            ///dt = location.BindDivision();
            //ViewBag.fname = dt;
            //foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            //{
            //    items.Add(new SelectListItem { Text = @dr["DIV_NAME"].ToString(), Value = @dr["DIV_CODE"].ToString() });
            //}

            //ViewBag.DivisionCode = items;
            //// ViewBag.ToLocation = items;
            //#endregion
            return View();
        }

        [HttpPost]
        public JsonResult GetProduceNursery(string purchasesCategory)
        {
            List<SelectListItem> items = new List<SelectListItem>();
           

            if (!String.IsNullOrEmpty(purchasesCategory))
            {
                if (purchasesCategory.Equals("Forest Produce"))
                {
                    items.Add(new SelectListItem { Text = "Tender Leaves", Value = "Tender Leaves" });
                    items.Add(new SelectListItem { Text = "Bamboo", Value = "Bamboo" });
                    items.Add(new SelectListItem { Text = "Timber", Value = "Timber" });
                    items.Add(new SelectListItem { Text = "Fuel Wood", Value = "Fuel Wood" });

                }
                else if (purchasesCategory.Equals("Nursery"))
                {

                }

            }
            return Json(new SelectList(items, "Value", "Text"));
        }

        

        [HttpPost]
        public JsonResult getNursery(string Vil)
        {
            ForestProduce dal = new ForestProduce();
            dal.Villages = Vil;
            DataSet ds = dal.Nursery();
            ViewBag.fname = ds.Tables[0];
            List<SelectListItem> nurs = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                nurs.Add(new SelectListItem { Text = @dr["Nursery_Name"].ToString(), Value = @dr["Nursery_Name"].ToString() });
            }
            return Json(new SelectList(nurs, "Value", "Text"));
        }

        [HttpPost]
        public ActionResult InfoForm(string Command, FormCollection form)
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            InfoForm IF = new InfoForm();
            ForestProduce fp = new ForestProduce();
            if (Command == "submit")
            {
                IF.District = form["Districts"].ToString();
                IF.Village = form["Village"].ToString();
                IF.Name = form["Nursery"].ToString();
                ds = IF.PlantCount();
                IF.Distribution = ds.Tables[0].Rows[0][0].ToString();
                IF.Department = ds.Tables[1].Rows[0][0].ToString();
                IF.BigPlant = ds.Tables[2].Rows[0][0].ToString();

                IF.Type = "Distribution";
                ds1 = IF.PlantType("Distribution");
                ViewData["PType"] = ds1.Tables[0];

                ds2 = IF.PlantType("Department");
                ViewData["PType1"] = ds2.Tables[0];

                ds3 = IF.PlantType("Big Plant");
                ViewData["PType2"] = ds3.Tables[0];
                Session["aa"] = null;
                return View(IF);
            }
            else if (Command == "Payment")
            {
                return RedirectToAction("Payment", "ForestProduce");
            }
            else if (Command == "Cancel")
            {

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("ForestProduce", fp);
            }
        }
        public ActionResult Payment()
        {
            InfoForm IF = new InfoForm();
            ForestProduce fp = new ForestProduce();
            if (Session["RequestId"] != null)
            {
                int totalPrice = 0;
                IF.EnterBy = Session["User"].ToString();
                DataTable dtShop;
                //dsShop = IF.GetShoppinglist(Session["RequestId"].ToString());
                dtShop = Session["aa"] as DataTable;
                foreach (System.Data.DataRow dr in dtShop.Rows)
                {
                    if (dr["Status"].ToString() == "ShoppingCart")
                    {
                        totalPrice += Convert.ToInt32(dr["Total_Price"].ToString());
                    }
                }
                if (dtShop.Rows.Count > 0)
                {
                    ViewData.Model = dtShop.Select("Status='ShoppingCart'").AsEnumerable();
                    Session["TotalCost"] = totalPrice;
                }
                if (ViewData.Model != null && totalPrice != 0)
                {
                    //return View("Payment");
                    return View();
                }
                else
                {
                    return View("ForestProduce", fp);
                }
            }
            else
            {
                return View("InfoForm");
            }
        }
        //[HttpPost]
        //public void Pay()
        //{

        //    Payment pay = new Payment();
        //    string reqstr = pay.RequestString("FMDSS", Session["RequestId"].ToString(), Session["TotalCost"].ToString(), "http://localhost:49806/ForestProduce/Payment", Session["User"].ToString(), "", "");
        //    string checksum = pay.sha256(reqstr);
        //    string RequestChecksum = reqstr + "|" + checksum;
        //    EncryptDecrypt3DES encryptDecrypt = new EncryptDecrypt3DES(RequestChecksum);
        //    string encrypt = encryptDecrypt.encrypt(RequestChecksum);

        //    // string checksum = sha256(request);
        //    // string RequestChecksum = request + "|" + checksum;
        //    //string encrypted = Encrypt(RequestChecksum, false);
        //    //EncryptDecrypt3DES encryptDecrypt = new EncryptDecrypt3DES(RequestChecksum);
        //    //string encrypt = encryptDecrypt.encrypt(RequestChecksum);
        //    //string dcrypt = encryptDecrypt.decrypt(encrypt);
        //    //Response.Redirect("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt);
        //    // posttopage("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt);

        //    WebRequest request = WebRequest.Create("http://emitra.gov.in/backoffice/EM_TPS.jsp?trnParams=" + encrypt);
        //    WebResponse response = request.GetResponse();
        //    Stream dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = reader.ReadToEnd();
        //}

        private void posttopage(string URL)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat(@"<body style='background-color:#F0F0F0;' onload='document.forms[""form""].submit()'>");
            sb.AppendFormat("<form name='form' action='{0}' method='post'>", URL);
            sb.AppendFormat("<div style='float:left; width:100%; height:100%;'>");
            sb.AppendFormat("<div style='float:left; width:100%; height:100%; margin-top:10%;'>	");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center; font-size:30px; color:#525252; margin:0 0 50px 0;'>Please wait while you are being redirected for </div>", " payment");
            sb.AppendFormat("<div style='float:left; width:100%; text-align:center;'>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("</div>");
            sb.AppendFormat("<div>");
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
        }


        [HttpPost]
        public JsonResult getSpecies(string DistName, string Village, string Nursery, string SelectType)
        {
            InfoForm IF = new InfoForm();
            IF.District = DistName;
            IF.Village = Village;
            IF.Name = Nursery;
            IF.Type = SelectType;
            DataSet ds = IF.SpeciesType();
            ViewBag.fname = ds.Tables[0];
            List<SelectListItem> Species = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                Species.Add(new SelectListItem { Value = @dr["Plant_Id"].ToString(), Text = @dr["Plant_Species"].ToString() });
            }
            return Json(new SelectList(Species, "Value", "Text"));
        }
        [HttpPost]
        public JsonResult getBigplant()
        {
            InfoForm IF = new InfoForm();

            DataSet ds = IF.SpeciesBigPlant();
            ViewBag.fname = ds.Tables[0];
            List<SelectListItem> bigplant = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.fname.Rows)
            {
                bigplant.Add(new SelectListItem { Text = @dr["Plant_Species"].ToString(), Value = @dr["Plant_Species"].ToString() });
            }
            return Json(new SelectList(bigplant, "Value", "Text"));
        }
        [HttpPost]
        public string getTotalFees(string DistName, string Village, string Nursery, string SelectType, string Species, string BigPlants, string PlantCount)
        {
            InfoForm IF = new InfoForm();
            IF.District = DistName;
            IF.Village = Village;
            IF.Name = Nursery;
            IF.Type = SelectType;
            IF.Plant_Species = Species;
            IF.BigPlant = BigPlants;
            IF.Total_Plant = PlantCount;
            Session["Total_Plant"] = IF.Total_Plant;
            IF.getTotalCost();
            if (IF.Calulatedfees != null)
            {
                Session["Calulatedfees"] = IF.Calulatedfees.ToString();
                return "ok" + "#" + IF.Calulatedfees.ToString();
            }
            if (IF.ExceedRequest != null)
            {
                return "Exceed" + "#" + IF.ExceedRequest.ToString();
            }
            else if (IF.NotFound != null)
            {
                return "NF" + "#" + IF.NotFound.ToString();
            }
            else
            {
                return "NF" + "#";
            }
        }

        public ActionResult NurseryMonitor(string Command, FormCollection form)
        {
            InfoForm IF = new InfoForm();
            DataSet ds = new DataSet();
            //  System.IO.StringWriter sw = new System.IO.StringWriter();
            if (Command == "Save")
            {
                //IF.District = form["Districts"].ToString();
                //IF.Village = form["Village"].ToString();
                //IF.Name = form["Nursery"].ToString();
                //IF.Type = form["SelectType"].ToString();
                //ds.ReadXml(Server.MapPath("~/Views/Shared/"+ Session["filename"].ToString() + ".xml"));                
                //ds.WriteXml(sw);
                //string strXml = sw.ToString();
                //int recordsInserted=IF.InsertXml(strXml);
                //ViewData["RecordInsert"] = recordsInserted;
                IF.Id = 0;
                IF.District = "";
                IF.Village = "";
                IF.Name = "";
                IF.Type = "";
                IF.Plant_Species = "";
                IF.Total_Plant = "";
                IF.Rate_Per_Plant = "";
                IF.EnterBy = Session["User"].ToString();
                IF.Option = 2;
                string str = IF.Insert_NurseryInfo();

                ViewData["RecordInsert"] = str;
            }
            if (Command == "Cancel")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public JsonResult getNurserytbl(string DistName, string Village, string Nursery, string SelectType)
        {
            InfoForm IF = new InfoForm();
            IF.District = DistName;
            IF.Village = Village;
            IF.Name = Nursery;
            IF.Type = SelectType;

            DataSet ds = IF.PlantType(SelectType);
            ViewBag.fname = ds.Tables[0];
            List<InfoForm> listitem = new List<InfoForm>();
            if (ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    listitem.Add(new InfoForm { Plant_Species = dr["Plant_Species"].ToString(), Total_Plant = dr["Total_Plant"].ToString(), Rate_Per_Plant = dr["Rate_Per_Plant"].ToString() });
                }
            }
            return Json(listitem, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddSpeciesInfo(int id, string District, string Village, string Nursery, string SelectType, string Species, string noofplants, string rateperplant, string action)
        {
            InfoForm IF = new InfoForm();
            DataSet ds = new DataSet();
            string status = "";
            try
            {
                IF.Id = id;
                IF.District = District;
                IF.Village = Village;
                IF.Name = Nursery;
                IF.Type = SelectType;
                IF.Plant_Species = Species;
                IF.Total_Plant = noofplants;
                IF.Rate_Per_Plant = rateperplant;
                IF.EnterBy = Session["User"].ToString();
                if (action == "Insert")
                {
                    IF.Option = 1;
                }
                else if (action == "Delete")
                {
                    IF.Option = 3;
                }
                else
                {

                }
                status = IF.Insert_NurseryInfo();
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            if (status == "Insert")
            {
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Delete")
            {
                IF.Action = "Delete";
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
            else
            {
                IF.Action = "Update";
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult PostSpeciesInfo(int id, string Species, string noofplants, string rateperplant, string action)
        {
            #region XmlCoding
            InfoForm IF = new InfoForm();
            DataSet ds = new DataSet();
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                IF.Plant_Species = Species;
                IF.Total_Plant = noofplants;
                IF.Rate_Per_Plant = rateperplant;
                IF.Action = action;

                if (action == "Edit" || action == "Delete")
                {
                    IF.Id = id;
                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["Plant_Id"].ToString() == IF.Id.ToString())
                                {
                                    if (action == "Delete" || action == "Edit")
                                    {
                                        ds.Tables[0].Rows[i].Delete();
                                    }
                                    else
                                    {
                                        ds.Tables[0].Rows[IF.Id - 1]["Plant_Id"] = IF.Id.ToString();
                                        ds.Tables[0].Rows[IF.Id - 1]["Plant_Species"] = IF.Plant_Species.ToString();
                                        ds.Tables[0].Rows[IF.Id - 1]["Total_Plant"] = IF.Total_Plant.ToString();
                                        ds.Tables[0].Rows[IF.Id - 1]["Rate_Per_Plant"] = IF.Rate_Per_Plant.ToString();
                                    }
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {

                                        ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                                    }
                                    else
                                    {
                                        if (System.IO.File.Exists(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml")) == true)
                                        {
                                            System.IO.File.Delete(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                                        }
                                    }

                                }

                            }
                        }

                    }
                }
                else
                {
                    IF.Id = id;
                    string filename = DateTime.Now.Ticks.ToString();
                    if (Session["filename"] == null)
                    {
                        XmlDeclaration decl = xmldoc.CreateXmlDeclaration("1.0", "UTF-16", "");
                        xmldoc.InsertBefore(decl, xmldoc.DocumentElement);
                        XmlElement RootNode = xmldoc.CreateElement("Nursery");
                        xmldoc.AppendChild(RootNode);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/" + filename + ".xml"));
                        Session["filename"] = filename;
                    }
                    else
                    {
                        xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    }

                    XmlElement PLANT_TYPE = xmldoc.CreateElement("Plant_Type");
                    XmlElement ID = xmldoc.CreateElement("Plant_Id");
                    XmlElement SPECIES = xmldoc.CreateElement("Plant_Species");
                    XmlElement TOTAL = xmldoc.CreateElement("Total_Plant");
                    XmlElement RATE = xmldoc.CreateElement("Rate_Per_Plant");

                    ID.InnerText = IF.Id.ToString();
                    SPECIES.InnerText = Species;
                    TOTAL.InnerText = noofplants;
                    RATE.InnerText = rateperplant;


                    xmldoc.Load(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    ds.ReadXml(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["Plant_Id"].ToString() == IF.Id.ToString())
                                {

                                    ds.Tables[0].Rows[i].Delete();
                                    ds.WriteXml(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                                    break;
                                }
                            }
                        }
                    }


                    PLANT_TYPE.AppendChild(ID);
                    PLANT_TYPE.AppendChild(SPECIES);
                    PLANT_TYPE.AppendChild(TOTAL);
                    PLANT_TYPE.AppendChild(RATE);
                    xmldoc.DocumentElement.AppendChild(PLANT_TYPE);
                    xmldoc.Save(Server.MapPath("~/Views/Shared/" + Session["filename"].ToString() + ".xml"));
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            return Json(IF, JsonRequestBehavior.AllowGet);
            #endregion
        }

        [HttpPost]
        public JsonResult ShopingCartInfo(int id, string Species, string BigPlants, string NoPlant, string TFees, string PurchaseStatus)
        {
            InfoForm IF = new InfoForm();
            DataSet ds = new DataSet();

            string status = "";
            try
            {
                string RequestId = DateTime.Now.Ticks.ToString();
                if (Session["RequestId"] == null)
                {
                    Session["RequestId"] = RequestId;
                }
                DataTable dt = new DataTable();
                if (dt.Rows.Count == 0)
                {
                    dt.Columns.Add("Item_Id");
                    dt.Columns.Add("Request_Id");
                    dt.Columns.Add("Item");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Total_Price");
                    dt.Columns.Add("EnterBy");
                    dt.Columns.Add("Status");
                }

                IF.Id = id;
                IF.Plant_Species = Species;
                IF.BigPlant = BigPlants;
                IF.Total_Plant = NoPlant;
                IF.Rate_Per_Plant = TFees;
                IF.EnterBy = Session["User"].ToString();
                IF.OnlinePurchaseStatus = PurchaseStatus;
                if (PurchaseStatus == "addToCart")
                {
                    IF.OnlinePurchaseStatus = "ShoppingCart";
                }
                DataRow dtrow = dt.NewRow();
                dtrow["Item_Id"] = id.ToString();
                dtrow["Request_Id"] = Session["RequestId"].ToString();
                dtrow["Item"] = Species.ToString();
                dtrow["Quantity"] = NoPlant.ToString();
                dtrow["Total_Price"] = Convert.ToInt32(TFees);
                dtrow["EnterBy"] = Session["User"].ToString();
                dtrow["Status"] = IF.OnlinePurchaseStatus;
                dt.Rows.Add(dtrow);
                DataTable dts = new DataTable();
                if (Session["aa"] != null)
                {
                    dts = Session["aa"] as DataTable;
                    //if (PurchaseStatus == "addToCart" || PurchaseStatus == "wishlist")
                    //{
                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        if (dts.Rows[i]["Item_Id"].ToString() == id.ToString())
                        {
                            dts.Rows[i].Delete();
                            dts.AcceptChanges();
                            IF.Action = "Update";
                        }
                    }
                    //}
                    //else {
                    //    dt.Merge(dts);
                    //    dt.AcceptChanges();    
                    //}
                    dt.Merge(dts);
                    dt.AcceptChanges();
                }

                Session["aa"] = dt;
                if (PurchaseStatus == "wishlist")
                {
                    status = IF.Insert_Wishlist(Session["RequestId"].ToString());
                }
                else if (PurchaseStatus == "ShoppingCart")
                {
                    status = "ShoppingCart";
                    if (IF.Action == "")
                    {
                        IF.Action = "Insert";
                    }
                }
                else if (PurchaseStatus == "addToCart")
                {
                    status = IF.RemoveWishlist();
                    IF.Action = "Update";
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error" + ex);
            }
            if (status == "ShoppingCart")
            {
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Inserted")
            {
                IF.Action = "Insert";
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
            else if (status == "Updated")
            {
                IF.Action = "Update";
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(IF, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult WishListStatus()
        {
            DataSet ds = new DataSet();
            InfoForm IF = new InfoForm();
            IF.EnterBy = Session["User"].ToString();
            ds = IF.GetWishlst();
            //XmlDocument xmldoc = new XmlDocument();
            List<InfoForm> listitem = new List<InfoForm>();
            //xmldoc.Load(Server.MapPath("~/Views/Shared/WishList.xml"));
            //ds.ReadXml(Server.MapPath("~/Views/Shared/WishList.xml"));
            if (ds.Tables.Count > 0)
            {
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {

                    listitem.Add(new InfoForm { SL_NO = dr["Item_Id"].ToString(), Plant_Species = dr["Item"].ToString(), Total_Plant = dr["Quantity"].ToString(), Rate_Per_Plant = dr["Total_Price"].ToString() });
                }
            }
            return Json(listitem, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string RemoveWishlist(string Item_Id)
        {
            InfoForm IF = new InfoForm();
            IF.EnterBy = Session["User"].ToString();
            IF.Id = Convert.ToInt32(Item_Id);
            DataTable dts = new DataTable();
            if (Session["aa"] != null)
            {
                dts = Session["aa"] as DataTable;
                for (int i = 0; i < dts.Rows.Count; i++)
                {
                    if (dts.Rows[i]["Item_Id"].ToString() == Item_Id)
                    {
                        dts.Rows[i].Delete();
                        dts.AcceptChanges();
                    }
                }
            }
            Session["aa"] = dts;
            string status = IF.RemoveWishlist();
            return status + "#";
        }
        [HttpPost]
        public JsonResult AddToWishList(int slno, string item, string quantity, string fees, string action)
        {
            InfoForm IF = new InfoForm();
            DataSet ds = new DataSet();
            XmlDocument xmldoc = new XmlDocument();
            int flag = 0;

            //if (action == "Edit" || action == "Delete")
            //{
            //    IF.Id =slno;
            //}
            //else
            //{
            //    IF.Id = slno + 1;
            //}
            IF.Id = slno;
            IF.Plant_Species = item;
            IF.Total_Plant = quantity;
            IF.Rate_Per_Plant = fees;
            IF.Action = action;
            xmldoc.Load(Server.MapPath("~/Views/Shared/WishList.xml"));
            ds.ReadXml(Server.MapPath("~/Views/Shared/WishList.xml"));

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["SL_NO"].ToString() == IF.Id.ToString())
                        {
                            if (action == "Delete")
                            {
                                ds.Tables[0].Rows[IF.Id - 1].Delete();
                            }
                            else
                            {
                                ds.Tables[0].Rows[IF.Id - 1]["SL_NO"] = IF.Id.ToString();
                                ds.Tables[0].Rows[IF.Id - 1]["ITEM"] = IF.Plant_Species.ToString();
                                ds.Tables[0].Rows[IF.Id - 1]["QUANTITY"] = IF.Total_Plant.ToString();
                                ds.Tables[0].Rows[IF.Id - 1]["FEES"] = IF.Rate_Per_Plant.ToString();
                            }
                            ds.WriteXml(Server.MapPath("~/Views/Shared/WishList.xml"));
                            flag = 1;
                            break;
                        }
                        else
                        {
                            flag = 0;
                        }
                    }
                    if (flag == 0)
                    {
                        XmlElement PLANT_TYPE = xmldoc.CreateElement("PLANT_TYPE");
                        XmlElement SL_NO = xmldoc.CreateElement("SL_NO");
                        XmlElement ITEM = xmldoc.CreateElement("ITEM");
                        XmlElement QUANTITY = xmldoc.CreateElement("QUANTITY");
                        XmlElement FEES = xmldoc.CreateElement("FEES");

                        SL_NO.InnerText = IF.Id.ToString();
                        ITEM.InnerText = item;
                        QUANTITY.InnerText = quantity;
                        FEES.InnerText = fees;

                        PLANT_TYPE.AppendChild(SL_NO);
                        PLANT_TYPE.AppendChild(ITEM);
                        PLANT_TYPE.AppendChild(QUANTITY);
                        PLANT_TYPE.AppendChild(FEES);

                        xmldoc.DocumentElement.AppendChild(PLANT_TYPE);
                        xmldoc.Save(Server.MapPath("~/Views/Shared/WishList.xml"));
                    }
                }
            }
            else
            {

                XmlElement PLANT_TYPE = xmldoc.CreateElement("PLANT_TYPE");
                XmlElement SL_NO = xmldoc.CreateElement("SL_NO");
                XmlElement ITEM = xmldoc.CreateElement("ITEM");
                XmlElement QUANTITY = xmldoc.CreateElement("QUANTITY");
                XmlElement FEES = xmldoc.CreateElement("FEES");

                SL_NO.InnerText = IF.Id.ToString();
                ITEM.InnerText = item;
                QUANTITY.InnerText = quantity;
                FEES.InnerText = fees;


                PLANT_TYPE.AppendChild(SL_NO);
                PLANT_TYPE.AppendChild(ITEM);
                PLANT_TYPE.AppendChild(QUANTITY);
                PLANT_TYPE.AppendChild(FEES);

                xmldoc.DocumentElement.AppendChild(PLANT_TYPE);
                xmldoc.Save(Server.MapPath("~/Views/Shared/WishList.xml"));

            }

            return Json(IF, JsonRequestBehavior.AllowGet);
        }
    }
}
