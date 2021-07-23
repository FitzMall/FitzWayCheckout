var opt = {
    autoOpen: false,
    modal: false//,
    //width: 550,
    //height: 650,
    //title: 'Details'
};

$(document).ready(function () {

    $('#new').click(function () {
        //var metadataValues = '1047866';
        $.ajax({
            type: 'POST',
            url: '/Inspection/Details',
            data: '{metadataValues: "" }',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (response) {
                $('#dialog').html(response);
                $('#dialog').show('fade', 100);
                $('#SearchBox').hide();

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.statusText);
            }
        });
    });

    $('#search').click(function () {
        var metadataValues = $('#MetaDataValue1').val() + ',' +
            $('#MetaDataValue2').val() + ',' +
            $('#MetaDataValue3').val() + ',' +
            $('#MetaDataValue4').val() + ',' +
            $('#MetaDataValue5').val() + ',' +
            $('#MetaDataValue6').val() + ',' +
            $('#MetaDataValue7').val() + ',,'+

        $.ajax({
            type: 'POST',
            url: '/Inspection/Details',
            data: '{metadataValues: "' + metadataValues + '" }',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (response) {
                $('#dialog').html(response);
                $('#dialog').show('fade', 100);
                $('#SearchBox').hide();;
                var table = $('#matchingVehicles').DataTable();
                table.columns.adjust().draw();
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.statusText);
            }
        });
    });

    $('#userSearch').click(function () {
        var metadataValues = ',,,,,' + $('#stockNumber').val() + ',' + $('#vin').val();

        $.ajax({
            type: 'POST',
            url: '/Inspection/Details',
            data: '{metadataValues: "' + metadataValues + '" }',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (response) {
                    $('#dialog').html(response);
                    $('#dialog').show('fade', 100);
                    $('#SearchBox').hide();;
                    var table = $('#matchingVehicles').DataTable();
                    table.columns.adjust().draw();
                //if (response.isUrl)
                //    window.location = response.newurl;
                //else
                //{
                //    $('#dialog').html(response.results);
                //    $('#dialog').show('fade', 100);
                //    $('#SearchBox').hide();;
                //    var table = $('#matchingVehicles').DataTable();
                //    table.columns.adjust().draw();
                //}
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.statusText);
            }
        });
    });



});