$(document).ready(function () {
    $('#calendar').fullCalendar({
        height: 500,
        theme: true,
        themeSystem: 'bootstrap3',
        tooltip:'Please click for day wise schedule.',
        header:
        {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        buttonText: {
            today: 'today',
            month: 'month',
            week: 'week',
            day: 'day'
        },

        events: function (start, end, timezone, callback) {
            $.ajax({
                url: '../EventManagement/GetEventCalendar',
                type: "GET",
                dataType: "JSON",

                success: function (result) {
                    var events = [];

                    $.each(result, function (i, data) {
                        // debugger;
                        events.push(
                       {
                           ID: data.ID,
                           title: data.Title,
                           description: data.Desc,
                           start: moment(data.Start_Date).format('YYYY-MM-DD HH:mm'),
                           end: moment(data.End_Date).format('YYYY-MM-DD HH:mm'),
                           backgroundColor: "#9501fc",
                           borderColor: "#fc0101"
                       });
                    });

                    callback(events);
                }
            });
        },

        eventClick: function (calEvent, jsEvent, view) {
            //alert('Clicked on: ' + date.format('DD-MM-YYYY HH:mm') + ', Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY + ', Current view:' + view.name);
            //alert(calEvent.ID);
            $.ajax({
                type: 'GET',
                url: '../EventManagement/GetEventCalendarWithID?ID=' + calEvent.ID,
                success: function (response) {
                    //debugger;
                    $('#modalWorkFlowDetails').show();
                    $('#txtEventTitle').val(response[0]["Title"]);
                    $('#txtEventName').val(response[0]["EventName"]);
                    $('#txtEventDescription').val(response[0]["Desc"]);
                    $('#AdminTemplateUpdateForSMS').val(response[0]["SMSTemplate"]);
                    var htmldata = response[0]["EmailTemplate"];
                    CKEDITOR.instances['AdminTemplateUpdate'].setData(htmldata);
                    $('#txtEventEDateTime').inputmask("datetime", {
                        mask: "1-2-y h:s",
                        placeholder: "dd-mm-yyyy hh:mm",
                        leapday: "-02-29",
                        separator: "-"
                        //alias: "dd-mm-yyyy"
                    });
                    $('#txtEventSDateTime').inputmask("datetime", {
                        mask: "1-2-y h:s",
                        placeholder: "dd-mm-yyyy hh:mm",
                        leapday: "-02-29",
                        separator: "-"
                        //alias: "dd-mm-yyyy"
                    });
                    $('#txtEventSDateTime').val(moment(response[0]["Start_Date"]).format('DD-MM-YYYY HH:mm'));
                    $('#txtEventEDateTime').val(moment(response[0]["End_Date"]).format('DD-MM-YYYY HH:mm'));
                    if(response[0]["ActiveStatus"]=="True")
                    {
                        $("#ActiveStatus").prop("checked", true);
                    }
                    else
                    {
                        $("#ActiveStatus").prop("checked", false);
                    }
                    $('#chkEmail').prop('checked', true);
                    $('#divEmail').show();
                    $('#chkSMS').prop('checked', true);
                    $('#divSMS').show();
                },
                error: function (ex) {
                    alert(ex.error);
                }
            });
        },

        eventRender: function (event, element) {
            element.qtip(
            {
                content: event.description
            });
        },

        editable: false,
        dayClick: function (date, jsEvent, view) {
            //alert('Clicked on: ' + date.format('DD-MM-YYYY HH:mm') + ', Coordinates: ' + jsEvent.pageX + ',' + jsEvent.pageY + ', Current view:' + view.name);
            $('#txtEventSDateTime').val(date.format('DD-MM-YYYY HH:mm'));
            $('#txtEventEDateTime').val(date.format('DD-MM-YYYY HH:mm'));
            $('#txtEventEDateTime').inputmask("datetime", {
                mask: "1-2-y h:s",
                placeholder: "dd-mm-yyyy hh:mm",
                leapday: "-02-29",
                separator: "-"
                //alias: "dd-mm-yyyy"
            });
            $('#txtEventSDateTime').inputmask("datetime", {
                mask: "1-2-y h:s",
                placeholder: "dd-mm-yyyy hh:mm",
                leapday: "-02-29",
                separator: "-"
                //alias: "dd-mm-yyyy"
            });
            $('#txtEventTitle').val('');
            $('#txtEventName').val('');
            $('#txtEventDescription').val('');
            $('#AdminTemplateUpdate').html('');
            $('#AdminTemplateUpdateForSMS').text('');
            $("#ActiveStatus").prop("checked", true);
            $('#modalWorkFlowDetails').hide();
            $.ajax({
                type: 'GET',
                url: '../EventManagement/GetEventCalendar',
                dataType: 'HTML',
                success: function (data) {
                    $('#agendaDiv').html(data);
                    $('#modalWorkFlowDetails').show();
                },
                error: function (ex) {
                    alert(ex.error);
                }
            });
        }
    });


});