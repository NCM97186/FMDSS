<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="offense.aspx.cs" Inherits="ForestryWebGIS.markoffense.offense" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <!-- #include file ="../dojopackage.html" -->​
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>Mark Offense location</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/themes/calcite/dijit/calcite.css" />
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/themes/calcite/esri/esri.css" />
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
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
    </style>
    <script src="https://code.jquery.com/jquery-2.2.4.min.js"></script>    
    <script src="//js.arcgis.com/3.16/"></script>
    <script>
        var legendLayers = [];
        var visible = [];
        var app = {};
        var dynamicForest, adminBoundry, retURL, locCentroid, flagCount = null;
        retURL = '<% = returnURL %>';
        app.villageCode = '<% = villCode %>';
        app.offcatid = '<% = offenceCatID %>';
        app.offdistid = '<% = districtID %>';
        app.offblockid = '<% = blocknameID %>';
        app.offgpnmid = '<% = gpnameID %>';
        require([
        "esri/map",
        "dojo/_base/array",
        "esri/layers/ArcGISDynamicMapServiceLayer",
        "esri/dijit/Legend",
        "esri/tasks/geometry",
        "esri/tasks/QueryTask",
        "esri/tasks/query",
        "esri/graphicsUtils",
        "esri/symbols/SimpleFillSymbol",
        "esri/symbols/SimpleLineSymbol",
        "esri/symbols/SimpleMarkerSymbol",
        "esri/graphic",
        "esri/geometry/geometryEngine",
        "esri/Color",
            "DojoClass/TokenAuth",
        "esri/toolbars/draw",
        "dojo/text!../scripts/serviceURL.json",
        "dojo/dom",
        "dojo/promise/all",
        "dojo/on",
        "dojo/query",
        "dojo/parser",
        "dojo/dom-style",
        "dojo/domReady!"
        ], function (
          Map, array, ArcGISDynamicMapServiceLayer, Legend, Geometry, QueryTask, Query, graphicsUtils, SimpleFillSymbol, SimpleLineSymbol, SimpleMarkerSymbol, Graphic, geometryEngine, Color, TokenAuth,Draw, serviceURL, dom, all, on, query, parser, domStyle) {
            parser.parse();
            serviceUrl = JSON.parse(serviceURL);
            app.queryURL = serviceUrl.villageQueryUrl;
            map = new Map("mapDiv", {
                sliderOrientation: "horizontal",
                sliderPosition: "top-center",
                logo: false
            });
            map.on("load", function () {
                document.getElementById("noMap").style.display = "none";
                app.queryTask = new QueryTask(app.queryURL);
                app.query = new Query();
                app.query.returnGeometry = true;
                app.query.outFields = ["CENSUS_NM_2011"];
                app.query.where = "CENSUS_CD_2011 ='" + app.villageCode + "'";
                execution = app.queryTask.execute(app.query);
                app.promises = all([execution]);
                app.promises.then(function (data) {
                    app.symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color("#C9302C"), 3), new Color([246, 249, 160, 0.40]));
                    var myFeatureExtent = graphicsUtils.graphicsExtent(data[0].features);
                    map.graphics.add(new Graphic(data[0].features[0].geometry, app.symbol));
                    app.Geom = data[0].features[0].geometry;
                    map.setExtent(myFeatureExtent);
                    document.getElementById("lblVillage").innerHTML = data[0].features[0].attributes["CENSUS_NM_2011"];
                    map.disableDoubleClickZoom();
                    map.hideZoomSlider();
                    map.disableMapNavigation();
                });
            });
            drawToolbar = new Draw(map);
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
            var mybutton = dom.byId("btnPoint");
            on(mybutton, "click", function (evt) {
                drawToolbar.deactivate();
                app.symbol = null;
                app.geometry = null;
                app.graphic = null;
                flagCount = 0;
                drawToolbar.activate(Draw.POINT);
                drawToolbar.on("draw-complete", executePointQry);
                map.disableDoubleClickZoom();
                map.disableMapNavigation();
            });
            function executePointQry(evtObj) {
                if (flagCount == 1)
                    return;
                flagCount = 1;
                drawToolbar.deactivate();
                var withinGeom = false;
                if (app.Geom != null) {
                    withinGeom = geometryEngine.within(evtObj.geometry, app.Geom);
                }
                if (withinGeom == true) {
                    document.getElementById("pointid").style.display = "none";
                    var marker = new SimpleLineSymbol();
                    marker.setWidth(3);
                    marker.setColor(new Color([0, 92, 230, 1]));
                    app.symbol = new SimpleMarkerSymbol();
                    app.symbol.setAngle(0);
                    app.symbol.setColor(new Color([255, 255, 255, 0.25]));
                    app.symbol.setOutline(marker);
                    app.symbol.setSize(25);
                    app.symbol.setPath("M16,3.5c-4.142,0-7.5,3.358-7.5,7.5c0,4.143,7.5,18.121,7.5,18.121S23.5,15.143,23.5,11C23.5,6.858,20.143,3.5,16,3.5z M16,14.584c-1.979,0-3.584-1.604-3.584-3.584S14.021,7.416,16,7.416S19.584,9.021,19.584,11S17.979,14.584,16,14.584z");
                    app.symbol.setStyle(SimpleMarkerSymbol.STYLE_PATH);
                    locCentroid = dojo.number.format(evtObj.geographicGeometry.y, { places: 4 }) + "," + dojo.number.format(evtObj.geographicGeometry.x, { places: 4 });
                    document.getElementById("resultid").style.display = "block";
                    app.retData = { "Location": locCentroid };
                    app.offenceData = { "OffenseCategoryID": app.offcatid, "DistrictID": app.offdistid, "BlocknameID": app.offblockid, "GPNameID": app.offgpnmid, "villageId": app.villageCode };
                    document.getElementById("resultData").innerHTML = "Location : " + dojo.number.format(evtObj.geographicGeometry.y, { places: 3 }) + "N," + dojo.number.format(evtObj.geographicGeometry.x, { places: 3 }) + "E";
                    app.geometry = evtObj.geometry;
                    app.graphic = new Graphic(app.geometry, app.symbol);
                    map.graphics.add(app.graphic);
                }
                else
                    alert("Drawn Point is out of Village boundary. Please draw point with-in provided village boundary!!")
            }
            app.resetGraphic = function () {
                map.graphics.clear();
            }
        });
        function submitResult() {
            document.getElementById("retData").value = app.retData.Location;
            document.getElementById("offenceData").value = JSON.stringify(app.offenceData);
            document.getElementById("offenseform").action = retURL;
            document.getElementById("offenseform").submit();

        }
        function windowClose() {
            document.getElementById("offenseform").submit();
            window.open('', '_parent', '');
            window.close();
        }
        function reDraw() {
            document.getElementById("retData").value = "NA";
            document.getElementById("offenceData").value = "NA";
            app.offenceData = null;
            document.getElementById("resultData").innerHTML = "";
            document.getElementById("resultid").style.display = "none";
            document.getElementById("pointid").style.display = "block";
            app.resetGraphic();
        }
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
                                        <li class="list-group-item list-group-item-info">Village Name :                                             
                                    <label id="lblVillage" class="text-uppercase text-center text-danger"></label>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div id="drawDiv" class="panel panel-warning">
                                <div class="panel-heading">
                                    <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-cloud-upload" aria-hidden="true"></span>&nbsp;Function Option</h5>
                                </div>
                                <div class="panel-body" style="padding: 4px;">
                                    <div id="pointid" class="col-md-12">
                                        <button id="btnPoint" data-dojo-type="dijit/form/Button">
                                            Draw Point
                                        </button>
                                    </div>
                                    <div style="padding: 1px">&ensp;</div>
                                    <div id="resultid" class="col-md-12" style="display: none">
                                        <div class="text-danger text-capitalize" id="resultData" style="overflow-wrap: break-word; text-wrap: normal; background-color: #FFEEB8; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                                        </div>
                                        <div style="padding: 1px">&ensp;</div>
                                        <div class='col-md-6'><a id='btnRedraw' onclick='reDraw();' class='btn btn-warning'>Re-Draw</a></div>
                                        <div class='col-md-6'><a id='btnSubmit' onclick='submitResult();' class='btn btn-success'>Submit</a></div>
                                    </div>
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
                                      <div class="pull-left">
                                     <marquee class="text-muted"><b><i> Disclaimer:  </i> Information from the GIS functionality is indicative and will not be considered in legal matters.</b></marquee>                                                                      
                                </div>
                                <div class="pull-right">
                                    <a id="logo_raj" class="logo_raj img-responsive center-block" href="http://www.doitc.rajasthan.gov.in" target="_blank" rel = 'noopener noreferrer'>
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
    <form id="offenseform" name="" method="post">
        <input type="hidden" id="retData" name="retData" value="NA" />
        <input type="hidden" id="offenceData" name="offenceData" value="NA" />
    </form>
</body>
</html>
