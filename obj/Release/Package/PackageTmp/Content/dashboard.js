$(document).ready(function () {
    var content;
    getdata();
    $('.count').each(function () {
        $(this).prop('Counter', 0).animate({
            Counter: $(this).text()
        }, {
            duration: 4000,
            easing: 'linear',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });
    
});
//$(document).ajaxStart(function () {
    
//    $("div#divLoading").removeClass("hide").addClass('show');
//});
//$(document).ajaxStop(function () {
//    $("div#divLoading").removeClass("show").addClass('hide');
//});
function GoPlantationMonitoring() {
    var spUrl = '../SystemAdmin/PostUserDetails';
    window.open(spUrl, "blank");
}
function OpenWindow(id) {
    //Edit by Sunny for EventManagemnet or My schedular
    if (id == 5) {
        //alert(id);
        window.location.href = "/EventManagement/EventManagement";
    }
    else {
        $.ajax({
            type: 'GET',
            url: "../SystemAdmin/GetModal?ModuleId=" + id,
            dataType: 'html',
            success: function (data) {
                $('#modelPlace').html(data);
                $("#myModal").modal('show');
            },
            error: function (ex) {
                alert(ex.error);
            }
        });
    }
}
function getdata() {
    $.ajax({
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        url: "../SystemAdmin/GetEmailStatus",
        dataType: 'JSON',
        success: function (data) {
            //console.log(data);
            //debugger;
            if (data.length > 0) {
                content = data[0].EmailContent;
                $('#EmailContent').html(content);
                document.getElementById('#divEmailContent').style.display = "block";
            }
            else {
                document.getElementById('#divEmailContent').style.display = "none";
            }
        },
        complete: function () {
            // Schedule the next request when the current one's complete
            setInterval(getdata, 60000)
        },
        error: function (ex) {
            //alert(ex.error);
        }
    });
}