<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetRequest.aspx.cs" Inherits="ForestryWebGIS.permitlocation.GetRequest" %>

<!DOCTYPE html>
<html>
<!-- #include file ="../dojopackage.html" -->
​
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no">
    <title>FMDSS Web GIS Application</title>
    <link rel="stylesheet" href="https://js.arcgis.com/3.17/esri/css/esri.css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/dojo/1.10.4/dijit/themes/claro/claro.css" />
    <link href="../css/fileupload.css" rel="stylesheet" />
    <style>
        #mapDiv {
            height: 100%;
            min-height: 700px;
            margin: 0;
            padding: 0;
            width: 100%;
        }

        .logo_raj {
            position: absolute;
            width: 80px;
            bottom: 3px;
            right: 30px;
        }
    </style>
    <script src="https://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="../scripts/jquery.form.js"></script>
    <script src="../scripts/togeojson.js"></script>
    <script src="../scripts/shapefile.js" type="text/javascript"></script>
    <script src="../scripts/terraformer.min.js"></script>
    <script src="../scripts/terraformer-arcgis-parser.min.js"></script>
    <script src="https://gis.rajasthan.gov.in/arcgis_js_api/library/3.17/3.17/init.js"></script>
    <%--<script src="//js.arcgis.com/3.16/"></script>--%>
    <script>
        var map, featureLayer, fileGuID, fileName, symbol, mainGeo, dynamicForest, outputGeom, filePath, areas, length, locCentroid, returnData1, basemapGallery;
        var visible = []; var app = {}; var legendLayers = []; var nearPOI = [], promiseCall, exe_VillageQry, exe_POIQry, exe_WildQry, exe_WaterQry, exe_ForestyQry;
        require([
            "esri/map",
            "dojo/_base/array",
            "esri/layers/ArcGISDynamicMapServiceLayer",
            "esri/layers/FeatureLayer",
            "esri/dijit/Legend",
            "esri/layers/KMLLayer",
            "esri/tasks/query",
            "esri/tasks/geometry",
            "esri/tasks/QueryTask",
            "esri/geometry/Polygon",
            "esri/geometry/Polyline",
            "esri/symbols/SimpleFillSymbol",
            "esri/symbols/SimpleLineSymbol",
            "esri/tasks/ProjectParameters",
            "esri/tasks/BufferParameters",
            "esri/tasks/GeometryService",
            "esri/geometry/geometryEngine",
            "esri/graphic",
         "DojoClass/TokenAuth",
            "esri/geometry/scaleUtils",
            "esri/toolbars/draw",
            "dijit/form/Button",
            "esri/dijit/BasemapGallery",
            "dijit/Menu",
            "esri/Color",
            "dojo/text!../scripts/serviceURL.json",
            "dojo/promise/all",
            "dojo/sniff",
            "dojo/dom",
            "dojo/on",
            "dojo/query",
            "dojo/parser",
             "dijit/registry",
            "dojo/dom-style",
            "dojo/domReady!"
        ],
          function (
       Map, array, ArcGISDynamicMapServiceLayer, FeatureLayer, Legend, KMLLayer, Query, Geometry, QueryTask,
       Polygon, Polyline, SimpleFillSymbol, SimpleLineSymbol, ProjectParameters, BufferParameters, GeometryService,
       geometryEngine, Graphic, TokenAuth, scaleUtils, Draw, Button, BasemapGallery, MenuItem, Color, serviceURL, all, sniff, dom, on, query, parser, registry, domStyle) {
              parser.parse();
              serviceUrl = JSON.parse(serviceURL);
              esri.config.defaults.map.logoLink = "http://www.doitc.rajasthan.gov.in/";
              app.queryURL = serviceUrl.villageQueryUrl;
              var mygp = new GeometryService(serviceUrl.gpUrl);
              app.WaterUrl = serviceUrl.waterUrl;
              app.ForestUrl = serviceUrl.forestUrl;
              app.wildlifeUrl = serviceUrl.wildlifeUrl;
              var retUrl = '<% = returnURL %>';
              document.getElementById("fmdssform").action = retUrl;
              on(dom.byId("filetoupload"), "change", function (event) {
                  $("#fileUploadDiv").removeClass("col-md-6").addClass("col-md-12");
                  document.getElementById("drawDiv").style.display = "none";
                  fileName = event.target.value.toLowerCase();
                  if (sniff("ie")) { //filename is full path in IE so extract the file name
                      var arr = fileName.split("\\");
                      fileName = arr[arr.length - 1];
                  }
                  if (fileName.indexOf(".shp") !== -1 || fileName.indexOf(".kml") !== -1) {
                      document.getElementById("btnUpload").style.display = "block";
                      var orgName = document.getElementById("filetoupload").files[0].name;
                      document.getElementById("originalFileName").value = orgName;
                      map.graphics.clear();
                      $(document).ready(function () {
                          $('#uploadForm').on('submit', function (e) {
                              e.preventDefault();
                              $(this).ajaxSubmit({
                                  success: OnComplete,
                                  error: OnFail
                              });
                          });
                      });
                  }
                  else {
                      alert("Upload KML/Shape file Only!!");
                      document.getElementById("loading").style.display = "none";
                  }
              });
              map = new Map("mapDiv", {
                  sliderOrientation: "horizontal",
                  sliderPosition: "top-center",
                  zoom: 5,
                  showAttribution: false,
                  logo: false
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
              this.map.selectedArea = [];
              var shpArea, shpLength = 0;
              function ShowKML(kmlpath) {
                  var kmlpath1 = "../shapeFile/" + kmlpath;
                  $.ajax(kmlpath1).done(function (xml) {
                      map.graphics.clear();
                      var xmlDoc = jQuery.parseXML(xml);
                      var obj = toGeoJSON.kml(xmlDoc);
                      var requestFor = '<%= reqFor %>';
                      var sms;
                      var singleRingPolygon;
                      var reqGeo = "";
                      var curGeo = obj.features[0].geometry.type;
                      if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other" || requestFor == "AmritaDevi") {
                          reqGeo = "Polygon";
                          isValid = (curGeo == "Polygon");
                      }
                      else if (requestFor == "Cable" || requestFor == "Telephone" || requestFor == "Transmission" || requestFor == "Roads") {
                          reqGeo = "Polyline";
                          isValid = (curGeo == "LineString");
                      }
                      if (!isValid) {
                          alert("The Geometry of Uploaded file is type of " + curGeo + " while system expecting " + reqGeo + " !! Please upload valid " + reqGeo + " type geometry.");
                          document.getElementById("loading").style.display = "none";
                          return;
                      }
                      if (reqGeo == "Polygon") {
                          singleRingPolygon = new Polygon(obj.features[0].geometry.coordinates);
                          sms = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color(["#000000"]), 2), new Color([249, 4, 4, 0.75]));
                      }
                      else {
                          singleRingPolygon = new Polyline(obj.features[0].geometry.coordinates);
                          sms = new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([0, 0, 255]), 3);
                      }
                      var gr = new Graphic(singleRingPolygon, sms);
                      var extnt = gr.geometry.getExtent();
                      if (Math.round(extnt.xmax) > 80 || Math.round(extnt.ymax) > 31 || Math.round(extnt.xmin) < 67 || Math.round(extnt.ymin) < 22) {
                          alert("The Uploaded Shp/KML file is outside from Rajasthan Extent. Please ensure the uploaded Shape is having geographic cordinate system.");
                          document.getElementById("loading").style.display = "none";
                          return;
                      }
                      map.graphics.add(gr);
                      mainGeo = gr.geometry;
                      locCentroid = gr.geometry.getExtent().getCenter().x + "," + gr.geometry.getExtent().getCenter().y;

                      //Geometry1 Projection
                      var inSR = new esri.SpatialReference({
                          wkid: 4326
                      });
                      var outSR = new esri.SpatialReference({
                          wkid: 102100
                      });
                      var PrjParams = new esri.tasks.ProjectParameters();
                      PrjParams.geometries = [mainGeo];
                      PrjParams.outSR = outSR;
                      PrjParams.inSR = inSR;
                      mygp.project(PrjParams, function (reProjectedGeom) {
                          outputGeom = reProjectedGeom;
                          //Area Calculation
                          if (outputGeom[0].type == "polygon") {
                              shpArea = dojo.number.format(geometryEngine.geodesicArea(outputGeom[0], 109414), { places: 3 });
                              shpLength = dojo.number.format(geometryEngine.geodesicLength(outputGeom[0], 9036), { places: 2 });
                              document.getElementById("shapeArea").value = shpArea;
                              document.getElementById("shapeLength").value = shpLength;
                          }
                          else {
                              shpLength = dojo.number.format(geometryEngine.geodesicLength(outputGeom[0], 9036), { places: 2 });
                              document.getElementById("shapeLength").value = shpLength;
                          }
                          app.queryTask = new QueryTask(app.queryURL);
                          app.query = new Query();
                          app.query.returnGeometry = false;
                          app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,TEHSIL_CODE,FOREST_DIVCODE,FOREST_RANGECODE,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011,TEHSIL_NAME"]; //,FOREST_DIVCODE,FOREST_RANGECODE
                          app.query.geometry = gr.geometry;
                          exe_VillageQry = app.queryTask.execute(app.query);
                          //setup the buffer parameters
                          var params = new BufferParameters();
                          params.distances = [5];
                          params.outSpatialReference = map.spatialReference;
                          params.unit = GeometryService.UNIT_KILOMETER;
                          mygp.simplify([gr.geometry], function (geometries) {
                              params.geometries = geometries;
                              mygp.buffer(params, showBuffer);
                          });
                      }, function (error) { document.getElementById("loading").style.display = "none"; alert("REST Service is not responding : " + error); });
                  });
              }
              function RenderShpFile(shppath) {
                  var shpfile = "../shapeFile/" + shppath;
                  shapefile = new Shapefile({
                      shp: "" + shpfile + ""
                  }, function (shpData) {
                      map.graphics.clear();
                      var obj = shpData.geojson;
                      var requestFor = '<%= reqFor %>';
                      var sms;
                      var singleRingPolygon;
                      var reqGeo = "";
                      var curGeo = obj.features[0].geometry.type;
                      if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other" || requestFor == "AmritaDevi") {
                          reqGeo = "Polygon";
                          isValid = (curGeo == "Polygon");
                      }
                      else if (requestFor == "Cable" || requestFor == "Telephone" || requestFor == "Transmission" || requestFor == "Roads") {
                          reqGeo = "Polyline";
                          isValid = (curGeo == "LineString");
                      }
                      if (!isValid) {
                          alert("The Geometry of Uploaded file is type of " + curGeo + " while system expecting " + reqGeo + " !! Please upload valid " + reqGeo + " type geometry.");
                          document.getElementById("loading").style.display = "none";
                          return;
                      }

                      if (reqGeo == "Polygon") {
                          singleRingPolygon = new Polygon(obj.features[0].geometry.coordinates);
                          sms = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color(["#000000"]), 2), new Color([249, 4, 4, 0.75]));

                      }
                      else {
                          singleRingPolygon = new Polyline(obj.features[0].geometry.coordinates);
                          sms = new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([0, 0, 255]), 3);
                      }
                      var gr = new Graphic(singleRingPolygon, sms);
                      var extnt = gr.geometry.getExtent();
                      if (Math.round(extnt.xmax) > 80 || Math.round(extnt.ymax) > 31 || Math.round(extnt.xmin) < 67 || Math.round(extnt.ymin) < 22) {
                          alert("The Uploaded Shp/KML file is outside from Rajasthan Extent. Please ensure the uploaded Shape is having geographic cordinate system.");
                          document.getElementById("loading").style.display = "none";
                          return;
                      }
                      map.graphics.add(gr);
                      mainGeo = gr.geometry;
                      locCentroid = gr.geometry.getExtent().getCenter().x + "," + gr.geometry.getExtent().getCenter().y;

                      //Geometry1 Projection
                      var inSR = new esri.SpatialReference({
                          wkid: 4326
                      });
                      var outSR = new esri.SpatialReference({
                          wkid: 102100
                      });
                      var PrjParams = new esri.tasks.ProjectParameters();
                      PrjParams.geometries = [mainGeo];
                      PrjParams.outSR = outSR;
                      PrjParams.inSR = inSR;
                      mygp.project(PrjParams, function (reProjectedGeom) {
                          outputGeom = reProjectedGeom;
                          //Area Calculation
                          if (outputGeom[0].type == "polygon") {
                              shpArea = dojo.number.format(geometryEngine.geodesicArea(outputGeom[0], 109414), { places: 3 });
                              shpLength = dojo.number.format(geometryEngine.geodesicLength(outputGeom[0], 9036), { places: 2 });
                              document.getElementById("shapeArea").value = shpArea;
                              document.getElementById("shapeLength").value = shpLength;
                          }
                          else {
                              shpLength = dojo.number.format(geometryEngine.geodesicLength(outputGeom[0], 9036), { places: 2 });
                              document.getElementById("shapeLength").value = shpLength;
                          }
                          app.queryTask = new QueryTask(app.queryURL);
                          app.query = new Query();
                          app.query.returnGeometry = false;
                          app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,TEHSIL_CODE,FOREST_DIVCODE,FOREST_RANGECODE,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011,TEHSIL_NAME"]; //,FOREST_DIVCODE,FOREST_RANGECODE
                          app.query.geometry = gr.geometry;
                          exe_VillageQry = app.queryTask.execute(app.query);
                          //setup the buffer parameters
                          var params = new BufferParameters();
                          params.distances = [5];
                          params.outSpatialReference = map.spatialReference;
                          params.unit = GeometryService.UNIT_KILOMETER;
                          mygp.simplify([gr.geometry], function (geometries) {
                              params.geometries = geometries;
                              mygp.buffer(params, showBuffer);
                          });
                      }, function (error) { document.getElementById("loading").style.display = "none"; alert("REST Service is not responding : " + error); });
                  },
                       function (error)
                       { console.log("Error Found : " + error); });
              }

              //Manual Draw
              function manualDraw(evtObj) {
                  document.getElementById("loading").style.display = "block";
                  document.getElementById("loadingData").innerHTML = "The query task is resource and time consuming.<br/>Please be patience and do not click any where untill results come.";
                  map.disablePan();
                  fileGuID = '<%= Gis_ID %>';
                  filePath = fileGuID;
                  drawToolbar.deactivate();
                  app.drawGeometry = null;
                  app.drawGeometry = evtObj.geometry;
                  app.graphic = new Graphic(app.drawGeometry, app.symbol);
                  map.graphics.add(app.graphic);
                  //Geometry1 Projection
                  var inSR = new esri.SpatialReference({
                      wkid: 102100
                  });
                  var outSR = new esri.SpatialReference({
                      wkid: 4326
                  });
                  var PrjParams1 = new esri.tasks.ProjectParameters();
                  PrjParams1.geometries = [app.drawGeometry];
                  PrjParams1.outSR = outSR;
                  PrjParams1.inSR = inSR;
                  mygp.project(PrjParams1, function (projectedGeom) {
                      locCentroid = projectedGeom[0].getExtent().getCenter().x + "," + projectedGeom[0].getExtent().getCenter().y;
                      app.queryTask = new QueryTask(app.queryURL);
                      app.query = new Query();
                      app.query.returnGeometry = false;
                      app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,TEHSIL_CODE,FOREST_DIVCODE,FOREST_RANGECODE,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011,TEHSIL_NAME"]; //,FOREST_DIVCODE,FOREST_RANGECODE
                      app.query.geometry = app.drawGeometry;
                      exe_VillageQry = app.queryTask.execute(app.query);
                      //setup the buffer parameters
                      var params = new BufferParameters();
                      params.distances = [5];
                      params.outSpatialReference = map.spatialReference;
                      params.unit = GeometryService.UNIT_KILOMETER;
                      mygp.simplify([app.drawGeometry], function (geometries) {
                          params.geometries = geometries;
                          mygp.buffer(params, showBuffer);
                      });
                  });
                  //Area Calculation                                           
                  if (app.drawGeometry.type == "polygon") {
                      shpArea = dojo.number.format(geometryEngine.geodesicArea(app.drawGeometry, 109414), { places: 3 });
                      shpLength = dojo.number.format(geometryEngine.geodesicLength(app.drawGeometry, 9036), { places: 2 });
                      document.getElementById("shapeArea").value = shpArea;
                      document.getElementById("shapeLength").value = shpLength;
                  }
                  else {
                      shpLength = dojo.number.format(geometryEngine.geodesicLength(app.drawGeometry, 9036), { places: 2 });
                      document.getElementById("shapeLength").value = shpLength;
                  }
              }
              function showBuffer(bufferedGeometries) {
                  symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color("#444444"), 2), new Color([238, 238, 238, 0.2]));
                  app.geometry = null;
                  array.forEach(bufferedGeometries, function (bufgeom) {
                      var graphic = new Graphic(bufgeom, symbol);
                      map.graphics.add(graphic);
                      app.geometry = bufgeom;
                  });
                  map.setExtent(app.geometry.getExtent());
                  var requestFor = '<%= reqFor %>';
                  if (requestFor == "Mines") {
                      //Perform Water Query
                      app.waterTask = new QueryTask(app.WaterUrl);
                      app.waterQuery = new Query();
                      app.waterQuery.returnGeometry = true;
                      app.waterQuery.outFields = ["FEAT_TYPE,POLYGON_NM,FEAT_COD,POLYGON_ID"];
                      app.waterQuery.geometry = app.geometry;
                      exe_WaterQry = app.waterTask.execute(app.waterQuery);
                      //Perform Forest Query
                      app.forestTask = new QueryTask(app.ForestUrl);
                      app.forestQuery = new Query();
                      app.forestQuery.returnGeometry = true;
                      app.forestQuery.outFields = ["LEGALSTATU,BLOCK"];
                      app.forestQuery.geometry = app.geometry;
                      exe_ForestyQry = app.forestTask.execute(app.forestQuery);

                      //Perform WildArea Query
                      app.wildareaTask = new QueryTask(app.wildlifeUrl);
                      app.wildareaQuery = new Query();
                      app.wildareaQuery.returnGeometry = true;
                      app.wildareaQuery.outFields = ["Wild_Life"];
                      app.wildareaQuery.geometry = app.geometry;
                      exe_WildQry = app.wildareaTask.execute(app.wildareaQuery);
                      promiseCall = all([exe_VillageQry, exe_WaterQry, exe_ForestyQry, exe_WildQry]);
                      promiseCall.then(getQueryResults, errorinQry);
                  }
                  else {
                      promiseCall = all([exe_VillageQry]);
                      promiseCall.then(getQueryResults, errorinQry);
                      document.getElementById("waterdiv").style.display = "none";
                      document.getElementById("forestdiv").style.display = "none";
                      document.getElementById("wildareadiv").style.display = "none";
                      document.getElementById("loading").style.display = "none";
                  }
              }
              function getQueryResults(queryResults) {
                  $("#btnSubmit").val("Please Wait...").attr('disabled', 'disabled');
                  $("#reUpload").val("Please Wait...").attr('disabled', 'disabled');
                  this.map.selectedArea = [];
                  array.forEach(queryResults[0]['features'], function (feat) {
                      var admin_unit = { "Div_Cd": feat.attributes['DIVISION_CODE'], "Dist_Cd": feat.attributes['DISTRICT_CODE'], "Tehsil_Cd": feat.attributes['TEHSIL_CODE'], "Blk_Cd": feat.attributes['BLOCK_CODE'], "Gp_Cd": feat.attributes['GP_FINAL_CODE'], "Vlg_Cd": feat.attributes['CENSUS_CD_2011'], "FOREST_DIVCODE": feat.attributes['FOREST_DIVCODE'], "FOREST_RANGECODE ": feat.attributes['FOREST_RANGECODE'], "Div_NM": feat.attributes['DIVISION_NAME'], "Dist_NM": feat.attributes['DISTRICT_NAME_EN'], "Block_NM": feat.attributes['BLOCK_NAME'], "Village_NM": feat.attributes['CENSUS_NM_2011'], "Gp_NM": feat.attributes['GP_FINAL'], "areaName": "NA", "Tehsil_NM": feat.attributes['TEHSIL_NAME'] }; //, "FOREST_DIVCODE": feat.attributes['FOREST_DIVCODE'], "FOREST_RANGECODE ": feat.attributes['FOREST_RANGECODE']
                      if (queryResults[0]['features'].length > 0)
                          this.map.selectedArea.push(admin_unit);
                  });

                  var returnData = JSON.stringify(this.map.selectedArea);
                  var lbl = "One village found with-in area of interest.";
                  if (this.map.selectedArea.length > 1) lbl = "Total " + this.map.selectedArea.length + " Villages found with-in area of interest. ";
                  dom.byId("kmldiv").innerHTML = lbl;
                  document.getElementById("ids").value = returnData;
                  document.getElementById("gisid").value = fileGuID;
                  document.getElementById("successFlag").value = "true";
                  document.getElementById("locCentroid").value = locCentroid;
                  document.getElementById("filePath").value = filePath;
                  document.getElementById("resultDiv").style.display = "block";
                  document.getElementById("uploadDiv").style.display = "none";

                  //Perform POI Query Results

                  var poi_url = serviceUrl.poiUrl;
                  var lowest_dist = 100000; var lowest_dist_feature = null;
                  var mytemp = '';
                  $.each(this.map.selectedArea, function (i, val) {
                      app.POITask = new QueryTask(poi_url);
                      app.poiQuery = new Query();
                      app.poiQuery.returnGeometry = true;
                      app.poiQuery.where = "CENSUS_CD_='" + val.Vlg_Cd + "'";
                      app.poiQuery.outFields = ["POI_NAME,FACILITY_D,CENSUS_CD_"];
                      app.poiQuery.geometry = app.geometry;
                      exe_POIQry = app.POITask.execute(app.poiQuery, function (poiServ) {
                          if (poiServ['features'].length > 0) {
                              if (poiServ['features'].length == 1) {
                                  val.areaName = poiServ['features'][0].attributes['POI_NAME'] + "-" + poiServ['features'][0].attributes['Facility_D'];
                                  nearPOI.push(poiServ['features'][0].attributes['POI_NAME']);
                                  mytemp += poiServ['features'][0].attributes['POI_NAME'];
                              }
                              else {
                                  try {
                                      $.each(poiServ['features'], function (index, featList) {
                                          if (mainGeo == null && typeof mainGeo === 'undefined')
                                              var dist = dojo.number.format(geometryEngine.distance(app.drawGeometry, featList.geometry, 9036), { places: 2 });
                                          else
                                              var dist = dojo.number.format(geometryEngine.distance(outputGeom[0], featList.geometry, 9036), { places: 2 });
                                          if (dist < lowest_dist) {
                                              lowest_dist = dist;
                                              nearPOI.push(featList.attributes['POI_NAME']);
                                              lowest_dist_feature = featList.attributes['POI_NAME'] + "-" + featList.attributes['Facility_D'];
                                              val.areaName = lowest_dist_feature;
                                              mytemp += featList.attributes['POI_NAME'] + ",";
                                          }
                                      });
                                      lowest_dist = 100000;
                                      lowest_dist_feature = null;
                                      //document.getElementById("ids").value = JSON.stringify(this.map.selectedArea);
                                      nearPOI = [];
                                  }
                                  catch (e) {
                                      alert("Error in " + e); document.getElementById("loading").style.display = "none";
                                  }
                              }
                              document.getElementById("ids").value = JSON.stringify(this.map.selectedArea);
                              dom.byId("poidiv").innerHTML = mytemp;
                          }
                          else
                              dom.byId("poidiv").innerHTML = "There is no POI found nearby.";
                      });
                  });

                  if (queryResults.length > 1) {
                      //Water Query Result Start

                      if (queryResults[1]['features'].length > 0) {
                          var dist;
                          $.each(queryResults[1]['features'], function (i, feats) {
                              if (mainGeo == null && typeof mainGeo === 'undefined')
                                  dist = dojo.number.format(geometryEngine.distance(app.drawGeometry, feats.geometry, 9036), { places: 2 });
                              else
                                  dist = dojo.number.format(geometryEngine.distance(outputGeom[0], feats.geometry, 9036), { places: 2 });
                              if (dist < lowest_dist) {
                                  lowest_dist = dist;
                                  lowest_dist_feature = feats.attributes['FEAT_TYPE'] + "," + feats.attributes['POLYGON_NM'];
                              }
                          });
                          document.getElementById("nearbywaterbody").value = lowest_dist_feature;
                          document.getElementById("nearbywaterbodydistance").value = lowest_dist;
                          if (lowest_dist == 0) {
                              dom.byId("waterdiv").innerHTML = "The waterbody " + lowest_dist_feature + " is found inside area of interest.";
                          }
                          else {
                              dom.byId("waterdiv").innerHTML = "The nearest water body is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest. ";
                          }
                          lowest_dist = 100000;
                          lowest_dist_feature = null;
                      }
                      else {
                          dom.byId("waterdiv").innerHTML = "There is no water body found with in 5 KM.";
                      }

                      //Forest Query Result Start

                      if (queryResults[2]['features'].length > 0) {
                          var geom1 = null;
                          var geom2 = null;
                          if (mainGeo == null && typeof mainGeo === 'undefined') {
                              if (app.drawGeometry.type == "polygon")
                                  geom1 = app.drawGeometry;
                          }
                          else {
                              if (mainGeo.type == "polygon")
                                  geom1 = outputGeom[0];
                          }
                          try {
                              var dist;
                              $.each(queryResults[2]['features'], function (i, feat) {
                                  if (mainGeo == null && typeof mainGeo === 'undefined')
                                      dist = dojo.number.format(geometryEngine.distance(app.drawGeometry, feat.geometry, 9036), { places: 2 });
                                  else
                                      dist = dojo.number.format(geometryEngine.distance(outputGeom[0], feat.geometry, 9036), { places: 2 });
                                  if (dist < lowest_dist) {
                                      lowest_dist = dist;
                                      lowest_dist_feature = feat.attributes['BLOCK'] + "," + feat.attributes['LEGAL_STAT'];
                                      if (lowest_dist == 0)
                                          geom2 = feat.geometry;
                                  }
                              });
                              if (lowest_dist > 0) {
                                  document.getElementById("iswithinforest").value = "No";
                                  dom.byId("forestdiv").innerHTML = "The nearest forest area is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest.";
                              }
                              else {
                                  if (!geometryEngine.contains(geom2, geom1)) {
                                      var forestoverlayGeom = geometryEngine.difference(geom1, geom2);
                                      var overlayArea = dojo.number.format(geometryEngine.geodesicArea(forestoverlayGeom, 109414), { places: 3 });
                                      document.getElementById("inForestArea").value = dojo.number.format(shpArea - overlayArea, { places: 3 });
                                      document.getElementById("outForestArea").value = overlayArea;
                                  }
                                  else
                                      document.getElementById("inForestArea").value = shpArea;
                                  document.getElementById("iswithinforest").value = "Yes";
                                  dom.byId("forestdiv").innerHTML = "Forest Block (" + lowest_dist_feature + ") intersecting area of interest.";
                              }
                              document.getElementById("nearbyforestdistance").value = lowest_dist;
                          }
                          catch (ex) {
                              dist = 0;
                              lowest_dist = 100000;
                              lowest_dist_feature = null;
                              alert(ex);
                          }
                          lowest_dist = 100000;
                          lowest_dist_feature = null;
                      }
                      else {
                          document.getElementById("iswithinforest").value = "No";
                          dom.byId("forestdiv").innerHTML = "There is no forest area found with in 5 KM.";
                      }

                      // WildLife Query Result

                      if (queryResults[3]['features'].length > 0) {
                          $.each(queryResults[3]['features'], function (i, feats) {
                              if (mainGeo == null && typeof mainGeo === 'undefined')
                                  dist = dojo.number.format(geometryEngine.distance(app.drawGeometry, feats.geometry, 9036), { places: 2 });
                              else
                                  dist = dojo.number.format(geometryEngine.distance(outputGeom[0], feats.geometry, 9036), { places: 2 });
                              if (dist < lowest_dist) {
                                  lowest_dist = dist;
                                  lowest_dist_feature = feats.attributes['Wild_Life'];
                              }
                          });
                          if (lowest_dist > 0) {
                              document.getElementById("iswithinforest").value = "No";
                              dom.byId("wildareadiv").innerHTML = "The nearest Wildlife sanctuary/National park is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest.";
                          }
                          else {
                              document.getElementById("iswithinforest").value = "Yes";
                              dom.byId("wildareadiv").innerHTML = "Wildlife sanctuary/National park (" + lowest_dist_feature + ") intersecting area of interest.";
                          }
                          document.getElementById("nearbywildlifedistance").value = lowest_dist;
                          document.getElementById("nearbywildlife").value = lowest_dist_feature;
                          lowest_dist = 100000;
                          lowest_dist_feature = null;
                      }
                      else {
                          document.getElementById("nearbywildlife").value = "No";
                          dom.byId("wildareadiv").innerHTML = "There is no Wildlife sanctuary/National park found with in 5 KM.";
                      }
                  }
                  document.getElementById("loading").style.display = "none";
                  $("#btnSubmit").removeAttr('disabled');
                  $("#reUpload").removeAttr('disabled');
              }
              function errorinQry(qryError) {
                  console.log(qryError);
              }
              function OnComplete(result) {
                  try {
                      var fileExt = result.split('.').pop();
                      fileExt = fileExt.toLowerCase();
                      fileGuID = result.substr(0, result.indexOf('.'));
                      if (fileExt == "shp") {
                          filePath = window.location.hostname + ":801/shapeFile/" + result;
                          //document.getElementById("filePath").value = filePath;
                          document.getElementById("loading").style.display = "block";
                          document.getElementById("loadingData").innerHTML = "Finding nearest village/POI/waterbodies/forest/wildlife area...";
                          RenderShpFile(result);
                      }
                      else if (fileExt == "kml") {
                          filePath = window.location.hostname + ":801/shapeFile/" + result;
                          //document.getElementById("filePath").value = filePath;
                          document.getElementById("loading").style.display = "block";
                          document.getElementById("loadingData").innerHTML = "Finding nearest village/POI/waterbodies/forest/wildlife area...";
                          ShowKML(result);
                      }
                      else {
                          alert("Upload KML/Shape file only!!");
                          document.getElementById("loading").style.display = "none";
                      }
                  } catch (e) {
                      document.getElementById("loading").style.display = "none";
                      alert("File Upload Failed due to " + e);
                  }
              }
              function OnFail(result) {
                  alert('Upload Request Failed');
                  document.getElementById("loading").style.display = "none";
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
                      document.getElementById("fileUploadDiv").style.display = "none";
                      document.getElementById("drawDiv").style.display = "none";
                      var requestFor = '<%= reqFor %>';
                      drawToolbar.deactivate();
                      map.graphics.clear();
                      if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other" || requestFor == "AmritaDevi") {
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
          });

          function submitResult() {
              var finalCordinates, textToWrite;
              if (mainGeo == null && typeof mainGeo === 'undefined') {
                  if (app.drawGeometry.type == "polyline") {
                      finalCordinates = JSON.stringify(app.drawGeometry.paths[0]);
                      //textToWrite = JSON.stringify(app.geometry.rings[0]);
                  }
                  else {
                      finalCordinates = JSON.stringify(app.drawGeometry.rings[0]);
                      //textToWrite = JSON.stringify(app.geometry.rings[0]);
                  }
              }
              else {
                  if (mainGeo.type == "polyline") {
                      finalCordinates = JSON.stringify(outputGeom[0].paths[0]);
                      //textToWrite = JSON.stringify(app.geometry.rings[0]);
                  }
                  else {
                      finalCordinates = JSON.stringify(outputGeom[0].rings[0]);
                      //textToWrite = JSON.stringify(app.geometry.rings[0]);
                  }
              }
             <%-- var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });
              var fileNameToSaveAs = '<%= Gis_ID %>' + ".txt";
              var saveFolder = "shapeFile/";
              var downloadLink = document.createElement("a");
              downloadLink.download = fileNameToSaveAs;
              downloadLink.innerHTML = "Download File";
              if (window.webkitURL != null)                 
                  downloadLink.href = window.webkitURL.createObjectURL(textFileAsBlob);            
              else {                
                  downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
                  downloadLink.onclick = destroyClickedElement;
                  downloadLink.style.display = "none";
                  document.body.appendChild(downloadLink);
              }
              downloadLink.click();--%>
              $.ajax({
                  type: "POST",
                  datatype: "json",
                  url: "../services/requestHandler.ashx",
                  data: { reuestGisId: fileGuID, shapeCordinates: finalCordinates, requestFor: "UpdateCordinates" },
                  async: false,
                  success: function (result) {
                      document.getElementById("fmdssform").submit();
                  },
                  error: function (error) {
                      alert("Requested URL is not responding!!");
                      document.getElementById("loading").style.display = "none";
                      windowClose();
                  }
              });
          }
          function windowClose() {
              document.getElementById("successFlag").value = "false";
              document.getElementById("fmdssform").submit();
              //window.open('', '_parent', '');
              //window.close();
          }
          function ChkGeometry(curGeo, requestFor) {
              var isValid = false;
              if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other") {
                  isValid = (curGeo == "Polygon");
                  if (!isValid) {
                      alert("The Geometry of Uploaded file is not a Polygon!! Please upload Polygon type geometry.");
                      document.getElementById("loading").style.display = "none";
                  }
              }
              else if (requestFor == "Cable" || requestFor == "Telephone" || requestFor == "Transmission" || requestFor == "Roads") {
                  isValid = (curGeo == "Polyline");
                  if (!isValid) {
                      alert("The Geometry of Uploaded file is not a Polyline!! Please upload Polyline type geometry.");
                      document.getElementById("loading").style.display = "none";
                  }
              }
              return isValid;
          }
          function reUpload() {
              document.getElementById("kmldiv").innerHTML = "";
              document.getElementById("waterdiv").innerHTML = "";
              document.getElementById("forestdiv").innerHTML = "";
              document.getElementById("wildareadiv").innerHTML = "";
              document.getElementById("poidiv").innerHTML = "";
              document.getElementById("resultDiv").style.display = "none";
              document.getElementById("uploadDiv").style.display = "block";
              document.getElementById("btnUpload").style.display = "none";
              $("#fileUploadDiv").removeClass("col-md-12").addClass("col-md-6");
              $("#drawDiv").removeClass("col-md-12").addClass("col-md-6");
              document.getElementById("fileUploadDiv").style.display = "block";
              document.getElementById("drawDiv").style.display = "block";
          }
          function destroyClickedElement(event) {
              document.body.removeChild(event.target);
          }
    </script>
