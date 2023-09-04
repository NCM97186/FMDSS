var valuex = ""; //
var valuey = ""; //
var status = false;
function GetGISLatitudeLongitude(ADMINNAME, AdminCode) {
    //ADMINNAME = "district";
    //AdminCode = "110";

    var xmlHttp = new XMLHttpRequest();
    var url = "https://gistest1.rajasthan.gov.in/RajasthanGisService/RajGisService.svc/GetAdminCentroidData";
    var option =
            {
                "options":
                      {
                          "AdminType": ADMINNAME, // , "AdminCodes": []
                          "AdminCode": AdminCode	     //,    //, "AdminCodes": ["0811702"]

                      }
            }

    var body = JSON.stringify(option);
    xmlHttp.open("POST", url, true);
    xmlHttp.setRequestHeader("Content-Type", "application/json");
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState == 4) {
            
            alert(xmlHttp.readyState);
            status = true;
            var result = JSON.parse(JSON.parse(xmlHttp.responseText));
            var Data = result.AdminCentroidData.toString()


            var obj = $.parseJSON(Data);
           
            $.each(obj, function () {
                valuex = this['POINT_X'];
                valuey = this['POINT_Y'];
            });


        }
    };

    xmlHttp.send(body);
    if (status == true) {
        return { Latitude_Value: valuex, Longitude_Value: valuey }
    }
}