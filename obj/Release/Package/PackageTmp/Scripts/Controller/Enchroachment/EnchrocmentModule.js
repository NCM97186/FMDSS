function GetEncroachmentDetails(EnchId) {

    $.ajax({
        type: 'GET',
        url: "../Encroachment/GetEncroachmentDetails?EnchId=" + EnchId,
        dataType: 'html',
        success: function (data1) {
            console.log(data1)
            $('#myModalEnchrocment').html(data1);
            $('#myModal').modal('show');
        },
        error: function (ex) {
            alert(ex.error);
        }
    });

};