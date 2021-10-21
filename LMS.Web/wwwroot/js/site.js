// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    let container = $('#authorFormGroup');
    let count = 0;
    console.log('Before Literature Create method: ' + count);
    function AddAuthorRowOnForm(e) {
        e.preventDefault();

        let maxNoOfRows = 9;
        if (count < maxNoOfRows) {
            $(container).append('<div class="form-row border border-light pt-1 mb-2">' +
                '<div class= "form-group col-md-4">' +
                    '<label for="inputEmail4">First name</label>' +
                    '<input type="text" class="form-control" id="inputEmail4">' +
                '</div>' +
                '<div class= "form-group col-md-4" > ' +
                    '<label for="inputPassword4">Last name</label>' +
                    '<input type="text" class="form-control" id="inputPassword4">' +
                '</div>' +
                '<div class="form-group col-md-3">' +
                    '<label for="inputPassword4">Birthday</label >' +
                    '<input type="date" class="form-control" id="inputPassword4">' +
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

        count--;
        console.log('Remove: ' + count);
    });

}); // end doc ready