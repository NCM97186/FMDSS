<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetRequest1.aspx.cs" Inherits="ForestryWebGIS.permitlocation.GetRequest1" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no">
    <title>FMDSS Web GIS Application</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/css/esri.css" />
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="//ajax.googleapis.com/ajax/libs/dojo/1.10.4/dijit/themes/claro/claro.css" />
    <link href="../css/fileupload.css" rel="stylesheet" />
    <style>
        #mapDiv {
            height: 100%;
            min-height: 700px;
            margin: 0;
            padding: 0;
            width: 100%;
        }
    </style>
    <script src="//code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="../scripts/jquery.form.js"></script>
    <script src="../scripts/togeojson.js"></script>
    <script src="../scripts/shapefile.js" type="text/javascript"></script>
    <script src="../scripts/terraformer.min.js"></script>
    <script src="../scripts/terraformer-arcgis-parser.min.js"></script>
    <script src="https://js.arcgis.com/3.16/"></script>  
    <script>
        var map, featureLayer, fileGuID, fileName, symbol, mainGeo, dynamicForest, outputGeom, filePath, areas, length, locCentroid, returnData1, basemapGallery;
        var visible = []; var app = {}; var legendLayers = []; var nearPOI = [];
        require([
            "esri/map",
            "dojo/_base/array",
            "esri/layers/ArcGISDynamicMapServiceLayer",
            "esri/layers/FeatureLayer",
            "esri/dijit/Legend",
            "esri/layers/ImageParameters",
            "esri/layers/KMLLayer",
            "esri/tasks/query",
            "esri/tasks/geometry",
            "esri/tasks/QueryTask",
            "esri/tasks/DistanceParameters",
            "esri/geometry/Polygon",
            "esri/geometry/Polyline",
            "esri/symbols/SimpleFillSymbol",
            "esri/symbols/SimpleLineSymbol",
            "esri/tasks/ProjectParameters",
            "esri/tasks/BufferParameters",
            "esri/tasks/AreasAndLengthsParameters",
            "esri/tasks/GeometryService",
            "esri/geometry/geometryEngine",
            "esri/graphic",
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
       Map, array, ArcGISDynamicMapServiceLayer, FeatureLayer, Legend, ImageParameters, KMLLayer, Query, Geometry, QueryTask, DistanceParameters,
       Polygon, Polyline, SimpleFillSymbol, SimpleLineSymbol, ProjectParameters, BufferParameters, AreasAndLengthsParameters, GeometryService,
       geometryEngine, Graphic, scaleUtils, Draw, Button, BasemapGallery, MenuItem, Color,serviceURL, all, sniff, dom, on, query, parser, registry, domStyle) {
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
              this.map.selectedArea = [];
              this.map.waterArea = [];
              this.map.legDistance = [];
              this.map.queryCon = [];
              var shpArea = 0;
              function ShowKML(kmlpath) {
                  var kmlpath1 = "shapeFile/" + kmlpath;
                  $.ajax(kmlpath1).done(function (xml) {
                      map.graphics.clear();
                      var xmlDoc = jQuery.parseXML(xml);
                      var obj = toGeoJSON.kml(xmlDoc);
                      var requestFor = '<%= reqFor %>';
                      var sms;
                      var singleRingPolygon;
                      var reqGeo = "";
                      var isValid = null;
                      var curGeo = obj.features[0].geometry.type;
                      if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other") {
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
                      //Area Calculation                                           
                      if (mainGeo.type == "polygon") {
                          var polygongeom = '[{"rings":[' + JSON.stringify(mainGeo.rings[0]) + ']}]';
                          var areaunit = '{"areaUnit":"esriSquareKilometers"}';
                          $.ajax({
                              dataType: "json",
                              url: serviceUrl.areaNlengthUrl,
                              data: { f: "json", polygons: polygongeom, sr: mainGeo.spatialReference.wkid, lengthUnit: esri.tasks.GeometryService.UNIT_KILOMETER, areaUnit: areaunit, calculationType: "preserveShape" },
                              type: "POST",
                              async: false,
                              success: function (areaLength) {
                                  shpArea = dojo.number.format(areaLength.areas, { places: 3 });
                                  document.getElementById("shapeArea").value = dojo.number.format(areaLength.areas, { places: 3 });
                                  document.getElementById("shapeLength").value = dojo.number.format(areaLength.lengths, { places: 3 });
                              }, error: function (error) { colsole.log(error); }
                          });
                      }
                      else {
                          var polygongeom = '[{"paths":[' + JSON.stringify(mainGeo.paths[0]) + ']}]';
                          $.ajax({
                              dataType: "json",
                              url: serviceUrl.lengthUrl,
                              data: { f: "json", polylines: polygongeom, sr: mainGeo.spatialReference.wkid, lengthUnit: esri.tasks.GeometryService.UNIT_KILOMETER, calculationType: "preserveShape" },
                              type: "POST",
                              async: false,
                              success: function (LengthResult) {
                                  document.getElementById("shapeLength").value = dojo.number.format(LengthResult.lengths, { places: 3 });
                              }, error: function (error) { console.log(error); }
                          });
                      }
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
                          app.queryTask = new QueryTask(app.queryURL);
                          app.query = new Query();
                          app.query.returnGeometry = false;
                          app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,FOREST_DIVCODE,FOREST_RANGECODE,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011"]; //,FOREST_DIVCODE,FOREST_RANGECODE
                          app.query.geometry = gr.geometry;
                          var execution, promises;
                          execution = app.queryTask.execute(app.query);
                          promises = all([execution]);
                          promises.then(handleQueryResults, handleQueryResultsError);

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
                      var isValid = null;

                      var curGeo = obj.features[0].geometry.type;
                      if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other") {
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
                      //Area Calculation                                           
                      if (mainGeo.type == "polygon") {
                          var polygongeom = '[{"rings":[' + JSON.stringify(mainGeo.rings[0]) + ']}]';
                          var areaunit = '{"areaUnit":"esriSquareKilometers"}';
                          $.ajax({
                              dataType: "json",
                              url: serviceUrl.areaNlengthUrl,
                              data: { f: "json", polygons: polygongeom, sr: mainGeo.spatialReference.wkid, lengthUnit: esri.tasks.GeometryService.UNIT_KILOMETER, areaUnit: areaunit, calculationType: "preserveShape" },
                              type: "POST",
                              async: false,
                              success: function (areaLength) {
                                  shpArea = dojo.number.format(areaLength.areas, { places: 3 });
                                  document.getElementById("shapeArea").value = dojo.number.format(areaLength.areas, { places: 3 });
                                  document.getElementById("shapeLength").value = dojo.number.format(areaLength.lengths, { places: 3 });
                              }, error: function (error) { console.log(error); }
                          });
                      }
                      else {
                          var polygongeom = '[{"paths":[' + JSON.stringify(mainGeo.paths[0]) + ']}]';
                          $.ajax({
                              dataType: "json",
                              url: serviceUrl.lengthUrl,
                              data: { f: "json", polylines: polygongeom, sr: mainGeo.spatialReference.wkid, lengthUnit: esri.tasks.GeometryService.UNIT_KILOMETER, calculationType: "preserveShape" },
                              type: "POST",
                              async: false,
                              success: function (LengthResult) {
                                  document.getElementById("shapeLength").value = dojo.number.format(LengthResult.lengths, { places: 3 });
                              }, error: function (error) { console.log(error); }
                          });
                      }
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
                          app.queryTask = new QueryTask(app.queryURL);
                          app.query = new Query();
                          app.query.returnGeometry = false;
                          app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,FOREST_DIVCODE,FOREST_RANGECODE,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011"];
                          app.query.geometry = gr.geometry;
                          var execution, promises;
                          execution = app.queryTask.execute(app.query);
                          promises = all([execution]);
                          promises.then(handleQueryResults, handleQueryResultsError);

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
                      app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,FOREST_DIVCODE,FOREST_RANGECODE,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011"];
                      app.query.geometry = app.drawGeometry;
                      var execution, promises;
                      execution = app.queryTask.execute(app.query);
                      promises = all([execution]);
                      promises.then(handleQueryResults, handleQueryResultsError);
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
                      var polygongeom = '[{"rings":[' + JSON.stringify(app.drawGeometry.rings[0]) + ']}]';
                      var areaunit = '{"areaUnit":"esriSquareKilometers"}';
                      $.ajax({
                          dataType: "json",
                          url: serviceUrl.areaNlengthUrl,
                          data: { f: "json", polygons: polygongeom, sr: app.drawGeometry.spatialReference.wkid, lengthUnit: esri.tasks.GeometryService.UNIT_KILOMETER, areaUnit: areaunit, calculationType: "preserveShape" },
                          type: "POST",
                          async: false,
                          success: function (areaLength) {
                              shpArea = dojo.number.format(areaLength.areas, { places: 3 });
                              document.getElementById("shapeArea").value = dojo.number.format(areaLength.areas, { places: 3 });
                              document.getElementById("shapeLength").value = dojo.number.format(areaLength.lengths, { places: 3 });
                          }, error: function (error) { console.log(error); }
                      });
                  }
                  else {
                      var polygongeom = '[{"paths":[' + JSON.stringify(app.drawGeometry.paths[0]) + ']}]';
                      $.ajax({
                          dataType: "json",
                          url: serviceUrl.lengthUrl,
                          data: { f: "json", polylines: polygongeom, sr: app.drawGeometry.spatialReference.wkid, lengthUnit: esri.tasks.GeometryService.UNIT_KILOMETER, calculationType: "preserveShape" },
                          type: "POST",
                          async: false,
                          success: function (LengthResult) {
                              document.getElementById("shapeLength").value = dojo.number.format(LengthResult.lengths, { places: 3 });
                          }, error: function (error) { console.log(error); }
                      });
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
                      var execution1, promises1;
                      execution1 = app.waterTask.execute(app.waterQuery);
                      promises1 = all([execution1]);
                      promises1.then(handleWaterResults);

                      //Perform Forest Query
                      app.forestTask = new QueryTask(app.ForestUrl);
                      app.forestQuery = new Query();
                      app.forestQuery.returnGeometry = true;
                      app.forestQuery.outFields = ["LEGAL_STAT,BLOCK"];
                      app.forestQuery.geometry = app.geometry;
                      var execution2, promises2;
                      execution2 = app.forestTask.execute(app.forestQuery);
                      promises2 = all([execution2]);
                      promises2.then(handleForestResults);

                      //Perform WildArea Query
                      app.wildareaTask = new QueryTask(app.wildlifeUrl);
                      app.wildareaQuery = new Query();
                      app.wildareaQuery.returnGeometry = true;
                      app.wildareaQuery.outFields = ["Wild_Life"];
                      app.wildareaQuery.geometry = app.geometry;
                      var execution3, promises3;
                      execution3 = app.wildareaTask.execute(app.wildareaQuery);
                      promises3 = all([execution3]);
                      promises3.then(handleWildAreaResults);
                  }
                  else {
                      document.getElementById("waterdiv").style.display = "none";
                      document.getElementById("forestdiv").style.display = "none";
                      document.getElementById("wildareadiv").style.display = "none";
                      document.getElementById("loading").style.display = "none";
                  }
              }
              function handleQueryResults(results) {
                  this.map.selectedArea = [];
                  this.map.queryCon = [];
                  array.forEach(results[0]['features'], function (feat) {
                      var admin_unit = { "Div_Cd": feat.attributes['DIVISION_CODE'], "Dist_Cd": feat.attributes['DISTRICT_CODE'], "Blk_Cd": feat.attributes['BLOCK_CODE'], "Gp_Cd": feat.attributes['GP_FINAL_CODE'], "Vlg_Cd": feat.attributes['CENSUS_CD_2011'], "FOREST_DIVCODE": feat.attributes['FOREST_DIVCODE'], "FOREST_RANGECODE ": feat.attributes['FOREST_RANGECODE'], "Div_NM": feat.attributes['DIVISION_NAME'], "Dist_NM": feat.attributes['DISTRICT_NAME_EN'], "Block_NM": feat.attributes['BLOCK_NAME'], "Village_NM": feat.attributes['CENSUS_NM_2011'], "Gp_NM": feat.attributes['GP_FINAL'], "areaName": "NA" }; //, "FOREST_DIVCODE": feat.attributes['FOREST_DIVCODE'], "FOREST_RANGECODE ": feat.attributes['FOREST_RANGECODE']
                      if (results[0]['features'].length > 0) {
                          this.map.queryCon.push(feat.attributes['CENSUS_CD_2011']);
                          this.map.selectedArea.push(admin_unit);
                      }
                  });
                  returnData1 = this.map.selectedArea;
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
                  //document.getElementById("loading").style.display = "none";
                  //Perform POI Query
                  checkResult();
                  POIResults();
              }
              function handleWaterResults(results) {
                  //document.getElementById("loading").style.display = "block";
                  document.getElementById("loadingData").innerHTML = "Finding nearest waterbodies/forest/wildlife area...";
                  this.map.waterArea = [];
                  var distParam = new DistanceParameters();
                  distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER; //.UNIT_STATUTE_MILE;
                  if (mainGeo == null && typeof mainGeo === 'undefined') {
                      if (app.drawGeometry.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(app.drawGeometry.paths[0]) + ']}}';
                      }
                      else {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(app.drawGeometry.rings[0]) + ']}}';
                      }
                  }
                  else {
                      if (mainGeo.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(outputGeom[0].paths[0]) + ']}}';
                      }
                      else {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(outputGeom[0].rings[0]) + ']}}';
                      }
                  }
                  distParam.geodesic = true;
                  var lowest_dist = 1000000000; var lowest_dist_feature = null;
                  if (results[0]['features'].length > 0) {
                      array.forEach(results[0]['features'], function (feat) {

                          distParam.geometry2 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(feat.geometry.rings[0]) + ']}}'; //JSON.stringify(feat.geometry);
                          var dist_url = serviceUrl.distanceUrl;
                          $.ajax({
                              dataType: "json",
                              url: dist_url,
                              data: { sr: 102100, geometry1: distParam.geometry1, geometry2: distParam.geometry2, geodesic: true, distanceUnit: distParam.distanceUnit, f: "pjson" },
                              type: "POST",
                              async: false,
                              success: function (rec_distance) {
                                  var dist;
                                  try {
                                      dist = dojo.number.format(rec_distance.distance, { places: 2 });
                                  } catch (e) {
                                      dist = 0;
                                  }
                                  if (dist < lowest_dist) {
                                      lowest_dist = dist;
                                      lowest_dist_feature = feat.attributes['FEAT_TYPE'] + "," + feat.attributes['POLYGON_NM'];
                                  }
                              },
                              error: function (error)
                              { alert("Unable to Calculate distance : " + error); document.getElementById("loading").style.display = "none"; }
                          });
                      });
                      lowest_dist = dojo.number.format(lowest_dist, { places: 2 });
                      document.getElementById("nearbywaterbody").value = lowest_dist_feature;
                      document.getElementById("nearbywaterbodydistance").value = lowest_dist;
                      if (lowest_dist == 0) {
                          dom.byId("waterdiv").innerHTML = "The waterbody " + lowest_dist_feature + " is found inside area of interest.";
                      }
                      else {
                          dom.byId("waterdiv").innerHTML = "The nearest water body is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest. ";
                      }
                  }
                  else {
                      dom.byId("waterdiv").innerHTML = "There is no water body found with in 5 KM.";
                  }
                  checkResult();
              }
              function handleForestResults(results) {
                  //document.getElementById("loading").style.display = "block";
                  //document.getElementById("loadingData").innerHTML = "Finding nearest waterbodies/forest/wildlife area...";
                  //this.map.waterArea = [];
                  var geom1 = null;
                  var geom2 = null;
                  var distParam = new DistanceParameters();
                  distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER; //.UNIT_STATUTE_MILE;
                  if (mainGeo == null && typeof mainGeo === 'undefined') {
                      if (app.drawGeometry.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(app.drawGeometry.paths[0]) + ']}}';
                      }
                      else {
                          geom1 = app.drawGeometry;
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(app.drawGeometry.rings[0]) + ']}}';
                      }
                  }
                  else {
                      if (mainGeo.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(outputGeom[0].paths[0]) + ']}}';
                      }
                      else {
                          geom1 = outputGeom[0];
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(outputGeom[0].rings[0]) + ']}}';
                      }
                  }
                  distParam.geodesic = true;
                  var lowest_dist = 1000000000; var lowest_dist_feature = null;
                  if (results[0]['features'].length > 0) {
                      array.forEach(results[0]['features'], function (feat) {
                          distParam.geometry2 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(feat.geometry.rings[0]) + ']}}'; //JSON.stringify(feat.geometry);
                          var dist_url = serviceUrl.distanceUrl;
                          $.ajax({
                              dataType: "json",
                              url: dist_url,
                              data: { sr: 102100, geometry1: distParam.geometry1, geometry2: distParam.geometry2, geodesic: true, distanceUnit: distParam.distanceUnit, f: "pjson" },
                              type: "POST",
                              async: false,
                              success: function (rec_distance) {
                                  //;
                                  var dist;
                                  try {
                                      dist = dojo.number.format(rec_distance.distance, { places: 2 });
                                  } catch (e) {
                                      dist = 0;
                                  }
                                  if (dist < lowest_dist) {
                                      lowest_dist = dist;
                                      lowest_dist_feature = feat.attributes['BLOCK'] + "," + feat.attributes['LEGAL_STAT'];
                                  }
                                  if (dist == 0)
                                      geom2 = feat.geometry;
                              },
                              error: function (error)
                              { alert("Unable to Calculate distance : " + error); document.getElementById("loading").style.display = "none"; }
                          });
                      });
                      lowest_dist = dojo.number.format(lowest_dist, { places: 2 });
                      if (lowest_dist > 0) {
                          document.getElementById("iswithinforest").value = "No";
                          dom.byId("forestdiv").innerHTML = "The nearest forest area is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest.";

                      }
                      else {
                          var forestoverlayGeom = geometryEngine.difference(geom1, geom2);
                          var overlayArea = Math.round(geometryEngine.geodesicArea(forestoverlayGeom, "square-kilometers") * 100) / 100;
                          document.getElementById("inForestArea").value = shpArea - overlayArea;
                          document.getElementById("outForestArea").value = overlayArea;
                          document.getElementById("iswithinforest").value = "Yes";
                          dom.byId("forestdiv").innerHTML = "Forest Block (" + lowest_dist_feature + ") intersecting area of interest.";
                      }
                      document.getElementById("nearbyforestdistance").value = lowest_dist;
                  }
                  else {
                      document.getElementById("iswithinforest").value = "No";
                      dom.byId("forestdiv").innerHTML = "There is no forest area found with in 5 KM.";
                  }
                  checkResult();
              }
              function handleWildAreaResults(results) {
                  var distParam = new DistanceParameters();
                  distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
                  if (mainGeo == null && typeof mainGeo === 'undefined') {
                      if (app.drawGeometry.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(app.drawGeometry.paths[0]) + ']}}';
                      }
                      else {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(app.drawGeometry.rings[0]) + ']}}';
                      }
                  }
                  else {
                      if (mainGeo.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(outputGeom[0].paths[0]) + ']}}';
                      }
                      else {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(outputGeom[0].rings[0]) + ']}}';
                      }
                  }
                  distParam.geodesic = true;
                  var lowest_dist = 1000000000; var lowest_dist_feature = null;
                  if (results[0]['features'].length > 0) {
                      array.forEach(results[0]['features'], function (feat2) {
                          //  var admin_unit1 = feat.attributes['LEGAL_STAT'] + "-" + feat.attributes['BLOCK']   + "<hr class='hr' />";
                          // this.map.waterArea.push(admin_unit1);
                          array.forEach(feat2.geometry.rings, function (rg) {
                              var json_str = JSON.stringify(rg);
                              distParam.geometry2 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + json_str + ']}}'; //feat2.geometry.rings[0]
                              var dist_url = serviceUrl.distanceUrl;
                              $.ajax({
                                  dataType: "json",
                                  url: dist_url,
                                  data: { sr: 102100, geometry1: distParam.geometry1, geometry2: distParam.geometry2, geodesic: true, distanceUnit: distParam.distanceUnit, f: "pjson" },
                                  type: "POST",
                                  async: false,
                                  success: function (rec_distance) {
                                      var dist;
                                      try {
                                          dist = parseFloat(dojo.number.format(rec_distance.distance, { places: 2 }));
                                      } catch (e) {
                                          dist = 0;
                                      }
                                      if (dist < lowest_dist) {
                                          lowest_dist = dist;
                                          lowest_dist_feature = feat2.attributes['Wild_Life'];
                                      }
                                  },
                                  error: function (error)
                                  { alert("Unable to Calculate distance : " + error); document.getElementById("loading").style.display = "none"; }
                              });
                          });
                      });
                      lowest_dist = dojo.number.format(lowest_dist, { places: 2 });
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
                  }
                  else {
                      document.getElementById("nearbywildlife").value = "No";
                      dom.byId("wildareadiv").innerHTML = "There is no Wildlife sanctuary/National park found with in 5 KM.";
                  }
                  document.getElementById("loading").style.display = "none";
                  checkResult();
              }
              function handleQueryResultsError(error) {
                  alert("Rest Service is not responding : " + error);
              }
              function POIResults() {
                  document.getElementById("loading").style.display = "block";
                  document.getElementById("loadingData").innerHTML = "Finding nearest POI area...";
                  var distParam = new DistanceParameters();
                  distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
                  if (mainGeo == null && typeof mainGeo === 'undefined') {
                      if (app.drawGeometry.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(app.drawGeometry.paths[0]) + ']}}';
                      }
                      else {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(app.drawGeometry.rings[0]) + ']}}';
                      }
                  }
                  else {
                      if (mainGeo.type == "polyline") {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(outputGeom[0].paths[0]) + ']}}';
                      }
                      else {
                          distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(outputGeom[0].rings[0]) + ']}}';
                      }
                  }
                  distParam.geodesic = true;
                  var lowest_dist = 100000; var lowest_dist_feature = null;
                  nearPOI = [];
                  array.forEach(returnData1, function (name) {
                      var poi_url = serviceUrl.poiUrl;
                      $.ajax({
                          dataType: "json",
                          url: poi_url,
                          data: { where: "CENSUS_CD_='" + name.Vlg_Cd + "'", geometryType: "esriGeometryEnvelope", spatialRel: "esriSpatialRelIntersects", outFields: "POI_NAME,FACILITY_D,CENSUS_CD_", returnGeometry: true, f: "pjson" },
                          type: "POST",
                          async: false,
                          success: function (poiData) {
                              if (poiData['features'].length > 0) {
                                  if (poiData['features'].length == 1) {
                                      name.areaName = poiData['features'][0].attributes['POI_NAME'] + "-" + poiData['features'][0].attributes['Facility_D'];
                                      nearPOI.push(poiData['features'][0].attributes['POI_NAME']);
                                  }
                                  else {
                                      try {
                                          array.forEach(poiData['features'], function (feat3) {
                                              var json_str = '"x":' + JSON.stringify(feat3.geometry.x) + ',"y":' + JSON.stringify(feat3.geometry.y);
                                              distParam.geometry2 = '{"geometryType":"esriGeometryPoint","geometry":{' + json_str + '}}';
                                              var dist_url = serviceUrl.distanceUrl;
                                              $.ajax({
                                                  dataType: "json",
                                                  url: dist_url,
                                                  data: { sr: 102100, geometry1: distParam.geometry1, geometry2: distParam.geometry2, geodesic: true, distanceUnit: distParam.distanceUnit, f: "pjson" },
                                                  type: "POST",
                                                  async: false,
                                                  success: function (rec_distance) {
                                                      var dist;
                                                      try {
                                                          dist = rec_distance.distance;
                                                      } catch (e) {
                                                          dist = 0;
                                                      }
                                                      if (dist < lowest_dist) {
                                                          lowest_dist = dist;
                                                          nearPOI.push(feat3.attributes['POI_NAME']);
                                                          lowest_dist_feature = feat3.attributes['POI_NAME'] + "-" + feat3.attributes['Facility_D'];
                                                          name.areaName = lowest_dist_feature;
                                                      }
                                                  },
                                                  error: function (error)
                                                  { alert("Unable to Calculate distance : " + error); document.getElementById("loading").style.display = "none"; }
                                              });
                                          });
                                          dom.byId("poidiv").innerHTML = nearPOI;
                                          lowest_dist = 100000;
                                          lowest_dist_feature = null;
                                          document.getElementById("ids").value = JSON.stringify(returnData1);
                                      }
                                      catch (e) {
                                          alert("Error in " + e); document.getElementById("loading").style.display = "none";
                                      }
                                  }
                              }
                              else
                                  dom.byId("poidiv").innerHTML = "There is no POI found nearby.";
                          },
                          error: function (error)
                          { alert("Unable to find POI data: " + error); document.getElementById("loading").style.display = "none"; }
                      });
                  });
                  document.getElementById("loading").style.display = "none";
                  returnData1 = [];
                  checkResult();
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
                  url: "/services/requestHandler.ashx",
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
          function checkResult() {
              var requestFor = '<%= reqFor %>';
              if (requestFor == "Mines") {
                  if ($.trim($("#kmldiv").html()).length == 0 || $.trim($("#poidiv").html()).length == 0 || $.trim($("#waterdiv").html()).length == 0 || $.trim($("#wildareadiv").html()).length == 0 || $.trim($("#forestdiv").html()).length == 0) {
                      document.getElementById("loading").style.display = "none";
                      document.getElementById("loading").style.display = "block";
                      document.getElementById("loadingData").innerHTML = "The query task is resource and time consuming.<br/>Please be patience and do not click any where untill results come.";
                      $("#btnSubmit").val("Please Wait...").attr('disabled', 'disabled');
                      $("#reUpload").val("Please Wait...").attr('disabled', 'disabled');
                  }
                  else {
                      document.getElementById("loading").style.display = "none";
                      $("#btnSubmit").removeAttr('disabled');
                      $("#reUpload").removeAttr('disabled');
                  }
              }
              else {
                  if ($.trim($("#kmldiv").html()).length == 0 || $.trim($("#poidiv").html()).length == 0) {
                      document.getElementById("loading").style.display = "none";
                      document.getElementById("loading").style.display = "block";
                      document.getElementById("loadingData").innerHTML = "The query task is resource and time consuming.<br/>Please be patience and do not click any where untill results come.";
                      $("#btnSubmit").val("Please Wait...").attr('disabled', 'disabled');
                      $("#reUpload").val("Please Wait...").attr('disabled', 'disabled');
                  }
                  else {
                      document.getElementById("loading").style.display = "none";
                      $("#btnSubmit").removeAttr('disabled');
                      $("#reUpload").removeAttr('disabled');
                  }
              }
          }
    </script>
</head>
<body class="claro custom">
    <form enctype="multipart/form-data" action="/services/WebHandler.ashx" method="POST" id="uploadForm" runat="server">
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
