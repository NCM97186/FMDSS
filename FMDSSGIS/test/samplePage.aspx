<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="samplePage.aspx.cs" Inherits="ForestryWebGIS.test.samplePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>Sample Request Page</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/themes/calcite/dijit/calcite.css" />
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/themes/calcite/esri/esri.css">
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        #mapDiv {
            height: 100%;
            min-height: 680px;
            margin: 0;
            padding: 0;
            width: 100%;
        }
    </style>
    <script src="https://code.jquery.com/jquery-2.2.4.min.js"></script>
    <script src="//js.arcgis.com/3.16/"></script>
    <script>
        var legendLayers = [];
        var visible = [];
        var dynamicForest, adminBoundry;
        require([
         "esri/map",
        "dojo/_base/array",
        "esri/layers/ArcGISDynamicMapServiceLayer",
        "esri/dijit/Legend",
        "esri/tasks/query",
        "esri/tasks/geometry",
        "esri/tasks/QueryTask",
        "esri/graphic",
        "dojo/text!../scripts/serviceURL.json",
        "dojo/dom",
        "dojo/on",
        "dojo/query",
        "dojo/parser",
               "dojo/dom-style",
        "dojo/domReady!"
        ], function (
          Map, array, ArcGISDynamicMapServiceLayer, Legend, Query, Geometry, QueryTask,
     Graphic, serviceURL, dom, on, query, parser, domStyle) {
            parser.parse();
            serviceUrl = JSON.parse(serviceURL);
            map = new Map("mapDiv", {
                sliderOrientation: "horizontal",
                sliderPosition: "top-center"
            });
            map.on("load", function () {
                document.getElementById("noMap").style.display = "none";
            });

            adminBoundry = new ArcGISDynamicMapServiceLayer(serviceUrl.adminBoundryUrl, {
                "opacity": 1.0
            });
            dynamicForest = new ArcGISDynamicMapServiceLayer(serviceUrl.dynamicMapServerUrl, {
                "opacity": 0.9,
            });
            map.on('layers-add-result', function () {
                var legend = new Legend({
                    map: map,
                    layerInfos: legendLayers
                }, "legendDiv");
                legend.startup();
            });
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
        });
    </script>
</head>
<body class="calcite">
    <form id="myform" runat="server">
        <div class="container" style="width: 98.69%">
            <div class="panel panel-primary">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-lg-3">
                            <div class="panel panel-warning">
                                <div class="panel-heading">
                                    <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>&nbsp;Request By</h5>
                                </div>
                                <div class="panel-body">
                                    <ul class="list-group">
                                        <li class="list-group-item list-group-item-success">SSO ID :
                                    <asp:Label ID="lblSSO" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                        <li class="list-group-item list-group-item-info">Office ID :
                                    <asp:Label ID="lblOfc" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                        <li class="list-group-item list-group-item-warning">Village Name(s) :
                                            <span id="lbl3" class="text-danger" style="overflow-wrap: break-word; text-wrap: normal"></span>
                                        </li>
                                        <li class="list-group-item list-group-item-danger">Request For :
                                            <span id="lbl4" class="text-primary" style="overflow-wrap: break-word; text-wrap: normal"></span>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div id="drawDiv" class="panel panel-warning">
                                <div class="panel-heading">
                                    <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-cloud-upload" aria-hidden="true"></span>&nbsp;Function Option</h5>
                                </div>
                                <div class="panel-body" style="padding: 4px;">
                                    Function Text
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-9">
                            <div class="list-group-item list-group-item-warning">
                                <span class="info text-primary" id="layer_list"></span>
                            </div>
                            <div class="panel panel-primary">
                                <div class="panel-heading" style="padding: 2px">
                                    <h4 class="text-center" style="color: #ffffff; font-weight: 700"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;Draw Area of Interest on Map</h4>
                                </div>
                                <div class="panel-body" style="min-height: 680px;">
                                    <div class="col-lg-11" style="padding: 2px">
                                        <div id="noMap" style="display: block" class="text-capitalize text-center">
                                            <img src="/css/horizontal_loader.gif" class="img img-responsive center-block" /><br />
                                            Map is Loading...
                                        </div>
                                        <div id="mapDiv">
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
