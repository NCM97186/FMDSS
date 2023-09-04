<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetVillageIds.aspx.cs" Inherits="ForestryWebGIS.getvillagedata.GetVillageIds" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <!-- #include file ="../dojopackage.html" -->​
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>Get Village IDs</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/css/esri.css" />
    <link rel="stylesheet" href="https://js.arcgis.com/3.16/esri/themes/calcite/dijit/calcite.css" />
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
    <script src="https://code.jquery.com/jquery-1.12.3.min.js"></script>
    <script src="//js.arcgis.com/3.16/"></script>

    <script>
        var map, featureLayer, symbol, dynamicForest, areas, length, basemapGallery;
        var visible = []; var app = {}; var legendLayers = [];
        require([
            "esri/map",
            "dojo/_base/array",
            "esri/layers/ArcGISDynamicMapServiceLayer",
            "esri/layers/FeatureLayer",
            "esri/dijit/Legend",
            "esri/tasks/query",
            "esri/tasks/QueryTask",
            "esri/tasks/GeometryService",
            "esri/geometry/geometryEngine",
            "esri/graphic",
            "esri/geometry/scaleUtils",
            "esri/toolbars/draw",
            "esri/symbols/SimpleFillSymbol",
             "esri/symbols/SimpleLineSymbol",
            "dijit/form/Button",
            "esri/dijit/BasemapGallery",
            "dijit/Menu",
            "esri/Color",
                  "DojoClass/TokenAuth",
            "dojo/text!../scripts/serviceURL.json",
            "dojo/dom",
            "dojo/on",
            "dojo/query",
            "dojo/parser",
             "dijit/registry",
            "dojo/dom-style",
            "dojo/domReady!"
        ],
          function (
       Map, array, ArcGISDynamicMapServiceLayer, FeatureLayer, Legend, Query, QueryTask, GeometryService,
       geometryEngine, Graphic, scaleUtils, Draw, SimpleFillSymbol, SimpleLineSymbol, Button, BasemapGallery, MenuItem, Color,TokenAuth, serviceURL, dom, on, query, parser, registry, domStyle) {
              parser.parse();
              serviceUrl = JSON.parse(serviceURL);
              esri.config.defaults.map.logoLink = "http://www.doitc.rajasthan.gov.in/";
              app.queryURL = serviceUrl.villageQueryUrl;
              var retUrl = '<% = returnURL %>';
              document.getElementById("fmdssform").action = retUrl;
              map = new Map("mapDiv", {
                  sliderOrientation: "horizontal",
                  sliderPosition: "top-center",
                  zoom: 5,
                  showAttribution: false
              });
              map.on("load", function (evt) {
                  document.getElementById("uploadDiv").style.display = "block";
                  document.getElementById("noMap").style.display = "none";
              });
              drawToolbar = new Draw(map);
              map.on("extent-change", function (evt) {
                  dom.byId("scale").innerHTML = "Scale: <i>1:" + Math.round(scaleUtils.getScale(map)) + "</i>";
              });
              //Takes a URL to a non cached map service.
              var adminBoundry = new ArcGISDynamicMapServiceLayer(serviceUrl.adminBoundryUrl, {
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
              //BaseMap Toggle
              basemapGallery = new BasemapGallery({
                  showArcGISBasemaps: true,
                  map: map
              });
              dojo.connect(basemapGallery, "onLoad", function () {
                  dojo.forEach(basemapGallery.basemaps, function (basemap) {
                      dijit.byId("basemapMenu").addChild(
                        new dijit.MenuItem({
                            label: basemap.title,
                            onClick: dojo.hitch(this, function () {
                                this.basemapGallery.select(basemap.id);
                                this.map.addLayers(adminBoundry);
                            })
                        })
                      );
                  });
              });
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
              //dojo button request
              registry.byId("drawBtn").on("click", function (evt) {
                  var scale = scaleUtils.getScale(map);
                  if (Math.round(scale) > 315000) {
                      alert("This functionality is available at 1:315000 scale.\nPlease Zoom-In the map upto this scale.");
                  }
                  else {
                      map.enablePan();
                      map.disableDoubleClickZoom();
                      $("#drawDiv").removeClass("col-md-6").addClass("col-md-12");
                      document.getElementById("drawDiv").style.display = "none";
                      var requestFor = '<%= reqFor %>';
                      drawToolbar.deactivate();
                      map.graphics.clear();
                      if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other") {
                          app.symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]));
                          drawToolbar.activate(Draw.POLYGON);
                      }
                      else if (requestFor == "Cable" || requestFor == "Telephone" || requestFor == "Transmission" || requestFor == "Roads") {
                          app.symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_NULL, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]));
                          drawToolbar.activate(Draw.POLYLINE);
                      }
                      drawToolbar.on("draw-end", manualDraw);
                  }
              });
              //Manual Draw Action
              function manualDraw(evtObj) {
                  var gr = new Graphic(evtObj.geometry, app.symbol);
                  map.graphics.add(gr);
                  drawToolbar.deactivate();
                  app.queryTask = new QueryTask(app.queryURL);
                  app.query = new Query();
                  app.query.returnGeometry = false;
                  app.query.outFields = ["CENSUS_CD_2011,CENSUS_NM_2011"];
                  app.query.geometry = evtObj.geometry;
                  app.queryTask.execute(app.query, function (result) {
                      this.map.selectedArea = [];
                      array.forEach(result['features'], function (feat) {
                          var admin_unit = { "Vlg_Cd": feat.attributes['CENSUS_CD_2011'], "Village_NM": feat.attributes['CENSUS_NM_2011'] };
                          if (result['features'].length > 0)
                              this.map.selectedArea.push(admin_unit);
                      });
                      var returnData = JSON.stringify(this.map.selectedArea);
                      var lbl = "One village found with-in area of interest.";
                      if (this.map.selectedArea.length > 1) lbl = "Total " + this.map.selectedArea.length + " Villages found with-in area of interest. ";
                      dom.byId("villageDiv").innerHTML = lbl;
                      document.getElementById("ids").value = returnData;
                      document.getElementById("uploadDiv").style.display = "none";
                      document.getElementById("resultDiv").style.display = "block";
                  },
                  function (errors) {
                  });
                  //Area Calculation                                           
                  if (evtObj.geometry.type == "polygon") {
                      shpArea = dojo.number.format(geometryEngine.geodesicArea(evtObj.geometry, 109414), { places: 3 });
                      shpLength = dojo.number.format(geometryEngine.geodesicLength(evtObj.geometry, 9036), { places: 2 });
                      document.getElementById("shapeArea").value = shpArea;
                      document.getElementById("shapeLength").value = shpLength;
                  }
                  else {
                      shpLength = dojo.number.format(geometryEngine.geodesicLength(evtObj.geometry, 9036), { places: 2 });
                      document.getElementById("shapeLength").value = shpLength;
                  }
              }
          });
          function submitResult() {
              document.getElementById("fmdssform").submit();
          }
          function reUpload() {
              document.getElementById("kmldiv").innerHTML = "";
              document.getElementById("resultDiv").style.display = "none";
              document.getElementById("uploadDiv").style.display = "block";
          }
    </script>
