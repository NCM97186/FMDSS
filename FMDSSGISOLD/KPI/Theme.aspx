<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Theme.aspx.cs" Inherits="ForestryWebGIS.KPI.Theme" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
    <!-- #include file ="../dojopackage.html" -->​
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1,user-scalable=no" />
    <title>Thematic Page</title>
    <link rel="stylesheet" href="//js.arcgis.com/3.17/esri/themes/calcite/dijit/calcite.css" />
    <link rel="stylesheet" href="//js.arcgis.com/3.17/esri/themes/calcite/esri/esri.css" />
    <link href="../css/jquery-ui.css" rel="stylesheet" />
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        #mapDiv {
            height: 100%;
            min-height: 680px;
            margin: 0;
            padding: 0;
            width: 100%;
        }
         .panel-body {
            padding: 4px;
        }
           .logo_raj {
            position: absolute;
            width: 80px;
            bottom: 3px;
            right:30px;
        }
    </style>    
    <link rel="stylesheet" type="text/css" href="../css/SimpleColorPicker.css" />
    <script src="../scripts/jquery-2.1.1.min.js"></script>
    <script src="../scripts/SimpleColorPicker.js" type="text/javascript"></script>    
    <script src="../scripts/jquery-ui.js"></script>    
    <script src="//js.arcgis.com/3.17/"></script>
    <script>
        var symbol, disableSymbol;
        var currentAdminUnit, currentThemeUnit, currentTheme, labels, minValue,
            maxValue, currentRange, resultData, allConfig, thematicLayer;
        var theme = '<% = themeName %>';
        var themeUnit = '<% = adminType %>';
        var resultData = JSON.parse('<% = themeData %>');
        var caption = '<% = caption %>';
        require([
         "esri/map",
        "esri/layers/FeatureLayer",
        "esri/tasks/query",
        "esri/tasks/QueryTask",
        "esri/lang",
        "dojo/text!../scripts/themeConfig.json",
        "esri/symbols/SimpleFillSymbol",
        "esri/renderers/ClassBreaksRenderer",
        "esri/renderers/SimpleRenderer",
        "esri/layers/LabelClass",
        "esri/layers/LabelLayer",
        "esri/symbols/TextSymbol",
        "esri/graphicsUtils",
              "DojoClass/TokenAuth",
        "esri/Color",
        "esri/symbols/Font",
        "dojo/dom",
        "dojo/on",
        "dojo/parser",
        "dijit/TooltipDialog",
        "dijit/popup",
        "dojo/number",
        "dojo/dom-style",
        "dojo/domReady!"
        ], function (
          Map, FeatureLayer, Query, QueryTask,
      esriLang, config, SimpleFillSymbol, ClassBreaksRenderer, SimpleRenderer, LabelClass, LabelLayer,
     TextSymbol, graphicsUtils, TokenAuth,Color, Font, dom, on,
     parser, TooltipDialog, dijitPopup, number, domStyle) {
            parser.parse();
            allConfig = JSON.parse(config);
            map = new Map("mapDiv", {
                sliderOrientation: "horizontal",
                sliderPosition: "top-center",
                showLabels: true,
                logo:false
            });
            $("#btnApply").click(function () {
                var isValid = true;
                if ($("#range1from").val() == "" || $("#range1to").val() == "") {
                    isValid = false;
                }
                if (currentRange >= 2 && ($("#range2from").val() == "" || $("#range2to").val() == "")) {
                    isValid = false;
                }
                if (currentRange >= 3 && ($("#range3from").val() == "" || $("#range3to").val() == "")) {
                    isValid = false;
                }
                if (currentRange >= 4 && ($("#range4from").val() == "" || $("#range4to").val() == "")) {
                    isValid = false;
                }
                if (currentRange >= 5 && ($("#range5from").val() == "" || $("#range5to").val() == "")) {
                    isValid = false;
                }
                if (!isValid) {
                    alert("Range value can't be empty");
                    return;
                }
                if (parseInt($("#range1to").val()) < parseInt($("#range1from").val())) {
                    isValid = false;
                }
                if (currentRange >= 2 && (parseInt($("#range2to").val()) < parseInt($("#range2from").val()))) {
                    isValid = false;
                }
                if (currentRange >= 3 && (parseInt($("#range3to").val()) < parseInt($("#range3from").val()))) {
                    isValid = false;
                }
                if (currentRange >= 4 && ((parseInt($("#range4to").val()) < parseInt($("#range4from").val())))) {
                    isValid = false;
                }
                if (currentRange >= 5 && ((parseInt($("#range5to").val()) < parseInt($("#range5from").val())))) {
                    isValid = false;
                }
                if (!isValid) {
                    alert("All start range must be less than end range.");
                    return;
                }
                applyClassBreakRenderer();
            });

            intializeValues();
            function intializeValues() {
                //Getting Current, Lower (on which theme should be applied) Admin Layer data from config
                symbol = new SimpleFillSymbol();
                symbol.setColor(new Color(Color.fromHex(allConfig.defaultColor)));
                disableSymbol = new SimpleFillSymbol();
                disableSymbol.setColor(new Color(Color.fromHex(allConfig.notApplicableColor)));
                disableSymbol.opacity = 0.7;
                for (var i = 0; i < allConfig.adminUnits.length; i++) {
                    if (allConfig.adminUnits[i].unit == themeUnit) {
                        currentThemeUnit = allConfig.adminUnits[i];
                        break;
                    }
                }

                for (var i = 0; i < allConfig.Thematic_Maps.length; i++) {
                    if (allConfig.Thematic_Maps[i].theme == theme) {
                        currentTheme = allConfig.Thematic_Maps[i];
                        break;
                    }
                }
                $("#area").text(currentThemeUnit.displayName);
                $("#captionID").text(caption);
                $("#header").text('Thematic Renderer');

                for (var i = 1; i <= 5; i++)
                    $("#color" + i.toString()).simpleColorPicker();
                var distinctValue = [];
                for (var i = 0; i < resultData.length; i++) {
                    var obj = resultData[i];
                    if (!isNaN(obj[currentTheme.themeField]) && obj[currentTheme.themeField] != null) {
                        if (typeof (minValue) == 'undefined') {
                            minValue = 0;
                            maxValue = parseInt(obj[currentTheme.themeField]);
                        }
                        if (parseInt(obj[currentTheme.themeField]) < minValue)
                            minValue = parseInt(obj[currentTheme.themeField]);
                        if (parseInt(obj[currentTheme.themeField]) > maxValue)
                            maxValue = parseInt(obj[currentTheme.themeField]);
                        if (distinctValue.indexOf(parseInt(obj[currentTheme.themeField])) == -1)
                            distinctValue.push(parseInt(obj[currentTheme.themeField]));
                    }
                }
                currentRange = distinctValue.length >= 3 ? parseInt(currentTheme.defaultrange) : distinctValue.length;
                try {
                    $("#slider-range").slider({
                        min: currentRange,
                        max: 5,
                        range: "min",
                        value: currentRange,
                        slide: function (event, ui) {
                            currentRange = ui.value;
                            setDiv();
                            setRangeColorValues();
                            $("#range").val(currentRange);
                        }
                    });
                    $("#range").val(currentRange);
                } catch (e) {
                    alert("Error - _populateSlider : " + e.message, "Error");
                }
                var featureCollection = {
                    layerDefinition: getLayerDefinition(),
                    featureSet: null
                };

                thematicLayer = new FeatureLayer(featureCollection, {
                    mode: esri.layers.FeatureLayer.MODE_SNAPSHOT,
                    id: 'thematicLayer'
                });

                $("#range1to").change(function () {
                    if (currentRange >= 2 && $("#range1to").val() != "")
                        $("#range2from").val(parseInt($("#range1to").val()) + 1);
                });
                $("#range2to").change(function () {
                    if (currentRange >= 3 && $("#range2to").val() != "")
                        $("#range3from").val(parseInt($("#range2to").val()) + 1);
                });
                $("#range3to").change(function () {
                    if (currentRange >= 4 && $("#range3to").val() != "")
                        $("#range4from").val(parseInt($("#range3to").val()) + 1);
                });

                $("#range4to").change(function () {
                    if (currentRange >= 5 && $("#range4to").val() != "")
                        $("#range5from").val(parseInt($("#range4to").val()) + 1);
                });
                getBoundary();
            }

            function getBoundary() {
                var queryBoundary = new Query();
                var queryTask = new QueryTask(currentThemeUnit.url);
                queryBoundary.where = "1=1";
                queryBoundary.returnGeometry = true;
                var outfield = [];
                outfield.push(currentThemeUnit.linkField_GIS);
                outfield.push(currentThemeUnit.nameField);
                if (currentThemeUnit.geometry != "esriGeometryPoint") {
                    outfield.push(currentThemeUnit.areaField);
                }
                queryBoundary.outFields = outfield;
                queryTask.execute(queryBoundary, queryCompleted, function (error) {
                    console.log(error);
                });
            }

            function queryCompleted(featureSet) {
                var features = [];
                for (var i = 0; i < featureSet.features.length; i++) {
                    var dataAvailable = false;
                    var grp = featureSet.features[i];
                    for (var k = 0; k < resultData.length; k++) {
                        var obj = resultData[k];
                        if (obj[currentThemeUnit.linkField_MIS] == grp.attributes[currentThemeUnit.linkField_GIS]) {
                            if (obj[currentTheme.themeField] != null && obj[currentTheme.themeField] != undefined) {
                                dataAvailable = true;
                                grp.attributes["disabled"] = false;
                                grp.attributes[currentTheme.themeField] = obj[currentTheme.themeField];
                            }
                        }
                    }
                    if (!dataAvailable) {
                        grp.attributes["disabled"] = true;
                        grp.attributes[currentTheme.themeField] = '';
                    }
                    if (i == 0)
                        thematicLayer.spatialReference = grp.geometry.spatialReference;
                    thematicLayer.add(grp);
                }

                map.setExtent(graphicsUtils.graphicsExtent(thematicLayer.graphics));
                setForThematic();
                setDiv();
                setRangeColorValues();
                applyClassBreakRenderer();
            }


            function getLayerDefinition() {
                var layerDefinition = {
                    "geometryType": currentThemeUnit.geometry,
                    "objectIdField": "ObjectID",
                    "fields": [{
                        "name": "ObjectID",
                        "alias": "ObjectID",
                        "type": "esriFieldTypeOID"
                    },
                    {
                        "name": "disabled",
                        "alias": "disabled",
                        "type": "esriFieldTypeString"
                    },
                    {
                        "name": currentThemeUnit.linkField_GIS,
                        "alias": currentThemeUnit.linkField_GIS,
                        "type": "esriFieldTypeInteger"
                    },
                     {
                         "name": currentThemeUnit.nameField,
                         "alias": currentThemeUnit.nameFieldAlias,
                         "type": "esriFieldTypeString"
                     },
                     {
                         "name": currentTheme.themeField,
                         "alias": currentTheme.themeFieldAlias,
                         "type": currentTheme.themeFieldType
                     }
                    ]
                };
                return layerDefinition;
            }

            convertArea = function (value, key, data) {
                return Math.round(Math.round(value) / 1000000 * 100) / 100;
            };

            function applyClassBreakRenderer() {
                var renderer = new ClassBreaksRenderer(symbol, currentTheme.themeField);
                for (var i = 0; i < currentRange; i++) {
                    var minval = parseInt($("#range" + (i + 1).toString() + "from").val());
                    var maxval = parseInt($("#range" + (i + 1).toString() + "to").val());;
                    var colorcode = Color.fromHex($("#color" + (i + 1).toString()).val());
                    renderer.addBreak(minval, maxval, new SimpleFillSymbol().setColor(new Color(colorcode)));
                }
                thematicLayer.setRenderer(renderer);
                makeDisableGraphics(disableSymbol);
            }

            function makeDisableGraphics(disableSymbol) {
                for (var i = 0; i < thematicLayer.graphics.length; i++) {
                    if (thematicLayer.graphics[i].attributes["disabled"] == true)
                        thematicLayer.graphics[i].setSymbol(disableSymbol);
                }
                thematicLayer.redraw();
            }

            function setRangeColorValues() {
                var interval = maxValue - minValue;
                var evenInterval = Math.round(interval / currentRange);
                for (var i = 1; i <= currentRange; i++) {
                    $("#range" + i.toString() + "from").val(i == 1 ? minValue : parseInt($("#range" + (i - 1).toString() + "to").val()) + 1);
                    $("#range" + i.toString() + "to").val(i == currentRange ? maxValue : (evenInterval * i));
                    if (!$('#color' + i.toString()).val()) {
                        $('#color' + i.toString()).val(allConfig.rangeColors[i - 1]);
                        $('#color' + i.toString()).css({ 'background-color': allConfig.rangeColors[i - 1] });
                    }
                }
            }

            function setDiv() {
                for (var i = 1; i <= 5; i++) {
                    $("#divRange" + i.toString() + " > input").val('').css({ 'background-color': '' });
                    $("#divRange" + i.toString()).children().prop('disabled', !(i <= currentRange));
                }
            }


            function setForThematic() {
                var widgetexist = dijit.byId('tooltipDialog');
                if (widgetexist) {
                    widgetexist.destroyRecursive(true);
                }

                thimaticdialog = new TooltipDialog({
                    id: "tooltipDialog",
                    style: "position: absolute; width: 250px; font: normal normal normal 10pt Helvetica;z-index:100"
                });

                thematicLayer.on("mouse-over", function (evt) {
                    var t = '<div><span style="float:right"><a href="#" onclick="dijit.popup.close(thimaticdialog);return false;">x</a></span>';
                    t = t + "<b>" + currentThemeUnit.nameFieldAlias + "</b>: <b>${" + currentThemeUnit.nameField + "}</b><br/><br/>";
                    t = t + "<b>" + currentTheme.themeFieldAlias + "</b>: <b>${" + currentTheme.themeField + "}</b><br/>";
                    t = t + '</div>';
                    var content = esriLang.substitute(evt.graphic.attributes, t);
                    thimaticdialog.setContent(content);
                    domStyle.set(thimaticdialog.domNode, "opacity", 0.85);
                    dijitPopup.open({
                        popup: thimaticdialog,
                        x: evt.pageX,
                        y: evt.pageY
                    });
                });

                var labelSymbol = new TextSymbol().setFont(new Font("12pt").setWeight(Font.WEIGHT_BOLD));
                var labelRenderer = new SimpleRenderer(labelSymbol);
                labels = new LabelLayer({ id: "labels" });
                labels.addFeatureLayer(thematicLayer, labelRenderer, "{" + currentThemeUnit.nameField + "}");
                map.addLayer(thematicLayer);
                map.addLayer(labels);
                document.getElementById("noMap").style.display = "none";
            }
        });
      
    </script>
