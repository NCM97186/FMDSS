$(document).ready(function () {
    $(document).on('click', '.btnDeleteDocument', function () {
        RemoveDocument(this);
    });  
});



function RemoveDocument(cntrl) {
    if (confirm("Are you sure you want to delete this?")) {
        var tempID = $(cntrl).closest('tr').find('td:first').text();
        //alert(tempID);
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/RemoveUplodedFile",
            dataType: 'json',
            data: { TempID: tempID }
        }).done(function (response) {
            if (!response.isError) {
                $(cntrl).closest('tr').remove();
            }
            alert(response.returnMsg);

        });
    }
}

function UploadData(docType) {
    if (window.FormData !== undefined) {
        var fileUpload = null;
        var cntrlID = "";
        var cntrlTblID = '';
        var isValid = true;
        var documentLevel = 1;
        var docTypeText = '';

        switch (docType) {
            case 1: fileUpload = $("#fuKhasraCompartment").get(0);
                cntrlID = "fuKhasraCompartment";
                cntrlTblID = "tblKhasraCompartmentFiles"; break;
            case 2: fileUpload = $("#fuMemberDetails").get(0);
                cntrlID = "fuMemberDetails";
                cntrlTblID = "tblMemberDetailsFiles"; break;
            case 3: fileUpload = $("#fuBorderVillage").get(0);
                cntrlID = "fuBorderVillage";
                cntrlTblID = "tblBorderVillage"; break;
            case 21: fileUpload = $("#fuScheduledTribe").get(0);
                cntrlID = "fuScheduledTribe";
                cntrlTblID = "tblScheduledTribeFiles"; break;
            case 22: fileUpload = $("#fuForestDweller").get(0);
                cntrlID = "fuForestDweller";
                cntrlTblID = "tblForestDwellerFiles"; break;
            case 23: fileUpload = $("#fuMemberDetails").get(0);
                cntrlID = "fuMemberDetails";
                cntrlTblID = "tblMemberDetailsFiles"; break;
            case 25: fileUpload = $("#fuSurveyEvidence").get(0);
                cntrlID = "fuSurveyEvidence";
                cntrlTblID = "tblSurveyFiles"; break;
            default:
                if ($('#ddlAdditionalEvidenceTypeID').val() != '') {
                    fileUpload = $("#fuAdditionalEvidence").get(0);
                    cntrlID = "fuAdditionalEvidence";
                    cntrlTblID = "tblAdditionalEvidence";
                    docType = $('#ddlAdditionalEvidenceTypeID>option:selected').val(); 
                    if ($('#ddlClaimTypeId').val() == '1') {
                        documentLevel = 4;//common
                    }
                    else {
                        documentLevel = 0;//common
                    }
                }
                else {
                    isValid = false;
                }
                break;

        }
        if (isValid) {
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
                fileData.append("docTypeID", docType);
                fileData.append("documentLevel", documentLevel);
            }
            if ($('#' + cntrlID).val() != '') {
                var size = GetFileSize(cntrlID);

                if (size > 2) {
                    alert('You can upload file up to 2 MB');
                    return false;
                }
                else {
                    var rowdata = "";

                    $.ajax({
                        url: rootURl + "ClaimRequest/UploadFiles",
                        type: "POST",
                        contentType: false, // Not to set any content header
                        processData: false, // Not to process data
                        data: fileData,
                        success: function (response) { 
                            alert(response.returnMsg); 
                            if (!response.isError) {
                                $("#" + cntrlTblID).empty();
                                var totalRows = $("#" + cntrlTblID).find('tr').length;
                                if (response.data.length > 0) {
                                    if (documentLevel == 0 || documentLevel == 4) {
                                        $.each(response.data, function (i, item) {
                                            rowdata += "<tr><td style=display:none;>" + item.TempID + "<td>" + (i + 1) + "</td><td name='dTypeName'>" + response.data[i].DocumentTypeName + "</td><td><a href='" + rootURl + item.DocumentPath + "' target='_blank'><img src='../images/jpeg.png' Width='30' /></a>" + item.DocumentName + "</td><td style='width:5%'>" + "<button type=button class='btn btn-danger btn-circle btnDeleteDocument' style=cursor:pointer ><i class='fa fa-times'></i></button>" + "</td></tr>";
                                        });
                                    }
                                    else {
                                        $.each(response.data, function (i, item) {
                                            rowdata += "<tr><td style=display:none;>" + item.TempID + "<td>" + (i + 1) + "</td><td><a href='" + rootURl + item.DocumentPath + "' target='_blank'><img src='../images/jpeg.png' Width='30' /></a>" + item.DocumentName + "</td><td style='width:5%'>" + "<button type=button class='btn btn-danger btn-circle btnDeleteDocument' style=cursor:pointer ><i class='fa fa-times'></i></button>" + "</td></tr>";
                                        });
                                    }
                                }
                                $("#" + cntrlTblID).append(rowdata);
                            }
                        },
                        error: function (err) {
                            alert(err.statusText);
                        }
                    });
                }
            }
            else { alert("Select file to upload"); }
        }
        else {
            alert("Please select evidence type");
            $('#' + cntrlID).val(''); return false;
        }
    } else {
        alert("FormData is not supported.");
    }
}

