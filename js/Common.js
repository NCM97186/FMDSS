$(document).ready(function () {

    var Url = $(location).attr('href');
    var index = Url.lastIndexOf("/") + 1;
    var filename = Url.substr(index);
    var str = filename.split('?');
    if (str[0] == "FixedPermission") {
        $("#PermissionServices").find("ul:first").addClass("in");
    }
    if (str[0] == "WorkOrderInvoice") {
        $("#PermissionServices").find("ul:first").addClass("in");
    }
    if (str[0] == "ResearchStudy") {
        $("#PermissionServices").find("ul:first").addClass("in");
    }
    if (str[0] == "OrganisingCamp") {
        $("#PermissionServices").find("ul:first").addClass("in");
    }
    if (str[0] == "FilmShooting") {
        $("#PermissionServices").find("ul:first").addClass("in");
    }


    if (str[0] == "fdmSubActivity") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "fdmActivity") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "fdmModel") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "AddProgram") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "fdmScheme") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "FdmAddProject") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }



    if (str[0] == "FdmBudgetEstimation") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "FDMBudgetEsimationUpper") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "FdmBudgetAllocation") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "SurveyBudgetEstimation") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "MicroPlan") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "CreditLetter") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }
    if (str[0] == "AllocationBudgetLOC") {
        $("#ForestDevelopment").find("ul:first").addClass("in");
    }


    $('#messageId').click(function () {

        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(""),
            url: "../ReadAllMessages/GetNewMessage",
            success: function (data) {

                if (data.length > 0) {

                    $.each(data, function (i, items) {

                        if (i == 0) {
                            $('#lbl_EmailFrom1').text(items.EmailFrom);
                            $('#lbl_EmailDate1').text(items.EnteredOn);
                            $('#div_Subject1').html(items.Subject);
                        }

                        if (i == 1) {
                            $('#lbl_EmailFrom2').text(items.EmailFrom);
                            $('#lbl_EmailDate2').text(items.EnteredOn);
                            $('#div_Subject2').html(items.Subject);
                        }

                        if (i == 2) {
                            $('#lbl_EmailFrom3').text(items.EmailFrom);
                            $('#lbl_EmailDate3').text(items.EnteredOn);
                            $('#div_Subject3').html(items.Subject);
                        }
                    });
                }
                else {

                    $('#ul_msg').css("display", "none");
                }


            },
            traditional: true,
            error: function (data) { console.log(data) }
        });
    });

    $('#aFavourite').click(function () {
        $('#favouriteName').val('');
        $('#errordiv1').hide();
        var Url = $(location).attr('href');
        var index = Url.lastIndexOf("/") + 1;
        var filename = Url.substr(index);


        //$('#favouriteName').val(filename);
        //$('#favouriteUrl').val(Url);

    })

    $('#btnSaveFavourite').click(function () {

        if ($('#favouriteName').val() == '') {
            $('#errordiv1').show();
            $('#errordiv1').html("Please enter the Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            return false;
        }
        else {
            $('#errordiv1').hide();
            var Url = $(location).attr('href');
            var index = Url.lastIndexOf("/") + 1;
            var filename = $('#favouriteName').val();



            var dataRequest = {
                PageUrl: Url,
                PageName: filename
            };
            $.ajax({
                type: 'POST',
                url: "../dashboard/SaveFavouritelink",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(dataRequest),
                success: function (data) {

                    if (data == true) {
                        if (window.location.href.indexOf("Dashboard") > -1) {

                            window.location.href = "Dashboard";
                        }
                        if (window.location.href.indexOf("?messagetype=1") > -1) {
                            window.location.href = "dashboard";
                        }
                        alert("Record Saved Successfully.");

                    }

                },
                error: function (ex) {
                    alert('Failed to retrieve states.' + ex);
                }
            });

        }
    })

})

function PrintData(bodyID) {
    // alert($("#" + bodyID).html());

    var divContents = $("#" + bodyID).html();
    console.log(divContents);
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    frameDoc.document.write('<html><head><link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="../css/mobile.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="../css/dashboard/main.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="../css/dashboard/dashboard.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('<link href="../css/dashboard/font-awesome.min.css" rel="stylesheet" type="text/css" />');
    frameDoc.document.write('</head><body>');
    frameDoc.document.write(divContents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);
}