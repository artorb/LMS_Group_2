// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
$(document).ready(function () {

    //******** Dynamically no of Authors for creation ********
    let container = $('#authorFormGroup');
    let count = 0;
    console.log('Before Literature Create method: ' + count);

    let authorsCount = $('#addRowBtn').data('authorscount');
    
    console.log('Current Author Count: ' + authorsCount);
    function AddAuthorRowOnForm(e) {
        e.preventDefault();
        
        let maxNoOfRows = 9;
        if (count < maxNoOfRows) {
            authorsCount++;
            console.log('Modified (next) Author Count: ' + authorsCount);

            $(container).append('<div class="form-row border border-light pt-1 mb-2">' +
                '<div class= "form-group col-md-4">' +
                   '<label for="Authors_' + authorsCount + '__FirstName" class="control-label">First name</label>' +
                   '<input id="Authors_' + authorsCount + '__FirstName" name="Authors[' + authorsCount + '].FirstName" class="form-control">' +
                '</div>' +
                '<div class= "form-group col-md-4" > ' +
                   '<label for="Authors_' + authorsCount + '__LastName" class="control-label">Last name</label>' +
                   '<input id="Authors_' + authorsCount + '__LastName" name="Authors[' + authorsCount + '].LastName" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-3">' +
                   '<label for="Authors_' + authorsCount + '__BirthDay" class="control-label">Birthday</label>' +
                '<input type="date" id="Authors_' + authorsCount + '__BirthDay" name="Authors[' + authorsCount + '].BirthDay" class="form-control">' +
                '</div>' +
                '<div class="form-group">' +
                    '<button type="button" class="btn" id="removeRowBtn"><span class="text-danger"><i class="fa fa-minus-circle"></i></span></button >' +
                '</div>' +
                '</div>');
            
            count++;
            console.log('Add: ' + count);
        }
        else {
            alert('Limit for adding authors has been reached!');
        }
    }
    $('#addRowBtn').click(AddAuthorRowOnForm);

    $(container).on('click', '#removeRowBtn', function (e) {
        e.preventDefault();
        e.target.closest('.form-row').remove();

        authorsCount--;
        count--;
        console.log('Remove: ' + count);
    });
    //******** end Dynamically no of Authors for creation ********

}); // end doc ready