function GetFileSize(fileid) {
    try {
        var fileSize = 0;
        fileSize = $("#" + fileid)[0].files[0].size //size in kb
        fileSize = fileSize / 1048576; //size in mb
        return fileSize;
    }
    catch (e) {
        alert("Error is :" + e);
    }
}

function AddClaimantDetails() {
    var bhamashahId = $('#txtClaimantBhamashahID').val().trim();
    var name = $('#txtClaimantName').val().trim();
    var fatherName = $('#txtFatherName').val().trim();
    var gender = $('#ddlGender').val().trim();
    var spouseName = $('#txtSpouseName').val().trim();
    var email = $('#txtEmail').val().trim();
    var mobile = $('#txtMobile').val().trim();
    var isValid = true; 
    ValidateClaimantData();

    if (isValid) {
        $('#dtClaimantDetails tr').each(function () {
            var cname = $(this).find(".CloneClaimantName").html();
            var cfname = $(this).find('.CloneFatherName').html();
            if (cname == name && cfname == fatherName) {
                alert("The Claimant Name already exits.");
                isValid = false;
            }
        });
    } 

    if (isValid) {
        $('#dtClaimantDetails').append($('#tmpClaimantDetails tr').clone());

        $('#dtClaimantDetails tr:last').each(function (i, row) {
            $(row).find('.CloneBhamashaID').html(bhamashahId);
            $(row).find('.CloneClaimantName').html(name);
            $(row).find('.CloneFatherName').html(fatherName);
            $(row).find('.CloneGender').html(gender);
            $(row).find('.CloneSpouseName').html(spouseName);
            $(row).find('.CloneEmail').html(email);
            $(row).find('.CloneMobile').html(mobile);

            $(row).find('.CloneAction').find('input[name="tmpCloneBhamashaID"]').attr('value', bhamashahId);
            $(row).find('.CloneAction').find('input[name="tmpCloneClaimantName"]').attr('value', name);
            $(row).find('.CloneAction').find('input[name="tmpCloneFatherName"]').attr('value', fatherName);
            $(row).find('.CloneAction').find('input[name="tmpCloneGender"]').attr('value', gender);
            $(row).find('.CloneAction').find('input[name="tmpCloneSpouseName"]').attr('value', spouseName);
            $(row).find('.CloneAction').find('input[name="tmpCloneEmail"]').attr('value', email);
            $(row).find('.CloneAction').find('input[name="tmpCloneMobile"]').attr('value', mobile);
        });

        UpdateClaimantElements();
        $('#divClamantDetails').find('input[type="text"]').val('');
    }

    function ValidateClaimantData() {
        if ($.isEmptyObject(name)) {
            alert("Please enter name"); isValid = false;
        }
        else if ($.isEmptyObject(fatherName)) {
            alert("Please enter father name"); isValid = false;
        }
        else if ($.isEmptyObject(mobile)) {
            alert("Please enter mobile"); isValid = false;
        }
        else if (!$.isEmptyObject(email) && !IsValidEmail(email)) {
            alert("Please enter valid email"); isValid = false;
        }
        else if (mobile.length < 10) {
            alert("Please enter valid mobile"); isValid = false;
        }
    }
}

