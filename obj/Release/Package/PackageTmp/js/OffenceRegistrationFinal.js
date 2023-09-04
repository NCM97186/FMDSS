
 var RootUrl = '@Url.Content("~/")';

//var $j = jQuery.noConflict();
$(document).ready(function () {      
    $(".datefield").datepicker({ yearRange: "-100:+0", dateFormat: 'dd/mm/yy', changeYear: true, onClose: function (dateText, inst) { $("[id$=auto]").focus(); } });
    $(".datefield").keypress(function (e) {
        e.preventDefault();
    });
       $('input:text,textarea').bind('keypress', function () {
          // $(this).css({ 'text-transform': 'uppercase' });
          //var str = $(this).val();
          // str = str.replace(/\s/g, '');
          // $(this).val(str);

           $(this).val($.trim($(this).val()));
       });
    $('.gridtable').DataTable({
        responsive: true
    });    
    $('#QuantityOfProduce,#AnimalWeight,#ArticleQuantity,#EquipmentSize,#Pincode,#PhoneNo,#WitnessAge,#GuarantorPincode,#Height').bind('keypress', function (evt) {
           var charCode = (evt.which) ? evt.which : evt.keyCode
           // var charCode = evt.which;

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
   
       $('#Description,#VechileMake,#VechileModel,#VechileChassisNo,#VechileEngineNo,#SpeciesName,#EquipmentMake,#EquipmentModel,#EquipmentCaliber,#EquipmentIdentificationNo,#AnimalScientificName,#AnimalDescription,#DescriptionOfAnimalArticle,#ResidentialAddress1,#ResidentialAddress2,#WitnessStatement,#CourtCaseNo,#ConvictionReason').keypress(function (e) {
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
       $('#VisitPlace,#VechileOwnerName,#AnimalCommanName,#ArticleAnimalScientificName,#ArticleAnimalCommanName,#NameOfAnimalArticle,#WitnessName,#FatherName,#SpouseName,#Caste,#CourtName,#CourtPlace').keypress(function (e) {
           if (e.ctrlKey || e.altKey) {
               e.preventDefault();
           } else {
               var key = (e.which) ? e.which : e.keyCode
               if (e.shiftKey) {
                   if (key == 64 || key == 33 || key == 35 || key == 36 || key == 37 || key == 94 || key == 38 || key == 42 || key == 40 || key == 41) {
                       e.preventDefault();
                   }
               }
               if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 97 && key <= 122) )) {
                   e.preventDefault();
               }
           }
       });

       $('#GuarantorAddress,#GuarantorResidentialAddress1,#GuarantorResidentialAddress2').keypress(function (e) {
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
       $('#GuarantorName,#GuarantorFatherName,#GuarantorCaste,#GuarantorNearestTehsil,#OffenderNearestPoliceStation,#OffenderNearestTehsil,#WitnessName1,#WitnessName2,#ClothesWorn,#ColorOfClothes,#PhysicalAppearance,#OtherSpecialDetails').keypress(function (e) {
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
          
       if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
           $('.hidebutton').hide();
           $('#btn_submit1').hide();
           $('#tab4,#tab5').removeClass('disabled');
           $('#tab4,#tab5').removeClass('disabledTab');
       }
       if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2' && $('#hdnDfoApproveStatus').val() != '') {
           $('.hidebutton').hide(); $('.hidebutton1').hide();
           $('#btn_submit1').hide();
           $('#tab4,#tab5,#tab6,#tab7').removeClass('disabled');
           $('#tab4,#tab5,#tab6,#tab7').removeClass('disabledTab');
       }
                            
       var activeTab;      
       if ($('#hdnTabInfo').val() == '') {
           activeTab = '#tab1default';
           $('#myTab a[href="' + activeTab + '"]').tab('show');
       }
       else {
           activeTab = $('#hdnTabInfo').val();
           $('#myTab a[href="' + activeTab + '"]').tab('show');
       }
       //if ($('#hdnWarrantTab').val() == "show") {
       //    $('#tab1,#tab2,#tab3,#tab6,#tab7,#btn_submit1,#btn_cancel,#btn_Previous').hide();
       //    $('#tab5').show();
       //    activeTab = '#tab5default1'
       //    $('.page-header').text('Warrant Registration');
       //    $('#myTab a[href="' + activeTab + '"]').tab('show');
       //}
       //else if ($('#hdnCourtCaseTab').val() == "show") {
       //    $('#tab1,#tab2,#tab3,#tab5,#tab7,#btn_submit1,#btn_cancel,#btn_Previous').hide();
       //    $('#tab6').show();
       //    activeTab = '#tab6default'
       //    $('#myTab a[href="' + activeTab + '"]').tab('show');
       //}
       //else if ($('#hdnIssueJamanatTab').val() == "show") {
       //    $('#tab1,#tab2,#tab3,#tab5,#tab6,#btn_submit1,#btn_cancel,#btn_Previous').hide();
       //    $('#tab7').show();
       //    activeTab = '#tab7default'
       //    $('#myTab a[href="' + activeTab + '"]').tab('show');
       //}
       //else {
       //    $('#tab1,#tab2,#tab3,#btn_submit1,#btn_cancel,#btn_Previous').show();
       //    $('#tab5,#tab6,#tab7').hide();
       //    activeTab = $('#hdnTabInfo').val();
       //    if (activeTab == 'undefined' || activeTab == '') {
       //        activeTab = '#tab1default';
       //    }
       //    $('#myTab a[href="' + activeTab + '"]').tab('show');
       //}        
              
    $('#FirstOfficer').change(function (e) {
        var firstofficer = { firstofficer: $("#FirstOfficer option:selected").text() }
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistrationfinal/getDesignation', // we are calling json method
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(firstofficer),
            success: function (data) {                     
                $('#FirstOfficerDesig').val(data);                      
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });
      
    $('#SecondOfficer').change(function (e) {
        var firstofficer = { firstofficer: $("#SecondOfficer option:selected").text() }
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistrationfinal/getDesignation', // we are calling json method
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(firstofficer),
            success: function (data) {                      
                $('#SecondOfficerDesig').val(data);
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });

    $('#ThirdOfficer').change(function (e) {
        var firstofficer = { firstofficer: $("#ThirdOfficer option:selected").text() }
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistrationfinal/getDesignation', // we are calling json method
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(firstofficer),
            success: function (data) {
                $('#ThirdOfficerDesig').val(data);
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });

    $('#FourthOfficer').change(function (e) {
        var firstofficer = { firstofficer: $("#FourthOfficer option:selected").text() }
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistrationfinal/getDesignation', // we are calling json method
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(firstofficer),
            success: function (data) {
                $('#FourthOfficerDesig').val(data);
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });


    $('#AnimalCommanName').change(function (e) {
        var AnimalName = { AnimalName: $("#AnimalCommanName option:selected").val() }
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistrationfinal/getAnimalScientficName', // we are calling json method
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(AnimalName),
            success: function (data) {
                $('#AnimalScientificName').val(data);
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });
   
    $('#Vechile').click(function(e){
    
        if ($('#Vechile').prop('checked')) {
            $('#DivVechile').show();
        }
        else {
            $('#DivVechile').hide();
        }
    })
    $('#ForestProduce').click(function (e) {

        if ($('#ForestProduce').prop('checked')) {
            $('#DivProduce').show();
        }
        else {
            $('#DivProduce').hide();
        }
    })
    $('#Equipment').click(function (e) {

        if ($('#Equipment').prop('checked')) {
            $('#DivEquipment').show();
        }
        else {
            $('#DivEquipment').hide();
        }
    })
    
    $('#Animal').click(function (e) {

        if ($('#Animal').prop('checked')) {
            $('#DivAnimal').show();
        }
        else {
            $('#DivAnimal').hide();
        }
    })
    $('#AnimalArticle').click(function (e) {

        if ($('#AnimalArticle').prop('checked')) {
            $('#DivAnimalArticle').show();
        }
        else {
            $('#DivAnimalArticle').hide();
        }
    })

    $('#btnsizedItemTeam').click(function (e) {

        if ($("#FirstOfficer option:selected").text() == '--Select--') {
            $("#FirstOfficer").focus();
            e.preventDefault();
        }
        else if ($("#SecondOfficer option:selected").text() == '--Select--') {
            $("#SecondOfficer").focus();
            e.preventDefault();
        }
        else if ($("#ThirdOfficer option:selected").text() == '--Select--') {
            $("#ThirdOfficer").focus();
            e.preventDefault();
        }
        else if ($("#FourthOfficer option:selected").text() == '--Select--') {
            $("#FourthOfficer").focus();
            e.preventDefault();
        }
        else {

        }
    })
    
    $('#btnsizedItem').click(function (e) {     
        
        if (!($('#Vechile').prop('checked') || $('#ForestProduce').prop('checked') || $('#Equipment').prop('checked') || $('#Animal').prop('checked') || $('#AnimalArticle').prop('checked'))) {
            alert('please select checkbox for sized item');
            e.preventDefault();
        }
        else {

        }
    })


    $('#btnUpload').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {

            var fileUpload = $("#UploadId").get(0);
            var files = fileUpload.files;
          
            // Create FormData object
            var fileData = new FormData();

            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            //    // Adding one more key to FormData object
            //    //fileData.append('username', ‘Manas’);
            if ($('#UploadId').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);

                        $("#hdUploadId").val(result.list2);
                        $('#UploadId').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else {
                alert("Select file to upload");
            }
            
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#btnUpload1').click(function () {

        //Checking whether FormData is available in browser
        if (window.FormData !== undefined) {
            var fileUpload = $("#UploadSignedStatement").get(0);
            var files = fileUpload.files;
            // Create FormData object
            var fileData = new FormData();
            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#UploadSignedStatement').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdUploadSignedStatement").val(result.list2);
                        $('#OPhotoIDURL').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
        else{
                alert("Select file to upload");
            }
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#VechileUploadDoc').change(function (e) {
        if ($('#VechileUploadDoc').val() != '') {
           // var iSize = ($("#VechileUploadDoc")[0].files[0].size / 1048576);
            var iSize = parseFloat($("#VechileUploadDoc")[0].files[0].size / 1024).toFixed(2);          
            UploadFileValidation($('#VechileUploadDoc').val(), iSize, $(this).attr('id'));
        }
    });

    $('#btnVechileUpload').click(function () {
                  
        if (window.FormData !== undefined) {
            var fileUpload = $("#VechileUploadDoc").get(0);
            var files = fileUpload.files;            
            var fileData = new FormData();          
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#VechileUploadDoc').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdVechileDoc").val(result.list2);
                      //  $('#VechileUploadDoc').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else { alert("Select file to upload"); }
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#ProduceUploadDoc').change(function (e) {
        if ($('#ProduceUploadDoc').val() != '') {
           // var iSize = ($("#ProduceUploadDoc")[0].files[0].size / 1048576);
            var iSize = parseFloat($("#ProduceUploadDoc")[0].files[0].size / 1024).toFixed(2);
            UploadFileValidation($('#ProduceUploadDoc').val(), iSize, $(this).attr('id'));
        }
    });

    $('#btnProduceUpload').click(function () {
        if (window.FormData !== undefined) {
            var fileUpload = $("#ProduceUploadDoc").get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#ProduceUploadDoc').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdProduceDoc").val(result.list2);
                        alert($("#hdProduceDoc").val());
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else { alert("Select file to upload"); }
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#EquipmentUploadDoc').change(function (e) {
        if ($('#EquipmentUploadDoc').val() != '') {
           // var iSize = ($("#EquipmentUploadDoc")[0].files[0].size / 1048576);
            var iSize = parseFloat($("#EquipmentUploadDoc")[0].files[0].size / 1024).toFixed(2);
            UploadFileValidation($('#EquipmentUploadDoc').val(), iSize, $(this).attr('id'));
        }
    });

    $('#District').change(function (e) {
        $("#Village").empty();
        $("#Village").append('<option value="' + '' + '">' +
                '--Select--' + '</option>');
        $.ajax({
            type: 'POST',
            url: RootUrl + 'OffenseRegistration/getVillage', // we are calling json method
            dataType: 'json',
            data: { dist_code: $j("#District option:selected").val() },
            success: function (states) {
                $.each(states, function (i, vill) {
                    $("#Village").append('<option value="' + vill.Value + '">' +
                     vill.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed to retrieve states.' + ex);
            }
        });
    });


    $('#btnEquipmentUpload').click(function () {      
        if (window.FormData !== undefined) {
            var fileUpload = $("#EquipmentUploadDoc").get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#EquipmentUploadDoc').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdEquipmentDoc").val(result.list2);
                      //  $('#EquipmentUploadDoc').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else { alert("Select file to upload"); }
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#AnimalUploadDoc').change(function (e) {
        if ($('#AnimalUploadDoc').val() != '') {
          //  var iSize = ($("#AnimalUploadDoc")[0].files[0].size / 1048576);
            var iSize = parseFloat($("#AnimalUploadDoc")[0].files[0].size / 1024).toFixed(2);
            UploadFileValidation($('#AnimalUploadDoc').val(), iSize, $(this).attr('id'));
        }
    });

    $('#btnAnimalUpload').click(function () {       
        if (window.FormData !== undefined) {
            var fileUpload = $("#AnimalUploadDoc").get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#AnimalUploadDoc').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdAnimalDoc").val(result.list2);
                       // $('#AnimalUploadDoc').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else { alert("Select file to upload"); }
        } else {
            alert("FormData is not supported.");
        }
    });

    $('#AnimalArticleUploadDoc').change(function (e) {
        if ($('#AnimalArticleUploadDoc').val() != '') {
          //  var iSize = ($("#AnimalArticleUploadDoc")[0].files[0].size / 1048576);
            var iSize = parseFloat($("#AnimalArticleUploadDoc")[0].files[0].size / 1024).toFixed(2);
            UploadFileValidation($('#AnimalArticleUploadDoc').val(), iSize, $(this).attr('id'));
        }
    });

    $('#btnArticleUpload').click(function () {
        if (window.FormData !== undefined) {
            var fileUpload = $("#AnimalArticleUploadDoc").get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            if ($('#AnimalArticleUploadDoc').val() != '') {
                $.ajax({
                    url: RootUrl + 'OffenseRegistrationfinal/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (result) {
                        alert(result.list1);
                        $("#hdArticleDoc").val(result.list2);
                      //  $('#AnimalArticleUploadDoc').val('');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else { alert("Select file to upload"); }
        } else {
            alert("FormData is not supported.");
        }
    });

    $("#PhotoURL").change(function (e) {
       // var iSize = ($("#PhotoURL")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#PhotoURL")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#PhotoURL').val('');
            $('#errordivDoc').show();
            $('#errordivDoc').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#PhotoURL').focus();
            return false;
        }
        var file = $("#PhotoURL").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            // split file name at dot
            var get_ext = file.split('.');
            // reverse name to check extension
            get_ext = get_ext.reverse();
            // check file type is valid as given in 'exts' array

            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#PhotoURL').val('');
                $('#errordivDoc').show();
                $('#errordivDoc').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#PhotoURL').focus();
                return false;
            } else {
                $('#errordivDoc').hide();
            }
        }
        else { $('#errordivDoc').hide(); }
    });

    $("#UploadId").change(function (e) {
       // var iSize = ($("#UploadId")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#UploadId")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#UploadId').val('');
            $('#errordivUploadId').show();
            $('#errordivUploadId').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#UploadId').focus();
            return false;
        }
        var file = $("#UploadId").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {       
            var get_ext = file.split('.');        
            get_ext = get_ext.reverse();         
            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#UploadId').val('');
                $('#errordivUploadId').show();
                $('#errordivUploadId').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#UploadId').focus();
                return false;
            } else {
                $('#errordivUploadId').hide();
            }
        }
        else { $('#errordivUploadId').hide(); }
    });

    $("#UploadSignedStatement").change(function (e) {
       // var iSize = ($("#UploadSignedStatement")[0].files[0].size / 1048576);
        var iSize = parseFloat($("#UploadSignedStatement")[0].files[0].size / 1024).toFixed(2);
        if (iSize > 100) {
            $('#UploadSignedStatement').val('');
            $('#errordivSigned').show();
            $('#errordivSigned').html("Upload ID should not be larger than 2MB!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            $('#UploadSignedStatement').focus();
            return false;
        }
        var file = $("#UploadId").val();
        var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];
        if (file) {
            var get_ext = file.split('.');
            get_ext = get_ext.reverse();
            if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {
                $('#UploadSignedStatement').val('');
                $('#errordivSigned').show();
                $('#errordivSigned').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field !" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
                $('#UploadSignedStatement').focus();
                return false;
            } else {
                $('#errordivSigned').hide();
            }
        }
        else { $('#errordivSigned').hide(); }
    });

    

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetFirstForestOfficer',
        success: function (data) {
            var d = data.split('#');        
            if (d[0] != '' && d[1] != '') {
                $('#FirstOfficer option').filter(function () { return $(this).val() == d[0] }).attr('selected', true);
                $('#FirstOfficerDesig').val(d[1]);
            }                        
        },

        traditional: true,
        error: function (data) { console.log(data) }
    });

        
   

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetVechil',
        success: function (data) {
          
            if (data.length > 0) {
                $('#Vechile').prop('checked', true);
                $('#DivVechile').show();
                $.each(data, function (i, items) {
                    var id = "'" + items.Vechilerowid + "'";
                    var count = 1;
                    count = count + i;
                    if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
                        var bardata = "<tr style='border:1px' class='" + items.Vechilerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.VechileRegistrationNo + "</td><td style='border:1px'>" + items.VechileType + "</td><td style='border:1px'>" + items.VechileChassisNo + "</td><td style='border:1px'>" + items.VechileEngineNo + "</td><td style='border:1px'></td></tr>";
                    }
                    else {
                        var bardata = "<tr style='border:1px' class='" + items.Vechilerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.VechileRegistrationNo + "</td><td style='border:1px'>" + items.VechileType + "</td><td style='border:1px'>" + items.VechileChassisNo + "</td><td style='border:1px'>" + items.VechileEngineNo + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteVechile(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    }
                    
                    $("#tblAddVechile").append(bardata);
                    $('#VechileRegistrationNo,#VechileOwnerName,#VechileMake,#VechileModel,#VechileChassisNo,#VechileEngineNo,#PastOffensesByVechile').val('');
                    $('#VechileType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetProduce',
        success: function (data) {
            if (data.length > 0) {
                $('#ForestProduce').prop('checked', true);
                $('#DivProduce').show();
                $.each(data, function (i, items) {
                    var id = "'" + items.Producerowid + "'";
                    var count = 1;
                    count = count + i;
                    if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
                        var bardata = "<tr style='border:1px' class='" + items.Producerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ProduceType + "</td><td style='border:1px'>" + items.SpeciesName + "</td><td style='border:1px'>" + items.QuantityOfProduce + "</td><td style='border:1px'></td></tr>";
                    }
                    else {
                        var bardata = "<tr style='border:1px' class='" + items.Producerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ProduceType + "</td><td style='border:1px'>" + items.SpeciesName + "</td><td style='border:1px'>" + items.QuantityOfProduce + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteProduce(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    }                    
                    $("#tbl_Produce").append(bardata);
                    $('#SpeciesName,#QuantityOfProduce').val('');
                    $('#ProduceType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetEquipment',
        success: function (data) {
            if (data.length > 0) {
                $('#Equipment').prop('checked',true);
                $('#DivEquipment').show();
                $.each(data, function (i, items) {
                    var id = "'" + items.Equipmentrowid + "'";
                    var count = 1;
                    count = count + i;
                    if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
                        var bardata = "<tr style='border:1px' class='" + items.Equipmentrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.EquipmentType + "</td><td style='border:1px'>" + items.EquipmentModel + "</td><td style='border:1px'>" + items.EquipmentIdentificationNo + "</td><td style='border:1px'></td></tr>";
                    }
                    else {
                        var bardata = "<tr style='border:1px' class='" + items.Equipmentrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.EquipmentType + "</td><td style='border:1px'>" + items.EquipmentModel + "</td><td style='border:1px'>" + items.EquipmentIdentificationNo + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteEquipment(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    }
                    
                    $("#tblEquipment").append(bardata);
                    $('#EquipmentMake,#EquipmentModel,#EquipmentCaliber,#EquipmentIdentificationNo,#EquipmentSize').val('');
                    $('#EquipmentType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetAnimal',
        success: function (data) {
            if (data.length > 0) {
                $('#Animal').prop('checked', true);
                $('#DivAnimal').show();
                $.each(data, function (i, items) {
                    var id = "'" + items.Animalrowid + "'";
                    var count = 1;
                    count = count + i;
                    if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
                        var bardata = "<tr style='border:1px' class='" + items.Animalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.AnimalScientificName + "</td><td style='border:1px'>" + items.AnimalCommanName + "</td><td style='border:1px'>" + items.AnimalWeight + "</td><td style='border:1px'></td></tr>";
                    }
                    else {                        

                        var bardata = "<tr style='border:1px' class='" + items.Animalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.AnimalScientificName + "</td><td style='border:1px'>" + items.AnimalCommanName + "</td><td style='border:1px'>" + items.AnimalWeight + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteAnimal(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    }                   
                    $("#tblAnimal").append(bardata);
                    $('#AnimalScientificName,#AnimalCommanName,#AnimalDescription,#AnimalWeight').val('');
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetAnimalArticle',
        success: function (data) {
            if (data.length > 0) {
                $('#AnimalArticle').prop('checked', true);
                $('#DivAnimalArticle').show();
                $.each(data, function (i, items) {
                    var id = "'" + items.ArticleAnimalrowid + "'";
                    var count = 1;
                    count = count + i;
                    if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
                        var bardata = "<tr style='border:1px' class='" + items.ArticleAnimalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ArticleAnimalScientificName + "</td><td style='border:1px'>" + items.ArticleAnimalCommanName + "</td><td style='border:1px'>" + items.NameOfAnimalArticle + "</td><td style='border:1px'></td></tr>";
                    }
                    else {
                        var bardata = "<tr style='border:1px' class='" + items.ArticleAnimalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ArticleAnimalScientificName + "</td><td style='border:1px'>" + items.ArticleAnimalCommanName + "</td><td style='border:1px'>" + items.NameOfAnimalArticle + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteAnimalArticle(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    }                    
                    $("#tblAnimalArticle").append(bardata);
                    $('#ArticleAnimalScientificName,#ArticleAnimalCommanName,#NameOfAnimalArticle,#DescriptionOfAnimalArticle,#ArticleQuantity').val('');
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetWitness',
        success: function (data) {
            if (data.length > 0) {
                $.each(data, function (i, items) {
                    var id = "'" + items.Witnessrowid + "'";
                    var count = 1;
                    count = count + i;
                    if ($('#hdnIsComplete').val() == '1' && $('#hdnApproveStatus').val() == '2') {
                        var bardata = "<tr style='border:1px' class='" + items.Witnessrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.WitnessName + "</td><td style='border:1px'>" + items.FatherName + "</td><td style='border:1px'>" + items.ResidentialAddress1 + "</td><td style='border:1px'>" + items.State + "</td><td style='border:1px'>" + "<button type=button class='btn btn-success btn-circle' data-toggle=modal data-target=#myModalWitness style=cursor:pointer onclick=ViewWitness(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td style='border:1px'></td><td style='border:1px'></td></tr>";
                    }
                    else {
                        var bardata = "<tr style='border:1px' class='" + items.Witnessrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.WitnessName + "</td><td style='border:1px'>" + items.FatherName + "</td><td style='border:1px'>" + items.ResidentialAddress1 + "</td><td style='border:1px'>" + items.State + "</td><td style='border:1px'>" + "<button type=button class='btn btn-success btn-circle' data-toggle=modal data-target=#myModalWitness style=cursor:pointer onclick=ViewWitness(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td style='border:1px'>" + "<button type=button class='btn btn-warning btn-circle' style=cursor:pointer onclick=EditWitness(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td style='border:1px'>" + "<button type=button class=class='btn btn-danger btn-circle' onclick=DeleteWitness(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    }                   
                    $("#tblWitness").append(bardata);
                    $('#WitnessName,#FatherName,#SpouseName,#Caste,#Category,#ResidentialAddress1,#ResidentialAddress2').val('');
                    $('#Pincode,#Village,#District,#State,#PhoneNo,#PhotoId,#UploadId,#WitnessAge,#StatementDate').val('');
                    $('#WitnessStatement,#UploadSignedStatement').val('');
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(''),
        url: RootUrl + 'OffenseRegistrationfinal/GetWarrant',
        success: function (data) {
            if (data.length > 0) {
                $.each(data, function (i, items) {
                    var id = "'" + items.Warrantrowid + "'";
                    $('#AppearancePlace').val(items.AppearancePlace);
                    var AppearancePlace = $('#AppearancePlace option:selected').text();
                    //var bardata = "<tr style='border:1px' class='" + items.Warrantrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.NameOfOffender + "</td><td style='border:1px'>" + items.ClothesWorn + "</td><td style='border:1px'>" + items.ColorOfClothes + "</td><td style='border:1px'>" + items.PhysicalAppearance + "</td><td style='border:1px'>" + "<button type=button class=get-delete onclick=DeleteWarrant(" + id + ")>Delete</button>" + "</td></tr>";
                    var bardata = "<tr style='border:1px' class='" + items.Warrantrowid + "'><td>" + items.NameOfOffender + "</td><td>" + items.ClothesWorn + "</td><td>" + items.ColorOfClothes + "</td><td>" + items.PhysicalAppearance + "</td><td>" + items.Appearancedate + "</td><td>" + items.Appearancetime + "</td><td>" + AppearancePlace + "</td><td></td></tr>";
                    $("#tblwarrant").append(bardata);
                    $('#NameOfOffender,#ClothesWorn,#ColorOfClothes,#PhysicalAppearance,#Height,#OtherSpecialDetails').val('');
                    $('#AppearancePlace option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            }
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

    $('#NameOfOffender').change(function (e) {
        //$("#tblwarrant").empty();
        var OffenderName = { OffenderName: $('#NameOfOffender option:selected').val() }
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(OffenderName),
            url: RootUrl + 'OffenseRegistrationfinal/GetWarrantDetail',
            success: function (data) {                
                if (data.length > 0) {
                    $.each(data, function (i, items) {
                        //var id = "'" + items.Warrantrowid + "'";
                        //var count = 1;
                        //count = count + i;                        
                        //var bardata = "<tr style='border:1px' class='" + items.Warrantrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.NameOfOffender + "</td><td style='border:1px'>" + items.ClothesWorn + "</td><td style='border:1px'>" + items.ColorOfClothes + "</td><td style='border:1px'>" + items.PhysicalAppearance + "</td></tr>";
                        //$("#tblwarrant").append(bardata); 
                        $('#hdnWarrantrowid').val(items.Warrantrowid);
                        $('#ClothesWorn').val(items.ClothesWorn);
                        $('#ColorOfClothes').val(items.ColorOfClothes);
                        $('#PhysicalAppearance').val(items.PhysicalAppearance);
                        $('#Height').val(items.Height);
                        $('#OtherSpecialDetails').val(items.OtherSpecialDetails);                        
                    });
                }
            },
            traditional: true,
            error: function (data) { console.log(data) }
        });


    });
});

function AddCrime() {  
    if ($('#VisitDate').val() == '') {
        $('#VisitDate').focus();        
    }
    else if ($('#VisitPlace').val() == '') {
        $('#VisitPlace').focus();
        $('#VisitPlace').attr('placeholder', 'Enter place');
    }
    else if ($('#VisitTime').val() == '') {
        $('#VisitTime').focus();
    }
    else if ($('#Description').val() == '') {
        $('#Description').focus();
        $('#Description').attr('placeholder', 'Enter description');
    }
    //else if ($('#PhotoURL').val().length == 0) {
    //    $('#PhotoURL').focus();     
    //}
    else {
       
        $("#tblCrime").empty();
            var Crimeinfo = {
                place: $('#VisitPlace').val(), date: $('#VisitDate').val(),
                time: $('#VisitTime').val(), discription: $('#Description').val(),
                photourl: $('#hdUploadCrime').val()
              
            }
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(Crimeinfo),
                url: RootUrl + 'OffenseRegistrationfinal/AddCrimeDescription',
                success: function (data) {
                    $.each(data, function (i, items) {
                        var id = "'" + items.Crimerowid + "'";
                        var count = 1;
                        count = count + i;
                        var bardata = "<tr style='border:1px' class='" + items.Crimerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.VisitDate + "</td><td style='border:1px'>" + items.VisitPlace + "</td><td style='border:1px'>" + items.VisitTime + "</td><td style='border:1px'>" + items.Description + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteCrime(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                        var filename = items.PhotoURL;                      
                        var str = filename.split('/');
                        if (items.PhotoURL != '') {
                            $('#viewdoc').show();
                            $('#viewdoc').attr('href', '../ForestProtectionDocument/' + str[2]);
                            $('#viewdoc').attr('target', '_blank');
                        }
                        $("#tblCrime").append(bardata);                       
                        $('#VisitDate,#VisitPlace,#VisitTime,#Description,#PhotoURL').val('');
                     
                    });
                },
                traditional: true,
                error: function (data) { console.log(data) }
            });       
    }
}

function DeleteCrime(ID) {
    $("#tblCrime").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteCrime',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Crimerowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.Crimerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.VisitDate + "</td><td style='border:1px'>" + items.VisitPlace + "</td><td style='border:1px'>" + items.VisitTime + "</td><td style='border:1px'>" + items.Description + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteCrime(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblCrime").append(bardata);
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });
}

function AddVechile() {
    if ($('#VechileRegistrationNo').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileRegistrationNo').focus();
        $('#VechileRegistrationNo').attr('placeholder', 'Enter Registration No.');
    }
    else if ($('#VechileOwnerName').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileOwnerName').focus();
        $('#VechileOwnerName').attr('placeholder', 'Enter Owner name');
    }
    else if ($('#VechileType option:selected').text() == '--Select--' && $('#Vechile').prop('checked')) {
        $('#VechileType').focus();
    }
    else if ($('#VechileMake').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileMake').focus();
        $('#VechileMake').attr('placeholder', 'Enter vechile make');
    }
    else if ($('#VechileModel').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileModel').focus();
        $('#VechileModel').attr('placeholder', 'Enter vechile model');
    }
    else if ($('#VechileChassisNo').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileChassisNo').focus();
        $('#VechileChassisNo').attr('placeholder', 'Enter chassis no.');
    }
    else if ($('#VechileEngineNo').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileEngineNo').focus();
        $('#VechileChassisNo').attr('placeholder', 'Enter engine no.');
    }
    else if ($('#VechileUploadDoc').val() == '' && $('#Vechile').prop('checked')) {
        $('#VechileUploadDoc').focus();
        $('#errordivUpload').show();
        $('#errordivUpload').html("upload valid file!!!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    }
    else {
        if ($('#Vechile').prop('checked')) {
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
                url: RootUrl + 'OffenseRegistrationfinal/AddVechile',
                success: function (data) {
                    $.each(data, function (i, items) {
                        var id = "'" + items.Vechilerowid + "'";
                        var count = 1;
                        count = count + i;
                        var bardata = "<tr style='border:1px' class='" + items.Vechilerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.VechileRegistrationNo + "</td><td style='border:1px'>" + items.VechileType + "</td><td style='border:1px'>" + items.VechileChassisNo + "</td><td style='border:1px'>" + items.VechileEngineNo + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteVechile(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                        $("#tblAddVechile").append(bardata);
                        $('#VechileRegistrationNo,#VechileOwnerName,#VechileMake,#VechileModel,#VechileChassisNo,#VechileEngineNo,#PastOffensesByVechile').val('');
                        $('#VechileType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                    });
                },
                traditional: true,
                error: function (data) { console.log(data) }
            });
        }
        else { alert('select Vechile checkbox to continue'); }
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
        url: RootUrl + 'OffenseRegistrationfinal/DeleteVechile',
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

function AddProduce() {
       
    if ($('#ProduceType option:selected').text() == '--Select--' && $('#ForestProduce').prop('checked')) {
        $('#ProduceType').focus();
    }
    else if ($('#SpeciesName').val() == '' && $('#ForestProduce').prop('checked')) {
        $('#SpeciesName').focus();
        $('#SpeciesName').attr('placeholder', 'Enter species name');
    }
    else if ($('#QuantityOfProduce').val() == '' && $('#ForestProduce').prop('checked')) {
        $('#QuantityOfProduce').focus();
        $('#QuantityOfProduce').attr('placeholder', 'Enter quantity');
    }
    else if ($('#ProduceUploadDoc').val() == '' && $('#ForestProduce').prop('checked')) {
        $('#ProduceUploadDoc').focus();
        $('#errordivUpload').show();
        $('#errordivUpload').html("upload valid file!!!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    }
    else {
        if ($('#ForestProduce').prop('checked')) {
            alert($('#hdProduceDoc').val());
            $("#tbl_Produce").empty();
            var Produceinfo = {
                ProduceType: $('#ProduceType option:selected').val(), SpeciesName: $('#SpeciesName').val(),
                QuantityOfProduce: $('#QuantityOfProduce').val(), ProduceUploadDoc: $('#hdProduceDoc').val()
            }
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(Produceinfo),
                url: RootUrl + 'OffenseRegistrationfinal/AddProduce',
                success: function (data) {
                    $.each(data, function (i, items) {
                        var id = "'" + items.Producerowid + "'";
                        var count = 1;
                        count = count + i;
                        var bardata = "<tr style='border:1px' class='" + items.Producerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ProduceType + "</td><td style='border:1px'>" + items.SpeciesName + "</td><td style='border:1px'>" + items.QuantityOfProduce + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteProduce(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                        $("#tbl_Produce").append(bardata);
                        $('#SpeciesName,#QuantityOfProduce').val('');
                        $('#ProduceType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                    });
                },
                traditional: true,
                error: function (data) { console.log(data) }
            });
        }
        else { alert('Select forest produce checkbox to continue'); }
    }
}

function DeleteProduce(ID) {
    $("#tbl_Produce").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteProduce',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Producerowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.Producerowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ProduceType + "</td><td style='border:1px'>" + items.SpeciesName + "</td><td style='border:1px'>" + items.QuantityOfProduce + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteVechile(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tbl_Produce").append(bardata);
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });
}

function AddEquipment() {

    if ($('#EquipmentType option:selected').text() == '--Select--') {
        $('#EquipmentType').focus();
    }
    else if ($('#EquipmentMake').val() == '') {
        $('#EquipmentMake').focus();
        $('#EquipmentMake').attr('placeholder', 'Enter equipment make');
    }
    else if ($('#EquipmentModel').val() == '') {
        $('#EquipmentModel').focus();
        $('#EquipmentModel').attr('placeholder', 'Enter equipment model');
    }
    else if ($('#EquipmentCaliber').val() == '') {
        $('#EquipmentCaliber').focus();
        $('#EquipmentCaliber').attr('placeholder', 'Enter caliber');
    }
    else if ($('#EquipmentIdentificationNo').val() == '') {
        $('#EquipmentIdentificationNo').focus();
        $('#EquipmentIdentificationNo').attr('placeholder', 'Enter identification no.');
    }
    else if ($('#EquipmentSize').val() == '') {
        $('#EquipmentSize').focus();
        $('#EquipmentSize').attr('placeholder', 'Enter equipmentSize size');
    }
    else if ($('#EquipmentUploadDoc').val() == '') {
        $('#EquipmentUploadDoc').focus();
        $('#errordivUpload').show();
        $('#errordivUpload').html("upload valid file!!!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    }
    else {
        if ($('#Equipment').prop('checked')) {
            $("#tblEquipment").empty();
            var Equipmentinfo = {
                EquipmentType: $('#EquipmentType option:selected').val(), EquipmentMake: $('#EquipmentMake').val(),
                EquipmentModel: $('#EquipmentModel').val(), EquipmentCaliber: $('#EquipmentCaliber').val(),
                EquipmentIdentificationNo: $('#EquipmentIdentificationNo').val(), EquipmentSize: $('#EquipmentSize').val(),
                EquipmentUploadDoc: $('#hdEquipmentDoc').val()
            }
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(Equipmentinfo),
                url: RootUrl + 'OffenseRegistrationfinal/AddEquipment',
                success: function (data) {
                    $.each(data, function (i, items) {
                        var id = "'" + items.Equipmentrowid + "'";
                        var count = 1;
                        count = count + i;
                        var bardata = "<tr style='border:1px' class='" + items.Equipmentrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.EquipmentType + "</td><td style='border:1px'>" + items.EquipmentModel + "</td><td style='border:1px'>" + items.EquipmentIdentificationNo + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteEquipment(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                        $("#tblEquipment").append(bardata);
                        $('#EquipmentMake,#EquipmentModel,#EquipmentCaliber,#EquipmentIdentificationNo,#EquipmentSize').val('');
                        $('#EquipmentType option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                    });
                },
                traditional: true,
                error: function (data) { console.log(data) }
            });
        }
        else { alert('Select equipment checkbox to continue');}
    }
}

function DeleteEquipment(ID) {
    $("#tblEquipment").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteEquipment',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Equipmentrowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.Equipmentrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.EquipmentType + "</td><td style='border:1px'>" + items.EquipmentModel + "</td><td style='border:1px'>" + items.EquipmentIdentificationNo + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteEquipment(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblEquipment").append(bardata);
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });
}

function AddAnimal() {

    if ($('#AnimalScientificName').val() == '') {
        $('#AnimalScientificName').focus();
        $('#AnimalScientificName').attr('placeholder', 'Enter scientific name');
    }
    else if ($('#AnimalCommanName').val() == '') {
        $('#AnimalCommanName').focus();
        $('#AnimalCommanName').attr('placeholder', 'Enter comman name');
    }
    else if ($('#AnimalDescription').val() == '') {
        $('#AnimalDescription').focus();
        $('#AnimalDescription').attr('placeholder', 'Enter description');
    }
    else if ($('#AnimalWeight').val() == '') {
        $('#AnimalWeight').focus();
        $('#AnimalWeight').attr('placeholder', 'Enter weight');
    }
    else if ($('#AnimalUploadDoc').val() == '') {
        $('#AnimalUploadDoc').focus();
        $('#errordivUpload').show();
        $('#errordivUpload').html("upload valid file!!!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    }
    else {
        if ($('#Animal').prop('checked')) {
            $("#tblAnimal").empty();
            var Animalinfo = {
                AnimalScientificName: $('#AnimalScientificName').val(), AnimalCommanName: $('#AnimalCommanName').val(),
                AnimalDescription: $('#AnimalDescription').val(), AnimalWeight: $('#AnimalWeight').val(), AnimalUploadDoc: $('#hdAnimalDoc').val()
            }
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(Animalinfo),
                url: RootUrl + 'OffenseRegistrationfinal/AddAnimal',
                success: function (data) {
                    $.each(data, function (i, items) {
                        var id = "'" + items.Animalrowid + "'";
                        var count = 1;
                        count = count + i;
                        var bardata = "<tr style='border:1px' class='" + items.Animalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.AnimalScientificName + "</td><td style='border:1px'>" + items.AnimalCommanName + "</td><td style='border:1px'>" + items.AnimalWeight + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteAnimal(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                        $("#tblAnimal").append(bardata);
                        $('#AnimalScientificName,#AnimalCommanName,#AnimalDescription,#AnimalWeight').val('');
                    });
                },
                traditional: true,
                error: function (data) { console.log(data) }
            });
        }
        else { alert('Select Animal checkbox to continue'); }
    }
}

function DeleteAnimal(ID) {
    $("#tblAnimal").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteAnimal',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Animalrowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.Animalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.AnimalScientificName + "</td><td style='border:1px'>" + items.AnimalCommanName + "</td><td style='border:1px'>" + items.AnimalWeight + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteAnimal(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblAnimal").append(bardata);
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });
}

function AddAnimalArticle() {

    if ($('#ArticleAnimalScientificName').val() == '') {
        $('#ArticleAnimalScientificName').focus();
        $('#ArticleAnimalScientificName').attr('placeholder', 'Enter scientific name');
    }
    else if ($('#ArticleAnimalCommanName').val() == '') {
        $('#ArticleAnimalCommanName').focus();
        $('#ArticleAnimalCommanName').attr('placeholder', 'Enter comman name');
    }
    else if ($('#NameOfAnimalArticle').val() == '') {
        $('#NameOfAnimalArticle').focus();
        $('#NameOfAnimalArticle').attr('placeholder', 'Enter article name');
    }
    else if ($('#DescriptionOfAnimalArticle').val() == '') {
        $('#DescriptionOfAnimalArticle').focus();
        $('#DescriptionOfAnimalArticle').attr('placeholder', 'Enter description');
    }
    else if ($('#ArticleQuantity').val() == '') {
        $('#ArticleQuantity').focus();
        $('#ArticleQuantity').attr('placeholder', 'Enter weight');
    }
    else if ($('#AnimalArticleUploadDoc').val() == '') {
        $('#AnimalUploadDoc').focus();
        $('#errordivUpload').show();
        $('#errordivUpload').html("upload valid file!!!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
    }
    else {
        if ($('#AnimalArticle').prop('checked')) {
            $("#tblAnimalArticle").empty();
            var Animalinfo = {
                ArticleAnimalScientificName: $('#ArticleAnimalScientificName').val(), ArticleAnimalCommanName: $('#ArticleAnimalCommanName').val(),
                NameOfAnimalArticle: $('#NameOfAnimalArticle').val(), DescriptionOfAnimalArticle: $('#DescriptionOfAnimalArticle').val(),
                ArticleQuantity: $('#ArticleQuantity').val(), AnimalArticleUploadDoc: $('#hdArticleDoc').val()
            }
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(Animalinfo),
                url: RootUrl + 'OffenseRegistrationfinal/AddAnimalArticle',
                success: function (data) {
                    $.each(data, function (i, items) {
                        var id = "'" + items.ArticleAnimalrowid + "'";
                        var count = 1;
                        count = count + i;
                        var bardata = "<tr style='border:1px' class='" + items.ArticleAnimalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ArticleAnimalScientificName + "</td><td style='border:1px'>" + items.ArticleAnimalCommanName + "</td><td style='border:1px'>" + items.NameOfAnimalArticle + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteAnimalArticle(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                        $("#tblAnimalArticle").append(bardata);
                        $('#ArticleAnimalScientificName,#ArticleAnimalCommanName,#NameOfAnimalArticle,#DescriptionOfAnimalArticle,#ArticleQuantity').val('');
                    });
                },
                traditional: true,
                error: function (data) { console.log(data) }
            });
        }
        else { alert('Select animal article to continue'); }
    }
}

function DeleteAnimalArticle(ID) {

    $("#tblAnimalArticle").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteAnimalArticle',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.ArticleAnimalrowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.ArticleAnimalrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.ArticleAnimalScientificName + "</td><td style='border:1px'>" + items.ArticleAnimalCommanName + "</td><td style='border:1px'>" + items.NameOfAnimalArticle + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteAnimal(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblAnimalArticle").append(bardata);
                $('#ArticleAnimalScientificName,#ArticleAnimalCommanName,#NameOfAnimalArticle,#DescriptionOfAnimalArticle,#ArticleQuantity').val('');
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

}

function AddWitness() {
    var pattern = new RegExp(/^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/);
    if ($('#WitnessName').val() == '') {
        $('#WitnessName').focus();
        $('#WitnessName').attr('placeholder', 'Enter witness name');
        return false;
    }
    else if ($('#FatherName').val() == '') {
        $('#FatherName').focus();
        $('#FatherName').attr('placeholder', 'Enter father name');
        return false;
    }
        //else if ($('#SpouseName').val() == '') {
        //    $('#SpouseName').focus();
        //    $('#SpouseName').attr('placeholder', 'Enter spouse name');
        //}
    else if ($('#Category option:selected').text() == '--Select--') {
        $('#Category').focus();
        $('#Category').attr('placeholder', 'Select category');
        return false;
    }
    else if ($('#ResidentialAddress1').val() == '') {
        $('#ResidentialAddress1').focus();
        $('#ResidentialAddress1').attr('placeholder', 'Enter address1');
        return false;
    }
        //else if ($('#ResidentialAddress2').val() == '') {
        //    $('#ResidentialAddress2').focus();
        //    $('#ResidentialAddress2').attr('placeholder', 'Enter address2');
        //}
    else if ($('#Pincode').val() == '') {
        $('#Pincode').focus();
        $('#Pincode').attr('placeholder', 'Enter pincode');
        return false;
    }
    else if ($('#State').val() == '') {
        $('#State').focus();
        $('#State').attr('placeholder', 'Enter State');
        return false;
    }
    else if ($('#District option:selected').text() == '--Select--') {
        $('#District').focus();
        $('#District').attr('placeholder', 'Enter district');
        return false;
    }
    else if ($('#Village').val() == '') {
        $('#Village').focus();
        $('#Village').attr('placeholder', 'Enter village');
        return false;
    }
    else if ($('#PhoneNo').val() == '') {
        $('#PhoneNo').focus();
        $('#PhoneNo').attr('placeholder', 'Enter phoneNo');
        return false;
    }
    else if (!pattern.test($('#EmailId').val())) {
        $('#EmailId').focus();
        alert('Enter valid emailId');
        return false;
    }
    else if ($('#PhotoId').val() == '') {
        $('#PhotoId').focus();
        $('#PhotoId').attr('placeholder', 'Enter photoId');
        return false;
    }
    else if ($('#UploadId').val().length == 0 && $('#hdUploadId').val() == '') {
        $('#UploadId').focus();
        $('#UploadId').attr('placeholder', 'Enter uploadId');
        return false;
    }
    else if ($('#WitnessAge').val() == '') {
        $('#WitnessAge').focus();
        $('#WitnessAge').attr('placeholder', 'Enter witness age');
        return false;
    }
    else if ($('#StatementDate').val() == '') {
        $('#StatementDate').focus();
        return false;
    }
    else if ($('#WitnessStatement').val() == '') {
        $('#WitnessStatement').focus();
        $('#WitnessStatement').attr('placeholder', 'Enter witness statement');
        return false;
    }
    else if ($('#UploadSignedStatement').val().length == 0 && $('#hdUploadSignedStatement').val() == '') {
        $('#UploadSignedStatement').focus();
        return false;
    }
    else {
        $("#tblWitness").empty();
        var Witnessinfo = {
            Witnessrowid: $('#hdnwitnessrowid').val(), EmailId: $("#EmailId").val(),
            WitnessName: $('#WitnessName').val(), FatherName: $('#FatherName').val(), SpouseName: $('#SpouseName').val(), Category: $('#Category').val(), Caste: $('#Caste').val(),
            ResidentialAddress1: $('#ResidentialAddress1').val(), ResidentialAddress2: $('#ResidentialAddress2').val(), Pincode: $('#Pincode').val(),
            Village: $('#Village').val(), District: $('#District option:selected').val(), State: $('#State option:selected').val(), PhoneNo: $('#PhoneNo').val(), PhotoId: $('#PhotoId').val(),
            UploadId: $('#hdUploadId').val(), WitnessAge: $('#WitnessAge').val(), StatementDate: $('#StatementDate').val(), WitnessStatement: $('#WitnessStatement').val(),
            UploadSignedStatement: $('#hdUploadSignedStatement').val()
        }
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(Witnessinfo),
            url: RootUrl + 'OffenseRegistrationfinal/AddWitness',
            success: function (data) {
                $.each(data, function (i, items) {
                    var id = "'" + items.Witnessrowid + "'";
                    var count = 1;
                    count = count + i;
                    //var bardata = "<tr style='border:1px' class='" + items.Witnessrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.WitnessName + "</td><td style='border:1px'>" + items.FatherName + "</td><td style='border:1px'>" + items.District + "</td><td style='border:1px'>" + items.State + "</td><td style='border:1px'>" + "<button type=button class=get-delete onclick=DeleteWitness(" + id + ")>Delete</button>" + "</td></tr>";
                    var bardata = "<tr style='border:1px' class='" + items.Witnessrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.WitnessName + "</td><td style='border:1px'>" + items.FatherName + "</td><td style='border:1px'>" + items.ResidentialAddress1 + "</td><td style='border:1px'>" + items.State + "</td><td style='border:1px'>" + "<button type=button class='btn btn-success btn-circle' data-toggle=modal data-target=#myModalWitness style=cursor:pointer onclick=ViewWitness(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td style='border:1px'>" + "<button type=button class='btn btn-warning btn-circle' onclick=EditWitness(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteWitness(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                    $("#tblWitness").append(bardata);
                    $('#WitnessName,#FatherName,#SpouseName,#Caste,#ResidentialAddress1,#ResidentialAddress2').val('');
                    $('#Pincode,#Village,#District,#State,#PhoneNo,#PhotoId,#UploadId,#WitnessAge,#StatementDate').val('');
                    $('#WitnessStatement,#UploadSignedStatement').val(''); $('#hdUploadId').val('');
                    $('#viewdoc,#viewdoc1').hide(); $('#hdUploadSignedStatement').val('');
                    $('#Category option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
            },
            traditional: true,
            error: function (data) { console.log(data) }
        });
    }
}

function EditWitness(ID) {
    $('#hdnwitnessrowid').val(ID);
    var tblinfo = { ID: ID }
    $("#tbdyScart").empty();
    $.ajax({
        type: 'POST',
        url: RootUrl + 'OffenseRegistrationfinal/EditDetails',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(tblinfo),
        success: function (data) {

            getvillage(data.District, data.Village);
            $("#WitnessName").val(data.WitnessName); $("#FatherName").val(data.FatherName); $("#SpouseName").val(data.SpouseName); $("#Category").val(data.Category);
            $("#Caste").val(data.Caste); $("#ResidentialAddress1").val(data.ResidentialAddress1); $("#ResidentialAddress2").val(data.ResidentialAddress2);
            $("#Pincode").val(data.Pincode); $("#District").val(data.District); $("#Village").val(data.Village); $("#State").val(data.State);
            $("#PhoneNo").val(data.PhoneNo); $("#EmailId").val(data.EmailId); $("#PhotoId").val(data.PhotoId);
            $("#WitnessAge").val(data.WitnessAge); $("#StatementDate").val(data.StatementDate); $("#WitnessStatement").val(data.WitnessStatement);
            $('#hdUploadId').val(data.UploadId); $('#hdUploadSignedStatement').val(data.UploadSignedStatement);
            var filename = data.UploadId;
            var str = filename.split('/');
            if (data.UploadId != '' && data.UploadId != null) {
                $('#viewdoc').show();
                $('#viewdoc').attr('href', '.././ForestProtectionDocument/' + str[2]);
                $('#viewdoc').attr('target', '_blank');
            }

            var filename1 = data.UploadSignedStatement;
            var str1 = filename1.split('/');
            if (data.UploadSignedStatement != '' && data.UploadSignedStatement != null) {
                $('#viewdoc1').show();
                $('#viewdoc1').attr('href', '.././ForestProtectionDocument/' + str1[2]);
                $('#viewdoc1').attr('target', '_blank');
            }
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });

}

function ViewWitness(ID) {
    // $('#hdnwitnessrowid').val(ID);
    var tblinfo = { ID: ID }
    $("#tbdyWitness").empty();
    $.ajax({
        type: 'POST',
        url: RootUrl + 'OffenseRegistrationfinal/EditDetails',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(tblinfo),
        success: function (data) {

            var filename = data.UploadId;
            if (data.UploadId != '' && data.UploadId != null) {
                var str = filename.split('/');
            }
            var filename1 = data.UploadSignedStatement;
            if (data.UploadSignedStatement != '' && data.UploadSignedStatement != null) {
                var str1 = filename1.split('/');
            }
            $("#District").val(data.District);
            $("#Category").val(data.Category);
            var district = $("#District option:selected").text();
            var Category = $("#Category option:selected").text();

            var bardata = "<tr><td>Witness Name</td><td>" + data.WitnessName +
                 "</td></tr><tr><td>Father Name</td><td>" + data.FatherName +
                 "</td></tr><tr><td>Spouse Name</td><td>" + data.SpouseName +
                 "</td></tr><tr><td>Category</td><td>" + Category +
                 "</td></tr><tr><td>Caste</td><td>" + data.Caste +
                 "</td></tr><tr><td>Witness Age</td><td>" + data.WitnessAge +
                 "</td></tr><tr><td>Address1</td><td>" + data.ResidentialAddress1 +
                 "</td></tr><tr><td>Address2</td><td>" + data.ResidentialAddress2 +
                 "</td></tr><tr><td>District</td><td>" + district +
                 "</td></tr><tr><td>State</td><td>" + data.State +
                 "</td></tr><tr><td>Pincode</td><td>" + data.Pincode +
                 "</td></tr><tr><td>PhoneNo</td><td>" + data.PhoneNo +
                 "</td></tr><tr><td>EmailId</td><td>" + data.EmailId +
                 "</td></tr><tr><td>Identity Proof</td><td><a href='.././ForestProtectionDocument/" + str[2] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>" +
                 "</td></tr><tr><td>Upload Signed Statement</td><td><a href='.././ForestProtectionDocument/" + str1[2] + "'  target='_blank'><img src='.././images/jpeg.png' Width='30' /></a></td></tr>" +
                 "</td></tr><tr><td>Statement Date</td><td>" + data.StatementDate +
                  "</td></tr><tr><td>Witness Statement</td><td>" + data.WitnessStatement +
                 "</td></tr>";
            $("#tbdyWitness").append(bardata);
            $('#District option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
            $('#Category option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
            $('#Village option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });

}

function getvillage(distcode, villcode) {

    $.ajax({
        type: 'POST',
        url: RootUrl + 'OffenseRegistration/getVillage', // we are calling json method
        dataType: 'json',
        data: { dist_code: distcode },
        success: function (states) {
            $.each(states, function (i, vill) {
                $("#Village").append('<option value="' + vill.Value + '">' +
                 vill.Text + '</option>');
            });
            $("#Village").val(villcode);
        },
        error: function (ex) {
            alert('Failed to retrieve states.' + ex);
        }
    });
}

function DeleteWitness(ID) {
    $("#tblWitness").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteWitness',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Witnessrowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr style='border:1px' class='" + items.Witnessrowid + "'><td style='border:1px'>" + count + "</td><td style='border:1px'>" + items.WitnessName + "</td><td style='border:1px'>" + items.FatherName + "</td><td style='border:1px'>" + items.ResidentialAddress1 + "</td><td style='border:1px'>" + items.State + "</td><td style='border:1px'>" + "<button type=button class='btn btn-success btn-circle' data-toggle=modal data-target=#myModalWitness style=cursor:pointer onclick=ViewWitness(" + id + ")><i class='fa fa-eye'></i></button>" + "</td><td style='border:1px'>" + "<button type=button class='btn btn-warning btn-circle' style=cursor:pointer onclick=EditWitness(" + id + ")><i class='fa fa-edit'></i></button>" + "</td><td style='border:1px'>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteWitness(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblWitness").append(bardata);
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

}

function AddWarrant() {

    if ($('#NameOfOffender').val() == '') {
        $('#NameOfOffender').focus();
        $('#NameOfOffender').attr('placeholder', 'Enter offender name');
    }
    else if ($('#ClothesWorn').val() == '') {
        $('#ClothesWorn').focus();
        $('#ClothesWorn').attr('placeholder', 'Enter clothes worn');
    }
    else if ($('#ColorOfClothes').val() == '') {
        $('#ColorOfClothes').focus();
        $('#ColorOfClothes').attr('placeholder', 'Enter color of clothes');
    }
    else if ($('#PhysicalAppearance').val() == '') {
        $('#PhysicalAppearance').focus();
        $('#PhysicalAppearance').attr('placeholder', 'Enter physical appearance');
    }
    else if ($('#Height').val() == '') {
        $('#Height').focus();
        $('#Height').attr('placeholder', 'Enter height');
    }
    else if ($('#OtherSpecialDetails').val() == '') {
        $('#OtherSpecialDetails').focus();
        $('#OtherSpecialDetails').attr('placeholder', 'Enter other special details');
    }
    else if ($('#Appearancedate').val() == '') {
        $('#Appearancedate').focus();
    }
    else if ($('#Appearancetime').val() == '') {
        $('#Appearancetime').focus();
    }
    else if ($('#AppearancePlace option:selected').val() == '') {
        $('#AppearancePlace').focus();
    }
    else if ($('#AppearancePlace option:selected').text() == '--Select--') {
        $('#AppearancePlace').focus();
    }
    else {
        var Warrantinfo = {
            NameOfOffender: $('#NameOfOffender').val(), ClothesWorn: $('#ClothesWorn').val(),
            ColorOfClothes: $('#ColorOfClothes').val(), PhysicalAppearance: $('#PhysicalAppearance').val(),
            Height: $('#Height').val(), OtherSpecialDetails: $('#OtherSpecialDetails').val(),
            Appearancedate: $('#Appearancedate').val(), Appearancetime: $('#Appearancetime').val(),
            AppearancePlace: $('#AppearancePlace option:selected').val(), Warrantrowid: $('#hdnWarrantrowid').val()
        }
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(Warrantinfo),
            url: RootUrl + 'OffenseRegistrationfinal/AddWarrant',
            success: function (data) {
               // $("#tblwarrant").empty();
                $.each(data, function (i, items) {
                    var id = "'" + items.Warrantrowid + "'";
                    $('#AppearancePlace').val(items.AppearancePlace);
                    var AppearancePlace = $('#AppearancePlace option:selected').text();
                    var bardata = "<tr><td style=display:none;>" + items.Warrantrowid + "</td><td>" + items.NameOfOffender + "</td><td>" + items.ClothesWorn + "</td><td>" + items.ColorOfClothes + "</td><td>" + items.PhysicalAppearance + "</td><td>" + items.Appearancedate + "</td><td>" + items.Appearancetime + "</td><td>" + AppearancePlace + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteWarrant(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";

                    //alert($("#tblwarrant").html());
                    //$('#tblWarrant tbody tr td:nth-child(1)').each(function () {
                    //    alert(items.Warrantrowid);
                    //   // alert($(this).text());
                    //    if (items.Warrantrowid == $(this).text()) {                            
                    //        $(this).closest('tr').remove();
                    //    }
                    //});
                    $("#tblwarrant").append(bardata);
                    $('#NameOfOffender,#ClothesWorn,#ColorOfClothes,#PhysicalAppearance,#Height,#OtherSpecialDetails,#Appearancedate,#Appearancetime,#hdnWarrantrowid').val('');
                    $('#AppearancePlace option').filter(function (e) { return $(this).text() == '--Select--' }).attr('selected', true);
                });
                                             
            },
            traditional: true,
            error: function (data) { console.log(data) }
        });
    }
}

function DeleteWarrant(ID) {

    $("#tblwarrant").empty();
    var RowId = {
        Id: ID
    };
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(RowId),
        url: RootUrl + 'OffenseRegistrationfinal/DeleteWarrant',
        success: function (data) {
            $.each(data, function (i, items) {
                var id = "'" + items.Warrantrowid + "'";
                var count = 1;
                count = count + i;
                var bardata = "<tr class='" + items.Warrantrowid + "'><td>" + items.NameOfOffender + "</td><td>" + items.ClothesWorn + "</td><td>" + items.ColorOfClothes + "</td><td>" + items.PhysicalAppearance + "</td><td>" + "<button type=button class='btn btn-danger btn-circle' onclick=DeleteWarrant(" + id + ")><i class='fa fa-times'></i></button>" + "</td></tr>";
                $("#tblwarrant").append(bardata);              
            });
        },
        traditional: true,
        error: function (data) { console.log(data) }
    });

}

function UploadFileValidation(fval, fsize, fname) {   
    var exts = ['jpeg', 'jpg', 'pdf', 'png', 'gif'];    
    if (fsize > 100) {          
           // $('#errordivUpload').show();
           // $('#errordivUpload').html("Upload ID should not be larger than 100kb!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
        alert('Upload document should not be larger than 100kb!');
        $('#' + fname).val('');
            $('#' + fname).focus();
            return false;
    }
    if (fval) {
        var get_ext = fval.split('.');
        get_ext = get_ext.reverse();
        if ($.inArray(get_ext[0].toLowerCase(), exts) == -1) {          
          //  $('#errordivUpload').show();
          //  $('#errordivUpload').html("Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field!" + "<i class='fa fa-times fa-fw' style='float: right;padding: 2px;'></i>");
            alert('Please upload only jpeg or jpg or pdf or png or gif file format in Upload ID Field!');
            $('#' + fname).val('');
            $('#' + fname).focus();
            return false;
        }
     }                   
}

  
function reload() {
    location.reload(true);
}