<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="budgethead.aspx.cs" Inherits="ForestryWebGIS.budgethead.budgethead" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<!-- #include file ="../dojopackage.html" -->
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>Budget Head Information</title>
    <link rel="stylesheet" href="https://js.arcgis.com/3.22/esri/css/esri.css" />
    <link rel="stylesheet" href="https://js.arcgis.com/3.22/dijit/themes/soria/soria.css" />
    <link rel="stylesheet" href="https://js.arcgis.com/3.22/dojox/layout/resources/ExpandoPane.css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />

    <style>
        #mapDiv {
            height: 100%;
            min-height: 680px;
            margin: 0;
            padding: 0;
            width: 100%;
        }

        .calcite .esriPopup .titlePane .title {
            padding-right: 2px;
        }

        .layout.content table td {
            padding: 0px 8px;
            text-transform: capitalize;
            text-align: left;
            font-weight: normal !important;
            font-size: 11px !important;
            color: #4a4a4a;
        }

        .infowindow .window .top .right {
            background: #ddd;
        }

        .layout.content table td:first-child {
            width: 128px !important;
        }
    </style>
    <script src="https://gis.rajasthan.gov.in/arcgis_js_api/library/3.17/3.17/init.js"></script>
    <script src="https://code.jquery.com/jquery-2.2.4.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <script>
        var map, content, dynamicForest, serviceUrl, currentConfig = null;
        var visible = []; var app = {}; var legendLayers = []; var nearPOI = [], promiseCall;
        var ResultData = [];
        var reqFor = '<% = requestFor %>';
        var reqType = '<% = _requestType %>';
        var reqCode = '<% = requestCode %>';
        var ssoid = '<% = ssoID %>';
        var _servcNM = '<% = _serviceNM %>';
        require([
            "esri/map",
            "dojo/_base/array",
            "esri/layers/ArcGISDynamicMapServiceLayer",
            "esri/layers/FeatureLayer",
              "esri/layers/GraphicsLayer",
            "esri/dijit/Legend",
            "esri/tasks/query",
            "esri/tasks/geometry",
            "esri/tasks/QueryTask",
            "esri/geometry/Polygon",
            "esri/geometry/Point",
            "esri/SpatialReference",
            "esri/symbols/SimpleFillSymbol",
            "esri/symbols/SimpleLineSymbol",
             "esri/Color",
              "esri/renderers/SimpleRenderer",
            "esri/geometry/geometryEngine",
            "esri/graphic",
            "esri/dijit/InfoWindow",
             "DojoClass/TokenAuth",
            "dojo/text!../scripts/serviceURL.json",
            "dojo/promise/all",
            "dojo/dom",
            "dojo/on",
            "dojo/query",
            "dojo/parser",
             "dojo/dom-construct",
             "dojo/dom-class",
            "dojo/dom-style",
            "esri/dijit/Popup",
            "esri/dijit/PopupTemplate",
            "dojo/domReady!"
        ],
          function (
       Map, array, ArcGISDynamicMapServiceLayer, FeatureLayer, GraphicsLayer, Legend, Query, Geometry, QueryTask,
       Polygon, Point, SpatialReference, SimpleFillSymbol, SimpleLineSymbol, Color, SimpleRenderer,
       geometryEngine, Graphic, InfoWindow, TokenAuth, serviceURL, all, dom, on, query, parser, domConstruct, domClass, domStyle, Popup, PopupTemplate) {
              parser.parse();
              serviceUrl = JSON.parse(serviceURL);
              esri.config.defaults.map.logoLink = "http://www.doitc.rajasthan.gov.in/";
              app.queryURL = serviceUrl.villageQueryUrl;
              var popup = new Popup({
                  //titleInBody: true
              }, domConstruct.create("div"));
              domClass.add(popup.domNode, "");
              map = new Map("mapDiv", {
                  sliderOrientation: "horizontal",
                  sliderPosition: "top-center",
                  zoom: 5,
                  showAttribution: false,
                  logo: false,
                  infoWindow: popup
              });
              map.on("load", function () {
                  document.getElementById("noMap").style.display = "none";
              });

              //Takes a URL to a non cached map service.
              var adminBoundry = new ArcGISDynamicMapServiceLayer(serviceUrl.adminBoundryUrl, {
                  "opacity": 1.0
              });
              dynamicForest = new ArcGISDynamicMapServiceLayer(serviceUrl.dynamicMapServerUrl, {
                  "opacity": 0.9,
              });
              map.on('layers-add-result', afterlayerAdd);
              legendLayers.push({ layer: dynamicForest, title: "Legends" });

              var polygonGraphics = new GraphicsLayer({ id: 'polygonGraphic' });

              on(polygonGraphics, "click", function (evt) {
                  showInfoWindow(evt.screenPoint);
              });
              map.addLayers([dynamicForest, adminBoundry, polygonGraphics]);
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
                  var legend = new Legend({
                      map: map,
                      layerInfos: legendLayers
                  }, "legendDiv");
                  legend.startup();
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

              if (reqType.toLowerCase() == "forest" || reqType.toLowerCase() == "wildlife") {
                  var queryTask = null;
                  // Query for the features with the given reqCode
                  var queryPoint = new Query();
                  queryPoint.returnGeometry = true;
                  if (reqFor.toLowerCase() == "circle") {
                      if (reqType.toLowerCase() == "forest") {
                          queryTask = new QueryTask("https://gis.rajasthan.gov.in/rajasthan/rest/services/Forest/Forest/MapServer/3");
                          queryPoint.where = "CIRCLE_CODE  in ('" + reqCode + "')";
                      }
                      else if (reqType.toLowerCase() == "wildlife") {
                          queryTask = new QueryTask("https://gis.rajasthan.gov.in/rajasthan/rest/services/Forest/Forest/MapServer/6");
                          queryPoint.where = "CIRCLE_CODE in ('" + reqCode + "')";
                      }
                  }
                  else if (reqFor.toLowerCase() == "division") {
                      if (reqType.toLowerCase() == "wildlife") {
                          queryTask = new QueryTask("https://gis.rajasthan.gov.in/rajasthan/rest/services/Forest/Forest/MapServer/6");
                          queryPoint.where = "DIV_CODE  in ('" + reqCode + "')";
                      }
                      else if (reqType.toLowerCase() == "forest") {
                          queryTask = new QueryTask("https://gis.rajasthan.gov.in/rajasthan/rest/services/Forest/Forest/MapServer/3");
                          queryPoint.where = "FOREST_DIVISION_CODE  in ('" + reqCode + "')";
                      }
                  }
                  //else if (reqFor.toLowerCase() == "wildlife") {
                  //    queryTask = new QueryTask("https://gis.rajasthan.gov.in/rajasthan/rest/services/Forest/Forest/MapServer/6");
                  //    queryPoint.where = "CIRCLE_CODE in ('" + reqCode + "')";
                  //}
                  queryTask.execute(queryPoint, function (featureSet) {
                      if (featureSet.features.length > 0) {
                          var polygon = new Polygon(featureSet.features[0].geometry);
                          var point = polygon.getCentroid();
                          var screenPoint = map.toScreen(point);
                          var divGraphic = new Graphic(featureSet.features[0].geometry, null);
                          polygonGraphics.clear();
                          var polygonSymbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
                                  new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                                  new Color([255, 0, 0]), 3), new Color([205, 255, 232, 0.25]));

                          var polygonRenderer = new SimpleRenderer(polygonSymbol);
                          polygonGraphics.setRenderer(polygonRenderer);
                          polygonGraphics.add(divGraphic);

                          var divExtent = divGraphic.geometry.getExtent();
                          map.setExtent(divExtent);
                          showInfoWindow(screenPoint);
                      }
                      else {
                          var point = new Point(73.9834, 26.5983, new SpatialReference({ wkid: 4326 }));
                          var screenPoint = map.toScreen(point);
                          showInfoWindow(screenPoint);
                      }
                  });
              }
              else {
                  var point = new Point(73.9834, 26.5983, new SpatialReference({ wkid: 4326 }));
                  //var screenPoint = map.toScreen(point);
                  showInfoWindow(point);
              }

              function showInfoWindow(evt) {
                  var _servURL;
                  if (_servcNM == "Allocation")
                      _servURL = "http://10.68.128.179/BudgetAllocationCircle/GetBudgetAllocation?reqFor=" + reqFor + "&reqType=" + reqType + "&Code=" + reqCode;
                  else if (_servcNM == "Expenditure")
                      _servURL = "http://10.68.128.179/BudgetAllocationCircle/GetBudgetAllocationExpenditure?reqFor=" + reqFor + "&reqType=" + reqType + "&Code=" + reqCode;
                  $.ajax({
                      type: "GET",
                      //url: "http://10.68.128.179/BudgetAllocationCircle/GetBudgetAllocationExpenditure?reqFor=" + reqFor + "&reqType=" + reqType + "&Code=" + reqCode,                    
                      url: _servURL,
                      dataType: "json",
                      data: '',
                      crossDomain: true,
                      async: true,
                      success: function (t) {
                          var table = ""; var content = "";
                          var iTbl = "<div id='tabDiv'><ul class='nav nav-tabs' id='plotInfoWindow'>";
                          var row = "";
                          if (t.length > 0) {
                              for (var i = 0; i < t.length; i++) {
                                  var tableId = "table" + i; row = "";
                                  if (i == 0) {
                                      iTbl += "<li class='content active'><a data-toggle='tab' href='#" + tableId + "' style='font-weight: bold'>" + (parseInt(i) + 1) + "</a></li>"
                                      table += "<div class='tab-pane in active' data-repeat='true'  id='" + tableId + "' style='height:auto; padding:0px;'><table class='table table-bordered table-striped table-hover' style='margin-bottom:0px; border-width: medium; overflow:auto'>";
                                  }
                                  else {
                                      iTbl += "<li><a data-toggle='tab' href='#" + tableId + "' style='font-weight: bold'>" + (parseInt(i) + 1) + "</a></li>";
                                      table += "<div class='tab-pane' data-repeat='true'  id='" + tableId + "' style='height:auto; padding:0px;'><table class='table table-bordered table-striped table-hover' style='margin-bottom:0px; border-width: medium; overflow:auto'>";
                                  }
                                  $.each(t[i], function (k, v) {
                                      row += "<tr style='background-color:rgb(224, 248, 247);'><td ><strong>" + k + "</strong></td><td align='right'><strong>" + v + "</strong></td></tr>";
                                  });
                                  table += row + "</table></div>";
                              }
                          }
                          else {
                              table = "<table class='table table-bordered table-striped table-hover' style='margin-bottom:0px; border-width: medium; overflow:auto'>";
                              row += "<tr style='background-color:rgb(224, 248, 247);'><td ><strong>No Data found on FMDSS</strong></td></tr>";
                              table += row + "</table>";
                          }
                          content += iTbl + "</ul><div class='tab-content' id='tabs'>" + table + "</div>";
                          //map.infoWindow.resize(400, 210);
                          map.infoWindow.setTitle("Budget Head Info");
                          map.infoWindow.setContent(content);
                          map.infoWindow.show(evt, map.getInfoWindowAnchor(evt));
                      },
                      error: function (err) {
                          alert(err);
                      }
                  });
              }
              function afterlayerAdd() {
                  map.infoWindow.resize(400, 210);
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
              }
              function getDatafromService() {
                  ResultData = [];
                  $.ajax({
                      url: currentConfig.serviceUrl + ssoid + "&reqCode=" + reqCode,
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
                                        <li class="list-group-item list-group-item-info">User Type :
                                    <asp:Label ID="userType" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
                                        <li class="list-group-item list-group-item-warning">Request For :
                                             <asp:Label ID="boundryType" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></li>
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
                                    <h4 class="text-center" style="color: #ffffff; font-weight: 700"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;Budget Head Information</h4>
                                </div>
                                <div class="panel-body" style="min-height: 680px;">
                                    <div class="col-lg-11" style="padding: 2px">
                                        <div id="noMap" style="display: block" class="text-capitalize text-center">
                                            <img src="../css/horizontal_loader.gif" class="img img-responsive center-block" /><br />
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
