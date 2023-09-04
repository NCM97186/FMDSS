
function GetMemberId(id, adhar, cast, Category, address, DistrictName) {

    $('#myModalDashboard').modal('hide');

    $.ajax({
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        data: { AckId: id, Adhar: adhar },
        url: RootUrl + "FPMParivad/GetMemberDetails",
        success: function (data) {
            $('#txt_OffenderName').val(data[0].NameEng);
            $('#txt_OFatherName').val(data[0].FnameEng);
            $('#txt_OSpouseName').val(data[0].SnameEng);
            $('#txt_OEmailID').val(data[0].Email);
            $('#txt_OPhoneNo').val(data[0].Mobile);
            // $('#txt_OCaste').val(Category);
            //$("#txt_OCaste").find("option[text=" + Category + "]").attr("selected", true);
            $("#txt_OCaste option:contains(" + Category + ")").attr('selected', 'selected');

            $('#txt_OffenderAge').val(data[0].Dob);



            $('#ddlOState').val(1);
            $('#txt_HCaste').val(cast);

            //$('#ddlODistrict').val(DistrictName);
            //$("#ddlODistrict").find("option[text=" + DistrictName + "]").attr("selected", true);
            $("#ddlODistrict option:contains(" + DistrictName + ")").attr('selected', 'selected');
            $('#txt_OAddress1').val(address);

            console.log(data);

        },
        error: function (ex) { alert(ex); }

    });
    //$.unblockUI();
}


$(document).ready(function () {

    $('#txtBhamasha').text('');

    $('#btnGetBhamasha').click(function () {

        if ($('#txtBhamasha').val() == '') {
            alert('Enter Bhamasha Id.');
            $('#txtBhamasha').focus();
            return;
        }

        var BhamashaId = $('#txtBhamasha').val();

        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: { BhamashaId: BhamashaId },
            url: RootUrl + "FPMParivad/GetBhamashaData",
            success: function (data) {

                if (typeof (data.errorcode) === 'undefined') {
                    $('#myModalDashboard').modal('show');
                    $('#tblMemberDetails').html(data);
                    console.log(data);
                }
                else {
                    alert(data.errorDescription);
                }
            },
            error: function (ex) { alert(ex); }

        });
        // $.unblockUI();

    });

    $('#ddlBhamsha').change(function () {
        //$("#divBhamasha").show();
        if ($('#ddlBhamsha').val() == 1) {
            $("#divBhamasha").show();
            EnableDisable(true);
        }
        else {
            $("#divBhamasha").hide();
            EnableDisable(false);
        }
    });

    $('#txtBhamasha').keyup(function () {
        $("input").val(function (i, val) {
            return val.toUpperCase();
        });
    });


});

function EnableDisable(flag) {
    $('#txt_OffenderName').prop("disabled", flag);
    $('#txt_OFatherName').prop("disabled", flag);
    $('#txt_OSpouseName').prop("disabled", flag);
    $('#txt_OCaste').prop("disabled", flag);
    $('#txt_HCaste').prop("disabled", flag);
    $('#txt_OAddress1').prop("disabled", flag);
    $('#ddlOState').prop("disabled", flag);
    $('#ddlODistrict').prop("disabled", flag);
    $('#txt_OffenderAge').prop("disabled", flag);
}



