<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dnm.aspx.cs" Inherits="ForestryWebGIS.DNM.dnm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <!-- #include file ="../dojopackage.html" -->​
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>Depot Nursery Management</title>
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/themes/calcite/dijit/calcite.css" />
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/themes/calcite/esri/esri.css" />
    <style>
        #mapDiv {
            height: 100%;
            min-height: 680px;
            margin: 0;
            padding: 0;
            width: 100%;
        }
          .logo_raj {
            position: absolute;
            width: 80px;
            bottom: 3px;
            right:30px;
        }

        .calcite .esriPopup .titlePane .title {
            padding-right: 2px;
        }
    </style>

    <script src="//code.jquery.com/jquery-2.2.4.min.js"></script>    
    <script src="//js.arcgis.com/3.16/"></script>
    <script>
        var map, content, dynamicForest, serviceUrl, currentConfig = null;
        var ResultData = [];
        var legendLayers = [];
        var visible = [];
        var reqFor = '<% = requestFor %>';
        var reqID = '<% = requestID %>';
        var ssoID = '<% = ssoID %>';
        require([
        "esri/map",
        "esri/graphic",
        "esri/layers/ArcGISDynamicMapServiceLayer",
        "esri/layers/FeatureLayer",
        "esri/symbols/SimpleMarkerSymbol",
        "esri/tasks/query",
        "esri/Color",
        "esri/dijit/Legend",
        "dojo/_base/array",
              "DojoClass/TokenAuth",
        "dojo/query",
        "dojo/parser",
        "dojo/dom",
         "dojo/on",
        "dojo/text!../scripts/serviceURL.json",
        "dojo/domReady!"
        ],
        function (
          Map, Graphic, ArcGISDynamicMapServiceLayer, FeatureLayer, SimpleMarkerSymbol, Query, Color,
           Legend, array,TokenAuth, query, parser, dom, on, serviceURL) {
            parser.parse();
            serviceUrl = JSON.parse(serviceURL);
            for (var i = 0; i < serviceUrl.layers.length; i++) {
                if (serviceUrl.layers[i].layerName == reqFor) {
                    currentConfig = serviceUrl.layers[i];
                }
            }
            getDatafromService();
            map = new Map("mapDiv", {
                sliderOrientation: "horizontal",
                sliderPosition: "top-center",
                logo: false
            });
            map.on("load", function () {
                document.getElementById("noMap").style.display = "none";
            });
            var adminBoundry = new ArcGISDynamicMapServiceLayer(serviceUrl.adminBoundryUrl, {
                "opacity": 1.0,
                id: "adminBoundry"
            });
            dynamicForest = new ArcGISDynamicMapServiceLayer(serviceUrl.dynamicMapServerUrl, {
                "opacity": 0.9,
                id: "forestBoundry"
            });

            map.on('layers-add-result', afterlayerAdd);
            legendLayers.push({ layer: dynamicForest, title: "Legends" });
            map.addLayers([adminBoundry, dynamicForest]);
            dynamicForest.on("load", buildLayerList);
            function buildLayerList() {
                var items = array.map(dynamicForest.layerInfos, function (info, index) {
                    if (info.defaultVisibility) {
                        visible.push(info.id);
                    }
                    return "<input type='checkbox' class='list_item'" + (info.defaultVisibility ? "checked=checked" : "") + "' id='" + info.id + "'' />&nbsp;<label for='" + info.id + "'>" + info.name + "</label>&nbsp;";
                });
                var ll = dom.byId("layer_list");
                ll.innerHTML = items.join(' ');
                dynamicForest.setVisibleLayers(visible);
                on(ll, "click", updateLayerVisibility);
            }
            function updateLayerVisibility() {
                var inputs = query(".list_item");
                var input;
                visible = [];
                array.forEach(inputs, function (input) {
                    if (input.checked) {
                        visible.push(input.id);
                    }
                });
                //if there aren't any layers visible set the array to be -1
                if (visible.length === 0) {
                    visible.push(-1);
                }
                dynamicForest.setVisibleLayers(visible);
            }
            document.getElementById("lbl3").innerHTML = reqFor;
            var forestLayer = null;
            forestLayer = new FeatureLayer(currentConfig.layerUrl, {
                mode: FeatureLayer.MODE_ONDEMAND,
                outFields: ["*"],
                id: "forestLayer"
            });

            // Query for the features with the given object ID

            var queryPoint = new Query();
            var serviceIds = '0';
            queryPoint.outFields = [currentConfig.titleField, currentConfig.layerIDField];
            serviceIds = array.map(ResultData, function (data) {
                return data.GIS_ID;
            });
            if (reqFor == "Depot")
                queryPoint.where = "ASSETTYPE='Depot' and OBJECTID in(" + serviceIds + ")";
            else
                queryPoint.where = "ASSETTYPE='Nursery' and OBJECTID in(" + serviceIds + ")";
            forestLayer.queryFeatures(queryPoint, function (featureSet) {
                for (var i = 0; i < featureSet.features.length; i++) {
                    var graphic = featureSet.features[i];
                    graphic.setSymbol(forestLayer.renderer.getSymbol(graphic));
                    map.graphics.add(graphic);
                }
            });
            function afterlayerAdd() {
                map.infoWindow.resize(350, 210);
                var forestLayer = map.getLayer("forestLayer");
                var title, graphicAttributes, Id;
                map.graphics.on("click", function (evt) {
                    content = "";
                    graphicAttributes = evt.graphic.attributes;
                    if (ResultData.length == 0) {
                        content = "No Data";
                    }
                    else {
                        for (var i = 0; i < ResultData.length; i++) {
                            var obj = ResultData[i];
                            if (obj[currentConfig.serviceIDField] == graphicAttributes[currentConfig.layerIDField]) {
                                for (var j = 0; j < currentConfig.contentFields.length; j++) {
                                    content += currentConfig.contentFields[j].alias + ": <b>" + obj[currentConfig.contentFields[j].fieldName] + " </b><br>";
                                }
                                content += "<br>";
                            }
                        }
                    }
                    map.infoWindow.setTitle(graphicAttributes[currentConfig.titleField]);
                    map.infoWindow.setContent(content);
                    map.infoWindow.show(evt.screenPoint, map.getInfoWindowAnchor(evt.screenPoint));
                });
                var legend = new Legend({
                    map: map,
                    layerInfos: legendLayers
                }, "legendDiv");
                legend.startup();
            }
            function getDatafromService() {
                ResultData = [];
                $.ajax({
                    url: currentConfig.serviceUrl + ssoID + "&reqID=" + reqID,
                    type: "GET",
                    dataType: "json",
                    crossOrigin: true,
                    async: false,
                    success: function (result) {
                        ResultData = result;
                    },
                    error: function (error) {
                        alert("result_error_" + error.statusText);
                    }
                });
            }
        });
           </script>
