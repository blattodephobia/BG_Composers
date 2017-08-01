$(document).ready(function ()
{
    $('.collapse')
    .on('shown.bs.collapse', function ()
    {
        var link = $(this).parent().find("[data-toggle='collapse']");
        var icon = link.find("i");
        icon.removeClass().addClass(icon.data("icon-hide"));
    })
    .on('hidden.bs.collapse', function ()
    {
        var link = $(this).parent().find("[data-toggle='collapse']");
        var icon = link.find("i");
        icon.removeClass().addClass(icon.data("icon-show"));
    });
});