</head>
<body class="calcite">
    <div class="container" style="width: 98.69%">
        <div class="panel panel-success">
            <div class="panel-body">
                <div class="row">
                    <div class="col-lg-3">
                        <div class="panel panel-success">
                            <div class="panel-heading">
                                <h5 class="text-left" style="color: #4584ee; font-weight: 700"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>&nbsp;Request By</h5>
                            </div>
                            <div class="panel-body">
                                <ul class="list-group">
                                    <li class="list-group-item list-group-item-success">SSO ID :
                                    <asp:Label ID="lblSSO" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                    <li class="list-group-item list-group-item-info">Office ID :
                                    <asp:Label ID="lblOfc" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                    <li class="list-group-item list-group-item-warning">Request For :
                                    <asp:Label ID="lblrequest" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                </ul>
                            </div>
                        </div>
                        <div id="uploadDiv" class="panel panel-success" style="display: none">
                            <div class="panel-heading">
                                <h5 class="text-left" style="color: #4584ee; font-weight: 700"><span class="glyphicon glyphicon-cloud-upload" aria-hidden="true"></span>&nbsp;Upload KML/Shape File</h5>
                            </div>
                            <div class="panel-body">
                                <div class="col-md-6" id="drawDiv">
                                    <button id="drawBtn" data-dojo-type="dijit/form/Button">
                                        Draw AOI
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div id="resultDiv" class="panel panel-success" style="display: none; z-index: 9999">
                            <div class="panel-heading">
                                <h5 class="text-left" style="color: #4584ee; font-weight: 700"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>&nbsp;Result</h5>
                            </div>
                            <div class="panel-body" style="height: auto">
                                <div class="text-danger" id="villageDiv" style="overflow-wrap: break-word; text-wrap: normal; border: 1px solid #ddd; border-radius: 5px; padding: 4px">
                                </div>
                                <div style="padding: 1px">&ensp;</div>
                                <div class='col-md-8'><a id='reUpload' onclick='reUpload();' class='btn btn-warning'>Re-Upload/Draw</a></div>
                                <div class='col-md-4'><a id='btnSubmit' onclick='submitResult();' class='btn btn-success'>Submit</a></div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-9">
                        <div class="list-group-item list-group-item-warning">
                            <span class="info text-primary" id="layer_list"></span>
                        </div>
                        <div class="panel panel-success">
                            <div class="panel-heading" style="padding: 2px;">
                                <div style="position: absolute; z-index: 99; font-weight: 600">
                                    <span id="scale" class="text-danger"></span>
                                </div>
                                <h4 class="text-center" style="color: #4584ee; font-weight: 700"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;Requested Permission Location</h4>
                                <button class="pull-right" style="margin-top: -40px" id="dropdownButton" label="Basemaps" data-dojo-type="dijit/form/DropDownButton">
                                    <div data-dojo-type="dijit/Menu" id="basemapMenu">
                                    </div>
                                </button>
                            </div>
                            <div class="panel-body" style="min-height: 700px; padding: 2px;">
                                <div class="col-lg-11" style="padding: 1px;">
                                    <div id="noMap" style="display: block" class="text-capitalize text-center">
                                        <img src="/css/horizontal_loader.gif" class="img img-responsive center-block" /><br />
                                        Map is Loading...
                                    </div>
                                    <div id="mapDiv">
                                    </div>
                                </div>
                                <div class="col-lg-1" style="padding: 1px;">
                                    <div id="legendDiv" class="text-left text-danger" style="border: 1px #83B869 solid">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="loading">
                        <div id="loading-image">
                            <label id="loadingData" style="padding-left: 33px; padding-top: 7px"></label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <form name="fmdssform" method="post" id="fmdssform">
        <input type="hidden" id="ids" name="ids" value="NA" />
        <input type="hidden" id="shapeArea" name="shapeArea" value="NA" />
        <input type="hidden" id="shapeLength" name="shapeLength" value="NA" />
    </form>
</body>
</html>