</head>
<body class="calcite">
    <form id="dnmform" runat="server">
        <div class="container" style="width: 98.69%">
            <div class="panel panel-primary">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-3">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h5 class="text-left" style="color: #ffffff; font-weight: 700"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>&nbsp;Request By</h5>
                                </div>
                                <div class="panel-body">
                                    <ul class="list-group">
                                        <li class="list-group-item list-group-item-success">SSO ID :
                                    <asp:Label ID="SSOName" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                        <li class="list-group-item list-group-item-info">Office ID :
                                    <asp:Label ID="portalName" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                        <li class="list-group-item list-group-item-warning">Request For(s) :
                                            <span id="lbl3" class="text-danger" style="overflow-wrap: break-word; text-wrap: normal"></span>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-9">
                            <div class="list-group-item list-group-item-warning">
                                <span class="info text-primary" id="layer_list"></span>
                            </div>
                            <div class="panel panel-primary">
                                <div class="panel-heading" style="padding: 2px">
                                    <h4 class="text-center" style="color: #ffffff; font-weight: 700"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;Nursery-Depot Management</h4>
                                </div>
                                <div class="panel-body" style="min-height: 680px;">
                                    <div class="col-lg-11" style="padding: 2px">
                                        <div id="noMap" style="display: block" class="text-capitalize text-center">
                                            <img src="../css/horizontal_loader.gif" class="img img-responsive center-block" /><br />
                                            Map is Loading...
                                        </div>
                                        <div id="mapDiv">
                                        </div>
                                     <div class="pull-left">
                                     <marquee class="text-muted"><b><i> Disclaimer:  </i> Information from the GIS functionality is indicative and will not be considered in legal matters.</b></marquee>                                                                      
                                </div>
                                <div class="pull-right">
                                    <a id="logo_raj" class="logo_raj img-responsive center-block" href="http://www.doitc.rajasthan.gov.in">
                                        <img src="../css/rajdharaa.png" alt="rajdharaa" /></a>
                                      <span class="text-right" style="margin-right:-25px;">&ensp;&copy;2016 DoIT&C</span>
                                </div>
                                    </div>
                                    <div class="col-lg-1" style="border: 1px #337AB7 solid">
                                        <div id="legendDiv" class="text-left text-danger">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
