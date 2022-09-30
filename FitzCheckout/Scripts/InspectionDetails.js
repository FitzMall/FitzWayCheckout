var opt = {
    autoOpen: false,
    modal: false//,
    //width: 550,
    //height: 650,
    //title: 'Details'
};

$(document).ready(function () {

    // Sections 1 - 4 and 6 have items that have checkboxes before their items
    const SECTION1ID = 1;
    const SECTION2ID = 2;
    const SECTION3ID = 3;
    const SECTION4ID = 4;
    const SECTION6ID = 6;

    //These items have checkboxes before their text and are the first items in their sections
    const SECTION1ITEMID = 1;
    const SECTION3ITEMID = 6;

    //Updates Sidebar highlights when clicked
    $('.sidebar a').click(function () {
        $(this).removeClass('complete').removeClass('warning').addClass('active').siblings().removeClass('active');
        let linkID = $(this).attr('id');
        let lastIndex = linkID.lastIndexOf('_') + 1;
        let sectionID = linkID.substring(lastIndex);
        let sectionTableID = 'Section_' + sectionID;
        $(document).find('.checklistSection').each(function () {                
            if ($(this).attr('id') !== sectionTableID)
            {
                let hasInitialCheckbox = $(this).find('tr:eq(3)').find('.initialCheckbox').length > 0 ? true : false;
                if (hasInitialCheckbox)
                {
                    CheckSectionsWithInitialCheckbox($(this));
                }
                else
                {
                    CheckItemlistSectionForComplete($(this));
                }
            }
        });
    })

    //Spare tire selection change - shows/hides tread measurement depending on selected value
    $(document).on('change', '.spareTireDropdown1', function () {

        var treadDropdown = $(this).parent('td').find('.spareTreadDropdown');
        if (this.value === 'Full Size') {
            treadDropdown.show();
        }
        else {
            treadDropdown.val('Select One');
            treadDropdown.hide();
        }
        CheckItemlistSectionForComplete($(this).closest('table'));

    });


    //Dropdown selection change: updates sidebar highlight on change
    $(document).on('change', '.treadDropdown1', function () {
        if ($(this).val() === "Select One")
        {
            $(this).parent('td').find('.measurementError').show();
        }
        else
        {
            $(this).parent('td').find('.measurementError').hide();
        }
        var sectionTable = $(this).closest('table');
        CheckItemlistSectionForComplete(sectionTable);
    });

    $(document).on('change', '.spareTireDropdown1', function () {
        let measurementError = $(this).parent('td').find('.measurementError')
        if ($(this).val() === "Select One") {
            measurementError.text('Select a spare tire type');
            measurementError.show();
        }
        else {
            $(this).parent('td').find('.measurementError').hide();
        }
        var sectionTable = $(this).closest('table');
        CheckItemlistSectionForComplete(sectionTable);
    });

    $(document).on('change', '.brakeLiningDropdown', function () {
        if ($(this).val() === "Select One") {
            $(this).parent('td').find('.measurementError').show();
        }
        else {
            $(this).parent('td').find('.measurementError').hide();
        }
        var sectionTable = $(this).closest('table');
        CheckItemlistSectionForComplete(sectionTable);
    });

    $(document).on('change', '.spareTreadDropdown', function () {
        if ($(this).val() === "Select One") {
            $(this).parent('td').find('.measurementError').show();
        }
        else {
            $(this).parent('td').find('.measurementError').hide();
        }
        var sectionTable = $(this).closest('table');
        CheckItemlistSectionForComplete(sectionTable);
    });




    //Handles Section 1 and 3 checkbox changes: checks/unchecks section 2 or 4 "N/A" checkboxes

    $(document).on('click', '.initialCheckbox', function () {
        var sectionTable = $(this).closest('table');
        let sectionID = sectionTable.attr('data-itemID');
        let itemID = $(this).attr('id');
        let checked = $(this).prop('checked');
        if (sectionID == SECTION1ID || sectionID == SECTION3ID) {

            let nextSectionTable = sectionTable.next('table');
            let initialCheckbox = nextSectionTable.find('tr:eq(2)').find('td:eq(0)').find('.initialCheckbox');
            if (itemID == SECTION1ITEMID || itemID == SECTION3ITEMID)
            {
                //make sure the other checkboxes in the section (1 or 3) are unchecked if checked is true
                sectionTable.find('tr').each(function (index) {
                    if (index > 2 && checked) {
                        let firstcheckbox = $(this).find('td:eq(0)').find('.initialCheckbox');
                        firstcheckbox.prop('checked', false);
                    }
                })
                //handle the related sections (2, 4)
                nextSectionTable.find('tr').each(function (index) {
                    let firstcheckbox = $(this).find('td:eq(0)').find('.initialCheckbox');
                    if (index == 2 && firstcheckbox.length > 0) //the N/A checkbox for sections 2 or 4
                    {
                        if (checked) {//if the first checkbox of sections 1&3 are checked, check the N/A checkboxes of 2 and 4
                            firstcheckbox.prop('checked', true);
                            firstcheckbox.prop('disabled', false);
                        }
                        else {
                            firstcheckbox.prop('checked', false);
                            firstcheckbox.prop('disabled', true);
                        }
                    }
                    else //if the first checkbox of 1 or 3 are checked uncheck the rest of the checkboxes in sections 2 or 4
                    {
                        if (checked) {
                            firstcheckbox.prop('checked', false);
                        }
                    }
                });
            } else {
                //Make sure the N/A checkboxes are unchecked and disabled
                let i = 0;
                nextSectionTable.find('tr:eq(2)').find('.initialCheckbox').prop('checked', false);
                nextSectionTable.find('tr:eq(2)').find('.initialCheckbox').prop('disabled', true);
            }
        }

        sectionTable.find('tr').each(function (index) {
            if (index > 1 && checked && $(this).attr('data-itemid') != itemID) {
                let firstcheckbox = $(this).find('td:eq(0)').find('.initialCheckbox');
                firstcheckbox.prop('checked', false);
            }
        })


        CheckSectionsWithInitialCheckbox(sectionTable);
    })

    //$(document).on('click', '.initialCheckbox', function () {
    //    var sectionTable = $(this).closest('table');
    //    let sectionID = sectionTable.attr('data-itemID');
    //    let itemID = $(this).attr('id');
    //    let checked = $(this).prop('checked');
    //    if ((sectionID == SECTION1ID && itemID == SECTION1ITEMID) || (sectionID == SECTION3ID && itemID == SECTION3ITEMID))
    //    {
    //        //make sure the other checkboxes in the section (1 or 3) are unchecked if checked is true
    //        sectionTable.find('tr').each(function (index) {
    //            if (index > 2 && checked)
    //            {
    //                let firstcheckbox = $(this).find('td:eq(0)').find('.initialCheckbox');
    //                firstcheckbox.prop('checked', false);
    //            }
    //        })

    //        //handle the related sections (2, 4)
    //        let nextSectionTable = sectionTable.next('table');
    //        let initialCheckbox = nextSectionTable.find('tr:eq(2)').find('td:eq(0)').find('.initialCheckbox');
    //        nextSectionTable.find('tr').each(function (index) {
    //            let firstcheckbox = $(this).find('td:eq(0)').find('.initialCheckbox');
    //            if (index ==2 && firstcheckbox.length > 0) //the N/A checkbox for sections 2 or 4
    //            {
    //                if (checked) {//if the first checkbox of sections 1&3 are checked, check the N/A checkboxes of 2 and 4
    //                    firstcheckbox.prop('checked', true);
    //                    firstcheckbox.prop('disabled', false);
    //                }
    //                else {
    //                    firstcheckbox.prop('checked', false);
    //                    firstcheckbox.prop('disabled', true);
    //                }
    //            }
    //            else //if the first checkbox of 1 or 3 are checked uncheck the rest of the checkboxes in sections 2 or 4
    //            {
    //                if (checked) {
    //                    firstcheckbox.prop('checked', false);
    //                }
    //            }
    //        });
    //    }

    //    sectionTable.find('tr').each(function (index) {
    //        if (index > 1 && checked && $(this).attr('data-itemid') != itemID) {
    //            let firstcheckbox = $(this).find('td:eq(0)').find('.initialCheckbox');
    //            firstcheckbox.prop('checked', false);
    //        }
    //    })


    //    CheckSectionsWithInitialCheckbox(sectionTable);
    //})

    //These functions make sure only one checkbox is checked (for pass, fail, n/a)
    $(document).on('click', '.ChecklistItem1', function () {
        var input = $(this).find('input');
        if (input.attr('type') === 'checkbox' && input.css('visibility') === 'visible') {
            if ($(this).find('input').is(':checked')) {
                $(this).parent('tr').find('.checkboxItem2').attr('checked', false);
                $(this).parent('tr').find('.checkboxItem3').attr('checked', false);
  		$(this).parent('tr').find('.checkboxItem4').attr('checked', false);
                $(this).parent('tr').find('.checkboxItem4').attr('disabled', true);
                $(this).parent('tr').find('.errorMessage').hide();
            }
            else {
                $(this).parent('tr').find('.errorMessage').show();
            }
            CheckItemlistSectionForComplete($(this).closest('table'));
        }
    });

    $(document).on('click', '.ChecklistItem2', function () {
        var input = $(this).find('input');
        if (input.attr('type') === 'checkbox' && input.css('visibility') === 'visible') {
            if ($(this).find('input').is(':checked')) {
                $(this).parent('tr').find('.checkboxItem1').attr('checked', false);
                $(this).parent('tr').find('.checkboxItem3').attr('checked', false);
		$(this).parent('tr').find('.checkboxItem4').attr('checked', false);
                $(this).parent('tr').find('.checkboxItem4').attr('disabled', false);
                $(this).parent('tr').find('.errorMessage').hide();
            }
            else {
                $(this).parent('tr').find('.errorMessage').show();
                $(this).parent('tr').find('.checkboxItem4').attr('disabled', true);
            }
            CheckItemlistSectionForComplete($(this).closest('table'));
        }
    });
    $(document).on('click', '.ChecklistItem3', function () {
        var input = $(this).find('input');
        if (input.attr('type') === 'checkbox' && input.css('visibility') === 'visible') {
            if ($(this).find('input').is(':checked')) {
                $(this).parent('tr').find('.checkboxItem1').attr('checked', false);
                $(this).parent('tr').find('.checkboxItem2').attr('checked', false);
		$(this).parent('tr').find('.checkboxItem4').attr('checked', false);
                $(this).parent('tr').find('.checkboxItem4').attr('disabled', true);
                $(this).parent('tr').find('.errorMessage').hide();
            }
            else {
                $(this).parent('tr').find('.errorMessage').show();
            }
            CheckItemlistSectionForComplete($(this).closest('table'));
        }
    });


    // Updates sidebar highlights when "Repaired to Pass" checkbox is checked/unchecked
    $(document).on('click', '.ChecklistItem4', function () {
        CheckItemlistSectionForComplete($(this).closest('table'));
    })

    //These functions aet all checkboxes in a column as checked for the section 
    $(document).on('click', '.checkAll1', function () {
        var table = $(this).closest('table');
        table.find('.itemlist').each(function () {
            if ($(this).find('.AllowAutoChecking').val() === 'True') {
                $(this).find('.checkboxItem1').prop('checked', true);
                $(this).find('.checkboxItem2').prop('checked', false);
                $(this).find('.checkboxItem3').prop('checked', false);
                $(this).find('.checkboxItem4').prop('checked', false);
                $(this).find('.checkboxItem4').prop('disabled', true);
                $(this).find('.errorMessage').hide();
            }
        });
        CheckItemlistSectionForComplete(table);
    });

    $(document).on('click', '.checkAll2', function () {
        var table = $(this).closest('table');
        table.find('.itemlist').each(function () {
            if ($(this).find('.AllowAutoChecking').val() === 'True') {
                $(this).find('.checkboxItem1').prop('checked', false);
                $(this).find('.checkboxItem2').prop('checked', true);
                $(this).find('.checkboxItem3').prop('checked', false);
                $(this).find('.checkboxItem4').prop('checked', false);
                $(this).find('.errorMessage').hide();
            }
        });
        CheckItemlistSectionForComplete(table);
    });

    $(document).on('click', '.checkAll3', function () {
        var table = $(this).closest('table');
        table.find('.itemlist').each(function () {
            if ($(this).find('.AllowAutoChecking').val() === 'True') {
                $(this).find('.checkboxItem1').prop('checked', false);
                $(this).find('.checkboxItem2').prop('checked', false);
                $(this).find('.checkboxItem3').prop('checked', true);
                $(this).find('.checkboxItem4').prop('checked', false);
                $(this).find('.checkboxItem4').prop('disabled', true);
                $(this).find('.errorMessage').hide();
            }
        });
        CheckItemlistSectionForComplete(table);
    });

    $(document).on('click', '.checkAll4', function () {
        var table = $(this).closest('table');
        table.find('.itemlist').each(function () {
            if ($(this).find('.AllowAutoChecking').val() === 'True') {
                $(this).find('.checkboxItem1').prop('checked', false);
                $(this).find('.checkboxItem2').prop('checked', false);
                $(this).find('.checkboxItem3').prop('checked', false);
                $(this).find('.checkboxItem4').prop('checked', true);
                $(this).find('.errorMessage').hide();
            }
        });
        CheckItemlistSectionForComplete(table);
    });



    //Make sure all required metadata textboxes are filled in and save the checklist
    $(document).on('click', '.saveForm', function () {
        var hasErrors = false;
        if ($('#IsMeta1Required').val() === 'True' && $('#detailMetaDataValue1').val() === '') {
            $('#metaData1Error').show();
            hasErrors = true;
        }

        if ($('#IsMeta2Required').val() === 'True' && $('#detailMetaDataValue2').val() === '') {
            $('#metaData2Error').show();
            hasErrors = true;
        }

        if ($('#IsMeta3Required').val() === 'True' && $('#detailMetaDataValue3').val() === '') {
            $('#metaData3Error').show();
            hasErrors = true;
        }

        if ($('#IsMeta4Required').val() === 'True' && $('#detailMetaDataValue4').val() === '') {
            $('#metaData4Error').show();
            hasErrors = true;
        }

        if ($('#IsMeta5Required').val() === 'True' && $('#detailMetaDataValue5').val() === '') {
            $('#metaData5Error').show();
            hasErrors = true;
        }

        if ($('#IsMeta6Required').val() === 'True' && $('#detailMetaDataValue6').val() === '') {
            $('#metaData6Error').show();
            hasErrors = true;
        }

        if ($('#IsMeta7Required').val() === 'True' && $('#detailMetaDataValue7').val() === '') {
            $('#metaData7Error').show();
            hasErrors = true;
        }

        if (!hasErrors) {
            var data = 'submit=Save&' + $('#SaveChecklist').serialize();
            $.ajax({
                url: '/FitzwayCheckout/Inspection/Save',
                type: 'post',
                dataType: 'json',
                data: data,
                success: function (data) {
                    window.location.replace(data.newUrl);
                },
                failure: function (response) {
                    alert('failure: ' + response.responseText + ": InspectionDetails.js, checklist");
                },
                error: function (response) {
                    alert('error: ' + response.statusText + ": InspectionDetails.js, checklist");
                }
            });
        }
    });

    //Mark Complete functionality
    $(document).on('click', '.submitChecklist', function () {
        //This checks two types of conditions for the checklist to be complete:
        //      1) If the items in the section have a checkbox at the beginning, at least one item in that section has to be checked
        //          - EXCEPTION If the first checkbox in Section 1 or 3 are checked, skip section 2 (when section 1 checked) and skip section 4 (when 3 checked)
        //      2) Otherwise, the item has 'option' checkboxes (1 - 4) and one of those has to be checked.
        var oldSectionID = 0;
        var startsWithCheckbox = false;
        var hasCheckedCheckbox = false;
        var hasErrors = false;
        var skipSection = false;
        var missingFixedToPass = false;

        $('.itemlist').each(function () {
            var currentSectionID = $(this).closest('table').attr('data-itemid');

            //is the section good?
            if (oldSectionID !== 0) {
                if (oldSectionID !== currentSectionID) {
                    if (startsWithCheckbox) {
                        if ((oldSectionID == SECTION2ID || oldSectionID == SECTION4ID) && skipSection) {
                            skipSection = false;
                            $(this).closest('table').prev('table').find('.sectionErrorMessage').hide();
                        }
                        else {
                            var returnedErrors = HasInitialCheckboxErrors($(this), oldSectionID, hasCheckedCheckbox);
                            hasErrors = hasErrors ? hasErrors : returnedErrors;
                        }
                    }
                    oldSectionID = currentSectionID;
                    hasCheckedCheckbox = false;
                    startsWithCheckbox = false;
                }
            }

            //Do the checklist items start with a checkbox
            var checkbox = $(this).find('td:eq(0)').find('input');
            if (checkbox.length > 0) {
                if (!startsWithCheckbox) {
                    startsWithCheckbox = true;

                    // If we're at the first checkbox in section 1 or 3 and it's checked, we can skip section 2 (if currentSectionID = 1) or section 4 (if currentSectionID = 3) 
                    if ((currentSectionID == SECTION1ID || currentSectionID == SECTION3ID) && checkbox.prop('checked')) {
                        skipSection = true;
                    }
                }

                if (checkbox.prop('checked'))
                    hasCheckedCheckbox = true;

            }
            else {
                var errors = CheckItemRowsForErrors($(this), hasErrors, missingFixedToPass)
                hasErrors = errors.hasErrors;
                missingFixedToPass = errors.missingFixedToPass;
            }
            if (oldSectionID === 0)
                oldSectionID = currentSectionID;

        });
        if (!hasErrors) {
            var confirmContinue = true;
            //If there are failed items that haven't been fixed, confirm that they still want to mark checkout complete
            if (missingFixedToPass) {
                if (!confirm('Not all failed items have been marked Fixed To Pass.\n\n        Continue Submitting Inspection?')) {
                    confirmContinue = false;
                }
            }

            if (confirmContinue) {
                var data = 'submit=Submitted&' + $('#SaveChecklist').serialize();
                $.ajax({
                    url: '/FitzwayCheckout/Inspection/Save',
                    type: 'post',
                    dataType: 'json',
                    data: data,
                    success: function (response) {
                        if (typeof response.pdfUrl !== 'undefined') {
                            window.open(response.pdfUrl, ' _blank');
                        }
                        window.location.replace(response.newUrl);
                    },
                    failure: function (response) {
                        alert('failure: ' + response.responseText + ": InspectionDetails.js, SaveChecklist");
                    },
                    error: function (response) {
                        alert('error: ' + response.statusText + ": InspectionDetails.js, SaveChecklist");
                    }
                });
            }
        }

    });

    //Displays/hides section level error messages (sections 1-4, 6) when complete/incomplete
    function HasInitialCheckboxErrors(currentRow, oldSectionID, hasCheckedCheckbox) {
        var hasErrors = false;
        var prevTable = currentRow.closest('table').prev('table');
        if (prevTable.attr('data-itemid') === oldSectionID) {
            if (!hasCheckedCheckbox) {
                prevTable.find('.sectionErrorMessage').show();
                hasErrors = true;
            }
            else {
                prevTable.find('.sectionErrorMessage').hide();
            }
        }
        else {
            //this loops through previous tables until the correct table is found (needed because section 5 uses y/n checkboxes)
            do {
                prevTable = prevTable.prev('table');
            }
            while (prevTable.attr('data-itemid') !== oldSectionID);

            if (!hasCheckedCheckbox) {
                prevTable.find('.sectionErrorMessage').show();
                hasErrors = true;
            }
            else {
                prevTable.find('.sectionErrorMessage').hide();
            }
        }
        return hasErrors;
    }


    //Displays error messages for a row that is missing a checked checkbox or selected item in a dropdown list
    function CheckItemRowsForErrors(currentRow, hasErrors, missingFixedToPass) {
        var option1 = currentRow.find('td:eq(1)').find('input');
        var option2 = currentRow.find('td:eq(2)').find('input');
        var option3 = currentRow.find('td:eq(3)').find('input');
        var option4 = currentRow.find('td:eq(4)').find('input');
        if (option1.attr('type') === 'checkbox') {
            if (!option1.prop('checked') && !option2.prop('checked') && !option3.prop('checked') && !option4.prop('checked')) {
                currentRow.find('.errorMessage').show();
                hasErrors = true;
            }
            else if (option2.prop('checked') && option4.length > 0 && !option4.prop('checked')) {
                missingFixedToPass = true;
            }
            else {
                currentRow.find('.errorMessage').hide();
            }
        }

        var spareTireDropdown = currentRow.find('.spareTireDropdown1');
            if (spareTireDropdown.length > 0) {
                if (spareTireDropdown.val() === 'Select One') {
                    //spare tire type must be selected error
                    currentRow.find('.measurementError').text('Select a spare tire type');
                    currentRow.find('.measurementError').show();
                    hasErrors = true;
                }
                else if (spareTireDropdown.val() === 'Full Size') {
                    var measurementDropdown = currentRow.find('.spareTreadDropdown');
                    if (measurementDropdown.val() === 'Select One') {
                        // measurement error 
                        currentRow.find('.measurementError').text('Select a tire tread measurement');
                        currentRow.find('.measurementError').show();
                        hasErrors = true;
                    }
                    else {
                        currentRow.find('.measurementError').hide();
                    }
                }
                else {
                    currentRow.find('.measurementError').hide();
                }
            }
            var treadDropDown = currentRow.find('.treadDropdown1');
            if(treadDropDown.length > 0)
            {
                if (treadDropDown.val() === 'Select One') {
                    //houston we have a problem
                    currentRow.find('.measurementError').show();
                    hasErrors = true;
                }
                else {
                    currentRow.find('.measurementError').hide();
                }
            }

            var brakeLiningDropdown = currentRow.find('.brakeLiningDropdown');
            if (brakeLiningDropdown.length > 0) {
                if (brakeLiningDropdown.val() === 'Select One') {
                    //houston we have a problem
                    currentRow.find('.measurementError').show();
                    hasErrors = true;
                }
                else {
                    currentRow.find('.measurementError').hide();
                }
            }
            return {
            hasErrors,
            missingFixedToPass
        }

    }

    function IsSectionComplete(sectionTable) {
        var isSectionComplete = false;

        isSectionComplete = CheckSectionsWithInitialCheckbox(sectionTable);

        return isSectionComplete
    }

    //Sections that have items with a single checkbox at the beginning of items: makes sure that at least one checkbox is checked
    function CheckSectionsWithInitialCheckbox(sectionTable) {
        var sectionID = sectionTable.attr('data-itemid');
        var sectionLinkID = '#SecitonLink_' + sectionTable.attr('data-itemID');
        var skipItemChecks = false;
        if (sectionID == SECTION2ID || sectionID == SECTION4ID)
        {
            //check to see if the first item is the previous section (1 or 3) is checked
            var prevSectionTable = sectionTable.prev('table');
            var firstCheckbox = prevSectionTable.find('tr:eq(2)').find('.initialCheckbox');

            //if it isn't checked, remove the 'complete' class
            if(firstCheckbox.length > 0 && firstCheckbox.prop('checked'))
            {
                skipItemChecks = true;
            }
        }
        else
        {
            $(document).find(sectionLinkID).removeClass('complete');
        }
        var firstSectionItem = true;
        if (!skipItemChecks) {
            sectionTable.find('.itemlist').each(function () {
                var initialCheckbox = $(this).find('td:eq(0)').find('input');
                if (initialCheckbox.length > 0) {
                    if (initialCheckbox.is(':checked')) {
                        $(document).find(sectionLinkID).addClass('complete');
                        if ((sectionID == 1 || sectionID == 3) && firstSectionItem) {
                            var nextSectionLink = '#SecitonLink_' + (parseInt(sectionID) + 1);
                            $(document).find(nextSectionLink).find('tr:eq(2)').find('.initialCheckbox').prop('checked', 'true');
                            $(document).find(nextSectionLink).addClass('complete');
                        }
                        return false;
                    }
                    else {
                        $(document).find(sectionLinkID).removeClass('complete');
                        if ((sectionID == SECTION1ID || sectionID == SECTION3ID) && firstSectionItem) {
                            //var nextSectionLink = '#SecitonLink_' + (parseInt(sectionID) + 1);
                            //$(document).find(nextSectionLink).removeClass('complete');
                            CheckSectionsWithInitialCheckbox(sectionTable.next('table'));
                        }
                    }
                }
                firstSectionItem = false;
            });
        }
    }

    //Sections that have items with multiple possible choices: 
    //  - makes sure at leas one item is checked (for checkboxes)
    //  = makes sure all dropdown lists have valid selections
    function CheckItemlistSectionForComplete(sectionTable) {
        var sectionComplete = true;
        var sectionID = sectionTable.attr('data-itemID');
        var setSectionWarningClass = false;

        sectionTable.find('.itemlist').each(function () {
            var itemID = $(this).attr('data-itemID');

            var checkbox1 = $(this).find('.checkboxItem1');
            var tireTreadDropDown = $(this).find('.treadDropdown1'); //jjs previous var for first tread measurement when all on one line
            var spareTireDropDown = $(this).find('.spareTireDropdown1');
            var brakeLiningDropDown = $(this).find('.brakeLiningDropdown')

            if (checkbox1.length > 0) {
                var checkbox2 = $(this).find('.checkboxItem2');
                var checkbox3 = $(this).find('.checkboxItem3');
                if (!(checkbox1.length > 0 && checkbox1.prop('checked')) &&
                    !(checkbox2.length > 0 && checkbox2.prop('checked')) &&
                    !(checkbox3.length > 0 && checkbox3.prop('checked'))) {
                    sectionComplete = false;
                    return false;
                }
                else if (checkbox2.length > 0 && checkbox2.prop('checked')) {
                    var checkbox4 = $(this).find('.checkboxItem4');
                    if (checkbox4.length > 0 && !checkbox4.prop('checked')) {
                        setSectionWarningClass = true;
                    }
                }
            }
            if (tireTreadDropDown.length > 0) {
                if (tireTreadDropDown.val() === 'Select One')
                {
                    sectionComplete = false;
                    return false;
                }
            }
            if (spareTireDropDown.length > 0) {
                var spareTreadDropdown = $(this).find('.spareTreadDropdown');
                if (spareTireDropDown.val() == 'Select One' ||
                (spareTireDropDown.val() == 'Full Size' && spareTreadDropdown.length > 0 && spareTreadDropdown.val() == 'Select One')) {
                    sectionComplete = false;
                    return false;
                }
            }
            if (brakeLiningDropDown.length > 0) {
                if (brakeLiningDropDown.val() === 'Select One') {
                    sectionComplete = false;
                    return false;
                }
            }

        });
        var sectionLinkID = '#SecitonLink_' + sectionTable.attr('data-itemID');
        if (sectionComplete)
        {
            if (setSectionWarningClass)
            {
                $(document).find(sectionLinkID).removeClass('complete');
                $(document).find(sectionLinkID).addClass('warning');
            }
            else
            {
                $(document).find(sectionLinkID).removeClass('warning');
                $(document).find(sectionLinkID).addClass('complete');
            }
        }
        else {
            $(document).find(sectionLinkID).removeClass('warning');
            $(document).find(sectionLinkID).removeClass('complete');
        }
    }

    //Goes through all sections to set the sidebar highlights/display error messages
    $(function () {
        $(document).find('.checklistSection').each(function () {
            var table = $(this).closest('table');
            //if the section has items starting with a checkbox
            if (table.find('tr:eq(2)').find('td:eq(0)').find('input').attr('class') === 'initialCheckbox') {
                CheckSectionsWithInitialCheckbox(table);
            }
            else {
                CheckItemlistSectionForComplete(table);
            }
        });
    });

    //Displays "working" gif on ajax calls
    $(document).on({
        ajaxStart: function () {
            $('body').addClass('loading');
        },
        ajaxStop: function () {
            $('body').removeClass('loading');
        }
    });

});