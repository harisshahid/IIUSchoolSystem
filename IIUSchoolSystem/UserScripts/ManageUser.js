$(document).ready(function () {

    $('#CreateOrUpdateUser').click(function () {
        var vl = $('#userForm').valid();
        if (vl) {
            CreateOrUpdateUser();
        }
    });
    $('#createUser').click(function () {
        $('#createAccount').slideToggle(500);
        ResetFields();
        $("#UserName").removeAttr('readonly');
        $('#headingUser').html("");
        $('#headingUser').html('<i class="icon-reorder"></i>New Account');
    });
    $('#cancelUserCreation').click(function () {
        $("#userForm input[type='text']").removeClass("error");
        $('#createAccount').slideToggle(500);
        $("#Id").val(0);
        ResetFields();
        $("#UserName").removeAttr('readonly');
        $('#headingUser').html("");
        $('#headingUser').html('<i class="icon-reorder"></i>New Account');
    });
});

function EditUser(id) {
    var request = "{'id':'" + id + "'}";

    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'GetUser',
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
                FillEditForm(data.User);
            }
            else {
                Notify(data.message, 'error');
            }
        },
        error: OnError
    });
}

function FillEditForm(data) {

    $("#userForm input[type='text']").removeClass("error");

    $('#Id').val(data.Id);
    $('#FirstName').val(data.FirstName);
    $('#LastName').val(data.LastName);
    $('#SchoolId').val(data.SchoolId);
    $('#Email').val(data.Email);
    $('#Phone').val(data.Phone);
    $('#Cell').val(data.Cell);
    $('#UserName').val(data.UserName);
    $('#Password').val(data.Password);
    $('#RoleId').val(data.RoleId);
    $('#Address').val(data.Address);
    $('#City').val(data.City);
    $('#Zip').val(data.Zip);
    $('#StateId').val(data.StateId);
    $('#hdnGuid').val(data.GUID);
    $('#Active').prop('checked', data.Active);

    $('#headingUser').html("");
    $('#headingUser').html('<i class="icon-reorder"></i>Update Account');

    $("#UserName").attr('readonly', 'readonly');
    $('#createAccount').slideToggle(500);
}

function CreateOrUpdateUser() {
    var request = {
        Id: $("#Id").val(), FirstName: $('#FirstName').val(), LastName: $('#LastName').val(), Email: $('#Email').val(),
        Phone: $('#Phone').val(), Cell: $('#Cell').val(), UserName: $('#UserName').val(), Password: $('#Password').val(), RoleId: $('#RoleId').val(), Active: $('#Active').is(':checked'), GUID: $('#hdnGuid').val(),
        Address: $('#Address').val(), City: $('#City').val(), Zip: $('#Zip').val(), StateId: $('#StateId').val()
    };

    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'CreateOrUpdateUser',
        data: JSON.stringify({ 'model': request }),
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
                $('#createAccount').slideToggle(500);
                ResetFields();
                RefreshGrid("user-grid");
            }
            else {
                Notify(data.message, 'error');
            }
            $("#Id").val(0);
        },
        error: OnError
    });
    RefreshGrid("user-grid");
}

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
                    DeleteUser(id);
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

function DeleteUser(id) {

    var request = "{'Id':'" + id + "'}";
    $.ajax({
        type: "POST",
        url: GetSiteRoot() +'Delete',
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
                RefreshGrid("user-grid");
            } else {
                Notify(data.message, 'error');
            }
        },
        error: OnError
    });
}

