<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewonGis.aspx.cs" Inherits="ForestryWebGIS.gisview.ViewonGis" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <!-- #include file ="../dojopackage.html" -->​
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>View on GIS Application</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.16/esri/css/esri.css" />
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body {
            line-height: 1.12345;
        }

        #mapDiv {
            height: 100%;
            min-height: 700px;
            margin: 0;
            padding: 0;
            width: 100%;
        }

        #loading {
            width: 100%;
            height: 100%;
            top: 0px;
            left: 0px;
            position: fixed;
            display: none;
            opacity: 0.9;
            background-color: #000;
            z-index: 99;
            font-family: Arial;
            color: forestgreen;
            font-weight: 700;
            font-size: larger;
            text-align: center;
        }

        #loading-image {
            position: absolute;
            background: #ffffff url("https://www.cebglobal.com/shl/images/general/ajax-loader.gif") no-repeat left;
            top: 35%;
            left: 35%;
            z-index: 100;
            padding: 8px;
            border-radius: 6px;
        }

        .col-md-2 {
            padding: 2px;
        }

        .col-md-3 {
            padding: 2px;
        }

        .col-md-5 {
            padding: 2px;
        }

        .col-md-6 {
            padding: 2px;
        }

        .col-md-8 {
            padding: 2px;
        }

        .list-group-item {
            padding: 6px 10px;
        }
          .logo_raj {
            position: absolute;
            width: 80px;
            bottom: 3px;
            right:30px;
        }

    </style>
    <script src="//code.jquery.com/jquery-1.12.2.min.js"></script>        
    <script src="//js.arcgis.com/3.16/"></script>
    <script type="text/javascript">
        var map, featureLayer, fileGuID, fileName, symbol, mainGeo, dynamicForest, filePath, areas, length, returnData1;
        var visible = []; var app = {}; var legendLayers = []; var nearPOI = [];
        require([
        "esri/map",
        "dojo/_base/array",
        "esri/layers/ArcGISDynamicMapServiceLayer",
        "esri/layers/FeatureLayer",
        "esri/dijit/Legend",
        "esri/layers/ImageParameters",
        "esri/tasks/query",
        "esri/tasks/geometry",
        "esri/tasks/QueryTask",
        "esri/tasks/DistanceParameters",
        "esri/geometry/Polygon",
        "esri/geometry/Polyline",
        "esri/symbols/SimpleFillSymbol",
        "esri/symbols/SimpleLineSymbol",
        "esri/tasks/ProjectParameters",
        "esri/geometry/normalizeUtils",
        "esri/tasks/BufferParameters",
        "esri/tasks/AreasAndLengthsParameters",
              "DojoClass/TokenAuth",
        "esri/tasks/GeometryService",
        "esri/geometry/geometryEngine",
        "esri/graphic",
        "esri/Color",
         "dojo/text!../scripts/serviceURL.json",
        "dojo/promise/all",
        "dojo/dom",
        "dojo/on",
        "dojo/query",
        "dojo/parser",
        "dojo/dom-style",
        "dojo/domReady!"
        ],
    function (
 Map, array, ArcGISDynamicMapServiceLayer, FeatureLayer, Legend, ImageParameters, Query, Geometry, QueryTask, DistanceParameters,
 Polygon, Polyline, SimpleFillSymbol, SimpleLineSymbol, ProjectParameters, normalizeUtils, BufferParameters, AreasAndLengthsParameters,TokenAuth,
 GeometryService, geometryEngine, Graphic, Color, serviceURL, all, dom, on, query, parser, domStyle) {
        parser.parse();
        serviceUrl = JSON.parse(serviceURL);
        app.queryURL = serviceUrl.villageQueryUrl;
        var mygp = new GeometryService(serviceUrl.gpUrl);
        app.WaterUrl = serviceUrl.waterUrl;
        app.ForestUrl = serviceUrl.forestUrl;
        app.wildlifeUrl = serviceUrl.wildlifeUrl;
        setTimeout(function () {
            document.getElementById("loading").style.display = "block";
            viewOnGis();
        }, 1000);
        map = new Map("mapDiv", {
            sliderOrientation: "horizontal",
            sliderPosition: "top-center",
            zoom: 5,
            logo: false
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
        this.map.selectedArea = [];
        this.map.waterArea = [];
        this.map.legDistance = [];
        var shpArea = 0;

        function viewOnGis() {
            document.getElementById("loadingData").innerHTML = "Fetching Results for file " + '<%= fName %>';
            var requestFor = '<%= reqFor %>';
            var shpCordinate = '<%= shpCordinates %>';
            var shpArr = [];
            var sms;
            var singleRingPolygon;
            var reqGeo = null;
            var curGeo = null;
            var polygonJson = null;
            map.graphics.clear();
            if (requestFor == "Forest" || requestFor == "Mines" || requestFor == "Hospital" || requestFor == "School" || requestFor == "Industry" || requestFor == "Power" || requestFor == "Sawmill" || requestFor == "Other") {
                reqGeo = "Polygon";
                curGeo = "Polygon";
                isValid = (curGeo == "Polygon");
            }
            else if (requestFor == "Cable" || requestFor == "Telephone" || requestFor == "Transmission" || requestFor == "Roads") {
                reqGeo = "Polyline";
                curGeo = "LineString"
                isValid = (curGeo == "LineString");
            }
            if (!isValid) {
                alert("The Geometry of Uploaded file is type of " + curGeo + " while system expecting " + reqGeo + " !! Please upload valid " + reqGeo + " type geometry.");
                document.getElementById("loading").style.display = "none";
                return;
            }
            if (reqGeo == "Polygon") {
                shpArr = JSON.parse(shpCordinate);
                polygonJson = { "rings": [shpArr], "spatialReference": { "wkid": 102100 } };
                singleRingPolygon = new Polygon(polygonJson);
                sms = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color(["#000000"]), 2), new Color([249, 4, 4, 0.75]));
            }
            else {
                shpArr = JSON.parse(shpCordinate);
                polygonJson = { "paths": [shpArr], "spatialReference": { "wkid": 102100 } };
                singleRingPolygon = new Polyline(polygonJson);
                sms = new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color([0, 0, 255]), 3);
            }
            var gr = new Graphic(singleRingPolygon, sms);
            var extnt = gr.geometry.getExtent();
            if (Math.round(extnt.xmax) > 8713359 || Math.round(extnt.ymax) > 3529302 || Math.round(extnt.xmin) < 7734875 || Math.round(extnt.ymin) < 2639075) {
                alert("Shp/KML file is outside from Rajasthan Extent. Please ensure the uploaded Shape is having geographic cordinate system.");
                document.getElementById("loading").style.display = "none";
                return;
            }
            map.graphics.add(gr);
            mainGeo = gr.geometry;
            //Area Calculation                                           
            if (mainGeo.type == "polygon") {
                if ('<%= viewAll %>' == 'Y') {
                    shpArea = dojo.number.format(geometryEngine.geodesicArea(mainGeo, 109414), { places: 3 });
                    shpLength = dojo.number.format(geometryEngine.geodesicLength(mainGeo, 9036), { places: 2 });
                    document.getElementById("lblLength").innerHTML = "Length";
                    document.getElementById("shapeArea").innerHTML = shpArea + "Sq.km";
                    document.getElementById("shapeLength").innerHTML = shpLength + "km";
                }
            }
            else {
                if ('<%= viewAll %>' == 'Y') {
                    shpLength = dojo.number.format(geometryEngine.geodesicLength(mainGeo, 9036), { places: 2 });
                    document.getElementById("lblLength").innerHTML = "Perimeter";
                    document.getElementById("shapeArea").innerHTML = "NA";
                    document.getElementById("shapeLength").innerHTML = shpLength + "km";;
                }
            }
            app.queryTask = new QueryTask(app.queryURL);
            app.query = new Query();
            app.query.returnGeometry = false;
            app.query.outFields = ["DIVISION_CODE,DISTRICT_CODE,BLOCK_CODE,GP_FINAL_CODE,GP_FINAL,CENSUS_CD_2011,DISTRICT_NAME_EN,DIVISION_NAME,BLOCK_NAME,CENSUS_NM_2011"];
            app.query.geometry = gr.geometry;
            var execution, promises;
            execution = app.queryTask.execute(app.query);
            promises = all([execution]);
            promises.then(handleQueryResults, handleQueryResultsError);
            //setup the buffer parameters
            var params = new BufferParameters();
            params.distances = [0, 5];
            params.outSpatialReference = map.spatialReference;
            params.unit = GeometryService.UNIT_KILOMETER;
            mygp.simplify([gr.geometry], function (geometries) {
                params.geometries = geometries;
                mygp.buffer(params, showBuffer);
            }, function (error) { alert("Some Error on Buffer Creation " + error); });
        }
        function showBuffer(bufferedGeometries) {
            switch (bufferedGeometries[0].type) {
                case "polyline":
                    symbol = new SimpleLineSymbol(SimpleLineSymbol.STYLE_DASH, new Color("#1a74e4"), 1);
                    break;
                case "polygon":
                    symbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID, new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID, new Color("#444444"), 2), new Color([238, 238, 238, 0.2]));
                    break;
            }
            array.forEach(bufferedGeometries, function (geometry) {
                var graphic = new Graphic(geometry, symbol);
                map.graphics.add(graphic);
                app.geometry = geometry;
            });
            map.setExtent(app.geometry.getExtent());
            if ('<%= viewAll %>' == 'Y') {
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
            array.forEach(results[0]['features'], function (feat) {
                var admin_unit = { "Div_Cd": feat.attributes['DIVISION_CODE'], "Dist_Cd": feat.attributes['DISTRICT_CODE'], "Blk_Cd": feat.attributes['BLOCK_CODE'], "Gp_Cd": feat.attributes['GP_FINAL_CODE'], "Vlg_Cd": feat.attributes['CENSUS_CD_2011'], "Div_NM": feat.attributes['DIVISION_NAME'], "Dist_NM": feat.attributes['DISTRICT_NAME_EN'], "Block_NM": feat.attributes['BLOCK_NAME'], "Village_NM": feat.attributes['CENSUS_NM_2011'], "Gp_NM": feat.attributes['GP_FINAL'], "areaName": "NA" }; //, "FOREST_DIVCODE": feat.attributes['FOREST_DIVCODE'], "FOREST_RANGECODE ": feat.attributes['FOREST_RANGECODE']
                if (results[0]['features'].length > 0) {
                    this.map.selectedArea.push(admin_unit);
                }
            });
            returnData1 = this.map.selectedArea;
            var returnData = JSON.stringify(this.map.selectedArea);
            var lbl = "One village found with-in area of interest.";
            if (this.map.selectedArea.length > 1) lbl = "Total " + this.map.selectedArea.length + " Villages found with-in area of interest. ";
            dom.byId("kmldiv").innerHTML = lbl;
            var vill_Items = array.map(this.map.selectedArea, function (VillageData) { return VillageData.Village_NM; });
            document.getElementById("lblVillages").innerHTML = vill_Items;
            //document.getElementById("resultDiv").style.display = "block";           
            //Perform POI Query
            POIResults();
        }
        function handleWaterResults(results) {
            document.getElementById("loading").style.display = "block";
            //document.getElementById("loadingData").innerHTML = "Finding nearest waterbodies/forest/wildlife area...";
            this.map.waterArea = [];
            var distParam = new DistanceParameters();
            distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER; //.UNIT_STATUTE_MILE;
            if (mainGeo.type == "polyline")
                distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(mainGeo.paths[0]) + ']}}'; // inputPoints[inputPoints.length - 2];                 
            else
                distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(mainGeo.rings[0]) + ']}}'; // inputPoints[inputPoints.length - 2];                 
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
                document.getElementById("divMines").style.display = "block";
                document.getElementById("nearbywaterbody").innerHTML = lowest_dist_feature;
                document.getElementById("nearbywaterbodydistance").innerHTML = lowest_dist;
                if (lowest_dist == 0) {
                    dom.byId("waterdiv").innerHTML = "The waterbody " + lowest_dist_feature + " is found inside area of interest.";
                }
                else {
                    dom.byId("waterdiv").innerHTML = "The nearest water body is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest. ";
                }
            }
            else
                dom.byId("waterdiv").innerHTML = "There is no water body found with in 5 KM.";
            document.getElementById("loading").style.display = "none";
        }
        function handleForestResults(results) {
            document.getElementById("loading").style.display = "block";
            //document.getElementById("loadingData").innerHTML = "Finding nearest waterbodies/forest/wildlife area...";
            //this.map.waterArea = [];
            var geom2 = null;
            var distParam = new DistanceParameters();
            distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER; //.UNIT_STATUTE_MILE;
            if (mainGeo.type == "polyline")
                distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(mainGeo.paths[0]) + ']}}'; // inputPoints[inputPoints.length - 2];                 
            else
                distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(mainGeo.rings[0]) + ']}}'; // inputPoints[inputPoints.length - 2];                 
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
                    document.getElementById("iswithinforest").innerHTML = "No";
                    document.getElementById("inForestArea").innerHTML = "NA";
                    document.getElementById("outForestArea").innerHTML = "NA";
                    dom.byId("forestdiv").innerHTML = "The nearest forest area is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest.";
                }
                else {
                    var forestoverlayGeom = geometryEngine.difference(mainGeo, geom2);
                    var overlayArea = (Math.round(geometryEngine.geodesicArea(forestoverlayGeom, "square-kilometers") * 100) / 100);
                    document.getElementById("inForestArea").innerHTML = shpArea - overlayArea + "Sq.km";
                    document.getElementById("outForestArea").innerHTML = overlayArea + "Sq.km";
                    document.getElementById("iswithinforest").innerHTML = "Yes";
                    dom.byId("forestdiv").innerHTML = "Forest Block (" + lowest_dist_feature + ") intersecting area of interest.";
                }
                document.getElementById("nearbyforestdistance").innerHTML = lowest_dist;
            }
            else {
                document.getElementById("iswithinforest").innerHTML = "No";
                document.getElementById("inForestArea").innerHTML = "NA";
                document.getElementById("outForestArea").innerHTML = "NA";
                dom.byId("forestdiv").innerHTML = "There is no forest area found with in 5 KM.";
            }
            document.getElementById("loading").style.display = "none";
        }
        function handleWildAreaResults(results) {
            document.getElementById("loading").style.display = "block";
            var distParam = new DistanceParameters();
            distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
            if (mainGeo.type == "polyline") {
                distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(mainGeo.paths[0]) + ']}}';
            }
            else {
                distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(mainGeo.rings[0]) + ']}}';
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
                    document.getElementById("iswithinforest").innerHTML = "No";
                    dom.byId("wildareadiv").innerHTML = "The nearest Wildlife sanctuary/National park is " + lowest_dist_feature + " which is " + lowest_dist + " KM away from area of interest.";
                }
                else {
                    document.getElementById("iswithinforest").innerHTML = "Yes";
                    dom.byId("wildareadiv").innerHTML = "Wildlife sanctuary/National park (" + lowest_dist_feature + ") intersecting area of interest.";
                }
                document.getElementById("nearbywildlifedistance").innerHTML = lowest_dist;
                document.getElementById("nearbywildlife").innerHTML = lowest_dist_feature;
            }
            else {
                document.getElementById("nearbywildlifedistance").innerHTML = "NA";
                document.getElementById("nearbywildlife").innerHTML = "No";
                dom.byId("wildareadiv").innerHTML = "There is no Wildlife sanctuary/National park found with in 5 KM.";
            }
            document.getElementById("loading").style.display = "none";
        }
        function handleQueryResultsError(error) {
            alert("Rest Service is not responding : " + error);
        }
        function POIResults() {
            //document.getElementById("loading").style.display = "block";
            var distParam = new DistanceParameters();
            distParam.distanceUnit = esri.tasks.GeometryService.UNIT_KILOMETER;
            if (mainGeo.type == "polyline") {
                distParam.geometry1 = '{"geometryType":"esriGeometryPolyline","geometry":{"paths": [' + JSON.stringify(mainGeo.paths[0]) + ']}}';
            }
            else {
                distParam.geometry1 = '{"geometryType":"esriGeometryPolygon","geometry":{"rings": [' + JSON.stringify(mainGeo.rings[0]) + ']}}';
            }
            distParam.geodesic = true;
            var lowest_dist = 100000; var lowest_dist_feature = null;
            array.forEach(returnData1, function (name) {
                var poi_url = serviceUrl.poiUrl;
                $.ajax({
                    dataType: "json",
                    url: poi_url,
                    data: { where: "CENSUS_CD_='" + name.Vlg_Cd + "'", geometryType: "esriGeometryEnvelope", spatialRel: "esriSpatialRelIntersects", outFields: "POI_NAME,FACILITY_D,CENSUS_CD_", returnGeometry: true, returnIdsOnly: false, returnCountOnly: false, returnZ: false, returnM: false, returnDistinctValues: false, returnTrueCurves: false, f: "pjson" },
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
            document.getElementById("resultDiv").style.display = "block";
            document.getElementById("loading").style.display = "none";
            returnData1 = [];
        }
    });
          </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" style="width: 98%; padding: 2px;">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h4 class="text-center" style="color: #ff7d07; font-weight: 700"><span class="glyphicon glyphicon-globe" aria-hidden="true"></span>&nbsp;View on GIS Application</h4>
                </div>
                <div class="panel-body" style="min-height: 700px; padding: 2px;">
                    <div id="loading">
                        <div id="loading-image">
                            <label id="loadingData" style="padding-left: 33px; padding-top: 7px"></label>
                        </div>
                    </div>
                    <div class="col-md-2 text-center">
                        <ul>
                            <li class="list-group-item list-group-item-warning text-uppercase">SSO ID :
                    <asp:Label ID="lbl1" runat="server"></asp:Label></li>
                        </ul>
                    </div>
                    <div class="col-md-3 text-center">
                        <ul>
                            <li class="list-group-item list-group-item-danger text-uppercase">OFC ID :
                    <asp:Label ID="lbl2" runat="server"></asp:Label></li>
                        </ul>
                    </div>
                    <div class="col-md-2 text-center">
                        <ul>
                            <li class="list-group-item list-group-item-info">Request For :
                    <asp:Label ID="lbl4" runat="server" Font-Bold="true"></asp:Label></li>
                        </ul>
                    </div>
                    <div class="col-md-5 text-left">
                        <ul>
                            <li class="list-group-item list-group-item-success">File Name :
                            <label id="lbl3" runat="server"></label>
                            </li>
                        </ul>
                    </div>
                    <div class="col-md-9" style="padding: 2px;">
                        <div class="list-group-item list-group-item-warning">
                            <span class="info text-primary" id="layer_list"></span>
                        </div>
                        <div class="panel panel-info">
                            <div class="panel-body" style="padding: 0px">
                                <div class="col-md-1">
                                    <div id="legendDiv" class="text-left text-danger">
                                    </div>
                                </div>
                                <div class="col-md-11">
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
                                    <a id="logo_raj" class="logo_raj img-responsive center-block" href="http://www.doitc.rajasthan.gov.in">
                                        <img src="../css/rajdharaa.png" alt="rajdharaa" /></a>
                                      <span class="text-right" style="margin-right:-25px;">&ensp;&copy;2016 DoIT&C</span>
                                </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" style="padding: 2px;">
                        <div id="resultDiv" class="panel panel-success" style="display: none; z-index: 9999">
                            <div class="list-group-item list-group-item-warning">
                                <label class="info text-primary"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>&nbsp;Result</label>
                            </div>
                            <div class="panel panel-warning">
                                <div class="panel-body" style="padding: 2px">
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
                                    <div class="text-capitalize" id="returnDiv" style="overflow-wrap: break-word; text-wrap: normal;">
                                        <div class="list-group-item list-group-item-warning">
                                            <label class="info text-primary"><span class="glyphicon glyphicon-info-sign" aria-hidden="true"></span>&nbsp;Kml/Shape Data</label>
                                        </div>
                                        <div style="padding: 1px">&ensp;</div>
                                        <div id="divMines" style="display: none; font-size: 12px;">
                                            <div class="col-md-6">
                                                Near By Water Body :
                                            </div>
                                            <div class="col-md-6">
                                                <label id="nearbywaterbody"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Near By Water Body Distance :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="nearbywaterbodydistance"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Near By Wildlife :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="nearbywildlife"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Near By Wildlife Distance : 
                                            </div>
                                            <div class="col-md-4">
                                                <label id="nearbywildlifedistance"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Near By Forest Distance :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="nearbyforestdistance"></label>
                                            </div>
                                            <div class="col-md-8">
                                                With in Forest :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="iswithinforest"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Within Forest Area :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="inForestArea"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Outside Forest Area :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="outForestArea"></label>
                                            </div>
                                            <div class="col-md-8">
                                                Area :
                                            </div>
                                            <div class="col-md-4">
                                                <label id="shapeArea"></label>
                                            </div>
                                            <div class="col-md-8">
                                                <span id="lblLength"></span>
                                            </div>
                                            <div class="col-md-4">
                                                <label id="shapeLength"></label>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            Vilages :
                                        </div>
                                        <div class="col-md-9">
                                            <span id="lblVillages" class="text-justify text-left text-primary"></span>
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
