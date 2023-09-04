$(document).ready(function () {

    $(".datefield").datepicker({ maxDate: new Date(), dateFormat: 'dd/mm/yy', changeYear: true, onClose: function (dateText, inst) { $("[id $=auto]").focus(); } });


    $('#Addknown-Offender').on('click', function () {
        $(".Unknown").hide();
        $(".known").show();
    });
    $('#Unknown-Offender').on('click', function () {
        $(".known").hide();
        $(".Unknown").show();
    });



    $('#ddlOState').change(function () {
        var oState = $("#ddlOState option:selected").val();

        if (oState == "1") {
            $("#txtdistrict,#txtvillage").val('');
            $('#divtxtDistrict,#divtxtVillage').hide();
            $('#divDDlDistrict,#divDDlVillage').show();
        }
        else if (oState == "2") {
            $("#ddlODistrict").val($("#ddlODistrict option:first").val());
            $('#divtxtDistrict,#divtxtVillage').show();
            $('#divDDlDistrict,#divDDlVillage').hide();
        }
        else {
            $('#divtxtDistrict').hide();
            $('#divDDlDistrict').show();
            $("#ddlODistrict").val($("#ddlODistrict option:first").val());
            $("#txtdistrict,#txtvillage").val('');
        }
    });






    var rowdata = "";
    var ID = { id: $("#hdFPMOffenseCode").val() }
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(ID),
        url: RootURl + 'CitizenParivadRegistration/GetMultiOffenderDetails',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].OffenderName != '') {
                    var id = "'" + data[i].OOffenderrowid + "'";
                    rowdata = "<tr class='rowid'><td style=display:none;>" + data[i].OOffenderrowid + "</td><td>" + data[i].OffenderName + "</td><td>" + data[i].OFatherName + "</td><td>" + data[i].OAddress1 + "</td><td>" + "<button type=button class='btn btn-success btn-circle'  data-toggle=modal data-target=#myModalOffender style=cursor:pointer onclick=ViewOffender(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td>" + "<button type=button class='btn btn-warning btn-circle' style=cursor:pointer onclick=EditOffender(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' style=cursor:pointer onclick=DeleteOffender(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    $("#tb_OffenderInfo").append(rowdata);
                    $("#tbl_Offenderinfo").css("display", "block");
                }

            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootURl + 'CitizenParivadRegistration/GetVehicleDetails',
        success: function (data) {

            if (data.length > 0) {
                $('#DivVechile').show();
                $.each(data, function (i, items) {
                    var id = "'" + items.Vechilerowid + "'";
                    var count = 1;
                    count = count + i;
                    var bardata = "<tr class='" + items.Vechilerowid + "'><td>" + count + "</td><td>" + items.VechileRegistrationNo + "</td><td>" + items.VechileOwnerName + "</td><td>" + items.VechileChassisNo + "</td><td>" + items.VechileEngineNo + "</td><td></td></tr>";
                    $("#tblAddVechile").append(bardata);
                    $('#VechileRegistrationNo,#VechileOwnerName,#VechileMake,#VechileModel,#VechileChassisNo,#VechileEngineNo,#PastOffensesByVechile').val('');
                    $('#VechileType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });



    $('#ddl_OffenceCategory').on('change', function () {

        if ($(this).val() == "2") {
            $("#Wildlife-Data").hide();
            $("#Both-Data").hide();
            $("#Forest-Data").show();
            $("#ddl_BWOffenseSubCategoryForest").val($("#ddl_BWOffenseSubCategoryForest option:first").val());
            $("#ddl_BFOffenseSubCategoryForest").val($("#ddl_BFOffenseSubCategoryForest option:first").val());
            $("#ddl_BForestProtectionAct").val($("#ddl_BForestProtectionAct option:first").val());
            $("#ddl_BWildlifeProtectionAct").val($("#ddl_BWildlifeProtectionAct option:first").val());
            $("#ddl_WOffenseSubCategoryForest").val($("#ddl_WOffenseSubCategoryForest option:first").val());
            $("#ddl_WWildlifeProtectionAct").val($("#ddl_WWildlifeProtectionAct option:first").val());
        }
        else if ($(this).val() == "3") {
            $("#Wildlife-Data").hide();
            $("#Forest-Data").hide();
            $("#Both-Data").show();
            $("#ddl_FOffenseSubCategoryForest").val($("#ddl_FOffenseSubCategoryForest option:first").val());
            $("#ddl_FForestProtectionAct").val($("#ddl_FForestProtectionAct option:first").val());
            $("#ddl_WOffenseSubCategoryForest").val($("#ddl_WOffenseSubCategoryForest option:first").val());
            $("#ddl_WWildlifeProtectionAct").val($("#ddl_WWildlifeProtectionAct option:first").val());
        }
        else if ($(this).val() == "1") {
            $("#Forest-Data").hide();
            $("#Both-Data").hide();
            $("#Wildlife-Data").show();
            $("#ddl_BWOffenseSubCategoryForest").val($("#ddl_BWOffenseSubCategoryForest option:first").val());
            $("#ddl_BFOffenseSubCategoryForest").val($("#ddl_BFOffenseSubCategoryForest option:first").val());
            $("#ddl_FOffenseSubCategoryForest").val($("#ddl_FOffenseSubCategoryForest option:first").val());
            $("#ddl_FForestProtectionAct").val($("#ddl_FForestProtectionAct option:first").val());
            $("#ddl_BForestProtectionAct").val($("#ddl_BForestProtectionAct option:first").val());
            $("#ddl_BWildlifeProtectionAct").val($("#ddl_BWildlifeProtectionAct option:first").val());
        }

    });




    $('#ddlODistrict').change(function (e) {
        $("#txt_OVillageCode").empty();
        $("#txt_OVillageCode").append('<option value="' + '' + '">' +
                '--Select--' + '</option>');
        $.ajax({
            type: 'POST',
            url: RootURl + 'CitizenParivadRegistration/getVillage', // we are calling json method
            dataType: 'json',
            data: { dist_code: $("#ddlODistrict option:selected").val() },
            success: function (states) {
                $.each(states, function (i, vill) {
                    $("#txt_OVillageCode").append('<option value="' + vill.Value + '">' +
                     vill.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });


    $('.ComplainFound').hide();
    $('.ComplainNotFound').hide();
    $('#ComplainFound').change(function (e) {

        if ($(this).val() == 'Yes') {

            $('.ComplainFound').show();
            $('.ComplainNotFound').hide();
        }
        else {
            $('.ComplainFound').hide();
            $('.ComplainNotFound').show();
        }

    });

    $('#VehicleSeized').change(function (e) {

        if ($(this).val() == 'Yes') {

            $('#DivVechile').show();
        }
        else {
            $('#DivVechile').hide();
        }
    });

    $('#btnUploadStatement').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#OffenderStatementDoc").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#OffenderStatementDoc').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'CitizenParivadRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdOffenderStatementDoc").val(result.list2);
                        $('#OffenderStatementDoc').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
        } else {
            alert("FormData is not supported.");
        }
    });


    $("#OffenderStatementDoc").change(function (e) {
        //var iSize = ($("#OPhotoIDURL")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#OffenderStatementDoc")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#OffenderStatementDoc').val('');
            $('#errordivOffenderStatementDoc').show();
            $('#errordivOffenderStatementDoc').html("Upload ID should not be larger than 100 KB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#OffenderStatementDoc').focus();
            return false;
        }
        var file = $("#OffenderStatementDoc").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#OffenderStatementDoc').val('');
                $('#errordivOffenderStatementDoc').show();
                $('#errordivOffenderStatementDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#OffenderStatementDoc').focus();
                return false;
            } else {
                $('#errordivOffenderStatementDoc').hide();
            }
        }
        else { $('#errordivOffenderStatementDoc').hide(); }

    });



    $("#btn_addOffender").bind("click", function () {

        if ($("#Addknown-Offender").prop("checked")) {
            if ($('#txt_OffenderName').val() == '') {
                $('#errordivOffenderName').show();
                $('#errordivOffenderName').html("Please enter the Name of the Offender!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OffenderName').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOffenderName').hide(); errorCount = ""; }

            if ($('#txt_OFatherName').val() == '') {
                $('#errordivOFatherName').show();
                $('#errordivOFatherName').html("Please enter the Father Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OFatherName').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOFatherName').hide(); errorCount = ""; }

            if ($('#txt_OAddress1').val() == '') {
                $('#errordivOAddress1').show();
                $('#errordivOAddress1').html("Please enter the Residential Address1!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OAddress1').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOAddress1').hide(); errorCount = ""; }

            if ($('#ddlOState').val() == '' || $('#ddlOState').val() == null || $('#ddlOState').val() == '0') {
                $('#errordivddlOState').show();
                $('#errordivddlOState').html("Please Select the State!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddlOState').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivddlOState').hide(); errorCount = ""; }
            var oState = $("#ddlOState option:selected").val();
            if (oState == "1") {

                if ($('#ddlODistrict').val() == '' || $('#ddlODistrict').val() == null || $('#ddlODistrict').val() == '0') {

                    $('#errordivOddlDistrict').show();
                    $('#errordivOddlDistrict').html("Please Select the District!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                    $('#ddlODistrict').focus();
                    errorCount = "1";
                    return false;
                }
                else { $('#errordivOddlDistrict').hide(); errorCount = ""; }

                if ($('#txt_OVillageCode').val() == '' || $('#txt_OVillageCode').val() == null || $('#txt_OVillageCode').val() == '0') {
                    $('#errordivddlVillName').show();
                    $('#errordivddlVillName').html("Please enter the City/Village Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                    $('#txt_OVillageCode').focus();
                    errorCount = "1";
                    return false;
                }
                else { $('#errordivddlVillName').hide(); errorCount = ""; }
            }
            else if (oState == "2") {
                if ($('#txtdistrict').val() == '') {
                    $('#errordivddlODistrict1').show();
                    $('#errordivddlODistrict1').html("Please enter the District!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                    $('#txtdistrict').focus();
                    errorCount = "1";
                    return false;
                }
                else { $('#errordivddlODistrict1').hide(); errorCount = ""; }

                if ($('#txtvillage').val() == '') {
                    $('#errordivddlVillName1').show();
                    $('#errordivddlVillName1').html("Please enter the village!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                    $('#txtvillage').focus();
                    errorCount = "1";
                    return false;
                }
                else { $('#errordivddlVillName1').hide(); errorCount = ""; }
            }


            if ($('#txt_OffenderStatement').val() == '') {
                $('#errordivOffenderStatement').show();
                $('#errordivOffenderStatement').html("Please enter the Offender statement!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OffenderStatement').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOffenderStatement').hide(); errorCount = ""; }
        }
        if ($("#Unknown-Offender").prop("checked")) {

            if ($('#txt_ONumberOfOffender').val() == '') {
                $('#errordivONumberOfOffender').show();
                $('#errordivONumberOfOffender').html("Please enter the Number of Offenders!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_ONumberOfOffender').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivddlVillName').hide(); errorCount = ""; }

        }
        if (errorCount == '') {
            PostOffenderInfo();
        }
    });

    $("#MokaPunchnama").change(function (e) {
        var iSize = $("#MokaPunchnama")[0].files[0].size;
        //var iSize = parseFloat($("#MokaPunchnama")[0].files[0].size / 1024).toFixed(2);      
        if (iSize > 2097152) {
            $('#MokaPunchnama').val('');
            $('#errordivGriftariPunchnamaDoc').show();
            $('#errordivGriftariPunchnamaDoc').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#MokaPunchnama').focus();
            return false;
        }
        var file = $("#MokaPunchnama").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array
            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#MokaPunchnama').val('');
                $('#errordivGriftariPunchnamaDoc').show();
                $('#errordivGriftariPunchnamaDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#MokaPunchnama').focus();
                return false;
            } else {
                $('#errordivGriftariPunchnamaDoc').hide();
            }
        }
        else { $('#errordivGriftariPunchnamaDoc').hide(); }

    });



    $("#NagriNaka").change(function (e) {
        var iSize = $("#NagriNaka")[0].files[0].size;
        // var iSize = parseFloat($("#NagriNaka")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 2097152) {
            $('#NagriNaka').val('');
            $('#errordivNagriNakaDoc').show();
            $('#errordivNagriNakaDoc').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#NagriNaka').focus();
            return false;
        }
        var file = $("#NagriNaka").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#NagriNaka').val('');
                $('#errordivNagriNakaDoc').show();
                $('#errordivNagriNakaDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#NagriNaka').focus();
                return false;
            } else {
                $('#errordivNagriNakaDoc').hide();
            }
        }
        else { $('#errordivNagriNakaDoc').hide(); }

    });

    $("#WitnessRecord1").change(function (e) {
        var iSize = $("#WitnessRecord1")[0].files[0].size;
        //var iSize = parseFloat($("#WitnessRecord1")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 2097152) {
            $('#WitnessRecord1').val('');
            $('#errordivWitnessRecord1').show();
            $('#errordivWitnessRecord1').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#WitnessRecord1').focus();
            return false;
        }
        var file = $("#WitnessRecord1").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#WitnessRecord1').val('');
                $('#errordivWitnessRecord1').show();
                $('#errordivWitnessRecord1').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#WitnessRecord1').focus();
                return false;
            } else {
                $('#errordivWitnessRecord1').hide();
            }
        }
        else { $('#errordivWitnessRecord1').hide(); }

    });

    $("#WitnessRecord2").change(function (e) {
        var iSize = $("#WitnessRecord2")[0].files[0].size;
        //var iSize = parseFloat($("#WitnessRecord2")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 2097152) {
            $('#WitnessRecord2').val('');
            $('#errordivWitnessRecord2').show();
            $('#errordivWitnessRecord2').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#WitnessRecord2').focus();
            return false;
        }
        var file = $("#WitnessRecord2").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#WitnessRecord2').val('');
                $('#errordivWitnessRecord2').show();
                $('#errordivWitnessRecord2').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#WitnessRecord2').focus();
                return false;
            } else {
                $('#errordivWitnessRecord2').hide();
            }
        }
        else { $('#errordivWitnessRecord2').hide(); }

    });

    $("#WitnessRecord3").change(function (e) {
        var iSize = $("#WitnessRecord3")[0].files[0].size;
        //var iSize = parseFloat($("#WitnessRecord3")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 2097152) {
            $('#WitnessRecord3').val('');
            $('#errordivWitnessRecord3').show();
            $('#errordivWitnessRecord3').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#WitnessRecord3').focus();
            return false;
        }
        var file = $("#WitnessRecord3").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#WitnessRecord3').val('');
                $('#errordivWitnessRecord3').show();
                $('#errordivWitnessRecord3').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#WitnessRecord3').focus();
                return false;
            } else {
                $('#errordivWitnessRecord3').hide();
            }
        }
        else { $('#errordivWitnessRecord3').hide(); }

    });

    $("#FieldInspection").change(function (e) {
        var iSize = $("#FieldInspection")[0].files[0].size;
        // var iSize = parseFloat($("#FieldInspection")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 2097152) {
            $('#FieldInspection').val('');
            $('#errordivFieldInspection').show();
            $('#errordivFieldInspection').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#FieldInspection').focus();
            return false;
        }
        var file = $("#FieldInspection").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#FieldInspection').val('');
                $('#errordivFieldInspection').show();
                $('#errordivFieldInspection').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#FieldInspection').focus();
                return false;
            } else {
                $('#errordivFieldInspection').hide();
            }
        }
        else { $('#errordivFieldInspection').hide(); }

    });

    $("#Recomendation").change(function (e) {
        var iSize = $("#Recomendation")[0].files[0].size;
        //var iSize = parseFloat($("#Recomendation")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 2097152) {
            $('#Recomendation').val('');
            $('#errordivRecomendation').show();
            $('#errordivRecomendation').html("file should not be larger than 2 mb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#Recomendation').focus();
            return false;
        }
        var file = $("#Recomendation").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#Recomendation').val('');
                $('#errordivRecomendation').show();
                $('#errordivRecomendation').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#Recomendation').focus();
                return false;
            } else {
                $('#errordivRecomendation').hide();
            }
        }
        else { $('#errordivRecomendation').hide(); }

    });

    $('.btnSubmit').on('click', function (e) {

        if ($('#ddl_OffenseSeverity').val() == '' || $('#ddl_OffenseSeverity').val() == null || $('#ddl_OffenseSeverity').val() == '0') {
            // $('#collapse3').collapse('show');
            $('#errordivOSeverity').show();
            $('#errordivOSeverity').html("Select the Offense severity!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_OffenseSeverity').focus();
            return false;
        }
        else { $('#errordivOSeverity').hide(); }

        if ($('#ddl_ForestType').val() == '' || $('#ddl_ForestType').val() == null || $('#ddl_ForestType').val() == '0') {
            // $('#collapse3').collapse('show');
            $('#errordivForestType').show();
            $('#errordivForestType').html("Select forest type!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_ForestType').focus();
            return false;
        }
        else { $('#errordivForestType').hide(); }
        if ($('#ddl_OffenceCategory').val() == '' || $('#ddl_OffenceCategory').val() == null || $('#ddl_OffenceCategory').val() == '0') {
            // $('#collapse3').collapse('show');
            $('#errordivOffenseCategory').show();
            $('#errordivOffenseCategory').html("Please Select the Offense Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_OffenceCategory').focus();
            return false;
        }
        else { $('#errordivOffenseCategory').hide(); }

        var drpOcategory = $("#ddl_OffenceCategory").val();

        if (drpOcategory == "1") {

            if ($('#ddl_WWildlifeProtectionAct').val() == '' || $('#ddl_WWildlifeProtectionAct').val() == null || $('#ddl_WWildlifeProtectionAct').val() == '0') {
                // $('#collapse3').collapse('show');
                $('#errordivSectionWildLife').show();
                $('#errordivSectionWildLife').html("Please Select the Section under Wildlife Protection Act 1972!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_WWildlifeProtectionAct').focus();
                return false;
            }
            else { $('#errordivSectionWildLife').hide(); }

            if ($('#ddl_WOffenseSubCategoryForest').val() == '' || $('#ddl_WOffenseSubCategoryForest').val() == null || $('#ddl_WOffenseSubCategoryForest').val() == '0') {
                //  $('#collapse3').collapse('show');
                $('#errordivOWildLife').show();
                $('#errordivOWildLife').html("Please Select the Offense Sub-Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_WOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOWildLife').hide(); }

        }
        else if (drpOcategory == "2") {

            if ($('#ddl_FForestProtectionAct').val() == '' || $('#ddl_FForestProtectionAct').val() == null || $('#ddl_FForestProtectionAct').val() == '0') {
                // $('#collapse3').collapse('show');
                $('#errordivOFSection').show();
                $('#errordivOFSection').html("Please Select the Section under Forest Protection Act 1953!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_FForestProtectionAct').focus();
                return false;
            }
            else { $('#errordivOFSection').hide(); }

            if ($('#ddl_FOffenseSubCategoryForest').val() == '' || $('#ddl_FOffenseSubCategoryForest').val() == null || $('#ddl_FOffenseSubCategoryForest').val() == '0') {
                //  $('#collapse3').collapse('show');
                $('#errordivOFCategory').show();
                $('#errordivOFCategory').html("Please Select the Offense Sub-Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_FOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOFCategory').hide(); }

        }
        else if (drpOcategory == "3") {

            if ($('#ddl_BForestProtectionAct').val() == '' || $('#ddl_BForestProtectionAct').val() == null || $('#ddl_BForestProtectionAct').val() == '0') {
                //   $('#collapse3').collapse('show');
                $('#errordivOBForestSection').show();
                $('#errordivOBForestSection').html("Please Select the Section under Forest Protection Act 1953!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BForestProtectionAct').focus();
                return false;
            }
            else { $('#errordivOBForestSection').hide(); }

            if ($('#ddl_BFOffenseSubCategoryForest').val() == '' || $('#ddl_BFOffenseSubCategoryForest').val() == null || $('#ddl_BFOffenseSubCategoryForest').val() == '0') {
                //  $('#collapse3').collapse('show');
                $('#errordivOBFCategory').show();
                $('#errordivOBFCategory').html("Please Select Offense Sub-Category (as per rajasthan forest ACT)!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BFOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOBFCategory').hide(); }

            if ($('#ddl_BWildlifeProtectionAct').val() == '' || $('#ddl_BWildlifeProtectionAct').val() == null || $('#ddl_BWildlifeProtectionAct').val() == '0') {
                //   $('#collapse3').collapse('show');
                $('#errordivOBWildSection').show();
                $('#errordivOBWildSection').html("Please Select the Section under Wildlife Protection Act 1972!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BWildlifeProtectionAct').focus();
                return false;
            }
            else { $('#errordivOBWildSection').hide(); }

            if ($('#ddl_BWOffenseSubCategoryForest').val() == '' || $('#ddl_BWOffenseSubCategoryForest').val() == null || $('#ddl_BWOffenseSubCategoryForest').val() == '0') {
                //  $('#collapse3').collapse('show');
                $('#errordivOBWCategory').show();
                $('#errordivOBWCategory').html("Please Select Offense Sub-Category (as per rajasthan wildlife ACT)!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BWOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOBWCategory').hide(); }
        }

        if (!($('#Addknown-Offender').is(':checked') || $('#Unknown-Offender').is(':checked'))) {
            alert('select Known or Unknown Offender');
            return false;
        }
        else if ($('#Addknown-Offender').is(':checked')) {
            var rowCount = $('#tbl_Offenderinfo tr').not('thead tr').length;
            if (!rowCount) { rowCount == 0; }
            if (parseInt(rowCount, 10) == 0) {
                // $('#knownOffender').collapse('show');
                alert('Enter known offender details');
                return false;
            }
        }
        else if ($('#Unknown-Offender').is(':checked')) {
            if (parseInt($('#txt_ONumberOfOffender').val(), 10) == 0) {
                //  $('#UnknownOffender').collapse('show');
                $('#errordivUNumberOffenders').show();
                $('#errordivUNumberOffenders').html("Enter number of offender" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_ONumberOfOffender').focus();
                return false;
            }
            else {
                $('#errordivUNumberOffenders').hide();
            }
        }

        if ($('#VisitDate').val() == '') {
            //  $('#collapse2').collapse('show');
            $('#errordivDateOfVisit').show();
            $('#errordivDateOfVisit').html("Select date of visit" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#VisitDate').focus();
            return false;
        }
        else {
            $('#errordivDateOfVisit').hide();
        }
        if ($('#VisitPlace').val() == '') {
            //  $('#collapse2').collapse('show');
            $('#errordivVisitPlace').show();
            $('#errordivVisitPlace').html("Enter place of visit" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#VisitPlace').focus();
            return false;
        }
        else {
            $('#errordivVisitPlace').hide();
        }
        if ($('#ComplainFound').val() == '') {
            //  $('#collapse2').collapse('show');
            $('#errordivComplain').show();
            $('#errordivComplain').html("Select complain correct or incorrect" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ComplainFound').focus();
            return false;
        }
        else {
            $('#errordivComplain').hide();
        }
        if ($('#lstSeizedItem').val() == '' && $('#ComplainFound').val() == 'Yes') {
            // $('#collapse2').collapse('show');
            $('#errordivItemSeized').show();
            $('#errordivItemSeized').html("Select complain correct or incorrect" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#lstSeizedItem').focus();
            return false;
        }
        else {
            $('#errordivItemSeized').hide();
        }
        if ($('#VehicleSeized').val() == '' && $('#ComplainFound').val() == 'Yes') {
            //  $('#collapse2').collapse('show');
            $('#errordivVehicleSeized').show();
            $('#errordivVehicleSeized').html("Select vehicle seized" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#VehicleSeized').focus();
            return false;
        }
        else {
            $('#errordivVehicleSeized').hide();
        }

    });

    $(document).on('click', '.get-delete', function (e) {

        jQuery(this).closest('tr').remove();
        //var RowInfo = {
        //    emailId: $(this).val()
        //};
        //$.ajax({
        //    type: 'POST',
        //    contentType: 'application/json; charset=utf-8',
        //    data: JSON.stringify(RowInfo),
        //    url: RootURl + 'OffenseRegistration/DeleteOffenderData',
        //    success: function (data) {
        //        alert("Offender Saved Successfully!!!!");
        //    },
        //    error: function (ex) { alert(ex); }

        //});
        e.preventDefault();
    });

});


function getvillage(distcode, villcode) {

    $.ajax({
        type: 'POST',
        url: RootURl + 'CitizenParivadRegistration/getVillage', // we are calling json method
        dataType: 'json',
        data: { dist_code: distcode },
        success: function (states) {
            $.each(states, function (i, vill) {
                $("#txt_OVillageCode").append('<option value="' + vill.Value + '">' +
                 vill.Text + '</option>');
            });
            $('#txt_OVillageCode').val(villcode);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
}

function PostOffenderInfo() {
    var oState = $("#ddlOState option:selected").val();

    if (oState == "1") {
        var oDistrict = $("#ddlODistrict option:selected").val();
        var oVillage = $("#txt_OVillageCode").val();
    }

    else if (oState == "2") {
        var oDistrict = $("#txtdistrict").val();
        var oVillage = $("#txtvillage").val();
    }
    var offenderInfo = {
        OOffenderrowid: $('#hdnOOffenderrowid').val(),
        OffenderType: $('input[name="Offender"]:checked').val(),
        OffenderName: $("#txt_OffenderName").val(),
        OFatherName: $("#txt_OFatherName").val(),
        OAddress1: $("#txt_OAddress1").val(),
        OStateCode: $("#ddlOState option:selected").val(),
        ODistrictCode: oDistrict,
        OVillageCode: oVillage,
        OPhoneNo: $("#OPhoneNo").val(),
        OffenderStatement: $('#txt_OffenderStatement').val(),
        OffenderStatementDoc: $('#hdOffenderStatementDoc').val(),
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(offenderInfo),
        url: RootURl + 'CitizenParivadRegistration/OffenderData',
        success: function (data) {
            if (data.OffenderName != '') {
                var id = "'" + data.OOffenderrowid + "'";
                // Delete Option Commented// var rowdata = "<tr><td>" + count + "</td><td style=display:none;>" + data.OOffenderrowid + "</td><td>" + data.OffenderName + "</td><td>" + data.OFatherName + "</td><td>" + data.OCaste + "</td><td>" + data.OVillageCode + "</td><td style='border:1px'>" + "<button type=button class=get-view style=cursor:pointer onclick=EditOffender(" + id + ")>Edit</button>" + "</td><td>" + "<button class='get-delete' value='" + data.OEmailID + "'>Delete</button>" + "</td></tr>";
                var rowdata = "<tr><td style=display:none;>" + data.OOffenderrowid + "</td><td>" + data.OffenderName + "</td><td>" + data.OFatherName + "</td><td>" + data.OAddress1 + "</td><td>" + "<button type=button class='btn btn-success btn-circle'  data-toggle=modal data-target=#myModalOffender style=cursor:pointer onclick=ViewOffender(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td>" + "<button type=button class='btn btn-warning btn-circle' style=cursor:pointer onclick=EditOffender(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' style=cursor:pointer onclick=DeleteOffender(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $('#tbl_Offenderinfo tbody tr td:nth-child(1)').each(function () {
                    if (data.OOffenderrowid == $(this).text()) {
                        $(this).closest('tr').remove();
                    }
                });
                $("#tb_OffenderInfo").append(rowdata);
                $('#txt_OffenderName').val(''); $('#txt_OFatherName,#txt_OffenderStatement').val('');
                $('#txt_OAddress1').val('');
                $('#ddlOState option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#ddlODistrict option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#txt_OVillageCode option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $("#txtdistrict,#txtvillage").val('');
            }
            $("#tbl_Offenderinfo").css("display", "block");
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });
}

function EditOffender(ID) {
    $('#hdnOOffenderrowid').val(ID);
    var tblinfo = {
        ID: ID
    }
    $.ajax({
        type: 'POST',
        url: RootURl + 'CitizenParivadRegistration/EditDetails',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(tblinfo),
        success: function (data) {
            $('#txt_OffenderName').val(data.OffenderName); $('#txt_OFatherName').val(data.OFatherName);
            $('#txt_OSpouseName').val(data.OSpouseName); $('#txt_OCasteName').val(data.OCaste);
            $('#OClothesWorn').val(data.OClothesWorn); $('#OColorOfClothes').val(data.OColorOfClothes);
            $('#OPhysicalAppearance').val(data.OPhysicalAppearance); $('#Height').val(data.OHeight);
            $('#OOtherSpecialDetails').val(data.OOtherSpecialDetails); $('#txt_OAddress1').val(data.OAddress1);
            $('#txt_OAddress2').val(data.OAddress2); $('#ddlOState').val(data.OStateCode);
            $("#txt_OCategory").val(data.OCategory); $('#txt_OPincode').val(data.OPincode);
            $('#OPhoneNo').val(data.OPhoneNo); $('#txt_OEmailID').val(data.OEmailID);
            $('#ddl_OPhotoIDType').val(data.OPhotoIDType); $('#txt_OffenderAge').val(data.OffenderAge);
            $("#hdIDProof").val(data.OPhotoIDURL); $('#ArrestedOrdetained').val(data.ArrestedOrdetained);
            $('#hdOffenderStatementDoc').val(data.OffenderStatementDoc); $('#txt_OffenderStatement').val(data.OffenderStatement);
            $('#hdFardGriftriDoc').val(data.FardGriftri); $('#hdGriftariPunchnamaDoc').val(data.GriftariPunchnama);
            $('#hdNagriNakaDoc').val(data.NagriNaka); $('#hdJamaTalashiDoc').val(data.JamaTalashi);
            $('#hdMedicalReportDoc').val(data.MedicalReport);
            if (data.OStateCode == "1") {
                getvillage(data.ODistrictCode, data.OVillageCode);
                $("#txtdistrict,#txtvillage").val('');
                $('#divtxtDistrict,#divtxtVillage').hide();
                $('#divDDlDistrict,#divDDlVillage').show();
                $('#ddlODistrict').val(data.ODistrictCode);
                $('#txt_OVillageCode').val(data.OVillageCode);
            }
            if (data.OStateCode == "2") {
                $("#ddlODistrict").val($("#ddlODistrict option:first").val());
                $('#divtxtDistrict,#divtxtVillage').show();
                $('#divDDlDistrict,#divDDlVillage').hide();
                $("#txtdistrict").val(data.ODistrictCode);
                $("#txtvillage").val(data.OVillageCode);
            }

            if (data.ArrestedOrdetained == 'Arrested') {
                $('#InformToOffenderRelative').val(data.InformToOffenderRelative);
                $('#CommunicationMode').val(data.CommunicationMode);
                $('#CommunicationDate').val(data.CommunicationDate);

                if (data.FardGriftri != '' && data.FardGriftri != null) {
                    var filename2 = data.FardGriftri;
                    var str2 = filename2.split('/');
                    $('#viewdoc2').show();
                    $('#viewdoc2').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str2[3]);
                    $('#viewdoc2').attr('target', '_blank');
                }
                if (data.GriftariPunchnama != '' && data.GriftariPunchnama != null) {
                    var filename3 = data.GriftariPunchnama;
                    var str3 = filename3.split('/');
                    $('#viewdoc3').show();
                    $('#viewdoc3').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str3[3]);
                    $('#viewdoc3').attr('target', '_blank');
                }
                if (data.NagriNaka != '' && data.NagriNaka != null) {
                    var filename4 = data.NagriNaka;
                    var str4 = filename4.split('/');
                    $('#viewdoc4').show();
                    $('#viewdoc4').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str4[3]);
                    $('#viewdoc4').attr('target', '_blank');
                }
                if (data.JamaTalashi != '' && data.JamaTalashi != null) {
                    var filename5 = data.JamaTalashi;
                    var str5 = filename5.split('/');
                    $('#viewdoc5').show();
                    $('#viewdoc5').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str5[3]);
                    $('#viewdoc5').attr('target', '_blank');
                }
                if (data.MedicalReport != '' && data.MedicalReport != null) {
                    var filename6 = data.MedicalReport;
                    var str6 = filename6.split('/');
                    $('#viewdoc6').show();
                    $('#viewdoc6').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str6[3]);
                    $('#viewdoc6').attr('target', '_blank');
                }
                $('.arrested').show();
            }
            else {

                $('.arrested').hide();
            }
            var filename = data.OPhotoIDURL;
            if (data.OPhotoIDURL != '' && data.OPhotoIDURL != null) {
                var str = filename.split('/');
                $('#viewdoc').show();
                $('#viewdoc').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str[3]);
                $('#viewdoc').attr('target', '_blank');
            }

            //if (data.OffenderStatementDoc != 'N/A' && data.OffenderStatementDoc != null) {

            //    $('#ViewStatementdoc').show();
            //    $('#ViewStatementdoc').attr('href', data.OffenderStatementDoc);             
            //    $('#ViewStatementdoc').attr('target', '_blank');

            //}
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
}

function ViewOffender(ID) {
    //$('#hdnOOffenderrowid').val(ID);

    var tblinfo = {
        ID: ID
    }
    $("#tbdyOffender").empty();
    $.ajax({
        type: 'POST',
        url: RootURl + 'CitizenParivadRegistration/EditDetails',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(tblinfo),
        success: function (data) {

            $("#txt_OCategory").val(data.OCategory);
            var category = $("#txt_OCategory option:selected").text();
            $('#txt_OCategory option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);

            if (data.OStateCode == "1") {
                $("#txt_OVillageCode").val(data.OVillageCode);
                var village = $("#txt_OVillageCode option:selected").text();
                $('#txt_OVillageCode option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);

                $("#ddlODistrict").val(data.ODistrictCode);
                var district = $("#ddlODistrict option:selected").text();
                $('#ddlODistrict option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
            }
            if (data.OStateCode == "2") {

                var village = data.OVillageCode;
                var district = data.ODistrictCode;
            }

            var state = "";
            if (data.OStateCode == 1) {
                state = "Rajasthan";
            }
            else {
                state = "Others";
            }
            var bardata = "<tr><td>Offender Name</td><td>" + data.OffenderName +
            "</td></tr><tr><td>Father Name</td><td>" + data.OFatherName +
            "</td></tr><tr><td>Address1</td><td>" + data.OAddress1 +
            "</td></tr><tr><td>District</td><td>" + district +
            "</td></tr><tr><td>Village</td><td>" + village +
            "</td></tr><tr><td>State</td><td>" + state +
            "</td></tr><tr><td>Offender Statement</td><td>" + data.OffenderStatement +
            "</td></tr><tr><td>Statement Document</td><td>" + data.OffenderStatementDoc +
            "</td></tr>";
            $("#tbdyOffender").append(bardata);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
}

function AddVechile() {
    if ($('#VechileRegistrationNo').val() == '') {
        $('#VechileRegistrationNo').focus();
        $('#VechileRegistrationNo').attr('placeholder', 'Enter Registration No.');
    }
    else if ($('#VechileOwnerName').val() == '') {
        $('#VechileOwnerName').focus();
        $('#VechileOwnerName').attr('placeholder', 'Enter Owner name');
    }
    else if ($('#VechileType option:selected').text() == '--Select--') {
        $('#VechileType').focus();
    }
    else if ($('#VechileMake').val() == '') {
        $('#VechileMake').focus();
        $('#VechileMake').attr('placeholder', 'Enter vechile make');
    }
    else if ($('#VechileModel').val() == '') {
        $('#VechileModel').focus();
        $('#VechileModel').attr('placeholder', 'Enter vechile model');
    }
    else if ($('#VechileChassisNo').val() == '') {
        $('#VechileChassisNo').focus();
        $('#VechileChassisNo').attr('placeholder', 'Enter chassis no.');
    }
    else if ($('#VechileEngineNo').val() == '') {
        $('#VechileEngineNo').focus();
        $('#VechileChassisNo').attr('placeholder', 'Enter engine no.');
    }
    else {
        $("#tblAddVechile").empty();
        var vechileinfo = {
            VechileRegistrationNo: $('#VechileRegistrationNo').val(), VechileOwnerName: $('#VechileOwnerName').val(),
            VechileType: $('#VechileType').val(), VechileMake: $('#VechileMake').val(), VechileModel: $('#VechileModel').val(),
            VechileChassisNo: $('#VechileChassisNo').val(), VechileEngineNo: $('#VechileEngineNo').val(), PastOffensesByVechile: $('#PastOffensesByVechile').val(),
            VechileUploadDoc: $('#hdVechileDoc').val()
        }
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(vechileinfo),
            url: RootURl + 'CitizenParivadRegistration/AddVechile',
            success: function (data) {
                $.each(data, function (i, items) {
                    var id = "'" + items.Vechilerowid + "'";
                    var count = 1;
                    count = count + i;
                    var bardata = "<tr class='" + items.Vechilerowid + "'><td>" + count + "</td><td>" + items.VechileRegistrationNo + "</td><td>" + items.VechileOwnerName + "</td><td>" + items.VechileChassisNo + "</td><td>" + items.VechileEngineNo + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteVechile(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    $("#tblAddVechile").append(bardata);
                    $('#VechileRegistrationNo,#VechileOwnerName,#VechileMake,#VechileModel,#VechileChassisNo,#VechileEngineNo,#PastOffensesByVechile').val('');
                    $('#VechileType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            },
            traditional: true,
            error: function (data) { console.log(data) }
        });
    }
}

function DeleteVechile(ID) {
    $("#tblAddVechile").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootURl + 'CitizenParivadRegistration/DeleteVechile',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Vechilerowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.Vechilerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.VechileRegistrationNo + "</td><td style='border:1px'>" + items.VechileType + "</td><td style='border:1px'>" + items.VechileChassisNo + "</td><td style='border:1px'>" + items.VechileEngineNo + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteVechile(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblAddVechile").append(bardata);
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });
}

function DeleteOffender(ID) {
    //  $('#hdnOOffenderrowid').val(ID);
    var tblinfo = {
        ID: ID
    }
    $("#tb_OffenderInfo").empty();
    $.ajax({
        type: 'POST',
        url: RootURl + 'CitizenParivadRegistration/DeleteOffenderData',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(tblinfo),
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var id = "'" + data[i].OOffenderrowid + "'";
                rowdata = "<tr class='rowid'><td style=display:none;>" + data[i].OOffenderrowid + "</td><td>" + data[i].OffenderName + "</td><td>" + data[i].OFatherName + "</td><td>" + data[i].OAddress1 + "</td><td>" + data[i].OVillageCode + "</td><td style='border:1px'>" + "<button type=button class=get-view data-toggle=modal data-target=#myModalOffender style=cursor:pointer onclick=ViewOffender(" + id + ")>View</button>" + "</td><td style='border:1px'>" + "<button type=button class=get-view style=cursor:pointer onclick=EditOffender(" + id + ")>Edit</button>" + "</td><td style='border:1px'>" + "<button type=button class=get-view style=cursor:pointer onclick=DeleteOffender(" + id + ")>Delete</button>" + "</td></tr>";
                $("#tb_OffenderInfo").append(rowdata);
                $("#tbl_Offenderinfo").css("display", "block");
            }
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
}

function GetRTODetails() {

    var VechileRegistrationNo = $("#VechileRegistrationNo").val();

    $.ajax({
        type: 'POST',
        url: RootURl + 'CitizenParivadRegistration/GetRTOVechileRDetails', // we are calling json method
        dataType: 'json',
        data: { VechileRegistrationNumber: VechileRegistrationNo },
        success: function (ReturnData) {

            $("#VechileOwnerName").val(ReturnData.Rc_owner_name);
            // $("#VechileType").val() = "";
            $("#VechileMake").val(ReturnData.Rc_maker_desc);
            $("#VechileModel").val(ReturnData.Rc_maker_model);
            $("#VechileChassisNo").val(ReturnData.Rc_chasi_no);
            $("#VechileEngineNo").val(ReturnData.Rc_eng_no);


        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }

    });
}

function reload() {
    location.reload(true);
}