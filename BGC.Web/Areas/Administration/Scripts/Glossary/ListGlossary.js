$(document).ready(function()
{
    $(".btn-delete-entry").click(function ()
    {
        if (window.confirm($("#confirmDeleteMessage").val()))
        {
            var btn = $(this);
            var actionUrl = btn.data("delete-action");
            $.post(actionUrl).done(function ()
            {
                btn.closest("tr").remove();
            })
            .fail(function ()
            {
                window.alert($("#errorDeleteMessage").val());
            });
        }
    });
})