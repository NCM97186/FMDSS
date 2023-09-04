









function ShowKPIs(input) {

  
        $.ajax({
            type: 'POST',
            url: RootUrl + 'KPIs/getKPIdata', // we are calling json method
            dataType: 'json',
            data: { ActionType: input.value },
            success: function (states) {
               
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
  






    document.getElementById("ssoid").value = '@Session["SSOID"]';
    document.getElementById("OffenseCategoryID").value = document.getElementById("ddl_OffenseCategory").value;
    document.getElementById("DistrictID").value = document.getElementById("district").value;
    document.getElementById("BlocknameID").value = 1199;//document.getElementById("blockname").value;
    document.getElementById("GPNameID").value = 11999; //document.getElementById("ddlGPName").value;

    document.getElementById("villageId").value = document.getElementById("ddlVillName").value;
    document.getElementById("FPMparivadform").action = 'http://10.68.128.179/FMDSSGIS/markoffense/offense.aspx';
    document.getElementById("FPMparivadform").submit();
    return false;
}



