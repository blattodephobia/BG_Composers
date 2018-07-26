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
        var imagesArea = $("#uploadedImages");
        for (var i = 0; i < dt.files.length; i++)
        {
            var div = document.createElement("div");
            imagesArea.append(div);
            var formData = new FormData();
            formData.append("file", dt.files[i]);

            var xhr = new XMLHttpRequest();
            xhr.upload.onprogress = function (event)
            {
                if (event.lengthComputable)
                {
                    var localDiv = div;
                    var complete = (event.loaded / event.total * 100 | 0);
                }
            };

            xhr.onreadystatechange = function (e)
            {
                if (this.readyState == 4 && this.status == 200)
                {
                    var $localDiv = $(div);
                    var img = document.createElement("img");
                    img.src = this.responseText;
                    $localDiv.append($(img));
                    var $imgInputs = $("#imgInputs");
                    var imageIndex = $("#imgInputs > input.img-location").length;
                    var locationHtml = '<input name="Images[' + imageIndex + '].Location" type="hidden" value="' + this.responseText + '"></input>';
                    var preferredCheckBoxHtml = '<input name="Images[' + imageIndex + '].Preferred" type="checkbox" value="False"></input>';
                    $imgInputs.append(locationHtml);
                    $imgInputs.append(preferredCheckBoxHtml);
                    $("#uploadedImages").append($localDiv);
                }
            };

            var url = $(this).data("action-url");
            xhr.open("POST", url);
            xhr.send(formData);
        }
        return stopEvent(e);
    });
});