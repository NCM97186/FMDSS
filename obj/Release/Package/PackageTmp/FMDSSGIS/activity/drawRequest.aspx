<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="drawRequest.aspx.cs" Inherits="ForestryWebGIS.activity.drawRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <!-- #include file ="../dojopackage.html" -->​
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>FMDSS Activity Registration Request</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.17/esri/css/esri.css" />
    <link rel="stylesheet" href="//js.arcgis.com/3.17/esri/themes/calcite/dijit/calcite.css" />
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        #mapDiv {
            height: 100%;
            min-height: 680px;
            margin: 0;
            padding: 0;
            width: 100%;
        }

        .col-sm-9 {
            padding-left: 4px;
            padding-right: 4px;
        }

        .col-sm-3 {
            padding-left: 4px;
            padding-right: 4px;
        }

        .panel-body {
            padding: 4px;
        }

        .logo_raj {
            position: absolute;
            width: 80px;
            bottom: 3px;
            right: 30px;
        }
    </style>
    <script src="//code.jquery.com/jquery-1.12.3.min.js"></script>
    <script src="//js.arcgis.com/3.17/"></script>
    <script>
        var map, dynamicForest, attributes, flag, featUrl, finalGeom, jfmcshpLength, jfmcshpArea, plantshpLength, plantshpArea;
        var legendLayers = []; var visible = []; var app = {}; var mainGeom = []; var condition = [], locCentroid;
        var withinGeom, areatypeCode = false;
        var flagCount = null;
        app.retUrl = '<% = returnURL %>';
        app.reqFor = '<%= requestFor %>';
        app.village_ID = '<%= villageId %>';
        app.rdistID = '<%= retDistID %>';
        app.rBlkID = '<%= retBlockID %>';
        app.rGP_ID = '<%= retGPID %>';
        app.rngID = '<%= retRngID %>';
        require([
        "esri/map",
        "dojo/_base/array",
        "esri/layers/ArcGISDynamicMapServiceLayer",
        "esri/dijit/Legend",
        "esri/SpatialReference",
        "esri/tasks/query",
        "esri/tasks/geometry",
        "esri/tasks/QueryTask",
        "esri/symbols/SimpleFillSymbol",
        "esri/symbols/SimpleLineSymbol",
        "esri/geometry/geometryEngine",
        "esri/toolbars/draw",
        "esri/graphicsUtils",
        "esri/graphic",
            "DojoClass/TokenAuth",
        "esri/Color",
        "dojo/text!../scripts/serviceURL.json",
        "dijit/form/Button",
        "dojo/promise/all",
        "dojo/dom",
        "dojo/on",
        "dojo/query",
        "dojo/parser",
        "dojo/dom-style",
        "dojo/domReady!"
        ],
  function (
    Map, array, ArcGISDynamicMapServiceLayer, Legend, SpatialReference, Query, Geometry, QueryTask,
    SimpleFillSymbol, SimpleLineSymbol, geometryEngine, Draw, graphicsUtils, Graphic,TokenAuth, Color, serviceURL, Button, all, dom, on, query, parser, domStyle) {
      parser.parse();
      serviceUrl = JSON.parse(serviceURL);
      app.queryURL = serviceUrl.villageQueryUrl;
      document.getElementById("lbl4").innerHTML = app.reqFor;
      document.getElementById("drawform").action = app.retUrl;
      map = new Map("mapDiv", {
          sliderOrientation: "horizontal",
          sliderPosition: "top-center",
          logo: false
      });
      drawToolbar = new Draw(map);
      map.on("load", function () {
          document.getElementById("drawDiv").style.display = "block";
          document.getElementById("noMap").style.display = "none";
          jfmcshpLength = 0;
          jfmcshpArea = 0;
          plantshpArea = 0;
          plantshpLength = 0;
          if (app.reqFor == "JFMCArea" || app.reqFor == "MicroPlan") {
              if (app.reqFor == "JFMCArea")
                  areatypeCode = 2;
              else
                  areatypeCode = 3;
              // query task and query for execution
              app.dataID = app.village_ID.split(",");
              for (var i = 0; i < app.dataID.length; i++) {
                  condition.push("'" + app.dataID[i] + "'");
              }
              app.queryTask = new QueryTask(app.queryURL);
              app.query = new Query();
              var sr = new SpatialReference(4326);
              app.query.returnGeometry = true;
              app.query.outSpatialReference = sr;
              app.query.geometryPrecision = 5;
              app.query.outFields = ["CENSUS_NM_2011,DISTRICT_CODE,FOREST_DIVCODE,FOREST_RANGECODE"];
              app.query.where = "CENSUS_CD_2011 in (" + condition + ")";
              execution = app.queryTask.execute(app.query);
              app.promises = all([execution]);
              app.promises.then(function (data) {
                  var selectedVill = [];
                  condition = [];
                  app.villageGeom = [];
                  app.symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color("#C9302C"), 3), new Color([246, 249, 160, 0.40]));
                  var myFeatureExtent = graphicsUtils.graphicsExtent(data[0].features);
                  array.forEach(data[0].features, function (featResult) {
                      map.graphics.add(new Graphic(featResult.geometry, app.symbol));
                      selectedVill.push(featResult.attributes["CENSUS_NM_2011"]);
                      app.forestDivCode = featResult.attributes["FOREST_DIVCODE"];
                      app.forestRngCode = featResult.attributes["FOREST_RANGECODE"];
                      app.Dist_code = featResult.attributes["DISTRICT_CODE"];
                      app.villageGeom.push(geometryEngine.geodesicBuffer(featResult.geometry, "10", "meters"));
                  });
                  for (var i = 0; i < selectedVill.length; i++) {
                      condition.push(selectedVill[i]);
                  }
                  map.setExtent(myFeatureExtent);
                  document.getElementById("lbl3").innerHTML = condition;
              });
          }
          else
              areatypeCode = 4;
      });
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

      //draw result handler

      function executeDrawFunction(evtObj) {
          if (flagCount == 1)
              return;
          flagCount = 1;
          map.showZoomSlider();
          if (app.reqFor == "JFMCArea" && flag == "plantflag") {
              drawToolbar.deactivate();
              withinGeom = geometryEngine.within(evtObj.geographicGeometry, app.drawShpGeom);
              if (withinGeom == true) {
                  document.getElementById("drawDiv").style.display = "none";
                  document.getElementById("plantReg").style.display = "block";
                  //Area Calculation
                  plantshpArea = plantshpArea + Number(dojo.number.format(geometryEngine.geodesicArea(evtObj.geographicGeometry, 109414), { places: 3 }));
                  plantshpLength = plantshpLength + Number(dojo.number.format(geometryEngine.geodesicLength(evtObj.geographicGeometry, 9036), { places: 2 }));
                  document.getElementById("resultDiv").style.display = "block";
                  document.getElementById("resultArea").innerHTML = "Total JFMC Area : " + dojo.number.format(jfmcshpArea, { places: 3 }) + " Sq.KM";
                  document.getElementById("resultLength").innerHTML = "Total JFMC Perimeter : " + dojo.number.format(jfmcshpLength, { places: 2 }) + " KM";
                  document.getElementById("plantArea").innerHTML = "Total Plantation Area : " + plantshpArea + " Sq.KM";
                  document.getElementById("plantLength").innerHTML = "Total Plantation Length : " + plantshpLength + " KM";
                  finalGeom = null;
                  attributes = "'attributes' : { 'CREATEDBYSSOID' : '" + '<% = ssoid %>' + "','AREATYPE' : '" + areatypeCode + "','FORESTDIVCODE' : '" + app.forestDivCode + "','DISTRICTCODE' : '" + app.Dist_code + "','RANGECODE' : '" + app.forestRngCode + "','CREATEDON' : '" + '<% = DateTime.Now %>' + "'}";
                  finalGeom = array.map(mainGeom, function (geomVal) {
                      return "{'geometry' : {'rings' : [" + JSON.stringify(geomVal) + "]}," + attributes + "}";
                  });
                  app.symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([17, 153, 255]), 2), new Color([128, 255, 128, 0.30]));
                  app.geometry = evtObj.geographicGeometry;
                  app.graphic = new Graphic(app.geometry, app.symbol);
                  map.graphics.add(app.graphic);
                  $("#btnplant span").text("Draw More Plantation Area");
                  return;
              }
              else {
                  alert("Please draw with-in highlighted VFPMC area.");
              }
          }
          else {
              drawToolbar.deactivate();
              if (app.villageGeom.length != 0) {
                  for (c = 0; c < app.villageGeom.length; c++) {
                      withinGeom = geometryEngine.within(evtObj.geographicGeometry, app.villageGeom[c]);
                      if (withinGeom == true)
                          break;
                  }
              }
              if (withinGeom == true) {
                  //Area Calculation
                  jfmcshpArea = jfmcshpArea + Number(dojo.number.format(geometryEngine.geodesicArea(evtObj.geographicGeometry, 109414), { places: 3 }));
                  jfmcshpLength = jfmcshpLength + Number(dojo.number.format(geometryEngine.geodesicLength(evtObj.geographicGeometry, 9036), { places: 2 }));
                  $("#btnjfmc span").text("Draw More VFPMC Area");
                  app.drawShpGeom = null;
                  app.drawShpGeom = evtObj.geographicGeometry;
                  mainGeom.push(evtObj.geographicGeometry.rings[0]);
                  locCentroid = evtObj.geographicGeometry.getExtent().getCenter().x + "," + evtObj.geographicGeometry.getExtent().getCenter().y;
                  app.symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([0, 255, 0]), 2), new Color([255, 255, 128, 0.50]));
                  app.geometry = evtObj.geographicGeometry;
                  app.graphic = new Graphic(app.geometry, app.symbol);
                  map.graphics.add(app.graphic);
                  if (app.reqFor == "JFMCArea") {
                      document.getElementById("plantReg").style.display = "block";
                      document.getElementById("resultDiv").style.display = "none";
                  }
                  else {
                      plantshpArea = plantshpArea + Number(dojo.number.format(geometryEngine.geodesicArea(evtObj.geographicGeometry, 109414), { places: 3 }));
                      plantshpLength = plantshpLength + Number(dojo.number.format(geometryEngine.geodesicLength(evtObj.geographicGeometry, 9036), { places: 2 }));
                      document.getElementById("resultArea").innerHTML = "Total " + app.reqFor + " Area : " + dojo.number.format(jfmcshpArea, { places: 3 }) + " Sq.KM";
                      document.getElementById("resultLength").innerHTML = "Total " + app.reqFor + " Perimeter : " + dojo.number.format(jfmcshpLength, { places: 2 }) + " KM";
                      document.getElementById("plantArea").style.display = "none";
                      document.getElementById("plantLength").style.display = "none";
                      document.getElementById("plantReg").style.display = "none";
                      document.getElementById("resultDiv").style.display = "block";
                      finalGeom = null;
                      attributes = "'attributes' : { 'CREATEDBYSSOID' : '" + '<% = ssoid %>' + "','AREATYPE' : '" + areatypeCode + "','FORESTDIVCODE' : '" + app.forestDivCode + "','DISTRICTCODE' : '" + app.Dist_code + "','RANGECODE' : '" + app.forestRngCode + "','CREATEDON' : '" + '<% = DateTime.Now %>' + "'}";
                      finalGeom = array.map(mainGeom, function (geomVal) {
                          return "{'geometry' : {'rings' : [" + JSON.stringify(geomVal) + "]}," + attributes + "}";
                      });
                  }
              }
              else {
                  alert("Please draw with-in highlighted village boundary.");
              }
          }
      }
      var jfmcBtn = dojo.byId("btnjfmc");
      var plantBtn = dojo.byId("btnplant");
      on(jfmcBtn, "click", function () {
          drawToolbar.deactivate();
          app.symbol = null;
          app.geometry = null;
          app.graphic = null;
          withinGeom = false;
          flagCount = 0;
          flag = "jfmcflag";
          drawToolbar.activate(Draw.POLYGON);
          drawToolbar.on("draw-complete", executeDrawFunction);
          map.disableDoubleClickZoom();
          map.hideZoomSlider();
          // map.disableMapNavigation();
      });
      on(plantBtn, "click", function () {
          drawToolbar.deactivate();
          app.symbol = null;
          app.geometry = null;
          app.graphic = null;
          withinGeom = false;
          flagCount = 0;
          flag = "plantflag";
          drawToolbar.activate(Draw.POLYGON);
          drawToolbar.on("draw-complete", executeDrawFunction);
          map.disableDoubleClickZoom();
          map.hideZoomSlider();
          // map.disableMapNavigation();
      });

  });
  function submitResult() {
      var finalCordinates, retData;
      featUrl = serviceUrl.forestWorkingArea;
      finalCordinates = "[" + finalGeom + "]";
      $.ajax({
          url: featUrl,
          type: "POST",
          datatype: "json",
          crossDomain: true,
          data: { features: finalCordinates, rollbackOnFailure: true, f: "pjson" },
          success: function (result) {
              var jsonData = JSON.parse(result);
              var gisArray = [];
              for (var c = 0 ; c < jsonData.addResults.length; c++) {
                  gisArray.push(jsonData.addResults[c]['objectId']);
              }
              var postData = null;
              if (app.reqFor == "JFMCArea") {
                  retData = { "village_NM": condition.toString(), "DrawArea": jfmcshpArea, "DrawLength": jfmcshpLength, "PlantationArea": plantshpArea, "PlantationLength": plantshpLength, "Cordinates": locCentroid, "refGisId": gisArray.join(",") };
                  postData = { "DistrictID": app.rdistID, "BlocknameID": app.rBlkID, "GPNameID": app.rGP_ID, "RangeID": app.rngID, "VillageId": app.village_ID };
              }
              else if (app.reqFor == "MicroPlan" || app.reqFor == "Rajasthan") {
                  retData = { "village_NM": condition.toString(), "DrawArea": jfmcshpArea, "DrawLength": jfmcshpLength, "Cordinates": locCentroid, "refGisId": gisArray.join(",") };
                  postData = { "DistrictID": app.rdistID, "BlocknameID": app.rBlkID, "GPNameID": app.rGP_ID, "RangeID": app.rngID, "VillageId": app.village_ID };
              }
              document.getElementById("activityData").value = JSON.stringify(retData);
              document.getElementById("postbackData").value = JSON.stringify(postData);
              gisArray = [];
              app = {};
              document.getElementById("drawform").submit();
          },
          error: function (error) {
              alert("Requested URL is not responding!!");
              windowClose();
          }
      });
  }
  function windowClose() {
      document.getElementById("drawform").submit();
      window.open('', '_parent', '');
      window.close();
  }
  function reDraw() {
      document.getElementById("activityData").value = "NA";
      document.getElementById("postbackData").value = "NA";
      document.getElementById("resultArea").innerHTML = "";
      document.getElementById("resultLength").innerHTML = "";
      document.getElementById("drawDiv").style.display = "block";
      document.getElementById("resultDiv").style.display = "none";
  }

    </script>
