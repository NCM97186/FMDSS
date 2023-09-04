//Added on 15-04-2020 Mukesh Jangid
function RestrictSpecialChar2(event) {
    var regex = new RegExp("^[a-zA-Z0-9\\s-/-]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}
$('.StopCutCopyPaste').on("cut copy paste", function (e) {
    e.preventDefault();
});
$('.StopPaste').on("paste", function (e) {
    e.preventDefault();
});
// //////////////////////////////////////////////
function AllowNumericOnly(event) {
    var regex = new RegExp("^[0-9]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

function AllowNumberOnly(event) {
    var regex = new RegExp("^[0-9\\.]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

function RestrictSpecialChar(event) {
    var regex = new RegExp("^[a-zA-Z0-9\\s]+$");
    var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

function BackToList(actionName) {
    window.location.href = actionName;
}

function BindCircle(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetCircle",
            dataType: 'json',
            async: true,
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindCircle_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetCircle",
            dataType: 'json',
            async: true,
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindAllCircle(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetAllCircle",
            dataType: 'json',
            async: true,
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindAllCircle_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetAllCircle",
            dataType: 'json',
            async: true,
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindDivision(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetDivision",
            dataType: 'json',
            async: true,
            data: { "parentID": cntrl.value },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }

}

function BindDivision_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetDivision",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }

}

function BindAllDivision(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetAllDivision",
            dataType: 'json',
            async: true,
            data: { "parentID": cntrl.value },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindAllDivision_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetAllDivision",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindRange(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetRange",
            dataType: 'json',
            async: true,
            data: { "parentID": cntrl.value },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindRange_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetRange",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindAllRange(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetAllRange",
            dataType: 'json',
            async: true,
            data: { "parentID": cntrl.value },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindAllRange_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetAllRange",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindNaka(cntrl, targetCntrlID) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (cntrl.value != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetNaka",
            dataType: 'json',
            async: true,
            data: { "parentID": cntrl.value },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindNaka_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetNaka",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindTehsilNew_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetTehsilNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindBlockNew_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetBlockNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindGPNew_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetGPNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindVillageNew_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '<option value="">--Select--</option>';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetVillageNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown).val(selectedValue);
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
    }
}

function BindTehsilMultiple_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '';

    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetTehsilNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
                $.each(selectedValue.split(','), function (i, cntrlVal) {
                    $("#" + targetCntrlID).find("option[value=" + cntrlVal + "]").prop("selected", "selected");
                });
                $("#" + targetCntrlID).multiselect('rebuild');
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
        $("#" + targetCntrlID).multiselect('rebuild');
    }
}

function BindBlockMultiple_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetBlockNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);
                $.each(selectedValue.split(','), function (i, cntrlVal) {
                    $("#" + targetCntrlID).find("option[value=" + cntrlVal + "]").prop("selected", "selected");
                });
                $("#" + targetCntrlID).multiselect('rebuild');
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
        $("#" + targetCntrlID).multiselect('rebuild');
    }
}

function BindGPMultiple_Edit(parentID, targetCntrlID, selectedValue) {
    var hRootUrl = $('#hdnRootURL').val();
    var strDropdown = '';
    if (parentID != '') {
        $.ajax({
            type: 'POST',
            url: hRootUrl + "Common/GetGPNew",
            dataType: 'json',
            async: true,
            data: { "parentID": parentID },
            success: function (response) {
                if (response.data != null && !response.IsError) {
                    $.each(response.data, function (i, j) {
                        strDropdown += "<option value='" + j.Value + "'>" + j.Text + "</option>";
                    });
                }
                $("#" + targetCntrlID).html(strDropdown);

                $.each(selectedValue.split(','), function (i, cntrlVal) {
                    $("#" + targetCntrlID).find("option[value=" + cntrlVal + "]").prop("selected", "selected");
                });

                $("#" + targetCntrlID).multiselect('rebuild');
            }
        });
    }
    else {
        $("#" + targetCntrlID).html(strDropdown);
        $("#" + targetCntrlID).multiselect('rebuild');
    }
}

function ConfirmDialog(msg) {
    return confirm(msg);
}

function GetAttachedEvidence(objectID, objectTypeID) {
    var rootURl = $('#hdnRootURL').val();
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

function GetYearRange() {
    var currentYear = (new Date()).getFullYear();
    return (currentYear - 110 + ":" + (currentYear + 10));
}

function GetDatePickerFormat() {
    return 'dd/mm/yy';
}


