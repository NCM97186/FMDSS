define([
    "dojo/_base/declare",
    "dojo/_base/array",
    "dojo/_base/lang",
    "esri/ServerInfo",
    "dojo/cookie",
    "esri/urlUtils",
    'esri/config',
    "esri/tasks/GeometryService"
], function (declare, arrayUtils, lang, ServerInfo, cookie, urlUtils, esriConfig, GeometryService
) {
    var SingletonClass = declare(null, {
        credential: null,
        constructor: function () {
            urlUtils.addProxyRule({
                urlPrefix: "https://gis.rajasthan.gov.in",
                proxyUrl: "https://gis.rajasthan.gov.in/proxy/proxy.ashx"
            });
            esriConfig.defaults.io.timeout = 180000;
            esriConfig.defaults.corsDetection = false;
            esriConfig.defaults.alwaysUseProxy = true;
            //esriConfig.defaults.io.corsEnabledServers.push("gis.rajasthan.gov.in");
        }
    });
    if (!_instance) {
        var _instance = new SingletonClass();
    }
    return _instance;
});