function AddMemberDetails() {
    var bhamashahId = $('#txtMemberBhamashahID').val().trim();
    var name = $('#txtMemberName').val().trim();
    var fatherName = $('#txtMFatherName').val().trim();
    var gender = $('#ddlMGender').val().trim();
    var age = $('#txtMAge').val().trim();
    var email = $('#txtMEmail').val().trim();
    var mobile = $('#txtMMobile').val().trim();
    var isDependant = $('#ddlMIsDependant').val().trim();
    var isValid = true;
    ValidateMemberDetailsData();

    if (isValid) {
        $('#dtMemberDetails').append($('#tmpMemberDetails tr').clone());

        $('#dtMemberDetails tr:last').each(function (i, row) {
            $(row).find('.CloneBhamashaID').html(bhamashahId);
            $(row).find('.CloneMemberName').html(name);
            $(row).find('.CloneFatherName').html(fatherName);
            $(row).find('.CloneGender').html(gender);
            $(row).find('.CloneAge').html(age);
            $(row).find('.CloneEmail').html(email);
            $(row).find('.CloneMobile').html(mobile);
            $(row).find('.CloneIsDependant').html(isDependant);

            $(row).find('.CloneAction').find('input[name="tmpCloneBhamashaID"]').attr('value', bhamashahId);
            $(row).find('.CloneAction').find('input[name="tmpCloneMemberName"]').attr('value', name);
            $(row).find('.CloneAction').find('input[name="tmpCloneFatherName"]').attr('value', fatherName);
            $(row).find('.CloneAction').find('input[name="tmpCloneGender"]').attr('value', gender);
            $(row).find('.CloneAction').find('input[name="tmpCloneAge"]').attr('value', age);
            $(row).find('.CloneAction').find('input[name="tmpCloneEmail"]').attr('value', email);
            $(row).find('.CloneAction').find('input[name="tmpCloneMobile"]').attr('value', mobile);
            $(row).find('.CloneAction').find('input[name="tmpCloneIsDependant"]').attr('value', isDependant);
        });

        UpdateMemberElements();
        $('#divMemberDetails').find('input[type="text"]').val('');
    }

    function ValidateMemberDetailsData() {
        if ($.isEmptyObject(name)) {
            alert("Please enter name"); isValid = false;
        }
        else if ($.isEmptyObject(fatherName)) {
            alert("Please enter father name"); isValid = false;
        }
        else if ($.isEmptyObject(mobile)) {
            alert("Please enter mobile"); isValid = false;
        }
        else if (!$.isEmptyObject(email) && !IsValidEmail(email)) {
            alert("Please enter valid email"); isValid = false;
        }
        else if (mobile.length < 10) {
            alert("Please enter valid mobile"); isValid = false;
        }
    }
}

function AddGramSabhaMemberDetails() { 
    var bhamashahId = $('#txtMemberBhamashahID').val().trim();
    var name = $('#txtMemberName').val().trim();
    var fatherName = $('#txtMFatherName').val().trim();
    var gender = $('#ddlMGender').val().trim();
    var age = $('#txtMAge').val().trim();
    var email = $('#txtMEmail').val().trim();
    var mobile = $('#txtMMobile').val().trim();
    var isValid = true;

    ValidateMemberDetailsData();

    if (isValid) {
        $('#dtGramSabhaMemberDetails').append($('#tmpMemberGramSabha tr').clone());

        $('#dtGramSabhaMemberDetails tr:last').each(function (i, row) {
            $(row).find('.CloneBhamashaID').html(bhamashahId);
            $(row).find('.CloneMemberName').html(name);
            $(row).find('.CloneFatherName').html(fatherName);
            $(row).find('.CloneGender').html(gender);
            $(row).find('.CloneAge').html(age);
            $(row).find('.CloneEmail').html(email);
            $(row).find('.CloneMobile').html(mobile);

            $(row).find('.CloneAction').find('input[name="tmpCloneBhamashaID"]').attr('value', bhamashahId);
            $(row).find('.CloneAction').find('input[name="tmpCloneMemberName"]').attr('value', name);
            $(row).find('.CloneAction').find('input[name="tmpCloneFatherName"]').attr('value', fatherName);
            $(row).find('.CloneAction').find('input[name="tmpCloneGender"]').attr('value', gender);
            $(row).find('.CloneAction').find('input[name="tmpCloneAge"]').attr('value', age);
            $(row).find('.CloneAction').find('input[name="tmpCloneEmail"]').attr('value', email);
            $(row).find('.CloneAction').find('input[name="tmpCloneMobile"]').attr('value', mobile);
        });

        UpdateGramSabhaMemberElements(); 
        $('#divGSMemberDetails').find('input[type="text"]').val('');
    }

    function ValidateMemberDetailsData() {
        if ($.isEmptyObject(name)) {
            alert("Please enter name"); isValid = false;
        }
        else if ($.isEmptyObject(fatherName)) {
            alert("Please enter father name"); isValid = false;
        }
        else if ($.isEmptyObject(mobile)) {
            alert("Please enter mobile"); isValid = false;
        }
        else if (!$.isEmptyObject(email) && !IsValidEmail(email)) {
            alert("Please enter valid email"); isValid = false;
        }
        else if (mobile.length < 10) {
            alert("Please enter valid mobile"); isValid = false;
        }
    }
}

