using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FMDSS.Models.Home
{
    public class Home
    {
        public string GetCurrentMenus(Int16 CurrentRole)
        {
            if (CurrentRole == 0)
            {
                return "";
            }
            
            StringBuilder SB = new StringBuilder();

            List<Menus> OBJListMenus = new List<Menus>();
            OBJListMenus = (List<Menus>)HttpContext.Current.Session["Menus"];

            List<ROLEGROUPS> OBJROLEGROUPS = new List<ROLEGROUPS>();
            OBJROLEGROUPS = (List<ROLEGROUPS>)HttpContext.Current.Session["ROLEGROUPS"];
            
            var SelectedRole = OBJROLEGROUPS.Where(item => item.RoleId == CurrentRole ).FirstOrDefault();
            HttpContext.Current.Session["CurrentLayout"] = SelectedRole.DefaultLayout;

            HttpContext.Current.Session["CURRENT_ROLE"] = CurrentRole;
            var ListMenus1 = OBJListMenus.Where(item => item.RoleId == CurrentRole && item.ParentID == 0).ToList();

            string TargetBlank = "";
            string Nested = "";

            foreach (var row1 in ListMenus1)
            {
                if (row1.IsTargetBlank == true)
                {
                    TargetBlank = "target='_blank' rel ='noopener noreferrer'";
                }
                if (row1.IsNested == true)
                {
                    Nested = "<span class='fa arrow'></span>";
                }


                SB.Append("<li><a href='" + row1.PageURL + "' " + TargetBlank + "   ><i class='" + row1.IconClass + "'></i>" + row1.PageTitle + Nested + " </a>");

                var ListMenus2 = OBJListMenus.Where(item => item.ParentID == row1.PageID && item.RoleId == CurrentRole).ToList();

                if (ListMenus2.Count > 0)
                {
                    SB.Append("<ul class='nav nav-second-level collapse' aria-expanded='false'>");
                }
                foreach (var row2 in ListMenus2)
                {
                    TargetBlank = "";
                    Nested = "";

                    if (row2.IsTargetBlank == true)
                    {
                        TargetBlank = "target='_blank' rel ='noopener noreferrer'";
                    }
                    if (row2.IsNested == true)
                    {
                        Nested = "<span class='fa arrow'></span>";
                    }


                    SB.Append("<li><a href='" + row2.PageURL + "' " + TargetBlank + " ><i class='" + row2.IconClass + "'></i>" + row2.PageTitle + Nested + "</a>");

                    var ListMenus3 = OBJListMenus.Where(item => item.ParentID == row2.PageID && item.RoleId == CurrentRole).ToList();

                    if (ListMenus3.Count > 0)
                    {
                        SB.Append("<ul class='nav nav-third-level collapse' aria-expanded='false'>");
                    }
                    foreach (var row3 in ListMenus3)
                    {
                        TargetBlank = "";
                        Nested = "";

                        if (row3.IsTargetBlank == true)
                        {
                            TargetBlank = "target='_blank' rel ='noopener noreferrer'";
                        }
                        if (row3.IsNested == true)
                        {
                            Nested = "<span class='fa arrow'></span>";
                        }

                        SB.Append("<li><a href='" + row3.PageURL + "' " + TargetBlank + "><i class='" + row3.IconClass + "'></i>" + row3.PageTitle + Nested + "</a>");

                        SB.Append("</li>");

                    }

                    if (ListMenus3.Count > 0)
                    {
                        SB.Append("</ul>");
                    }


                    SB.Append("</li>");
                }
                if (ListMenus2.Count > 0)
                {
                    SB.Append("</ul>");
                }

                SB.Append("</li>");

            }

            return SB.ToString();
        }

        public string GetCurrentMenusForWildLifeKiosk(Int16 CurrentRole,List<Menus> Menu)
        {
            if (CurrentRole == 0)
            {
                return "";
            }

            StringBuilder SB = new StringBuilder();

            List<Menus> OBJListMenus = new List<Menus>();
            OBJListMenus = Menu;

            List<ROLEGROUPS> OBJROLEGROUPS = new List<ROLEGROUPS>();
            OBJROLEGROUPS = (List<ROLEGROUPS>)HttpContext.Current.Session["ROLEGROUPS"];

            var SelectedRole = OBJROLEGROUPS.Where(item => item.RoleId == CurrentRole).FirstOrDefault();
            HttpContext.Current.Session["CurrentLayout"] = " ~/Views/Shared/_kiosklayoutWildlife.cshtml";

            HttpContext.Current.Session["CURRENT_ROLE"] = CurrentRole;
            var ListMenus1 = OBJListMenus.Where(item =>  item.ParentID == 0).ToList();

            string TargetBlank = "";
            string Nested = "";

            foreach (var row1 in ListMenus1)
            {
                if (row1.IsTargetBlank == true)
                {
                    TargetBlank = "target='_blank'";
                }
                if (row1.IsNested == true)
                {
                    Nested = "<span class='fa arrow'></span>";
                }


                SB.Append("<li><a href='" + row1.PageURL + "' " + TargetBlank + "   ><i class='" + row1.IconClass + "'></i>" + row1.PageTitle + Nested + " </a>");

                var ListMenus2 = OBJListMenus.Where(item => item.ParentID == row1.PageID).ToList();

                if (ListMenus2.Count > 0)
                {
                    SB.Append("<ul class='nav nav-second-level collapse' aria-expanded='false'>");
                }
                foreach (var row2 in ListMenus2)
                {
                    TargetBlank = "";
                    Nested = "";

                    if (row2.IsTargetBlank == true)
                    {
                        TargetBlank = "target='_blank'";
                    }
                    if (row2.IsNested == true)
                    {
                        Nested = "<span class='fa arrow'></span>";
                    }


                    SB.Append("<li><a href='" + row2.PageURL + "' " + TargetBlank + " ><i class='" + row2.IconClass + "'></i>" + row2.PageTitle + Nested + "</a>");

                    var ListMenus3 = OBJListMenus.Where(item => item.ParentID == row2.PageID ).ToList();

                    if (ListMenus3.Count > 0)
                    {
                        SB.Append("<ul class='nav nav-third-level collapse' aria-expanded='false'>");
                    }
                    foreach (var row3 in ListMenus3)
                    {
                        TargetBlank = "";
                        Nested = "";

                        if (row3.IsTargetBlank == true)
                        {
                            TargetBlank = "target='_blank'";
                        }
                        if (row3.IsNested == true)
                        {
                            Nested = "<span class='fa arrow'></span>";
                        }

                        SB.Append("<li><a href='" + row3.PageURL + "' " + TargetBlank + "><i class='" + row3.IconClass + "'></i>" + row3.PageTitle + Nested + "</a>");

                        SB.Append("</li>");

                    }

                    if (ListMenus3.Count > 0)
                    {
                        SB.Append("</ul>");
                    }


                    SB.Append("</li>");
                }
                if (ListMenus2.Count > 0)
                {
                    SB.Append("</ul>");
                }

                SB.Append("</li>");

            }

            return SB.ToString();
        }
    }
}