</head>
<body class="calcite">
    <form id="myform" runat="server">
        <div class="container" style="width: 98.70%">
            <div class="panel panel-primary">
                <div class="panel-body">
                    <div class="col-md-3">
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
                                </ul>
                            </div>
                        </div>
                        <div id="drawDiv" class="panel panel-warning">
                            <div class="panel-heading">
                                <h5 class="text-left text-danger" style="font-weight: 700"><span class="glyphicon glyphicon-cloud-upload" aria-hidden="true"></span>&nbsp;<span id="header"></span></h5>
                            </div>
                            <div id="dataID" style="padding-left: 5px">
                                <div id="firstChild" class="row" style="padding-left: 5px; padding-top: 5px">
                                    <div class="col-xs-3" style="margin-bottom: 5px; margin-left: 0px; margin-right: 0px;">
                                        Area :
                                    </div>
                                    <div class="col-xs-9" style="margin-bottom: 5px;">
                                        <span id="area" style="overflow-wrap: break-word; text-wrap: normal"></span>
                                    </div>
                                </div>
                                <%-- <div class="row" style="padding-left: 5px">
                                        <div class="col-md-12" style="margin-bottom: 5px; margin-left: 0px; margin-right: 0px; vertical-align: text-top">
                                            Color Ranges<br />
                                        </div>
                                    </div>--%>

                                <%--  <div class="row" style="margin-bottom: 5px; height: 30px; text-align: center">
                                        <div class="col-md-8  col-lg-offset-1 center-block" style="margin-bottom: 5px; margin-left: 0px; margin-right: 0px; vertical-align: text-top; text-align: center">
                                          <div>
                                              <input type="text" id="range" readonly="readonly" style="border: 0; color: #f6931f; font-weight: bold" />
                                            <div id="slider-range" style="background: #F5DA81;"></div>
                                              </div>
                                        </div>
                                    </div>--%>

                                <div class="row" style="padding-left: 5px; text-align: center; height: 20px">
                                    <div class="col-md-12" style="margin-bottom: 5px; margin-left: 0px; margin-right: 0px; vertical-align: text-top">
                                        <input type="text" id="range" readonly="readonly" style="border: 0; color: #f6931f; font-weight: bold;" />
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 5px; padding-left: 5px; text-align: center">
                                    <div class="col-md-10 col-md-offset-1">
                                        <div id="slider-range" style="background: #F5DA81; text-align: center"></div>
                                    </div>
                                </div>

                                <div class="row" style="margin-bottom: 5px; padding-left: 5px;">
                                    <div id="divRange1" class="col-md-12">
                                        <input type="number" id="range1from" style="width: 26%;" />&nbsp; to &nbsp;<input type="number" id="range1to" style="width: 26%;" />&nbsp;<input id="color1" style="width: 30%;" />
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 5px; padding-left: 5px;">
                                    <div id="divRange2" class="col-md-12">
                                        <input type="number" id="range2from" style="width: 26%;" />&nbsp; to &nbsp;<input type="number" id="range2to" style="width: 26%;" />&nbsp;<input id="color2" style="width: 30%;" />
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 5px; padding-left: 5px;">
                                    <div id="divRange3" class="col-md-12">
                                        <input type="number" id="range3from" style="width: 26%;" />&nbsp; to &nbsp;<input type="number" id="range3to" style="width: 26%;" />&nbsp;<input id="color3" style="width: 30%;" />
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 5px; padding-left: 5px;">
                                    <div id="divRange4" class="col-md-12">
                                        <input type="number" id="range4from" style="width: 26%;" />&nbsp; to &nbsp;<input type="number" id="range4to" style="width: 26%;" />&nbsp;<input id="color4" style="width: 30%;" />
                                    </div>
                                </div>
                                <div class="row" style="margin-bottom: 5px; padding-left: 5px;">
                                    <div id="divRange5" class="col-md-12">
                                        <input type="number" id="range5from" style="width: 26%;" />&nbsp; to &nbsp;<input type="number" id="range5to" style="width: 26%;" />&nbsp;<input id="color5" style="width: 30%;" />
                                    </div>
                                </div>
                            </div>
                            <div style="padding: 5px"></div>
                            <div class="row" style="text-align: center;">
                                <button type="button" id="btnApply" class="btn btn-success" value="Apply" style="width: 8em;">Apply</button>
                            </div>
                            <div style="padding: 5px"></div>
                        </div>
                    </div>
                    <div class="col-md-9">
                        <div class="panel panel-primary">
                            <div class="panel-heading" style="padding: 2px">
                                <h4 class="text-center" style="color: #ffffff; font-weight: 700"><i class="glyphicon glyphicon-globe" aria-hidden="true"></i>&nbsp;<span id="captionID"></span></h4>
                            </div>
                            <div class="panel-body" style="min-height: 680px;">
                                <div class="col-lg-12" style="padding: 2px">
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
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