function AddBordingVillage() {
    var villID = $('#ddlVillageCodeForBV').val().trim();
    var villName = $('#ddlVillageCodeForBV>option:selected').text().trim();
    var isValid = true;

    ValidateBordingVillageData();

    if (isValid) {
        $('#dtBVillageDetails').append($('#tmpBorderingVillage tr').clone());

        $('#dtBVillageDetails tr:last').each(function (i, row) {
            $(row).find('.CloneVillageCode').html(villID);
            $(row).find('.CloneVillageName').html(villName);

            $(row).find('.CloneAction').find('input[name="tmpCloneVillageCode"]').attr('value', villID);
        });

        UpdateBorderingVillageElements();
        $('#divBVillageDetails').find('select').val('');
    }

    function ValidateBordingVillageData() {
        if ($.isEmptyObject(villID)) {
            alert("Please enter bordering village details"); isValid = false;
        }
        else {
            $('#dtBVillageDetails tr>td.CloneVillageCode').each(function () {
                if ($(this).text().trim().toUpperCase() == villID.toUpperCase()) {
                    alert("Bordering details already exist.");
                    isValid = false;
                }
            })
        }
    }
}

function BackToList(controllerName, actionName) {
    window.location.href = rootURl + controllerName + '/' + actionName;
}

function DeleteClaimantDetails(cntrl) {
    if (confirm('Are you sure you want to remove?')) {
        $(cntrl).closest('tr').remove();
        UpdateClaimantElements();
    }
}

function DeleteMemberDetails(cntrl) {
    if (confirm('Are you sure you want to remove?')) {
        $(cntrl).closest('tr').remove();
        UpdateMemberElements();
    }
}

function DeleteBorderingVillage(cntrl) {
    if (confirm('Are you sure you want to remove?')) {
        $(cntrl).closest('tr').remove();
        UpdateBorderingVillageElements();
    }
}

function GetClaimRequestPupose(claimTypeID) {
    if (claimTypeID == 2) {
        var strPurpose = '<option value="">--Select--</option>';
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetClaimRequestPupose",
            dataType: 'json',
            async: true,
            data: { "claimTypeID": claimTypeID },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strPurpose += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                    $("#ddlClaimRequestPurpose").html(strPurpose);
                }
            }
        });
    }
}

function GetDetails(type) {
    $('#hdnCurrentBhamashahActionValue').val(type);
    switch (type) {
        case "CD"://Claimant details
            var bhamashahNumber = $('#txtClaimantBhamashahID').val();
            if ($.isEmptyObject(bhamashahNumber.trim())) {
                alert("Please enter Bhamashah Id")
                return false;
            } else {
                $.ajax({
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    data: { BhamashaId: bhamashahNumber.toUpperCase() },
                    url: rootURl + "FPMParivad/GetBhamashaData"
                }).done(function (data) {
                    if (typeof (data.errorcode) === 'undefined') {
                        $('#myModalDashboard').modal('show');
                        $('#tblMemberDetails').html(data);
                    }
                    else {
                        alert(data.errorDescription);
                    }
                    }).error(function (ex) { 
                        console.log(ex);
                });
            }
            break;
        case "MD"://Member details
            var bhamashahNumber = $('#txtMemberBhamashahID').val();
            if ($.isEmptyObject(bhamashahNumber.trim())) {
                alert("Please enter Bhamashah Id")
                return false;
            } else {
                $.ajax({
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    data: { BhamashaId: bhamashahNumber.toUpperCase() },
                    url: rootURl + "FPMParivad/GetBhamashaData"
                }).done(function (data) {
                    if (typeof (data.errorcode) === 'undefined') {
                        $('#myModalDashboard').modal('show');
                        $('#tblMemberDetails').html(data);
                    }
                    else {
                        alert(data.errorDescription);
                    }
                    }).error(function (ex) {
                        console.log(ex);
                });
            }
            break;
    }
}

