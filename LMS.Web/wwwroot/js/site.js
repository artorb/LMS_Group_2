// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// File Upload
// 


$('button[name="btn-techer-register"]').on('click', function (e) {
    var $form = $(this).closest('form');
    e.preventDefault();
    $('#confirm-techer-creation').modal({
        backdrop: 'static',
        keyboard: false
    })
        .on('click', '#confirm', function (e) {
            $form.trigger('submit');
        });
});

$('#sucess-alert').delay(10000).fadeOut('slow');
