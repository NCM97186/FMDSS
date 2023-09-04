var RootURl = '@Url.Content("~/")';



$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
$(document).ready(function () {
   

    $(".datefield").datepicker({ maxDate: new Date(), dateFormat: 'dd/mm/yy', changeYear: true, onClose: function (dateText, inst) { $("[id $=auto]").focus(); } });
    var errorCount = '1';  
    $('#txt_OffenderName,#txt_OFatherName,#OClothesWorn,#OColorOfClothes,#OPhysicalAppearance,#txt_OSpouseName,#txt_OVillageCode').keypress(function (e) {
        if (e.ctrlKey || e.altKey) {
            e.preventDefault();
        } else {
            var key = (e.which) ? e.which : e.keyCode
            if (e.shiftKey) {
                if (key == 64 || key == 33 || key == 35 || key == 36 || key == 37 || key == 94 || key == 38 || key == 42 || key == 40 || key == 41) {
                    e.preventDefault();
                }
            }
            if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122))) {
                e.preventDefault();
            }
        }
    });

    $('#OHeight,#txt_SettlementAmount').bind('keypress', function (evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode
        if ((charCode == 46) || (charCode >= 97 && charCode <= 122) || (charCode >= 65 && charCode <= 90)) {
            return false;
        }
        else {
            if (charCode >= 48 && charCode <= 57 || charCode == 46) {
                return true;
            }
            else {
                evt.preventDefault();
            }
        }
    });

    $('#OOtherSpecialDetails,#txt_OAddress1,#txt_OAddress2,#txt_Description').keypress(function (e) {
        var kc = e.which;
        if (e.shiftKey) {
            if (kc == 64 || kc == 33 || kc == 35 || kc == 36 || kc == 37 || kc == 94 || kc == 38 || kc == 42 || kc == 40 || kc == 41) {
                e.preventDefault();
            }
        }
        if ((kc >= 97 && kc <= 122) || (kc >= 65 && kc <= 90) || (kc == 0 || kc == 8 || kc == 13 || kc == 95) || (kc >= 47 && kc <= 57) || (kc >= 44 && kc < 46) || (kc >= 40 && kc < 42) || (kc >= 96 && kc <= 105) || (kc == 32)) {

        }
        else {
            e.preventDefault();
        }
    });


  
    $('#txt_long').keydown(function (e) {
        var kc = e.keyCode;
        var position = $(this).val().length;       
            if (position == 2 && kc != 8) {               
                if (parseInt($(this).val().substring(0,2), 10) >= 67 && parseInt($(this).val().substring(0,2), 10) <= 78) {
                    var value = $('#txt_long').val();
                    $('#txt_long').val(value + '.');
                }                
            }
            if (position > 2 && kc != 8) {
                if (!(parseInt($(this).val().substring(0, 2), 10) >= 67 && parseInt($(this).val().substring(0, 2), 10) <= 78)) {
                    $('#errordivLongitude').show();
                    $('#errordivLongitude').html("Enter the GPS longitude between 67° to 78°!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                    $('#txt_long').focus();
                    e.preventDefault();
                }
            }            
    });

    $('#txt_lat').keydown(function (e) {
        var kc = e.keyCode;       
        var position = $(this).val().length;                               
        if (position == 2 && kc!=8) {
            if (parseInt($(this).val().substring(0, 2), 10) >= 23 && parseInt($(this).val().substring(0, 2), 10) <= 29) {
                var value = $('#txt_lat').val();
                $('#txt_lat').val(value + '.');
            }           
        }
        if (position > 2 && kc != 8) {
            if (!(parseInt($(this).val().substring(0, 2), 10) >= 23 && parseInt($(this).val().substring(0, 2), 10) <= 29)) {
                $('#errordivLatitude').show();
                $('#errordivLatitude').html("Enter the GPS latitude between 23° - 29°!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                e.preventDefault();
            }

        }
    });

    if ($("#hdUserRole").val() == "CITIZEN") {
        $("#btn_Previousform2").hide();
    }
    else {
        $("#btn_Previousform2").show();
    }
    var offcat = $("#hdfpmOffenseCat").val();
    if (offcat == "3") {
        $("#Wildlife-Data").hide();
        $("#Forest-Data").hide();
        $("#Both-Data").show();
        $("#ddl_FOffenseSubCategoryForest").val($("#ddl_FOffenseSubCategoryForest option:first").val());
        $("#ddl_FForestProtectionAct").val($("#ddl_FForestProtectionAct option:first").val());
        $("#ddl_WOffenseSubCategoryForest").val($("#ddl_WOffenseSubCategoryForest option:first").val());
        $("#ddl_WWildlifeProtectionAct").val($("#ddl_WWildlifeProtectionAct option:first").val());

    }
    else if (offcat == "2") {
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
    else if (offcat == "1") {
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

    $('#ddl_FForestProtectionAct').change(function (e) {
        var ddldist = $("#ddl_FForestProtectionAct option:selected").val();
        $("#ddl_FOffenseSubCategoryForest").empty();
        $("#ddl_FOffenseSubCategoryForest").append('<option value="' + 0 + '">--Select--</option>');
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistration/GetforestSubCat', // we are calling json method
            dataType: 'json',
            data: { FProtectionId: ddldist },
            success: function (SubCat) {
                $.each(SubCat, function (i, items) {
                    $("#ddl_FOffenseSubCategoryForest").append('<option value="' + items.Value + '">  ' + items.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });

    $('#ddl_BForestProtectionAct').change(function (e) {
        var ddldist = $("#ddl_BForestProtectionAct option:selected").val();
        $("#ddl_BFOffenseSubCategoryForest").empty();
        $("#ddl_BFOffenseSubCategoryForest").append('<option value="' + 0 + '">--Select--</option>');
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistration/GetforestSubCat', // we are calling json method
            dataType: 'json',
            data: { FProtectionId: ddldist },
            success: function (SubCat) {
                $.each(SubCat, function (i, items) {
                    $("#ddl_BFOffenseSubCategoryForest").append('<option value="' + items.Value + '">  ' + items.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });




    var formval = $("#hdFormNo").val();
    var userRole = $("#hdfpmUserRole").val();
    if (formval == "2" || formval == "3") {
        var rowdata = "";
        var ID = { id: $("#hdFPMOffenseCode").val(), UserRole: userRole }
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(ID),
            url: RootURl + 'OffenseRegistration/GetApplicantDetail',
            success: function (data) {

                rowdata = "<tr class='rowid'><td>" + data.OffenseCode + "</td><td>" + data.Category + "</td><td>" + data.Forest_Protection_Act + "</td><td>" + data.FOSubCategory + "</td><td>" + data.Wildlife_Protection_Act + "</td><td>" + data.WOSubCategory + "</td></tr>";
                //rowdata1 = "<tr class='rowid'><td>" + data.DateOfOffense + "</td><td>" + data.TimeOfOffense + "</td><td>" + data.OffenseDetail + "</td><td>" + data.OffensePlace + "</td><td>" + data.DIST_NAME + "</td><td>" + data.Ssoid + "</td></tr>";
                //rowdata2 = "<tr class='rowid'><td>" + data.VisitDate + "</td><td>" + data.VisitPlace + "</td><td>" + data.VisitTime + "</td><td>" + data.InvestiDescription + "</td></tr>";
                $("#tb_ApplicantDetails").append(rowdata);
                //$("#tb_ApplicantDetails11").append(rowdata1);
                //$("#tb_ApplicantDetails12").append(rowdata2);
                ////$("#tbl_Details11").css("display", "block");
                //$("#tbl_Details12").css("display", "block");
                //$("#tbl_Details1").css("display", "block");


                $("#tb_ApplicantDetails21").append(rowdata);
                //$("#tb_ApplicantDetails22").append(rowdata1);
                //$("#tb_ApplicantDetails23").append(rowdata2);
                ////$("#tbl_Details21").css("display", "block");
                //$("#tbl_Details22").css("display", "block");
                //$("#tbl_Details23").css("display", "block");



            },

            traditional: true,
            error: function (data) { console.log(data) }
        });

    }
    //Checked the value in edit mode Is the case compoundable

    var caseCompoundable = $("#hdOCompound").val();


    if (caseCompoundable == "Yes") {
        $('#NoCompundable').removeAttr('checked');
        $('#YesCompundable').attr('checked', 'checked');
        $('#txt_SettlementAmount').attr('disabled', false);

    }
    else (caseCompoundable == "No")
    {
        $('#YesCompundable').removeAttr('checked');
        $('#NoCompundable').attr('checked', 'checked');
        $('#txt_SettlementAmount').attr('disabled', true);

    }



    //Checked the value in edit mode Amount Paid for compound

    //var amountPay = $("#hdOIsPaid").val();

    //if (amountPay == "Pay-Now") {
    //    $('#PayLCompound').removeAttr('checked');
    //    $('#PayNCompound').attr('checked', 'checked');

    //}
    //else (amountPay == "Pay-Later")
    //{
    //    $('#PayNCompound').removeAttr('checked');
    //    $('#PayLCompound').attr('checked', 'checked');
    //}
    var hdOffenderType1 = $('#hdOffenderType').val();
    if (hdOffenderType1 == "UnKnown") {
        $(".Unknown").show();
        $(".known").hide();
        $('#Unknown-Offender').attr('checked', true);
    }
    else if (hdOffenderType1 == "Known") {
        $('#Addknown-Offender').attr('checked', true);
        $(".Unknown").hide();
        $(".known").show();
    }


    $('#btnSubmitDetails2').click(function () {
        if ($('#Addknown-Offender').is(':checked') || $('#Unknown-Offender').is(':checked')) {

        }
        else {
            alert("Select radio button for known/unknown offender");
            return false;
        }
        if ($("#Addknown-Offender").prop("checked")) {


            if ($("#tb_OffenderInfo").html() == '') {
                $('#errordivOUOffender').show();
                $('#errordivOUOffender').html("Please Fill atleast one Known/Unknown Offender Records!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                return false;
            }
            else { $('#errordivOUOffender').hide(); }

        }
        else if ($("#Unknown-Offender").prop("checked")) {

            if ($('#txt_ONumberOfOffender').val() == '') {
                $('#errordivONumberOfOffender').show();
                $('#errordivONumberOfOffender').html("Please enter the Number of Offenders!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_ONumberOfOffender').focus();
                return false;
            }
            else { $('#errordivONumberOfOffender').hide(); }

            //if ($('#txt_OffenderDescription').val() == '') {
            //    $('#errordivONumberOfOffender').show();
            //    $('#errordivONumberOfOffender').html("Please enter the Number of Offenders!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            //    $('#txt_OffenderDescription').focus();
            //    return false;
            //}
            //else { $('#errordivONumberOfOffender').hide(); }
        }
        if ($('#txt_OffenseStatementDate').val() == '') {
            $('#txt_OffenseStatementDate').focus();
            return false;
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
                    url: RootURl + 'OffenseRegistration/UploadFiles',
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

    $('#btnUpload').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#OPhotoIDURL").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#txt_EvidenceDocument').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'OffenseRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdIDProof").val(result.list2);
                        $('#OPhotoIDURL').val('');
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

    $("#OPhotoIDURL").change(function (e) {
        //var iSize = ($("#OPhotoIDURL")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#OPhotoIDURL")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#OPhotoIDURL').val('');
            $('#errordivOUploadID').show();
            $('#errordivOUploadID').html("Upload ID should not be larger than 100 KB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#OPhotoIDURL').focus();
            return false;
        }
        var file = $("#OPhotoIDURL").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#OPhotoIDURL').val('');
                $('#errordivOUploadID').show();
                $('#errordivOUploadID').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#OPhotoIDURL').focus();
                return false;
            } else {
                $('#errordivOUploadID').hide();
            }
        }
        else { $('#errordivOUploadID').hide(); }

    });





    $("#fileComplainantStatement").change(function (e) {
       // var iSize = ($("#fileComplainantStatement")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#fileComplainantStatement")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#fileComplainantStatement').val('');
            $('#errordivComplainantStatementDoc').show();
            $('#errordivComplainantStatementDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#fileComplainantStatement').focus();
            return false;
        }
        var file = $("#fileComplainantStatement").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#fileComplainantStatement').val('');
                $('#errordivComplainantStatementDoc').show();
                $('#errordivComplainantStatementDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#fileComplainantStatement').focus();
                return false;
            } else {
                $('#errordivComplainantStatementDoc').hide();
            }
        }
        else { $('#errordivComplainantStatementDoc').hide(); }

    });

    //if ($('#hdOffenderStatementDoc').val() != '' && $('#hdOffenderStatementDoc').val() != null) {
    //    var str = $('#hdOffenderStatementDoc').val().split('/');
    //    $('#viewStatement').attr('href', '../ForestProtectionDocument/OffenderDetails/' + str[3]);
    //    $('#viewStatement').attr('target', '_blank');
    //    $('#viewStatement').show();
    //}

    if ($('#hdComplainantStatementDoc').val() != '' && $('#hdComplainantStatementDoc').val() != null) {
        var str = $('#hdComplainantStatementDoc').val().split('/');
        $('#viewComplainant').attr('href', '../ForestProtectionDocument/OffenderDetails/' + str[3]);
        $('#viewComplainant').attr('target', '_blank');
        $('#viewComplainant').show();
    }

    $('#btnUpload1').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#file_CrimeScenePhoto1").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }



            $.ajax({
                url: RootURl + 'OffenseRegistration/UploadCrimeSceneFiles',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (result) {
                    var fname = result.list2.split('/');
                    alert(result.list1);
                    $("#hdCrimePicUrlPath1").val(result.list2);

                    $('#lblUpload1').text("1) " + fname[3]).css('color', 'blue');
                    $('#lblUpload2').text("2) " + fname[3]).css('color', 'blue');
                    $('#lblUpload3').text("3) " + fname[3]).css('color', 'blue');
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#btnUpload2').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#file_CrimeScenePhoto2").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }




            $.ajax({
                url: RootURl + 'OffenseRegistration/UploadCrimeSceneFiles',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (result) {

                    alert(result.list1);
                    $("#hdCrimePicUrlPath2").val(result.list2);

                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#btnUpload3').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#file_CrimeScenePhoto3").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            //    // Adding one more key to FormData object


            $.ajax({
                url: RootURl + 'OffenseRegistration/UploadCrimeSceneFiles',
                type: "POST",
                contentType: false, // Not to set any content header
                processData: false, // Not to process data
                data: fileData,
                success: function (result) {

                    alert(result.list1);
                    $("#hdCrimePicUrlPath3").val(result.list2);

                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    });



    $("#btnViewFile1").click(function () {

        var path = RootURl + $("#hdCrimePicUrlPath1").val();

        var pathurl = path.replace('~/', '');
        window.open(pathurl, '_blank');

    });
    $("#btnViewFile2").click(function () {

        var path = RootURl + $("#hdCrimePicUrlPath2").val();

        var pathurl = path.replace('~/', '');
        window.open(pathurl, '_blank');

    });
    $("#btnViewFile3").click(function () {

        var path = RootURl + $("#hdCrimePicUrlPath3").val();

        var pathurl = path.replace('~/', '');
        window.open(pathurl, '_blank');

    });

    $("#Bfile_CrimeScenePhoto1").change(function (e) {

        var iSize = ($("#Bfile_CrimeScenePhoto1")[0].files[0].size / 1048576);

        if (iSize > 2) {
            $('#Bfile_CrimeScenePhoto1').val('');
            $('#errordivEvidenceDocURL').show();
            $('#errordivEvidenceDocURL').html("Photographs/Document related to offense/crime scene should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#Bfile_CrimeScenePhoto1').focus();
            return false;


        }

        var file = $("#Bfile_CrimeScenePhoto1").val();

        var exts = ['jpeg', 'jpg', 'png', 'gif', 'pdf'];

        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#Bfile_CrimeScenePhoto1').val('');
                $('#errordivEvidenceDocURL').show();
                $('#errordivEvidenceDocURL').html("Please upload only jpeg or jpg or png or gif or pdf file format in Photographs/Document related to offense/crime scene Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#Bfile_CrimeScenePhoto1').focus();
                return false;
            } else {
                $('#errordivEvidenceDocURL').hide();
            }
        }
        else { $('#errordivEvidenceDocURL').hide(); }

    });
    $("#Bfile_CrimeScenePhoto2").change(function (e) {

        var iSize = ($("#Bfile_CrimeScenePhoto2")[0].files[0].size / 1048576);

        if (iSize > 2) {
            $('#Bfile_CrimeScenePhoto2').val('');
            $('#errordivEvidenceDocURL').show();
            $('#errordivEvidenceDocURL').html("Photographs/Document related to offense/crime scene should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#Bfile_CrimeScenePhoto2').focus();
            return false;


        }

        var file = $("#Bfile_CrimeScenePhoto2").val();

        var exts = ['jpeg', 'jpg', 'png', 'gif', 'pdf'];

        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#Bfile_CrimeScenePhoto2').val('');
                $('#errordivEvidenceDocURL').show();
                $('#errordivEvidenceDocURL').html("Please upload only jpeg or jpg or png or gif or pdf file format in Photographs/Document related to offense/crime scene Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#Bfile_CrimeScenePhoto2').focus();
                return false;
            } else {
                $('#errordivEvidenceDocURL').hide();
            }
        }
        else { $('#errordivEvidenceDocURL').hide(); }

    });
    $("#Bfile_CrimeScenePhoto3").change(function (e) {

        var iSize = ($("#Bfile_CrimeScenePhoto3")[0].files[0].size / 1048576);

        if (iSize > 2) {
            $('#Bfile_CrimeScenePhoto3').val('');
            $('#errordivEvidenceDocURL').show();
            $('#errordivEvidenceDocURL').html("Photographs/Document related to offense/crime scene should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#Bfile_CrimeScenePhoto3').focus();
            return false;


        }

        var file = $("#Bfile_CrimeScenePhoto3").val();

        var exts = ['jpeg', 'jpg', 'png', 'gif', 'pdf'];

        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#Bfile_CrimeScenePhoto3').val('');
                $('#errordivEvidenceDocURL').show();
                $('#errordivEvidenceDocURL').html("Please upload only jpeg or jpg or png or gif or pdf file format in Photographs/Document related to offense/crime scene Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#Bfile_CrimeScenePhoto3').focus();
                return false;
            } else {
                $('#errordivEvidenceDocURL').hide();
            }
        }
        else { $('#errordivEvidenceDocURL').hide(); }

    });

    $("#file_CrimeScenePhoto1").change(function (e) {

        var iSize = ($("#file_CrimeScenePhoto1")[0].files[0].size / 1048576);

        if (iSize > 2) {
            $('#file_CrimeScenePhoto1').val('');
            $('#errordivOBCrimeScene').show();
            $('#errordivOBCrimeScene').html("Photographs/Document related to offense/crime scene should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#file_CrimeScenePhoto1').focus();
            return false;
        }
        var file = $("#file_CrimeScenePhoto1").val();
        var exts = ['jpeg', 'jpg', 'png', 'gif', 'pdf'];

        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#file_CrimeScenePhoto1').val('');
                $('#errordivOBCrimeScene').show();
                $('#errordivOBCrimeScene').html("Please upload only jpeg or jpg or png or gif or pdf file format in Photographs/Document related to offense/crime scene Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#file_CrimeScenePhoto1').focus();
                return false;
            } else {
                $('#errordivOBCrimeScene').hide();
            }
        }
        else { $('#errordivOBCrimeScene').hide(); }

    });
    $("#file_CrimeScenePhoto2").change(function (e) {

        var iSize = ($("#file_CrimeScenePhoto2")[0].files[0].size / 1048576);

        if (iSize > 2) {
            $('#file_CrimeScenePhoto2').val('');
            $('#errordivOBCrimeScene').show();
            $('#errordivOBCrimeScene').html("Photographs/Document related to offense/crime scene should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#file_CrimeScenePhoto2').focus();
            return false;
        }

        var file = $("#file_CrimeScenePhoto2").val();

        var exts = ['jpeg', 'jpg', 'png', 'gif', 'pdf'];

        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#file_CrimeScenePhoto2').val('');
                $('#errordivOBCrimeScene').show();
                $('#errordivOBCrimeScene').html("Please upload only jpeg or jpg or png or gif or pdf file format in Photographs/Document related to offense/crime scene Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#file_CrimeScenePhoto2').focus();
                return false;
            } else {
                $('#errordivOBCrimeScene').hide();
            }
        }
        else { $('#errordivOBCrimeScene').hide(); }

    });
    $("#file_CrimeScenePhoto3").change(function (e) {

        var iSize = ($("#file_CrimeScenePhoto3")[0].files[0].size / 1048576);

        if (iSize > 2) {
            $('#file_CrimeScenePhoto3').val('');
            $('#errordivOBCrimeScene').show();
            $('#errordivOBCrimeScene').html("Photographs/Document related to offense/crime scene should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#file_CrimeScenePhoto3').focus();
            return false;


        }

        var file = $("#file_CrimeScenePhoto3").val();

        var exts = ['jpeg', 'jpg', 'png', 'gif', 'pdf'];

        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#file_CrimeScenePhoto3').val('');
                $('#errordivOBCrimeScene').show();
                $('#errordivOBCrimeScene').html("Please upload only jpeg or jpg or png or gif or pdf or pdf file format in Photographs/Document related to offense/crime scene Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#file_CrimeScenePhoto3').focus();
                return false;
            } else {
                $('#errordivOBCrimeScene').hide();
            }
        }
        else { $('#errordivOBCrimeScene').hide(); }

    });

    $(document).on('change', '#ddlOState', function () {
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

    if ($("#hdFormNo").val() == "3") {       
        var rowdata = "";
        var ID = { id: $("#hdFPMOffenseCode").val() }
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(ID),
            url: RootURl + 'OffenseRegistration/GetMultiOffenderDetails',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    //if (data[i].OStateCode == '1') {
                    //    getvillage(data[i].ODistrictCode, data[i].OVillageCode);
                    //}

                    var id = "'" + data[i].OOffenderrowid + "'";
                    rowdata = "<tr class='rowid'><td style=display:none;>" + data[i].OOffenderrowid + "</td><td>" + data[i].OffenderName + "</td><td>" + data[i].OFatherName + "</td><td>" + data[i].OAddress1 + "</td><td>" + data[i].OffenderAge + "</td><td>" + "<button type=button class='btn btn-success btn-circle'  data-toggle=modal data-target=#myModalOffender style=cursor:pointer onclick=ViewOffender(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td>" + "<button type=button class='btn btn-warning btn-circle' style=cursor:pointer onclick=EditOffender(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' style=cursor:pointer onclick=DeleteOffender(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    $("#tb_OffenderInfo").append(rowdata);
                    $("#tbl_Offenderinfo").css("display", "block");
                }
            },
            traditional: true,
            error: function (data) { console.log(data) }
        });
    }

    $('#btnNext').on('click', function () {
        if ($('#ApplicantName').val() == '') {
            $('#errordivName').show();
            $('#errordivName').html("Please enter the Applicant Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ApplicantName').focus();
            return false;
        }
        else { $('#errordivName').hide(); }
        if ($('#ddl_range').val() == '' || $('#ddl_range').val() == null || $('#ddl_range').val() == '0') {
            $('#errordivRange').show();
            $('#errordivRange').html("Please Select the Range!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_range').focus();
            return false;
        }
        else { $('#errordivRange').hide(); }

        if ($('#txt_Naka').val() == '') {
            $('#errordivNaka').show();
            $('#errordivNaka').html("Please enter the Naka!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_Naka').focus();
            return false;
        }
        else { $('#errordivNaka').hide(); }

        if ($('#txt_Compartment').val() == '') {
            $('#errordivCompartment').show();
            $('#errordivCompartment').html("Please enter the Beat!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_Compartment').focus();
            return false;
        }
        else { $('#errordivCompartment').hide(); }
        if ($('#txt_ForestBlock').val() == '') {
            $('#errordivForestBlock').show();
            $('#errordivForestBlock').html("Please enter the Forest Block!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_ForestBlock').focus();
            return false;
        }
        else { $('#errordivForestBlock').hide(); }

        if ($('#txt_Tehsil').val() == '') {
            $('#errordivTehsil').show();
            $('#errordivTehsil').html("Please enter the Tehsil!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_Tehsil').focus();
            return false;
        }
        else { $('#errordivTehsil').hide(); }

        if ($('#txt_OffensePlace').val() == '') {
            $('#errordivOffensePlace').show();
            $('#errordivOffensePlace').html("Please enter the Place of Offense/Landmark!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_OffensePlace').focus();
            return false;
        }
        else { $('#errordivOffensePlace').hide(); }
        if ($('#txt_DateOfOffense').val() == '') {
            $('#errordivDateOfOffense').show();
            $('#errordivDateOfOffense').html("Please select date of Offense!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_DateOfOffense').focus();
            return false;
        }
        else {
            $('#errordivDateOfOffense').hide();
        }
        if ($('#OffenseTime').val() == '') {
            $('#errordivDateOfOffense').show();
            $('#errordivOffenseTime').html("Please select time of Offense!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#OffenseTime').focus();
            return false;
        }
        else {
            $('#errordivOffenseTime').hide();
        }
        if ($('#txt_lat').val() == '') {
            $('#errordivLatitude').show();
            $('#errordivLatitude').html("Please enter the Latitude!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_lat').focus();
            return false;
        }
        else { $('#errordivLatitude').hide(); }

        if (($('#txt_lat').val().substr(0, 1) == '.') && ($('#txt_lat').val().length <= 1)) {
            $('#errordivLatitude').show();
            $('#errordivLatitude').html("Please enter the Latitude greator than 0!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_lat').focus();
            return false;
        }
        else { $('#errordivLatitude').hide(); }

        if ((parseFloat($('#txt_lat').val())) <= 0) {
            $('#errordivLatitude').show();
            $('#errordivLatitude').html("Please enter the Latitude greator than 0!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_lat').focus();
            return false;
        }
        else { $('#errordivLatitude').hide(); }

        if (!(parseInt($('#txt_lat').val().substring(0, 2), 10) >= 23 && parseInt($('#txt_lat').val().substring(0, 2), 10) <= 29)) {
            $('#errordivLatitude').show();
            $('#errordivLatitude').html("Enter the GPS latitude between 23° - 29°!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_lat').focus();
            return false;
        }
        else { $('#errordivLatitude').hide(); }

        if ($('#txt_long').val() == '') {
            $('#errordivLongitude').show();
            $('#errordivLongitude').html("Please enter the Longitude !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_long').focus();
            return false;
        }
        else { $('#errordivLongitude').hide(); }

        if (($('#txt_long').val().substr(0, 1) == '.') && ($('#txt_long').val().length <= 1)) {
            $('#errordivLongitude').show();
            $('#errordivLongitude').html("Please enter the Longitude greator than 0!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_long').focus();
            return false;
        }
        else { $('#errordivLongitude').hide(); }

        if ((parseFloat($('#txt_long').val())) <= 0) {
            $('#errordivLongitude').show();
            $('#errordivLongitude').html("Please enter the Longitude greator than 0!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_long').focus();
            return false;
        }
        else { $('#errordivLongitude').hide(); }

       
        if (!(parseInt($('#txt_long').val().substring(0, 2), 10) >= 67 && parseInt($('#txt_long').val().substring(0, 2), 10) <= 78)) {
            $('#errordivLongitude').show();
            $('#errordivLongitude').html("Enter the GPS longitude between 67° to 78°!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_long').focus();
            return false;
        }
        else { $('#errordivLongitude').hide(); }
        if ($('#txt_LandMark').val() == '') {
            $('#errordivLandMark').show();
            $('#errordivLandMark').html("Please enter the Land Mark!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_LandMark').focus();
            return false;
        }
        else { $('#errordivLandMark').hide(); }

        if ($('#txt_NakaDistance').val() == '') {
            $('#errordivNakaDistance').show();
            $('#errordivNakaDistance').html("Please enter the Distance from NAKA(In Km.) field!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_NakaDistance').focus();
            return false;
        }
        else { $('#errordivNakaDistance').hide(); }

        if (($('#txt_NakaDistance').val().substr(0, 1) == '.') && ($('#txt_NakaDistance').val().length <= 1)) {
            $('#errordivNakaDistance').show();
            $('#errordivNakaDistance').html("Please enter the Distance from NAKA(In Km.) field greator than 0.0001!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_NakaDistance').focus();
            return false;
        }
        else { $('#errordivNakaDistance').hide(); }

        if ((parseFloat($('#txt_NakaDistance').val())) < 0.0001) {
            $('#errordivNakaDistance').show();
            $('#errordivNakaDistance').html("Please enter the Distance from NAKA(In Km.) field greator than 0.0001!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_NakaDistance').focus();
            return false;
        }
        else { $('#errordivNakaDistance').hide(); }

        if ($('#txt_Compartment').val() == '') {
            $('#errordivNaka').show();
            $('#errordivNaka').html("Please enter valid naka!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_Compartment').focus();
            return false;
        }
        else { $('#errordivNaka').hide(); }

        if ($('#txt_ForestBlock').val() == '') {
            $('#errordivBlock').show();
            $('#errordivBlock').html("Please enter valid block!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_ForestBlock').focus();
            return false;
        }
        else { $('#errordivBlock').hide(); }

        if ($('#For_OffenceCategory option:selected').text() == '--Select--') {
            $('#errordivForOffenseCategory').show();
            $('#errordivForOffenseCategory').html("Please select offence category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#For_OffenceCategory').focus();
            return false;
        }
        else { $('#errordivForOffenseCategory').hide(); }
        if ($('#Offence_Description').val() == '') {
            $('#errordivOffenseDesc').show();
            $('#errordivOffenseDesc').html("Please enter valid description!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#Offence_Description').focus();
            return false;
        }
        else { $('#errordivOffenseDesc').hide(); }

        if ($('#ddl_ForestType').val() == '' || $('#ddl_ForestType').val() == null || $('#ddl_ForestType').val() == '0') {
            $('#errordivForestType').show();
            $('#errordivForestType').html("Please Select the Type of Forest!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_ForestType').focus();
            return false;
        }
        else { $('#errordivForestType').hide(); }
    });
    $('#Name').prop('checked', true);
    $('#Self').click(function (e) {
        if ($('#Self').prop('checked')) {
            $('#Name').prop('checked', false);
            $('#ApplicantName').val('Self');
        }
    });
    $('#Name').click(function (e) {
        if ($('#Name').prop('checked')) {
            $('#Self').prop('checked', false);
            $('#ApplicantName').val('');
            $('#ApplicantName').focus();
        }
    });

    $('.btnSubmit').on('click', function () {

        if ($('#ddl_OffenceCategory').val() == '' || $('#ddl_OffenceCategory').val() == null || $('#ddl_OffenceCategory').val() == '0') {
            $('#errordivOffenseCategory').show();
            $('#errordivOffenseCategory').html("Please Select the Offense Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_OffenceCategory').focus();
            return false;
        }
        else { $('#errordivOffenseCategory').hide(); }

        var drpOcategory = $("#ddl_OffenceCategory").val();

        if (drpOcategory == "1") {

            if ($('#ddl_WOffenseSubCategoryForest').val() == '' || $('#ddl_WOffenseSubCategoryForest').val() == null || $('#ddl_WOffenseSubCategoryForest').val() == '0') {
                $('#errordivOWildLife').show();
                $('#errordivOWildLife').html("Please Select the Offense Sub-Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_WOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOWildLife').hide(); }

            if ($('#ddl_WWildlifeProtectionAct').val() == '' || $('#ddl_WWildlifeProtectionAct').val() == null || $('#ddl_WWildlifeProtectionAct').val() == '0') {
                $('#errordivSectionWildLife').show();
                $('#errordivSectionWildLife').html("Please Select the Section under Wildlife Protection Act 1972!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_WWildlifeProtectionAct').focus();
                return false;
            }
            else { $('#errordivSectionWildLife').hide(); }

        }
        else if (drpOcategory == "2") {

            if ($('#ddl_FOffenseSubCategoryForest').val() == '' || $('#ddl_FOffenseSubCategoryForest').val() == null || $('#ddl_FOffenseSubCategoryForest').val() == '0') {
                $('#errordivOFCategory').show();
                $('#errordivOFCategory').html("Please Select the Offense Sub-Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_FOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOFCategory').hide(); }

            if ($('#ddl_FForestProtectionAct').val() == '' || $('#ddl_FForestProtectionAct').val() == null || $('#ddl_FForestProtectionAct').val() == '0') {
                $('#errordivOFSection').show();
                $('#errordivOFSection').html("Please Select the Section under Forest Protection Act 1953!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_FForestProtectionAct').focus();
                return false;
            }
            else { $('#errordivOFSection').hide(); }

        }
        else if (drpOcategory == "3") {

            if ($('#ddl_BWOffenseSubCategoryForest').val() == '' || $('#ddl_BWOffenseSubCategoryForest').val() == null || $('#ddl_BWOffenseSubCategoryForest').val() == '0') {
                $('#errordivOBWCategory').show();
                $('#errordivOBWCategory').html("Please Select Offense Sub-Category (as per rajasthan wildlife ACT)!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BWOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOBWCategory').hide(); }
            if ($('#ddl_BFOffenseSubCategoryForest').val() == '' || $('#ddl_BFOffenseSubCategoryForest').val() == null || $('#ddl_BFOffenseSubCategoryForest').val() == '0') {
                $('#errordivOBFCategory').show();
                $('#errordivOBFCategory').html("Please Select Offense Sub-Category (as per rajasthan forest ACT)!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BFOffenseSubCategoryForest').focus();
                return false;
            }
            else { $('#errordivOBFCategory').hide(); }


            if ($('#ddl_BForestProtectionAct').val() == '' || $('#ddl_BForestProtectionAct').val() == null || $('#ddl_BForestProtectionAct').val() == '0') {
                $('#errordivOBForestSection').show();
                $('#errordivOBForestSection').html("Please Select the Section under Forest Protection Act 1953!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BForestProtectionAct').focus();
                return false;
            }
            else { $('#errordivOBForestSection').hide(); }

            if ($('#ddl_BWildlifeProtectionAct').val() == '' || $('#ddl_BWildlifeProtectionAct').val() == null || $('#ddl_BWildlifeProtectionAct').val() == '0') {
                $('#errordivOBWildSection').show();
                $('#errordivOBWildSection').html("Please Select the Section under Wildlife Protection Act 1972!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ddl_BWildlifeProtectionAct').focus();
                return false;
            }
            else { $('#errordivOBWildSection').hide(); }




        }
        if ($('#ddl_OffenseSeverity').val() == '' || $('#ddl_OffenseSeverity').val() == null || $('#ddl_OffenseSeverity').val() == '0') {
            $('#errordivOSeverity').show();
            $('#errordivOSeverity').html("Please Select the Offense Severity:!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_OffenseSeverity').focus();
            return false;
        }
        else { $('#errordivOSeverity').hide(); }
        if ($('#txt_OPoliceStation option:selected').text() == '--Select--') {
            $('#errordivOPoliceStation').show();
            $('#errordivOPoliceStation').html("Please select the Nearest Police Station!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#txt_OPoliceStation').focus();
            errorCount = "1";
            return false;
        }
        else { $('#errordivOPoliceStation').hide(); errorCount = ""; }



    });


    $('#txt_Naka,#txt_Tehsil,#txt_ForestBlock,#txt_Compartment,#txt_BOffenseSubCategoryForest,#txt_BOffenseSubCategoryWild').keypress(function (e) {
        var kc = e.which;
        if (e.shiftKey) {
            if (kc == 64 || kc == 33 || kc == 35 || kc == 36 || kc == 37 || kc == 94 || kc == 38 || kc == 42 || kc == 40 || kc == 41) {
                e.preventDefault();
            }
        }
        if ((kc >= 97 && kc <= 122) || (kc >= 65 && kc <= 90) || (kc == 0 || kc == 13 || kc == 95 || kc == 37 || kc == 46 || kc == 38) || (kc >= 47 && kc <= 57) || (kc >= 44 && kc < 46) || (kc >= 40 && kc < 42) || (kc >= 96 && kc <= 105) || (kc == 32)) {

        }
        else {
            e.preventDefault();
        }
    });

    $('#txt_OAddress1,#txt_OAddress2,#txt_OVillageCode,#txtdistrict,#txt_OffenderStatement,#OffenderComplainant').keypress(function (e) {
        var kc = e.which;
        if (e.shiftKey) {
            if (kc == 64 || kc == 33 || kc == 35 || kc == 36 || kc == 37 || kc == 94 || kc == 38 || kc == 42 || kc == 40 || kc == 41) {
                e.preventDefault();
            }
        }
        if ((kc >= 97 && kc <= 122) || (kc >= 65 && kc <= 90) || (kc == 0 || kc == 13 || kc == 95 || kc == 37 || kc == 46 || kc == 38) || (kc >= 47 && kc <= 57) || (kc >= 44 && kc < 46) || (kc >= 40 && kc < 42) || (kc >= 96 && kc <= 105) || (kc == 32)) {

        }
        else {
            e.preventDefault();
        }
    });

    $('#txt_OPincode,#OPhoneNo,#txt_OffenderAge,txt_ONumberOfOffender,#Height').keypress(function (event) {

        return isOnlyNumber(event, this)

    });

    $('#txt_lat').keypress(function (event) {

        return isNumber(event, this)

    });

    $('#txt_long').keypress(function (event) {

        return isNumber(event, this)

    });
    $('#txt_NakaDistance').keypress(function (event) {

        return isNumber(event, this)

    });


    $(document).on('change', '#txt_lat', function (event) {
        var str = $('#txt_lat').val();

        var lastChar = str.slice(-1);
        if (lastChar == '.') {
            str = str.slice(0, -1);
        }
        $('#txt_lat').val(str);

        var str1 = str.match(/.{3}/g);

        var length = str1.length;
        for (var i = 0; i < 1; i++) {

            if (str1[i].indexOf(".") == -1) {

                $('#errordivLatitude').show();
                $('#errordivLatitude').html("Please enter the valid Latitude!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_lat').focus();
                $('#txt_lat').val('');
                return false;
                //}
            }
            else {
                $('#errordivLatitude').hide();
            }

            if (str.indexOf(".") == 1 && str.length < 2) {
                $('#errordivLatitude').show();
                $('#errordivLatitude').html("Please enter the valid Latitude!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_lat').focus();
                $('#txt_lat').val('');
                return false;
                //}
            }
            else {
                $('#errordivLatitude').hide();
            }

        }

    })
    $(document).on('change', '#txt_long', function (event) {
        var str = $('#txt_long').val();
        var lastChar = str.slice(-1);
        if (lastChar == '.') {
            str = str.slice(0, -1);

        }
        $('#txt_long').val(str);
        var str1 = str.match(/.{3}/g);

        var length = str1.length;

        for (var i = 0; i < 1; i++) {
            if (str1[i].indexOf(".") == -1) {

                $('#errordivLongitude').show();
                $('#errordivLongitude').html("Please enter the valid Longitude!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_long').focus();
                $('#txt_long').val('');
                return false;

            }
            else {
                $('#errordivLongitude').hide();
            }
            if (str.indexOf(".") == 1 && str.length < 2) {

                $('#errordivLongitude').show();
                $('#errordivLongitude').html("Please enter the valid Longitude!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_long').focus();
                $('#txt_long').val('');
                return false;

            }
            else {
                $('#errordivLongitude').hide();
            }
        }

    });

    $("#btn_addOffender").bind("click", function () {
        var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
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

            if ($('option:selected', $('#txt_OCategory')).index() == 0) {

                $('#errordivOCategory').show();
                $('#errordivOCategory').html("Please select the Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OCategory').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOCategory').hide(); errorCount = ""; }

            if ($('#txt_OCasteName').val() == '') {
                $('#errordivOCaste').show();
                $('#errordivOCaste').html("Please enter the Caste!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OCasteName').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOCaste').hide(); errorCount = ""; }

            if ($('#OClothesWorn').val() == '') {
                $('#errordivCloth').show();
                $('#errordivCloth').html("Please select the Cloth Worn!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#OClothesWorn').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivCloth').hide(); errorCount = ""; }
            if ($('#OColorOfClothes').val() == '') {
                $('#errordivClothColor').show();
                $('#errordivClothColor').html("Please enter the Color of Cloth!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#OColorOfClothes').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivClothColor').hide(); errorCount = ""; }
            if ($('#OPhysicalAppearance').val() == '') {
                $('#errordivPhysicalApp').show();
                $('#errordivPhysicalApp').html("Please enter the physical appearance!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#OPhysicalAppearance').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivPhysicalApp').hide(); errorCount = ""; }

            if ($('#Height').val() == '') {
                $('#errordivHeight').show();
                $('#errordivHeight').html("Please enter the Height!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#Height').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivHeight').hide(); errorCount = ""; }

            if ($('#OOtherSpecialDetails').val() == '') {
                $('#OtherSpecialDetails').show();
                $('#OtherSpecialDetails').html("Please enter the special details!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#OOtherSpecialDetails').focus();
                errorCount = "1";
                return false;
            }
            else { $('#OtherSpecialDetails').hide(); errorCount = ""; }

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
            if ($('#txt_OPincode').val() == '') {
                $('#errordivOPincode').show();
                $('#errordivOPincode').html("Please enter the Pin Code!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OPincode').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOPincode').hide(); errorCount = ""; }                        
            if (!pattern.test($('#txt_OEmailID').val()) && $('#txt_OEmailID').val() != '') {
                $('#errordivOEmailID').show();
                $('#errordivOEmailID').html("Please enter valid E-mail ID!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OEmailID').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOEmailID').hide(); errorCount = ""; }

            if ($('#txt_OffenderStatement').val() == '') {
                $('#errordivOffenderStatement').show();
                $('#errordivOffenderStatement').html("Please enter the Offender statement!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OffenderStatement').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOffenderStatement').hide(); errorCount = ""; }

            if ($('#txt_OffenderAge').val() == '') {
                $('#errordivOffenderAge').show();
                $('#errordivOffenderAge').html("Please enter the Age of Offender!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#txt_OffenderAge').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivOffenderAge').hide(); errorCount = ""; }

            if ($('#ArrestedOrdetained option:selected').text() == '--Select--') {
                $('#errordivArrested').show();
                $('#errordivArrested').html("Please select Offender arrested/detained!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#ArrestedOrdetained').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivArrested').hide(); errorCount = ""; }

            if ($('#InformToOffenderRelative option:selected').text() == '--Select--' && $('#ArrestedOrdetained option:selected').val() == 'Arrested') {
                $('#errordivRelative').show();
                $('#errordivRelative').html("Please select if inform to Offender's relative !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#InformToOffenderRelative').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivRelative').hide(); errorCount = ""; }
            if ($('#CommunicationMode option:selected').text() == '--Select--' && $('#ArrestedOrdetained option:selected').val() == 'Arrested') {
                $('#errordivCommMode').show();
                $('#errordivCommMode').html("Please select communication mode!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#CommunicationMode').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivCommMode').hide(); errorCount = ""; }

            if ($('#CommunicationDate').val() == '' && $('#ArrestedOrdetained option:selected').val() == 'Arrested') {
                $('#errordivCommDate').show();
                $('#errordivCommDate').html("Please enter communication date !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#CommunicationDate').focus();
                errorCount = "1";
                return false;
            }
            else { $('#errordivCommDate').hide(); errorCount = ""; }
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

    $('#ArrestedOrdetained').change(function (e) {

        if ($('#ArrestedOrdetained option:selected').val() == 'Arrested') {
            $('.arrested').show();
        }
        else {
            $('#InformToOffenderRelative option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
            $('#CommunicationMode option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
            $('#CommunicationDate,#FardGriftri,#GriftariPunchnama,#NagriNaka,#JamaTalashi,#MedicalReport').val('');
            $('#hdFardGriftriDoc,#hdGriftariPunchnamaDoc,#hdNagriNakaDoc,#hdJamaTalashiDoc,#hdMedicalReportDoc').val('');
            $('.arrested').hide();
        }

    });

    $('#btnUploadFardGriftri').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#FardGriftri").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#FardGriftri').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'OffenseRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdFardGriftriDoc").val(result.list2);
                        $('#FardGriftri').val('');
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

    $("#FardGriftri").change(function (e) {
     //   var iSize = ($("#FardGriftri")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#FardGriftri")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#FardGriftri').val('');
            $('#errordivFardGriftriDoc').show();
            $('#errordivFardGriftriDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#FardGriftri').focus();
            return false;
        }
        var file = $("#FardGriftri").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#FardGriftri').val('');
                $('#errordivFardGriftriDoc').show();
                $('#errordivFardGriftriDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#FardGriftri').focus();
                return false;
            } else {
                $('#errordivFardGriftriDoc').hide();
            }
        }
        else { $('#errordivFardGriftriDoc').hide(); }

    });

    $('#btnGriftariPunchnama').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#GriftariPunchnama").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#GriftariPunchnama').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'OffenseRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdGriftariPunchnamaDoc").val(result.list2);
                        $('#GriftariPunchnama').val('');
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

    $("#GriftariPunchnama").change(function (e) {
        //var iSize = ($("#GriftariPunchnama")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#GriftariPunchnama")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#GriftariPunchnama').val('');
            $('#errordivGriftariPunchnamaDoc').show();
            $('#errordivGriftariPunchnamaDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#GriftariPunchnama').focus();
            return false;
        }
        var file = $("#GriftariPunchnama").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#GriftariPunchnama').val('');
                $('#errordivGriftariPunchnamaDoc').show();
                $('#errordivGriftariPunchnamaDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#GriftariPunchnama').focus();
                return false;
            } else {
                $('#errordivGriftariPunchnamaDoc').hide();
            }
        }
        else { $('#errordivGriftariPunchnamaDoc').hide(); }

    });

    $('#btnUploadNagriNaka').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#NagriNaka").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#NagriNaka').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'OffenseRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdNagriNakaDoc").val(result.list2);
                        $('#NagriNaka').val('');
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

    $("#NagriNaka").change(function (e) {
       // var iSize = ($("#NagriNaka")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#NagriNaka")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#NagriNaka').val('');
            $('#errordivNagriNakaDoc').show();
            $('#errordivNagriNakaDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
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

    $('#btnUploadJamaTalashi').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#JamaTalashi").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#JamaTalashi').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'OffenseRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdJamaTalashiDoc").val(result.list2);
                        $('#JamaTalashi').val('');
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

    $("#JamaTalashi").change(function (e) {
       // var iSize = ($("#JamaTalashi")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#JamaTalashi")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#JamaTalashi').val('');
            $('#errordivJamaTalashiDoc').show();
            $('#errordivJamaTalashiDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#JamaTalashi').focus();
            return false;
        }
        var file = $("#JamaTalashi").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#JamaTalashi').val('');
                $('#errordivJamaTalashiDoc').show();
                $('#errordivJamaTalashiDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#JamaTalashi').focus();
                return false;
            } else {
                $('#errordivJamaTalashiDoc').hide();
            }
        }
        else { $('#errordivJamaTalashiDoc').hide(); }

    });


    $('#btnUploadMedicalReport').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#MedicalReport").get(0);
            var files = fileUpload.files;

            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#MedicalReport').val() == '') {

                alert('Select file to upload!!!');
            }
            else {
                $.ajax({
                    url: RootURl + 'OffenseRegistration/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdMedicalReportDoc").val(result.list2);
                        $('#MedicalReport').val('');
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

    $("#MedicalReport").change(function (e) {
       // var iSize = ($("#MedicalReport")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#MedicalReport")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#MedicalReport').val('');
            $('#errordivMedicalReportDoc').show();
            $('#errordivMedicalReportDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#MedicalReport').focus();
            return false;
        }
        var file = $("#MedicalReport").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#MedicalReport').val('');
                $('#errordivMedicalReportDoc').show();
                $('#errordivMedicalReportDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#MedicalReport').focus();
                return false;
            } else {
                $('#errordivMedicalReportDoc').hide();
            }
        }
        else { $('#errordivMedicalReportDoc').hide(); }

    });



    $('#ddlODistrict').change(function (e) {
        $("#txt_OVillageCode").empty();
        $("#txt_OVillageCode").append('<option value="' + '' + '">' +
                '--Select--' + '</option>');
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistration/getVillage', // we are calling json method
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

    $('#Addknown-Offender').on('click', function () {
        $(".Unknown").hide();
        $(".known").show();

    });

    $('#Unknown-Offender').on('click', function () {
        $(".known").hide();
        $(".Unknown").show();

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
    //var A = ["C", "D", "E", "F", "G"];
    //var B = ["3", "0", "4", "1", "2"];
    //sort(A, B);
});

//function sort(A, B) {
  
  //  $.each(B, function (i,val) {

       // alert(A.indexOf("C"));
   //     if (val == A.indexOf()) {
   //         A[val] = A[i];
   //     }
        //if (val == 1) {
        //    A[val] = "F";
        //}
        //if (val == 2) {
        //    A[val] = "G";
        //}
        //if (val == 3) {
        //    A[val] = "C";
        //}
        //if (val == 4) {
        //    A[val] = "E";
        //}       
  //  });

  //  alert(A);
        
 // }
    



function getvillage(distcode, villcode) {

    $.ajax({
        type: 'POST',
        url: RootUrl + 'OffenseRegistration/getVillage', // we are calling json method
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

function isOnlyNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode == 8) { return true; }
    if (charCode < 48 || charCode > 57)
        return false;
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
        OSpouseName: $("#txt_OSpouseName").val(),
        OCategory: $("#txt_OCategory").val(),
        OCaste: $("#txt_OCasteName").val(),
        ClothesWorn: $("#OClothesWorn").val(),
        ColorOfClothes: $("#OColorOfClothes").val(),
        PhysicalAppearance: $("#OPhysicalAppearance").val(),
        Height: $("#Height").val(),
        OtherSpecialDetails: $("#OOtherSpecialDetails").val(),
        OAddress1: $("#txt_OAddress1").val(),
        OAddress2: $("#txt_OAddress2").val(),
        OStateCode: $("#ddlOState option:selected").val(),
        ODistrictCode: oDistrict,
        OPincode: $("#txt_OPincode").val(),
        OVillageCode: oVillage,
        OPhoneNo: $("#OPhoneNo").val(),
        OEmailID: $("#txt_OEmailID").val(),
        OPhotoIDType: $("#ddl_OPhotoIDType option:selected").val(),
        OffenderAge: $("#txt_OffenderAge").val(),
        OPhotoIDURL: $("#hdIDProof").val(),
        ArrestedOrdetained: $('#ArrestedOrdetained option:selected').val(),
        InformToOffenderRelative: $('#InformToOffenderRelative option:selected').val(),
        CommunicationMode: $('#CommunicationMode option:selected').val(),
        CommunicationDate: $('#CommunicationDate').val(),
        OffenderStatement: $('#txt_OffenderStatement').val(),
        OffenderStatementDoc: $('#hdOffenderStatementDoc').val(),
        FardGriftri: $('#hdFardGriftriDoc').val(),
        GriftariPunchnama: $('#hdGriftariPunchnamaDoc').val(),
        NagriNaka: $('#hdNagriNakaDoc').val(),
        JamaTalashi: $('#hdJamaTalashiDoc').val(),
        MedicalReport: $('#hdMedicalReportDoc').val()
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(offenderInfo),
        url: RootURl + 'OffenseRegistration/OffenderData',
        success: function (data) {
            if (data.OffenderName != '') {
                var id = "'" + data.OOffenderrowid + "'";
                // Delete Option Commented// var rowdata = "<tr><td>" + count + "</td><td style=display:none;>" + data.OOffenderrowid + "</td><td>" + data.OffenderName + "</td><td>" + data.OFatherName + "</td><td>" + data.OCaste + "</td><td>" + data.OVillageCode + "</td><td style='border:1px'>" + "<button type=button class=get-view style=cursor:pointer onclick=EditOffender(" + id + ")>Edit</button>" + "</td><td>" + "<button class='get-delete' value='" + data.OEmailID + "'>Delete</button>" + "</td></tr>";
                var rowdata = "<tr><td style=display:none;>" + data.OOffenderrowid + "</td><td>" + data.OffenderName + "</td><td>" + data.OFatherName + "</td><td>" + data.OAddress1 + "</td><td>" + data.OffenderAge + "</td><td>" + "<button type=button class='btn btn-success btn-circle'  data-toggle=modal data-target=#myModalOffender style=cursor:pointer onclick=ViewOffender(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td>" + "<button type=button class='btn btn-warning btn-circle' style=cursor:pointer onclick=EditOffender(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' style=cursor:pointer onclick=DeleteOffender(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $('#tbl_Offenderinfo tbody tr td:nth-child(1)').each(function () {
                    if (data.OOffenderrowid == $(this).text()) {
                        $(this).closest('tr').remove();
                    }
                });
                $("#tb_OffenderInfo").append(rowdata);
                $('#txt_OffenderName').val(''); $('#txt_OFatherName').val('');
                $('#txt_OSpouseName').val(''); $('#txt_OCasteName').val('');
                $('#OClothesWorn').val(''); $('#OColorOfClothes').val('');
                $('#OPhysicalAppearance').val(''); $('#Height').val('');
                $('#OOtherSpecialDetails').val(''); $('#txt_OAddress1').val(''); $('#txt_OAddress2').val('');
                $('#hdnOOffenderrowid').val(''); $('#txt_OVillageCode').val(''); $('#txt_OPincode').val('');
                $('#OPhoneNo').val(''); $('#txt_OEmailID').val(''); $('#txt_OffenderAge').val('');
                $("#txt_OCategory option").filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#ddlOState option').filter(function (e) { return $(this).text() == '---Select---' }).attr('selected', true);
                $('#ddlODistrict option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#txt_OVillageCode option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#ddl_OPhotoIDType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#ArrestedOrdetained option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#InformToOffenderRelative option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#CommunicationMode option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                $('#CommunicationDate').val(''); $('#txt_OffenderStatement').val('');
                $('#viewdoc,#viewdoc1,#viewdoc2,#viewdoc3,#viewdoc4,#viewdoc5,#viewdoc6').hide();
                $("#txtdistrict,#txtvillage").val('');
            }
            $("#tbl_Offenderinfo").css("display", "block");
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $(document).on('click', '.get-delete', function (e) {

        jQuery(this).closest('tr').remove();
        var RowInfo = {
            emailId: $(this).val()
        };
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(RowInfo),
            url: RootURl + 'OffenseRegistration/DeleteOffenderData',
            success: function (data) {
                alert("Offender Saved Successfully!!!!");
            },
            error: function (ex) { alert(ex); }

        });
        e.preventDefault();
    });

}
function isNumber(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
         (charCode != 46 || $(element).val().indexOf('.') != -1) &&      // “.” CHECK DOT, AND ONLY ONE.
        (charCode < 48 || charCode > 57))
        return false;
}
function validateEmail($email) {
    var emailReg = /^([\w-\.]+@@([\w-]+\.)+[\w-]{2,4})?$/;
    if (!emailReg.test($email)) {
        return false;
    } else {
        return true;
    }
}

function EditOffender(ID) {
    $('#hdnOOffenderrowid').val(ID);
    var tblinfo = {
        ID: ID
    }
    $.ajax({
        type: 'POST',
        url: RootUrl + 'OffenseRegistration/EditDetails',
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

            var filename1 = data.OffenderStatementDoc;
            if (data.OffenderStatementDoc != '' && data.OffenderStatementDoc != null) {
                var str1 = filename1.split('/');
                $('#viewdoc1').show();
                $('#viewdoc1').attr('href', '.././ForestProtectionDocument/OffenderDetails/' + str1[3]);
                $('#viewdoc1').attr('target', '_blank');
            }
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
        url: RootUrl + 'OffenseRegistration/EditDetails',
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

            alert(data.OStateCode);
            var state = "";
            if (data.OStateCode == 1) {
                state = "Rajasthan";
            }
            else {
                state = "Others";
            }

            var bardata = "<tr><td>Offender Name</td><td>" + data.OffenderName +
            "</td></tr><tr><td>Father Name</td><td>" + data.OFatherName +
            "</td></tr><tr><td>Spouse Name</td><td>" + data.OSpouseName +
            "</td></tr><tr><td>Category</td><td>" + category +
            "</td></tr><tr><td>Caste</td><td>" + data.OCaste +
            "</td></tr><tr><td>Clothes Worn</td><td>" + data.OClothesWorn +
            "</td></tr><tr><td>Color Of Clothes</td><td>" + data.OColorOfClothes +
            "</td></tr><tr><td>Physical Appearance</td><td>" + data.OPhysicalAppearance +
            "</td></tr><tr><td>Height</td><td>" + data.OHeight +
            "</td></tr><tr><td>Other Special Details</td><td>" + data.OOtherSpecialDetails +
            "</td></tr><tr><td>Address1</td><td>" + data.OAddress1 +
            "</td></tr><tr><td>Address2</td><td>" + data.OAddress2 +
            "</td></tr><tr><td>District</td><td>" + district +
            "</td></tr><tr><td>Village</td><td>" + village +
            "</td></tr><tr><td>State</td><td>" + state +
            "</td></tr><tr><td>Pincode</td><td>" + data.OPincode +
            "</td></tr><tr><td>PhoneNo</td><td>" + data.OPhoneNo +
            "</td></tr><tr><td>EmailId</td><td>" + data.OEmailID +
            "</td></tr><tr><td>Offender Age</td><td>" + data.OffenderAge +
            "</td></tr><tr><td>Offender Statement</td><td>" + data.OffenderStatement +
            "</td></tr><tr><td>Arrested/detained</td><td>" + data.ArrestedOrdetained +
            "</td></tr><tr><td>Information given to offender's relative</td><td>" + data.InformToOffenderRelative +
            "</td></tr><tr><td>Communication mode</td><td>" + data.CommunicationMode +
            "</td></tr><tr><td>Communication date</td><td>" + data.CommunicationDate +
            "</td></tr>";

            var filename = data.OPhotoIDURL;
            if (data.OPhotoIDURL != '' && data.OPhotoIDURL != null) {
                var str = filename.split('/');
                var IdProof = "</td></tr><tr><td>Identity Proof</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str = '';
                var IdProof = "</td></tr><tr><td>Identity Proof</td><td>N/A</td></tr>";
            }
            var filename1 = data.OffenderStatementDoc;
            if (data.OffenderStatementDoc != '' && data.OffenderStatementDoc != null) {
                var str1 = filename1.split('/');
                var OffenderStatementDoc = "</td></tr><tr><td>OffenderStatement Upload File</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str1[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str1 = '';
                var OffenderStatementDoc = "</td></tr><tr><td>OffenderStatement Upload File</td><td>N/A</td></tr>";
            }

            if (data.FardGriftri != '' && data.FardGriftri != null) {
                var filename2 = data.FardGriftri;
                var str2 = filename2.split('/');
                var FardGriftri = "</td></tr><tr><td>Fard Griftari File</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str2[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str2 = '';
                var FardGriftri = "</td></tr><tr><td>Fard Griftari Upload File</td><td>N/A</td></tr>";
            }
            if (data.GriftariPunchnama != '' && data.GriftariPunchnama != null) {
                var filename3 = data.GriftariPunchnama;
                var str3 = filename3.split('/');
                var GriftariPunchnama = "</td></tr><tr><td>Griftari Punchnama File</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str3[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str3 = '';
                var GriftariPunchnama = "</td></tr><tr><td>Griftari Punchnama File</td><td>N/A</td></tr>";
            }
            if (data.NagriNaka != '' && data.NagriNaka != null) {
                var filename4 = data.NagriNaka;
                var str4 = filename4.split('/');
                var NagriNaka = "</td></tr><tr><td>Nagri Naka File</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str4[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str3 = '';
                var NagriNaka = "</td></tr><tr><td>Nagri Naka Upload File</td><td>N/A</td></tr>";
            }
            if (data.JamaTalashi != '' && data.JamaTalashi != null) {
                var filename5 = data.JamaTalashi;
                var str5 = filename5.split('/');
                var JamaTalashi = "</td></tr><tr><td>Jama Talashi File</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str5[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str4 = '';
                var JamaTalashi = "</td></tr><tr><td>Jama Talashi File</td><td>N/A</td></tr>";
            }
            if (data.MedicalReport != '' && data.MedicalReport != null) {
                var filename6 = data.MedicalReport;
                var str6 = filename6.split('/');
                var MedicalReport = "</td></tr><tr><td>Medical Report File</td><td><a href='.././ForestProtectionDocument/OffenderDetails/" + str6[3] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>";
            }
            else {
                var str6 = '';
                var MedicalReport = "</td></tr><tr><td>Medical Report File</td><td>N/A</td></tr>";
            }
            $("#tbdyOffender").append(bardata + IdProof + OffenderStatementDoc + FardGriftri + GriftariPunchnama + NagriNaka + JamaTalashi + MedicalReport);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
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
        url: RootUrl + 'OffenseRegistration/DeleteOffenderData',
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
function reload() {
    location.reload(true);
}







  