function GetMemberId(id, adhar, cast, Category, address, DistrictName, Village) {
    $('#myModalDashboard').modal('hide');

    $.ajax({
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        data: { AckId: id, Adhar: adhar },
        url: rootURl + "FPMParivad/GetMemberDetails",
        success: function (response) {
            SetData(response);
        }
    });

    function SetData(response) {
        var currentAction = $('#hdnCurrentBhamashahActionValue').val();
        if (currentAction == "CD") {
            $('#txtClaimantName').val(response[0].NameEng);
            $('#txtFatherName').val(response[0].FnameEng);
            $('#ddlGender').val(response[0].Gender);
            $('#txtEmail').val(response[0].Email);
            $('#txtMobile').val(response[0].Mobile);
        }
        else if (currentAction == "MD") {
            $('#txtMemberName').val(response[0].NameEng);
            $('#txtMFatherName').val(response[0].FnameEng);
            $('#ddlMGender').val(response[0].Gender);
            $('#txtMEmail').val(response[0].Email);
            $('#txtMMobile').val(response[0].Mobile);
            $('#txtMAge').val(response[0].Dob);
        }

    }
}

function LoadClaimRequest(cntrl) {
    var claimTypeId = $(cntrl).val();
    if (!$.isEmptyObject(claimTypeId)) {
        $.ajax({
            type: 'GET',
            url: rootURl + "ClaimRequestOT/LoadClaimRequest",
            data: { 'claimTypeId': $(cntrl).val() },
            dataType: 'html',
            async: true,
            success: function (data) {
                $('#divClaimRequestDetails').html(data);
                GetClaimRequestPupose(claimTypeId);
            },
            error: function (ex) {
                console.log(ex.error);
            }
        });
    }
}

function UpdateClaimantElements() {
    $('#dtClaimantDetails').find('td.CloneAction').each(function (i, j) {
        $(this).find('input[type="hidden"]').each(function (i1, j1) {
            $(this).attr('id', ($(this).attr('data-idtype') + "_" + i));
            $(this).attr('name', ($(this).data('nametype1') + '[' + i + '].' + $(this).data('nametype2')));
        });
    });
    $('.rowSkip').remove();
}

function UpdateMemberElements() {
    $('#dtMemberDetails').find('td.CloneAction').each(function (i, j) {
        $(this).find('input[type="hidden"]').each(function (i1, j1) {
            $(this).attr('id', ($(this).attr('data-idtype') + "_" + i));
            $(this).attr('name', ($(this).data('nametype1') + '[' + i + '].' + $(this).data('nametype2')));
        });
    });
    $('.rowSkip').remove();
}

function UpdateGramSabhaMemberElements() {
    $('#dtGramSabhaMemberDetails').find('td.CloneAction').each(function (i, j) {
        $(this).find('input[type="hidden"]').each(function (i1, j1) {
            $(this).attr('id', ($(this).attr('data-idtype') + "_" + i));
            $(this).attr('name', ($(this).data('nametype1') + '[' + i + '].' + $(this).data('nametype2')));
        });
    });
    $('.rowSkip').remove();
}

function UpdateBorderingVillageElements() {
    $('#dtBVillageDetails').find('td.CloneAction').each(function (i, j) {
        $(this).find('input[type="hidden"]').each(function (i1, j1) {
            $(this).attr('id', ($(this).attr('data-idtype') + "_" + i));
            $(this).attr('name', ($(this).data('nametype1') + '[' + i + '].' + $(this).data('nametype2')));
        });
    });
    $('.rowSkip').remove();
}