</head>
<body class="calcite">
    <div class="container" style="width: 98.70%">
        <div class="panel panel-warning">
            <div class="panel-heading">
                <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span>&nbsp;Request By</h5>
            </div>
            <div class="panel-body">
                <span class="col-sm-4 list-group-item list-group-item-success">SSO ID :
                                    <asp:Label ID="lblSSO" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></span>
                <span class="col-sm-2 list-group-item list-group-item-danger">Office ID :
                                    <asp:Label ID="lblOfc" runat="server" CssClass="text-uppercase text-center text-primary "></asp:Label></span>
                <span class="col-sm-4 list-group-item list-group-item-info">Village Name(s) :
                                            <span id="lbl3" class="text-danger" style="overflow-wrap: break-word; text-wrap: normal"></span>
                </span>
                <span class="col-sm-2 list-group-item list-group-item-warning">Request For :
                                            <span id="lbl4" class="text-primary" style="overflow-wrap: break-word; text-wrap: normal"></span>
                </span>
            </div>
        </div>
        <div class="panel panel-warning">
            <div class="panel-body">
                <div class="col-sm-3">
                    <div id="drawDiv" class="panel panel-warning" style="display: none">
                        <div class="panel-heading">
                            <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-cloud-upload" aria-hidden="true"></span>&nbsp;Select Draw Option</h5>
                        </div>
                        <div class="panel-body">
                            <div id="jfmcReg" class="col-md-12">
                                <button id="btnjfmc" class="btn-block" data-dojo-type="dijit/form/Button">
                                    <span>Draw JFMC Area</span>
                                </button>
                            </div>
                            <div style="padding: 1px">&ensp;</div>
                            <div id="plantReg" class="col-md-12" style="display: none;">
                                <button id="btnplant" class="btn-block" data-dojo-type="dijit/form/Button">
                                    <span>Draw Plantation Area</span>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div id="resultDiv" class="panel panel-warning" style="display: none; z-index: 9999; font-size: small">
                        <div class="panel-heading">
                            <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>&nbsp;Result</h5>
                        </div>
                        <div class="panel-body" style="height: auto">
                            <div class="text-danger text-capitalize" id="resultLength" style="overflow-wrap: break-word; text-wrap: normal; background-color: #FFEEB8; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                            </div>
                            <div style="padding: 1px">&ensp;</div>
                            <div class="text-success text-capitalize" id="resultArea" style="overflow-wrap: break-word; text-wrap: normal; background-color: #D9EDF7; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                            </div>
                            <div style="padding: 1px">&ensp;</div>
                            <div class="text-danger text-capitalize" id="plantArea" style="overflow-wrap: break-word; text-wrap: normal; background-color: #FFEEB8; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                            </div>
                            <div style="padding: 1px">&ensp;</div>
                            <div class="text-success text-capitalize" id="plantLength" style="overflow-wrap: break-word; text-wrap: normal; background-color: #D9EDF7; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                            </div>
                            <div style="padding: 1px">&ensp;</div>
                            <div class='col-md-6'><a id='btnRedraw' onclick='reDraw();' class='btn btn-warning'>Re-Draw</a></div>
                            <div class='col-md-6'><a id='btnSubmit' onclick='submitResult();' class='btn btn-success'>Submit</a></div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-9">
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
                                    <span class="text-right" style="margin-right: -25px;">&ensp;&copy;2016 DoIT&C</span>
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
    <form method="post" id="drawform" name="drawform">
        <input type="hidden" id="activityData" name="activityData" value="NA" />
        <input type="hidden" id="postbackData" name="postbackData" value="NA" />
    </form>
</body>
</html>
