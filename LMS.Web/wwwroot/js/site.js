// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.
$(document).ready(function () {

    //******** Dynamically no of Authors for creation ********
    let authorFormContainer = $('#authorFormContainer');
    let addAuthorJsFuncCount = 0;
    //console.log('Before Literature Create method: ' + addAuthorJsFuncCount);
    //let currentAuthorRowCount = parseInt($('#addAuthorRowBtn').data('currentAuthorRowCount'));
    let currentAuthorRowCount = 0;
    //console.log('Current Author Count: ' + currentAuthorRowCount);
    function AddRowsOnAuthorCreateForm(e) {
        e.preventDefault();
        
        let maxNoOfRows = 7;
        if (addAuthorJsFuncCount < maxNoOfRows) {
            currentAuthorRowCount++;
            //console.log('Modified (next) Author Count: ' + currentAuthorRowCount);

            $(authorFormContainer).append('<div class="form-row border border-light pt-1 mb-2">' +
                '<div class= "form-group col-md-4">' +
                    '<label for="Authors_' + currentAuthorRowCount + '__FirstName" class="control-label">First name</label>' +
                    '<input id="Authors_' + currentAuthorRowCount + '__FirstName" name="Authors[' + currentAuthorRowCount + '].FirstName" class="form-control">' +
                '</div>' +
                '<div class= "form-group col-md-4" > ' +
                    '<label for="Authors_' + currentAuthorRowCount + '__LastName" class="control-label">Last name</label>' +
                    '<input id="Authors_' + currentAuthorRowCount + '__LastName" name="Authors[' + currentAuthorRowCount + '].LastName" class="form-control">' +
                '</div>' +
                '<div class="form-group col-md-3">' +
                    '<label for="Authors_' + currentAuthorRowCount + '__BirthDate" class="control-label">Birthdate</label>' +
                    '<input type="date" id="Authors_' + currentAuthorRowCount + '__BirthDate" name="Authors[' + currentAuthorRowCount + '].BirthDate" class="form-control">' +
                '</div>' +
                '<div class="form-group">' +
                    '<button type="button" class="btn" id="removeAuthorRowBtn"><span class="text-danger"><i class="fa fa-minus-circle fa-lg"></i></span></button >' +
                '</div>' +
                '</div>');
            
            addAuthorJsFuncCount++;
            //console.log('Add: ' + addAuthorJsFuncCount);
        }
        else {
            alert('Limit for adding authors has been reached!');
        }
    }
    $('#addAuthorRowBtn').click(AddRowsOnAuthorCreateForm);

    $(authorFormContainer).on('click', '#removeAuthorRowBtn', function (e) {
        e.preventDefault();
        e.target.closest('.form-row').remove();

        currentAuthorRowCount--;
        addAuthorJsFuncCount--;
        //console.log('Remove: ' + addAuthorJsFuncCount);
    });
    //******** end Dynamically no of Authors for creation ********

}); // end doc ready