function UploadOption(value, uploadType) {
    switch (uploadType) {
        case "STribe":
            if (value == 1) {
                $('#divUSTribe').removeClass('hide');
                $('#divOTForestDweller').addClass('hide');
            }
            else {
                $('#divUSTribe').addClass('hide');
                $('#divOTForestDweller').removeClass('hide');
            }
            break;
        case "FDweller":
            if (value == 1) {
                $('#divUFDweller').removeClass('hide');
            }
            else {
                $('#divUFDweller').addClass('hide');
            }
            break;
    }
}
 
//Get Block by district
function ddlDistID_Change(cntrl) { 
    var strBlockID = '<option value="">--Select--</option>';
    var strTehsilID = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetBlockTehsil",
            dataType: 'json',
            async: true,
            data: { distID: $(cntrl).val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.blockList, function (i, j) {
                        strBlockID += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });

                    $.each(response.tehsilList, function (i, j) {
                        strTehsilID += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                else {
                    alert(response.msg);
                }
                $("#ddlBlockID").html(strBlockID);
                $("#ddlTehsilID").html(strTehsilID);
            },
            error: function (ex) {
                alert('Failed to retrieve data.' + ex);
            }
        });
    }
}

//Get Gram Panchayat by block
function ddlBlockID_Change(cntrl) {
    var $ddlGPID = $("#ddlGPID");
    var strGPID = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetGramPanchayat",
            dataType: 'json',
            async: true,
            data: { blockID: $(cntrl).val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strGPID += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                else {
                    alert(response.msg);
                }
                $ddlGPID.html(strGPID);
            },
            error: function (ex) {
                alert('Failed to retrieve data.' + ex);
            }
        });
    }
}

//Get Village by Gram panchayat
function ddlGPID_Change(cntrl) {
    var $ddlVillageCode = $("#ddlVillageCode");
    var strVillageCode = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetVillage",
            dataType: 'json',
            async: true,
            data: { gpID: $(cntrl).val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strVillageCode += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                else {
                    alert(response.msg);
                }
                $ddlVillageCode.html(strVillageCode);
            },
            error: function (ex) {
                alert('Failed to retrieve data.' + ex);
            }
        });
    }
}

//Get Block by district for bording village
function ddlDistIDForBV_Change(cntrl) {
    var $ddlBlockID = $("#ddlBlockIDForBV");
    var strBlockID = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetBlock",
            dataType: 'json',
            async: true,
            data: { distID: $(cntrl).val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strBlockID += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                else {
                    alert(response.msg);
                }
                $ddlBlockID.html(strBlockID);
            },
            error: function (ex) {
                alert('Failed to retrieve data.' + ex);
            }
        });
    }
}

//Get Gram Panchayat by block for bording village
function ddlBlockIDForBV_Change(cntrl) {
    var $ddlGPID = $("#ddlGPIDForBV");
    var strGPID = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetGramPanchayat",
            dataType: 'json',
            async: true,
            data: { blockID: $(cntrl).val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strGPID += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                else {
                    alert(response.msg);
                }
                $ddlGPID.html(strGPID);
            },
            error: function (ex) {
                alert('Failed to retrieve data.' + ex);
            }
        });
    }
}

//Get Village by Gram panchayat for bording village
function ddlGPIDForBV_Change(cntrl) {
    var $ddlVillageCode = $("#ddlVillageCodeForBV");
    var strVillageCode = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetVillage",
            dataType: 'json',
            async: true,
            data: { gpID: $(cntrl).val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strVillageCode += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                else {
                    alert(response.msg);
                }
                $ddlVillageCode.html(strVillageCode);
            },
            error: function (ex) {
                alert('Failed to retrieve data.' + ex);
            }
        });
    }
}

//Common functions
function IsValidEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function ValidateSSO(cntrl) {
    $.ajax({
        type: 'POST',
        url: rootURl + "ClaimRequestOT/IsValidSSO",
        dataType: 'json',
        async: true,
        data: { SSOID: $(cntrl).val() },
        success: function (response) {
            if (response == false) {
                alert("Please enter valid SSO ID");
                $(cntrl).val('');
            }
            return response;
        },
        error: function (ex) {
            alert('Failed to retrieve data.' + ex);
        }
    });
}

 
