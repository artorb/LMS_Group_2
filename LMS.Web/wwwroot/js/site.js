// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// File Upload
// 

function goBack() {
    window.history.back();
}

function addMore() {
    $('#dynamictable').append('<form asp-action="Create">< div asp-validation-summary="ModelOnly" class= "text-danger" >' +
        '</div ><div class="form-group"><label asp-for="Name" class="control-label"></label><input asp-for="Name" class="form-control" />' +
        '<span asp-validation-for="Name" class="text-danger"></span></div ><div class="form-group"><label asp-for="Description" class="control-label">' +
        '</label ><input asp-for="Description" class="form-control" /><span asp-validation-for="Description" class="text-danger"></span></div >' +
        '<div class="form-group"><label asp-for="StartDate" class="control-label"></label><input asp-for="StartDate" class="form-control" />' +
        '<span asp-validation-for="StartDate" class="text-danger"></span></div ><div class="form-group"><label asp-for="EndDate" class="control-label"></label>' +
        '<input asp-for="EndDate" class="form-control" /><span asp-validation-for="EndDate" class="text-danger"></span></div >' +
        '<div class="form-group"><input type="submit" value="Create" class="btn btn-primary" /></div></form > ');
}




$(document).ready(function () {
    let container = $('#authorFormGroup');
    let count = 0;
    console.log('Before Literature Create method: ' + count);
    function AddAuthorRowOnForm(e) {
        e.preventDefault();

        let maxNoOfRows = 9;
        if (count < maxNoOfRows) {
            $(container).append(
                '<div class="form-group">' +
                '<label asp-for="Name" class="control-label"></label>' +
                '<input asp-for="Name" class="form-control" />' +
                '</div>' +
                '<div class="form-group">' +
                '<label asp-for="Description" class="control-label"></label>' +
                '<input asp-for="Description" class="form-control" />' +
                '</div>' +
                '<div class="form-group">' +
                '<label asp-for="StartDate" class="control-label"></label>' +
                '<input asp-for="StartDate" class="form-control" />' +
                '</div>' +
                '<div class="form-group">' +
                '<label asp-for="EndDate" class="control-label"></label>' +
                '<input asp-for="EndDate" class="form-control" />' +
                '</div>'
            );
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





                    '<div class="form-group">'+             
                    '<label asp-for="Name" class="control-label"></label>' +
                    '<input asp-for="Name" class="form-control" />'+                  
                    '</div>'+
                    '<div class="form-group">'+          
                    '<label asp-for="Description" class="control-label"></label>'+
                    '<input asp-for="Description" class="form-control" />'+                  
                    '</div>' +
                    '<div class="form-group">' +           
                    '<label asp-for="StartDate" class="control-label"></label>' +
                    '<input asp-for="StartDate" class="form-control" />' +                  
                    '</div>' +
                    '<div class="form-group">' +      
                    '<label asp-for="EndDate" class="control-label"></label>' +
                    '<input asp-for="EndDate" class="form-control" />' +
                    '</div>' 


















//$(document).ready(function () {
//    var maxFields = 5;
//    var wrapper = $(".addInputFields");
//    var addButton = $("addInputFieldsButton");
//    var x = 1;
//    $(addButton).click(function (e) {
//        e.preventDefault();
//        if (x < maxFields) {
//            x++;
//            $(wrapper).append('<form asp-action="Create">< div asp-validation-summary="ModelOnly" class= "text-danger" ></div ><div class="form-group"><label asp-for="Name" class="control-label"></label><input asp-for="Name" class="form-control" /><span asp-validation-for="Name" class="text-danger"></span></div><div class="form-group"><label asp-for="Description" class="control-label"></label><input asp-for="Description" class="form-control" /><span asp-validation-for="Description" class="text-danger"></span></div><div class="form-group"><label asp-for="StartDate" class="control-label"></label><input asp-for="StartDate" class="form-control" /><span asp-validation-for="StartDate" class="text-danger"></span></div><div class="form-group"><label asp-for="EndDate" class="control-label"></label><input asp-for="EndDate" class="form-control" /><span asp-validation-for="EndDate" class="text-danger"></span></div><div class="form-group"><input type="submit" value="Create" class="btn btn-primary" /></div></form ><a href="#" class="removeField"><i class="fa fa-times"></i></a>')
//        }
//    });

//    $(wrapper).on("click", ".removeField", function (e) {
//        e.preventDefault();
//        $(this).parent('div').remove();
//        x--;
//    });
//};

//$(document).ready(function () {
//    var i = 1;
//    $('#add').click(function () {
//        i++;
//        $('#dynamictable').append('<form asp-action="Create" id="row' + i +'">< div asp-validation-summary="ModelOnly" class= "text-danger" ></div ><div class="form-group"><label asp-for="Name" class="control-label"></label><input asp-for="Name" class="form-control" /><span asp-validation-for="Name" class="text-danger"></span></div><div class="form-group"><label asp-for="Description" class="control-label"></label><input asp-for="Description" class="form-control" /><span asp-validation-for="Description" class="text-danger"></span></div><div class="form-group"><label asp-for="StartDate" class="control-label"></label><input asp-for="StartDate" class="form-control" /><span asp-validation-for="StartDate" class="text-danger"></span></div><div class="form-group"><label asp-for="EndDate" class="control-label"></label><input asp-for="EndDate" class="form-control" /><span asp-validation-for="EndDate" class="text-danger"></span></div><div class="form-group"><input type="submit" value="Create" class="btn btn-primary" /></div></form > <button name="remove" id="'+i+'" class="btn btn-danger btn_remove">X</button>')
//    });

//    $(document).on('click', '.btn_remove', function () {
//        var button_id = $(this).attr("id");
//        $("#row" + button_id + "").remove();
//    });

//    $('#submit').click(function () {
//        $.ajax({
//            url: "name.create",
//            method: "POST",
//            data: $('#add_name').serialize(),
//            success: function (data) {
//                alert(data);
//                $('#add_name')[0].reset(),
//            }
//        });

//    });

//});





