$(document).ready(function () {
    $(document).on('click', '#btnWFEvidenceCancel', function () {
        $('#modalAEvidence').modal('hide')
    }); 

    $(document).on('click', '.btnDeleteDocument', function () {
        RemoveDocument(this);
    });
});

function GetWorkFlowDetailsView(id) { 
    $.ajax({
        type: 'GET',
        url: rootURl + "ClaimRequest/GetWorkFlowDetails?userType=1&ClaimRequestDetailsID=" + id,
        dataType: 'html',
        success: function (data) {
            $('#modalWorkFlowDetails').html(data);
        },
        error: function (ex) {
            console.log(ex.error);
        }
    });
}

function PrintRaisedRequest(id) {
    $.ajax({
        type: 'GET',
        url: rootURl + "ClaimRequest/PrintRaisedRequest?userType=1&ClaimRequestDetailsID=" + id,
        dataType: 'html',
        success: function (data) {
            $('#modalWorkFlowDetails').html(data);
        },
        error: function (ex) {
            console.log(ex.error);
        }
    });
}

function SaveAndDownloadReceipt(id) {
    $.ajax({
        type: 'post',
        url: rootURl + "ClaimRequest/SaveAndDownloadReceipt?ClaimRequestDetailsID=" + id,
        dataType: 'json',
        success: function (data) {
            window.open(rootURl + data.pageURL, "_blank");
        },
        error: function (ex) {
            console.log(ex.error);
        }
    });
}

function GetAttachedEvidence(objectID, objectTypeID) { 
    $.ajax({
        type: 'GET',
        data: { 'objectID': objectID, 'objectTypeID': objectTypeID },
        url: rootURl + "Common/GetAttachedEvidence",
        dataType: 'html',
        success: function (data) {
            $('#modalAEvidence').html(data);
            $('#modalAEvidence').show();
        },
        error: function (ex) {
            console.log(ex.error);
        }
    });
}

function RequesterFormUpdateText(action) {
    $cntrl = $('#lnkRequestedFormView');
    if (action == "begin") { 
        if ($cntrl.attr("value") == 'hide') {
            $cntrl.html("Click here to hide requested form details").attr("value", "show");
            $('#divRequesterForm').html(''); 
            return true;
        }
        else if ($cntrl.attr("value") == 'show') {
            $cntrl.html("Click here to show requested form details").attr("value", "hide");
            $('#divRequesterForm').html('');
            return false;
        }
    }
    else{
        $('#divRequesterForm').find('.HideInfoForApprover').remove();
        $('#divRequesterForm').find('input,select,textarea,button').prop('disabled', true);
        $('#divRequesterForm').find('a[href="javascript:void(0)"]').removeAttr('onclick').attr("onclick", "NotAllowed()");
    } 
}

function ShowHideUploadOption() {
    if ($('#divUploadFile').hasClass("hide")) {
        $('#divUploadFile').removeClass("hide");
    }
    else {
        $('#divUploadFile').addClass("hide");
    }
}
 
function NotAllowed() {
    alert('Action can\'t be performed in view mode');
}

function CloseDialog(data) { 
    $('#btnCancel').trigger("click"); 
}

function BindActionReason(action) {
    $('#txtApproverComment').text('');
    var $ddlActionReason = $("#ddlActionReason");
    var strActionReason = '<option value="">--Select--</option>';

    if (action.value == '3') {
        $("#divActionReason").removeClass('hide');

        $.ajax({
            type: 'POST',
            url: rootURl + "ClaimRequest/GetActionReason",
            dataType: 'json',
            async: true,
            data: { "actionID": $('#ddlAction').val() },
            success: function (response) {
                if (!response.isError) {
                    $.each(response.data, function (i, j) {
                        strActionReason += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                    $ddlActionReason.html(strActionReason);
                }
            },
            error: function (ex) {
                console.log('Failed to retrieve data.' + ex);
            }
        });
    }
    else {
        $("#divActionReason").addClass('hide');
        $ddlActionReason.html(strActionReason);
    }
}

function ddlActionReason_Change(cntrl) {
    var existingComment = $('#txtApproverComment').val().split('#')[0];
    var rejectedComment = ''; 

    if ($(cntrl).find('option:selected').val() != "") {
        rejectedComment = '#' + $(cntrl).find('option:selected').text();
    } 

    $('#txtApproverComment').val(existingComment + rejectedComment);
}

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
        switch (docType) { 
            case 25: fileUpload = $("#fuSurveyEvidence").get(0);
                cntrlID = "fuSurveyEvidence";
                cntrlTblID = "tblSurveyFiles"; break;
            case 29: fileUpload = $("#fuApprovalEvidence").get(0);
                cntrlID = "fuApprovalEvidence";
                cntrlTblID = "tblApprovalFiles"; break; 
            case 37: fileUpload = $("#fuFRCCommitteeReport").get(0);
                cntrlID = "fuFRCCommitteeReport";
                cntrlTblID = "tblFRCCommitteeReportFiles"; break; 
            case 38: fileUpload = $("#fuGramSabhaSankalpDoc").get(0);
                cntrlID = "fuGramSabhaSankalpDoc";
                cntrlTblID = "tblGramSabhaSankalpDocFiles"; break; 
            case 39: fileUpload = $("#fuMOM").get(0);
                cntrlID = "fuMOM";
                cntrlTblID = "tblMOMFiles"; break; 
            
            default:
                if ($('#ddlAdditionalEvidenceTypeID').val() != '') {
                    fileUpload = $("#fuAdditionalEvidence").get(0);
                    cntrlID = "fuAdditionalEvidence";
                    cntrlTblID = "tblAdditionalEvidence";
                    docType = $('#ddlAdditionalEvidenceTypeID').val();
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
            }
            if ($('#' + cntrlID).val() != '') {
                var size = GetFileSize(cntrlID);

                if (size > 2) {
                    alert('You can upload file up to 2 MB');
                    $('#' + cntrlID).val(''); //added new
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
                                    $.each(response.data, function (i, item) {
                                        rowdata += "<tr><td style=display:none;>" + item.TempID + "</td><td>" + (i + 1) + "</td><td><a href='" + rootURl + item.DocumentPath + "' target='_blank'><img src='../images/jpeg.png' Width='30' /></a>" + item.DocumentName + "</td><td style='width:5%'>" + "<button type=button class='btn btn-danger btn-circle btnDeleteDocument' style=cursor:pointer ><i class='fa fa-times'></i></button>" + "</td></tr>";
                                    });
                                }
                                $("#" + cntrlTblID).append(rowdata);
                            }
                            $('#' + cntrlID).val(''); //added new
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