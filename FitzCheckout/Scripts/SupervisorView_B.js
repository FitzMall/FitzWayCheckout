$(document).ready(function () {
    $(document).on('change', '#statusList', function () {
        if($(this).val() === "Reopen")
        {
            $(document).find('#techniciansDiv').show();
        }
        else
        {
            $(document).find('#techniciansDiv').hide();
        }
    });

    $('#saveStatus').click(function () {
        let statusList = $(document).find('#statusList');
        let technicians = $(document).find('#technicians');

        if (statusList.val() === 'Select One')
        {
            alert('Please select a status');
        }
        else if (statusList.val() === 'Reopen' && technicians.val() == -1)
        {
            alert('Please select a Technician to re-open an inspection');
        }
        else
        {
            let recordID = $('#RecordID').val();
            let newStatus = statusList.val();
            let newTechnician = technicians.val();
            let newData = '{ recordID: "' + recordID + '",' +
                        'Status: "' + newStatus + '",' +
                        'Technician: "' + newTechnician + '"}';

            $.ajax({
                type: 'POST',
                url: '/FitzwayCheckout/Supervisor/ViewInspection',
                data: newData,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    if (typeof response.pdfUrl !== 'undefined')
                    {
                        window.open(response.pdfUrl, ' _blank');
                    }
                    window.location.replace(response.newUrl);
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.statusText);
                }
            });
        }
    });

    $('.close').click(function () {
        var i = 3;
    });
});