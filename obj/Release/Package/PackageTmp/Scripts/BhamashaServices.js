
function GetMemberId(id, adhar, cast, Category, address, DistrictName, Village) {
    $('#myModalDashboard').modal('hide');

    $.ajax({
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        data: { AckId: id, Adhar: adhar },
        url: "/FPMParivad/GetMemberDetails",
        success: function (data) {
            $('#txt_OffenderName').val(data[0].NameEng);
            $('#txt_OFatherName').val(data[0].FnameEng);
            $('#txt_OSpouseName').val(data[0].SnameEng);
            $('#txt_OEmailID').val(data[0].Email);
            $('#txt_OPhoneNo').val(data[0].Mobile);
            // $('#txt_OCaste').val(Category);
            //$("#txt_OCaste").find("option[text=" + Category + "]").attr("selected", true);
            $("#txt_OCategory option:contains(" + Category + ")").attr('selected', 'selected');
            $('#txt_OffenderAge').val(data[0].Dob);
            $('#ddlOState').val(1);
            $('#txt_OCasteName').val(cast);
            //$('#ddlODistrict').val(DistrictName);
            //$("#ddlODistrict").find("option[text=" + DistrictName + "]").attr("selected", true);
            $("#ddlODistrict option:contains(" + DistrictName + ")").attr('selected', 'selected');
            $('#txt_OAddress1').val(address);



            // Added by Arvind Kumar Sharma 20/07/2016
            // Start
            $("#txt_OVillageCode").empty();
            $("#txt_OVillageCode").append('<option value="' + '' + '">' +
                    '--Select--' + '</option>');
            var xxText;
            var xxValue;
            $.ajax({
                type: 'POST',
                url: '/OffenseRegistration/getVillage', // we are calling json method
                dataType: 'json',
                data: { dist_code: $("#ddlODistrict option:selected").val() },
                success: function (states) {
                    $.each(states, function (i, vill) {
                        $("#txt_OVillageCode").append('<option value="' + vill.Value + '">' + vill.Text.toUpperCase() + '</option>');

                        xxText = vill.Text.toUpperCase();
                        if (xxText == Village) {

                            xxValue = vill.Value;

                            $('#txt_OVillageCode option').filter(function () { return $(this).val() == xxValue }).attr('selected', true);
                            // return;
                        }

                    });
                },
                error: function (ex) {
                    alert('Failed to retrieve states.' + ex);
                }
            });
            // End
            // Added by Arvind Kumar Sharma 20/07/2016





            $('#WitnessName').val(data[0].NameEng);
            $('#FatherName').val(data[0].FnameEng);
            $('#SpouseName').val(data[0].SnameEng);
            $('#EmailId').val(data[0].Email);
            $('#PhoneNo').val(data[0].Mobile);
            $("#Category option:contains(" + Category + ")").attr('selected', 'selected');
            $('#WitnessAge').val(data[0].Dob);
            $('#State').val('Rajasthan');
            $('#Caste').val(cast);
            $("#District option:contains(" + DistrictName + ")").attr('selected', 'selected');
            $('#ResidentialAddress1').val(address);

            $('.WitnessName').val(data[0].NameEng);
            $('.ResidentialAddress1').val(address);



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

    $('#btnGetBhamashawitness').click(function () {

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
            $('#txtBhamasha').val('');
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
    $('#txt_OCategory').prop("disabled", flag);
    $('#txt_OCasteName').prop("disabled", flag);
    $('#txt_OAddress1').prop("disabled", flag);
    $('#ddlOState').prop("disabled", flag);
    $('#ddlODistrict').prop("disabled", flag);
    $('#txt_OffenderAge').prop("disabled", flag);
    $('#txt_OVillageCode').prop("disabled", flag); // Added by Arvind Kumar Sharma 21/07/2016
    $('#WitnessName').prop("disabled", flag);
    $('#FatherName').prop("disabled", flag);
    $('#SpouseName').prop("disabled", flag);
    $('#Category').prop("disabled", flag);
    $('#Caste').prop("disabled", flag);
    $('#ResidentialAddress1').prop("disabled", flag);
    $('#State').prop("disabled", flag);
    $('#District').prop("disabled", flag);
    $('#WitnessAge').prop("disabled", flag);
}



