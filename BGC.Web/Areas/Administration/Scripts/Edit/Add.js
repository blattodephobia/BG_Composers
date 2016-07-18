$(document).ready(function ()
{
    tinymce.init({
        selector: 'textarea.tinymce',
        height: 500,
        plugins: [
          'advlist autolink lists link image charmap print preview anchor',
          'searchreplace visualblocks code fullscreen paste',
          'insertdatetime media table contextmenu paste code'
        ],
        toolbar: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        content_css: [
          '//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css',
          '//www.tinymce.com/css/codepen.min.css'
        ],
        paste_data_images: true
    });
});

function stopEvent(e)
{
    if (e.preventDefault instanceof Function) e.preventDefault();

    return false;
}

$(document).ready(function ()
{
    $('#uploadArea').on("dragover", function (e)
    {
        var dt = e.dataTransfer || (e.originalEvent && e.originalEvent.dataTransfer);
        dt.dropEffect = "copy";
        return stopEvent(e);
    });
    $('#uploadArea').on("dragenter", function (e)
    {
        var dt = e.dataTransfer || (e.originalEvent && e.originalEvent.dataTransfer);
        dt.dropEffect = "copy";
        return stopEvent(e);
    });
    $('#uploadArea').on("drop", function (e)
    {
        var dt = e.dataTransfer || (e.originalEvent && e.originalEvent.dataTransfer);
        var input = $('#uploadInput');
        input[0].files = dt.files;
        $("#uploadArea").submit();
        return stopEvent(e);
    });
});