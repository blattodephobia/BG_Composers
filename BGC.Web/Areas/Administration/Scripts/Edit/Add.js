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

    $('.collapse').on('shown.bs.collapse', function ()
    {
        var link = $(this).parent().find("a[href='#" + this.id + "']");
        var icon = link.find("i");
        icon.removeClass().addClass(icon.data("icon-hide"));
    }).on('hidden.bs.collapse', function ()
    {
        var link = $(this).parent().find("a[href='#" + this.id + "']");
        var icon = link.find("i");
        icon.removeClass().addClass(icon.data("icon-show"));
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
                    localDiv.innerHTML = complete;
                }
            };

            xhr.onreadystatechange = function (e)
            {
                if (this.readyState == 4 && this.status == 200)
                {
                    var localDiv = div;
                    var img = document.createElement("img");
                    img.src = this.responseText;
                    localDiv.appendChild(img);
                }
            };

            xhr.open('POST', '/resources/upload');
            xhr.send(formData);
        }
        return stopEvent(e);
    });
});