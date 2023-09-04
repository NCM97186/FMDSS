define([
    "dojo/_base/declare",
    "dojo/_base/array",
    "dojo/_base/lang",
    "esri/ServerInfo",
    "dojo/cookie",
    "esri/urlUtils",
    'esri/config',

], function (declare, arrayUtils, lang, ServerInfo, cookie, urlUtils, esriConfig
) {
    var SingletonClass = declare(null, {
        credential: null,
        constructor: function () {
            urlUtils.addProxyRule({
                urlPrefix: "https://gistest1.rajasthan.gov.in/",
                proxyUrl: "https://gistest1.rajasthan.gov.in/proxy/proxy.ashx"
            });
            esriConfig.defaults.io.timeout = 180000;
           // esriConfig.defaults.io.corsEnabledServers.push("gistest1.rajasthan.gov.in");
            //esriConfig.defaults.io.corsEnabledServers.push("gistest1.rajasthan.gov.in");
        }
    });
    if (!_instance) {
        var _instance = new SingletonClass();
    }
    return _instance;
});