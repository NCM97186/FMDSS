$(document).ready(function (e) {


    $(".datefield").datepicker({ minDate: new Date(), dateFormat: GetDatePickerFormat(), changeYear: true, onClose: function (dateText, inst) { $("[id$=auto]").focus(); } });
    var currentDate = new Date();
    $("#durationfrom").datepicker("setDate", currentDate);

    $("#durationfrom").change(function () {
        $("#durationto").val('');
    });
    $("#durationto").change(function () {
        var str1 = $("#durationfrom").val();
        var str2 = $("#durationto").val();

        var L = 3, d1 = $("#durationfrom").val().split('/'), d2 = $("#durationto").val().split('/');
        d1 = new Date(+d1[2], parseInt(d1[1], 10) - 1, parseInt(d1[0], 10));
        d2 = new Date(+d2[2], parseInt(d2[1], 10) - 1, parseInt(d2[0], 10));

        if (new Date(d1) <= new Date(d2)) {
            // parseDate(str1, str2);
        }
        else {
            $("#durationto").val('');
            alert('Date Must be GreaterThen From Date');
        }
    });

    $('#durationfrom,#durationto').keydown(function (event)
    { return cancelBackspace(event) });


    function cancelBackspace(event) {
        if (event.keyCode == 8 || event.keyCode == 46) {
            return false;
        }
    }


    $("#ddlApplicantType").change(function () {
        var applicant = $("#ddlApplicantType option:selected").text();
        $('#appLicant').val(applicant);


        if ($('#ddlApplicantType').val() == '1' || $('#ddlApplicantType').val() == '0') {
            $('#dvAgency').css("display", "block");
        }
        else
            $('#dvAgency').css("display", "none");
    });



    $("#researchCategory").change(function () {
        var applicant = $("#researchCategory option:selected").text();
        $('#researchCat').val(applicant);


        if ($('#researchCategory').val() == 'Animal') {
            $('#dvAnimal').css("display", "block");
            $('#dvSpecies').css("display", "none");
        }


        if ($('#researchCategory').val() == 'Plant') {

            $('#dvSpecies').css("display", "block");
            $('#dvAnimal').css("display", "none");
        }

        if ($('#researchCategory').val() == 'AnimalPlant') {

            $('#dvAnimal').css("display", "block");
            $('#dvSpecies').css("display", "block");
        }

    });


    $('#btnReset').click(function (e) {
        $('input[type="text"],textarea,select').val('');
    });

    //$('#btnproced').click(function (e) {       
    //    if ($('#ddlApplicantType').val() == '' || $('#ddlApplicantType').val() == null || $('#ddlApplicantType').val() == '0') {
    //        $('#errordiv').show();
    //        $('#errordiv').html("Please fill the Applicant Type!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#ddlApplicantType").offset().top
    //        }, 600);
    //        return false;
    //    }

    //    else { $('#errordiv').hide(); }

    //    if ($('#ddlApplicantType').val() == '1') {
    //        if ($('#fatherName').val() == '') {
    //            $('#errordiv31').show();
    //            $('#errordiv31').html("Please Enter Your Father Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //            $('html, body').animate({
    //                scrollTop: $("#fatherName").offset().top
    //            }, 600);
    //            return false;
    //        }
    //        else { $('#errordiv31').hide(); }

    //        if ($('#qualification').val() == '') {
    //            $('#errordiv2').show();
    //            $('#errordiv2').html("Please Select Education Qualification!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //            $('html, body').animate({
    //                scrollTop: $("#qualification").offset().top
    //            }, 600);
    //            return false;
    //        }
    //        else { $('#errordiv2').hide(); }

    //        if ($('#college').val() == '') {
    //            $('#errordiv3').show();
    //            $('#errordiv3').html("Please Enter College/Institute Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //            $('html, body').animate({
    //                scrollTop: $("#college").offset().top
    //            }, 600);
    //            return false;
    //        }
    //        else { $('#errordiv3').hide(); }
    //    }


    //    if ($('#Subject').val() == '') {
    //        $('#errordiv4').show();
    //        $('#errordiv4').html("Please Enter Subject of Research!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#Subject").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv4').hide(); }

    //    if ($('#Procedure').val() == '') {
    //        $('#errordiv5').show();
    //        $('#errordiv5').html("Please Enter Procedure/Method!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#Procedure").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv5').hide(); }

    //    if ($('#durationfrom').val() == '') {
    //        $('#errordiv6').show();
    //        $('#errordiv6').html("Please Select From Date!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#durationfrom").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv6').hide(); }

    //    if ($('#durationto').val() == '') {
    //        $('#errordiv7').show();
    //        $('#errordiv7').html("Please Select To Date!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#durationto").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv7').hide(); }




    //    if ($('#researchCategory').val() == '' || $('#researchCategory').val() == null || $('#researchCategory').val() == '0') {
    //        $('#rerrordiv').show();
    //        $('#rerrordiv').html("Please Select Research Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#researchCategory").offset().top
    //        }, 600);
    //        return false;
    //    }

    //    else { $('#rerrordiv').hide(); }


    //    if ($('#ddl_Category').val() == '0') {
    //        $('#errordivcategory').show();
    //        $('#errordivcategory').html("Please Select Category!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#ddl_Category").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordivcategory').hide(); }


    //    if ($('#district').val() == '0') {
    //        $('#errordiv8').show();
    //        $('#errordiv8').html("Please Select District Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#district").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv8').hide(); }


    //    if ($('#locationId').val() == '0') {
    //        $('#errordiv9').show();
    //        $('#errordiv9').html("Please Select Location of Research!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#locationId").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv9').hide(); }

    //    if ($('#researchCategory').val() == 'Animal' || $('#researchCategory').val() == 'AnimalPlant') {

    //        if ($('#Animal_cat').val() == '') {

    //            $('#animalcaterrordiv').show();
    //            $('#animalcaterrordiv').html("Please Select Animal Category" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>")
    //            $('html, body').animate({
    //                scrollTop: $("#Animal_cat").offset().top
    //            }, 600);
    //            return false;
    //        }

    //        else { $('#animalcaterrordiv').hide(); }

    //        if ($('#AnimalName').val() == '') {

    //            $('#animalnameerrordiv').show();
    //            $('#animalnameerrordiv').html("Please Select Animal Name" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>")
    //            $('html, body').animate({
    //                scrollTop: $("#AnimalName").offset().top
    //            }, 600);
    //            return false;
    //        }

    //        else { $('#animalnameerrordiv').hide(); }
    //    }

    //    if ($('#researchCategory').val() == 'Plant' || $('#researchCategory').val() == 'AnimalPlant') {

    //        if ($('#SpeciesCat').val() == '') {

    //            $('#speciescaterrordiv').show();
    //            $('#speciescaterrordiv').html("Please Select Species Category" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>")
    //            $('html, body').animate({
    //                scrollTop: $("#SpeciesCat").offset().top
    //            }, 600);
    //            return false;
    //        }

    //        else { $('#speciescaterrordiv').hide(); }

    //        if ($('#SpeciesName').val() == '') {

    //            $('#speciesnameerrordiv').show();
    //            $('#speciesnameerrordiv').html("Please Select Species Name" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>")
    //            $('html, body').animate({
    //                scrollTop: $("#SpeciesName").offset().top
    //            }, 600);
    //            return false;
    //        }

    //        else { $('#speciesnameerrordiv').hide(); }
    //    }



    //    if ($('#Benefits').val() == '') {
    //        $('#errordiv10').show();
    //        $('#errordiv10').html("Please Enter Benefits!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#Benefits").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv10').hide(); }

    //    if ($('#ResSynopsis').val() == '') {
    //        $('#errordiv20').show();
    //        $('#errordiv20').html("Please upload synopsis!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#ResSynopsis").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv20').hide(); }

    //    if ($('#ResPresentation').val() == '' || $('#ResPresentation').val() == '0' || $('#ResPresentation').val() == null) {
    //        $('#errordiv21').show();
    //        $('#errordiv21').html("Please upload Presentation!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#ResPresentation").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv21').hide(); }

    //    if ($('#coordinatorName').val() == '0') {
    //        $('#errordiv11').show();
    //        $('#errordiv11').html("Please Enter Coordinator Name!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#coordinatorId").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordiv11').hide(); }

    //    if ($('#txt_AssistIDno').val() == '') {
    //        $('#errorIDNo').show();
    //        $('#errorIDNo').html("Please Enter Id Proof number" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#txt_AssistIDno").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errorIDNo').hide(); }

    //    if ($('#AssistIDProof').val() == '' || $('#AssistIDProof').val() == '0' || $('#AssistIDProof').val() == null) {
    //        $('#errordivUpload').show();
    //        $('#errordivUpload').html("Please upload Id Proof!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    //        $('html, body').animate({
    //            scrollTop: $("#AssistIDProof").offset().top
    //        }, 600);
    //        return false;
    //    }
    //    else { $('#errordivUpload').hide(); }

    //});


});

