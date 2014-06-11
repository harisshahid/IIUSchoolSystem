$(document).ready(function () {
    $('#CreateOrUpdateRole').click(function () {
        var vl = $('#roleForm').valid();
        if (vl) {
            CreateOrUpdateRole();
        }
    });
    $('#createNewRoleButton').click(function () {
        ResetFields();
        $('#createRole').slideToggle(500);
    });
    $('#cancelRoleCreation').click(function () {
        $("#Id").val(0);
        $('#createRole').slideToggle(500);
        ResetFields();
    });
});

function onDataBinding(e) {
}

function ConfirmDelete(id) {

    var messageTitle = "<div class='widget-header'><h4 class='smaller'><i class='fa fa-warning red'></i> Delete Confirmation</h4></div>";

    $("#dialog-confirm").removeClass('hide').dialog({
        resizable: false,
        width: '364',
        modal: true,
        buttons: [
            {
                html: "<i class='fa fa-trash-o bigger-110'></i>&nbsp; Delete",
                "class": "btn btn-danger btn-primary",
                click: function () {
                    DeleteRole(id);
                    $(this).dialog("close");
                }
            }
            ,
             {
                 html: "<i class='fa fa-times bigger-110'></i>&nbsp; Cancel",
                 "class": "btn btn-primary",
                 click: function () {
                     $(this).dialog("close");
                 }
             }
        ],
        show: 'blind',
        hide: 'fold',
        closeOnEscape: true,
    }).dialog('widget').find('.ui-dialog-title').html(messageTitle);
}

function DeleteRole(id) {
    var request = "{'Id':'" + id + "'}";

    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'DeleteRole',
        data: request,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data;
            if (msg.hasOwnProperty("d")) {
                data = msg.d;
            }
            else
                data = msg;
            if (data.success) {
                Notify(data.message, 'success');
                RefreshGrid("roles");
            } else {
                Notify(data.message, 'error');
            }
        },
        error: OnError
    });
}

function CreateOrUpdateRole() {
    var request = "{'Id':'" + $("#Id").val() + 
        "','Name':'" + $('#roleName').val() +
        "','Description':'" + $('#roleDescription').val().replace(/'/g, "\\'") +
        "','Active':'" + $('#Active').is(':checked') +
        "'}";
    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'CreateOrUpdateRole',
        data: request,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data;
            if (msg.hasOwnProperty("d")) {
                data = msg.d;
            }
            else
                data = msg;
            if (data.success) {
                Notify(data.message, 'success');
                $('#createRole').slideToggle(500);
                ResetFields();
                RefreshGrid("roles-grid");
            }
            else {
                Notify(data.message, 'error');
            }
            $("#Id").val(0);
        },
        error: OnError
    });
}

function EditRole(id) {
    var request = "{'id':'" + id + "'}";
    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'GetRole',
        data: request,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var data;
            if (msg.hasOwnProperty("d")) {
                data = msg.d;
            }
            else
                data = msg;
            if (data.success) {
                FillRoleEditForm(data.Role);
            }
            else {
                Notify(data.message, 'error');
            }
        },
        error: OnError
    });
}

function FillRoleEditForm(data) {
    $('#Id').val(data.Id);
    $('#roleName').val(data.Name);
    $('#roleDescription').val(data.Description);
    $('#Active').prop('checked', data.Active);
    $('#createRole').slideToggle(500);
}