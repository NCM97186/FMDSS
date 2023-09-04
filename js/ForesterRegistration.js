$(function (e) {
    $(".datefield").datepicker({ maxDate: new Date(), dateFormat: 'dd/mm/yy', changeYear: true, onClose: function (dateText, inst) { $("[id $=auto]").focus(); } });

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

    $('#txt_long').keydown(function (e) {
        var kc = e.keyCode;
        var position = $(this).val().length;
        if (position == 2 && kc != 8) {
            if (parseInt($(this).val().substring(0, 2), 10) >= 67 && parseInt($(this).val().substring(0, 2), 10) <= 78) {
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
        if (position == 2 && kc != 8) {
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

        //if (($('#txt_NakaDistance').val().substr(0, 1) == '.') && ($('#txt_NakaDistance').val().length <= 1)) {
        //    $('#errordivNakaDistance').show();
        //    $('#errordivNakaDistance').html("Please enter the Distance from NAKA(In Km.) field greator than 0.0001!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
        //    $('#txt_NakaDistance').focus();
        //    return false;
        //}
        //else { $('#errordivNakaDistance').hide(); }

        //if ((parseFloat($('#txt_NakaDistance').val())) < 0.0001) {
        //    $('#errordivNakaDistance').show();
        //    $('#errordivNakaDistance').html("Please enter the Distance from NAKA(In Km.) field greator than 0.0001!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
        //    $('#txt_NakaDistance').focus();
        //    return false;
        //}
        //else { $('#errordivNakaDistance').hide(); }

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


        if ($('#ddl_ForestType').val() == '' || $('#ddl_ForestType').val() == null ||  $('#ddl_ForestType').val() == '0') {
            $('#errordivForestType').show();
            $('#errordivForestType').html("Please Select the Type of Forest!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ddl_ForestType').focus();
            return false;
        }
        else { $('#errordivForestType').hide(); }

        if ($('#ForestOfficer').val() == '' || $('#ForestOfficer').val() == null || $('#ForestOfficer').val() == '0') {           
            $('#errordivForestOfficer').show();
            $('#errordivForestOfficer').html("Please Select Forester to be assign" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#ForestOfficer').focus();
            return false;
        }
        else { $('#errordivForestType').hide(); }

       
        
    });


});