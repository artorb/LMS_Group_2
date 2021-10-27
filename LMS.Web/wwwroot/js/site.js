// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// File Upload
// 

function goBack() {
    window.history.back();
}


$(document).ready(function () {

    //******** Dynamically module creation ********
    let container = $('#moduleFormGroup');
    let containerActivity = $('#activityFormGroup');
    let count = 0;
    console.log('Before Course Create method: ' + count);

    let authorsCount = $('#addRowBtn').data('authorscount');  

    console.log('Current Module Count: ' + authorsCount);

        function AddModuleRowOnForm(e) {
        e.preventDefault();

        let maxNoOfRows = 9;
        if (count < maxNoOfRows) {
            authorsCount++;
            console.log('Modified (next) Modules Count: ' + authorsCount);

            $(container).append('<div class="form-row border border-light pt-1 mb-2">' +
                '<div class= "form-group col-md-12">' +
                '<label for="Modules_' + authorsCount + '__ModuleName" class="control-label">Module name</label>' +
                '<input id="Modules_' + authorsCount + '__ModuleName" name="Modules[' + authorsCount + '].ModuleName" class="form-control">' +
                '</div>' +
                '<div class= "form-group col-md-12" > ' +
                '<label for="Modules_' + authorsCount + '__ModuleDescription" class="control-label">Description</label>' +
                '<input id="Modules_' + authorsCount + '__ModuleDescription" name="Modules[' + authorsCount + '].ModuleDescription" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-4">' +
                '<label for="Modules_' + authorsCount + '__ModuleStartDate" class="control-label">Start date</label>' +
                '<input type="date" id="Modules_' + authorsCount + '__ModuleStartDate" name="Modules[' + authorsCount + '].ModuleStartDate" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-4">' +
                '<label for="Modules_' + authorsCount + '__ModuleEndDate" class="control-label">End date</label>' +
                '<input type="date" id="Modules_' + authorsCount + '__ModuleEndDate" name="Modules[' + authorsCount + '].ModuleEndDate" class="form-control">' +
                '</div>' +       
                '<div class="form-group">' +
                '<button type="button" class="btn" id="removeRowBtn"><span class="text-danger"><i class="fa fa-minus-circle"></i></span></button >' +
                '</div>' +
                '</div>');

            count++;
            console.log('Add: ' + count);
        }
        else {
            alert('Limit for adding modules has been reached!');
        }
    }
    $('#addRowBtn').click(AddModuleRowOnForm);




    let activityCount = $('#addActivityRowBtn').data('authorscount');
  
    function AddActivityRowOnForm(e) {
        console.log('Modified (first) Activity Count: ' + activityCount);
        e.preventDefault();

        let maxNoOfRows = 9;
        if (count < maxNoOfRows) {
            activityCount++;
            console.log('Modified (next) Activity Count: ' + activityCount);

            $(containerActivity).append('<div class="form-row border border-light pt-1 mb-2">' +
                '<div class= "form-group col-md-12">' +
                '<label for="Activities_' + activityCount + '__ActivityName" class="control-label">Activity name</label>' +
                '<input id="Activities_' + activityCount + '__ActivityName" name="Activities[' + activityCount + '].ActivityName" class="form-control">' +
                '</div>' +
                '<div class= "form-group col-md-12" > ' +
                '<label for="Activities_' + activityCount + '__ActivityDescription" class="control-label">Description</label>' +
                '<input id="Activities_' + activityCount + '__ActivityDescription" name="Activities[' + activityCount + '].ActivityDescription" class="form-control">' +
                '</div>' +       
                '<div class="form-group col-md-4">' +
                '<label for="Activities_' + activityCount + '__ActivityStartDate" class="control-label">Start date</label>' +
                '<input type="date" id="Activities_' + activityCount + '__ActivityStartDate" name="Activities[' + activityCount + '].ActivityStartDate" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-4">' +
                '<label for="Activities_' + activityCount + '__ActivityEndDate" class="control-label">End date</label>' +
                '<input type="date" id="Activities_' + activityCount + '__ActivityEndDate" name="Activities[' + activityCount + '].ActivityEndDate" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-4">' +
                '<label for="Activities_' + activityCount + '__ActivityDeadline" class="control-label">Deadline</label>' +
                '<input type="date" id="Activities_' + activityCount + '__ActivityDeadline" name="Activities[' + activityCount + '].ActivityDeadline" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-4">' +
                '<label for="Activities_' + activityCount + '__ActivityTypeId" class="control-label">Type Id date</label>' +
                '<input type="number" id="Activities_' + activityCount + '__ActivityTypeId" name="Activities[' + activityCount + '].ActivityTypeId" class="form-control">' +
                '</div>' +
                '<div class="form-group">' +
                '<button type="button" class="btn" id="removeRowBtn"><span class="text-danger"><i class="fa fa-minus-circle"></i></span></button >' +
                '</div>' +
                '</div>');

            count++;
            console.log('Add: ' + count);
        }
        else {
            alert('Limit for adding activities has been reached!');
        }
    }
    $('#addActivityRowBtn').click(AddActivityRowOnForm);




    $(container).on('click', '#removeRowBtn', function (e) {
        e.preventDefault();
        e.target.closest('.form-row').remove();

        authorsCount--;
        count--;
        console.log('Remove: ' + count);
    });
    //******** end Dynamically modules creation ********

}); // end doc ready
