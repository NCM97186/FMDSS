<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Request.aspx.cs" Inherits="ForestryWebGIS.Request" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>FMDSS</title>
    <script src="https://code.jquery.com/jquery-2.2.3.min.js"></script>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function submitPost(evt, actionUrl) {
            if (evt.id == "mine_Req") {
                villagedraw
                document.getElementById("requestFor").value = "Mines";
                document.getElementById("fileName").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "cable_Req") {
                document.getElementById("requestFor").value = "Cable";
                document.getElementById("fileName").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            if (evt.id == "mine") {
                document.getElementById("requestFor").value = "Mines";
                document.getElementById("fileName").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "cble") {
                document.getElementById("requestFor").value = "Cable";
                document.getElementById("fileName").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "villagedraw") {
                document.getElementById("requestFor").value = "Mines";
                document.getElementById("fileName").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "gis_Req") {
                document.getElementById("requestFor").value = "Mines";
                document.getElementById("fileName").value = "95ddbc01-2561-465d-b5fd-e96217bcf11d.kml";
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "draw_Req") {
                document.getElementById("villageId").value = "099449,098845,098855";
                document.getElementById("type").disabled = true;
                document.getElementById("requestFor").value = "JFMCArea";
                document.getElementById("fileName").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "jfmc_Req") {
                document.getElementById("villageId").value = "099449,098845,098855,099331,099812";
                document.getElementById("type").value = "New";
                document.getElementById("requestFor").disabled = true;
                document.getElementById("fileName").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "depot_Req") {
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("requestFor").value = "Depot";
                document.getElementById("fileName").disabled = true;
                document.getElementById("returnURL").disabled = true;
                document.getElementById("reqID").value = "2";

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "nursery_Req") {
                document.getElementById("villageId").disabled = true;
                document.getElementById("type").disabled = true;
                document.getElementById("requestFor").value = "Nursery";
                document.getElementById("fileName").disabled = true;
                document.getElementById("returnURL").disabled = true;
                document.getElementById("reqID").value = "nur04";

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "offense_Req") {
                document.getElementById("villageId").value = "099449";
                document.getElementById("type").disabled = true;
                document.getElementById("requestFor").disabled = true;
                document.getElementById("fileName").disabled = true;
                document.getElementById("reqID").disabled = true;

                document.getElementById("themeName").disabled = true;
                document.getElementById("adminType").disabled = true;
                document.getElementById("themeData").disabled = true;
                document.getElementById("caption").disabled = true;
            }
            else if (evt.id == "thematic_Req") {
                document.getElementById("type").disabled = true;
                document.getElementById("requestFor").disabled = true;
                document.getElementById("fileName").disabled = true;
                document.getElementById("reqID").disabled = true;
                document.getElementById("villageId").disabled = true;
                document.getElementById("returnURL").disabled = true;                

                document.getElementById("themeName").value = 'VFPMC';
                document.getElementById("adminType").value = 'District';
                document.getElementById("themeData").value = '';
                document.getElementById("caption").value = "No. of VFPMCs";
            }
            document.getElementById("postData").action = actionUrl;
            document.getElementById("postData").submit();
        }
    </script>
</head>
<body>
    <div class="container">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <h2 class="text-center text-uppercase"><span class="glyphicon glyphicon-leaf" aria-hidden="true"></span>&nbsp;Forestry Web GIS</h2>
            </div>
            <div class="panel-body">
                <form id="postData" name="postData" method="post" runat="server">
                    <div class="row">
                        <div class="col-lg-4 text-center">
                            <a class='btn btn-success' href="javascript:void();" target="_parent" id="mine_Req" onclick="submitPost(this,'permitlocation/GetRequest.aspx');">Mine Request</a>
                            <a class='btn btn-danger' href="javascript:void();" target="_blank" id="cable_Req" onclick="submitPost(this,'permitlocation/GetRequest.aspx');">Cable Request</a>
                            <a class='btn btn-primary' href="javascript:void();" target="_blank" id="gis_Req" onclick="submitPost(this,'gisview/ViewonGis.aspx');">View on GIS</a>
                            <a class='btn btn-primary' href="javascript:void();" target="_blank" id="mine" onclick="submitPost(this,'permitlocation/GetRequest1.aspx');">OLD Mine Req</a>
                            <a class='btn btn-danger' href="javascript:void();" target="_blank" id="cble" onclick="submitPost(this,'permitlocation/GetRequest1.aspx');">OLD Line Req</a>
                            <a class='btn btn-danger' href="javascript:void();" target="_blank" id="villagedraw" onclick="submitPost(this,'getvillagedata/GetVillageIds.aspx');">Village Draw</a>
                        </div>
                        <div class="col-lg-4 text-center">
                            <%--,098845,098855,099331,099812--%>
                            <a class='btn btn-warning' href="javascript:void();" target="_blank" id="draw_Req" onclick="submitPost(this,'activity/drawRequest.aspx');">Draw Request</a> &nbsp;
                             <a class='btn btn-info' href="javascript:void();" target="_blank" id="thematic_Req" onclick="submitPost(this,'kpi/theme.aspx');">Thematic Page</a>
                        </div>
                        <div class="col-lg-4 text-center">
                            <a class='btn btn-info' href="javascript:void();" target="_blank" id="depot_Req" onclick="submitPost(this,'DNM/dnm.aspx');">Depot Request</a>
                            <a class='btn btn-info' href="javascript:void();" target="_blank" id="nursery_Req" onclick="submitPost(this,'DNM/dnm.aspx');">Nursery Request</a>
                            <a class='btn btn-info' href="javascript:void();" target="_blank" id="offense_Req" onclick="submitPost(this,'markoffense/offense.aspx');">Offense Request</a>
                        </div>
                    </div>
                    <input type="hidden" id="portalid" name="portalid" value="rajcomp123" />
                    <input type="hidden" id="ssoid" name="ssoid" value="ashokyadav" />
                    <input type="hidden" id="returnURL" name="returnURL" value="/fmdss.aspx" />
                    <input type="hidden" id="requestFor" name="requestFor" value="" />
                    <input type="hidden" id="villageId" name="villageId" value="" />
                    <input type="hidden" id="type" name="type" value="" />
                    <input type="hidden" id="fileName" name="fileName" value="" />
                    <input type="hidden" id="reqID" name="reqID" value="" />

                     <input type="hidden" id="themeName" name="themeName" value="" />
                    <input type="hidden" id="adminType" name="adminType" value="" />
                    <input type="hidden" id="themeData" name="themeData" value="" />
                    <input type="hidden" id="caption" name="caption" value="" />
                </form>
            </div>
        </div>
    </div>
</body>
</html>