</head>
<body class="claro custom">
    <form enctype="multipart/form-data" action="../services/WebHandler.ashx" method="POST" id="uploadForm" runat="server">
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
                                    <div class="col-md-6" id="fileUploadDiv">
                                        <label class="file-upload btn btn-primary">
                                            <span>Upload AOI</span>
                                            <input type="file" name="filetoupload" id="filetoupload" />
                                        </label>
                                        <asp:HiddenField ID="logID" runat="server" />
                                    </div>
                                    <div class="col-md-6" id="drawDiv">
                                        <button id="drawBtn" data-dojo-type="dijit/form/Button">
                                            Draw AOI
                                        </button>
                                    </div>
                                    <div>&nbsp;</div>
                                    <div class="col-md-12 text-center">
                                        <input type="submit" id="btnUpload" style="display: none" value="Upload" class="btn btn-success btn-block" />&ensp;
                                        <button type="button" onclick="windowClose()" class="btn btn-danger btn-block">Cancel</button>
                                    </div>
                                </div>
                            </div>
                            <div id="resultDiv" class="panel panel-success" style="display: none; z-index: 9999">
                                <div class="panel-heading">
                                    <h5 class="text-left" style="color: #4584ee; font-weight: 700"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>&nbsp;Result</h5>
                                </div>
                                <div class="panel-body" style="height: auto">
                                    <div class="text-danger" id="kmldiv" style="overflow-wrap: break-word; text-wrap: normal; border: 1px solid #ddd; border-radius: 5px; padding: 4px">
                                    </div>
                                    <div style="padding: 1px">&ensp;</div>
                                    <div class="text-danger" id="poidiv" style="overflow-wrap: break-word; text-wrap: normal; background-color: #FFFFAA; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                                    </div>
                                    <div style="padding: 1px">&ensp;</div>
                                    <div class="text-success" id="waterdiv" style="overflow-wrap: break-word; text-wrap: normal; background-color: #fcf8e3; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                                    </div>
                                    <div style="padding: 1px">&ensp;</div>
                                    <div class="text-primary" id="forestdiv" style="overflow-wrap: break-word; text-wrap: normal; background-color: #f5f5f5; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
                                    </div>
                                    <div style="padding: 1px">&ensp;</div>
                                    <div class="text-default" id="wildareadiv" style="overflow-wrap: break-word; text-wrap: normal; background-color: #D8E9D2; border: 1px solid #ddd; padding: 4px; border-radius: 5px">
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
    </form>
    <form method="post" id="fmdssform" name="fmdssform">
        <input type="hidden" id="ids" name="ids" value="NA" />
        <input type="hidden" id="gisid" name="gisid" value="NA" />
        <input type="hidden" id="nearbywaterbody" name="nearbywaterbody" value="NA" />
        <input type="hidden" id="nearbywaterbodydistance" name="nearbywaterbodydistance" value="NA" />
        <input type="hidden" id="nearbywildlife" name="nearbywildlife" value="NA" />
        <input type="hidden" id="nearbywildlifedistance" name="nearbywildlifedistance" value="NA" />
        <input type="hidden" id="nearbyforestdistance" name="nearbyforestdistance" value="NA" />
        <input type="hidden" id="iswithinforest" name="iswithinforest" value="NA" />
        <input type="hidden" id="inForestArea" name="inForestArea" value="NA" />
        <input type="hidden" id="outForestArea" name="outForestArea" value="NA" />
        <input type="hidden" id="iswithinplantation" name="iswithinplantation" value="NA" />
        <input type="hidden" id="iswithinaravali" name="iswithinaravali" value="NA" />
        <input type="hidden" id="successFlag" name="successFlag" value="NA" />
        <input type="hidden" id="locCentroid" name="locCentroid" value="NA" />
        <input type="hidden" id="filePath" name="filePath" value="NA" />
        <input type="hidden" id="shapeArea" name="shapeArea" value="NA" />
        <input type="hidden" id="shapeLength" name="shapeLength" value="NA" />
        <input type="hidden" id="originalFileName" name="originalFileName" value="NA" />
    </form>
</body>
</html>
